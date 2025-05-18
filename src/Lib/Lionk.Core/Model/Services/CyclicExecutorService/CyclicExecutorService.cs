// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using System.Diagnostics;
using Lionk.Core.Observable;
using Lionk.Log;

namespace Lionk.Core.Component.Cyclic;

/// <summary>
///     Service responsible for executing cyclic components and managing their execution cycle.
/// </summary>
public class CyclicExecutorService : ObservableElement, ICyclicExecutorService
{
    #region fields

    private const string ConfigFileName = "CyclicExecutorConfig.json";

    private readonly IComponentService _componentService;

    private readonly object _stateLock = new();

    private readonly Stopwatch _cycleStopwatch = new();

    private readonly ConcurrentDictionary<Guid, DateTime> _componentsTimeout = [];

    private CancellationTokenSource _cancellationTokenSource = new();

    private Task _executorTask = Task.CompletedTask;

    private CycleState _cycleState;

    private TimeSpan _meanCycleTime;

    private TimeSpan _maxCycleTime;

    private TimeSpan _lastExecutionTime;

    private long _nCycle;
    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="CyclicExecutorService" /> class.
    /// </summary>
    /// <param name="componentService">The service that provides access to components.</param>
    public CyclicExecutorService(IComponentService componentService)
    {
        _componentService = componentService;
        State = CycleState.Stopped;
    }

    #endregion

    #region properties

    /// <inheritdoc />
    public List<ICyclicComponent> Components => _componentService.GetInstancesOfType<ICyclicComponent>().ToList();

    /// <inheritdoc />
    public CycleState State
    {
        get => _cycleState;
        set => SetField(ref _cycleState, value);
    }

    /// <inheritdoc />
    public TimeSpan MeanCycleTime
    {
        get => _meanCycleTime;
        set => SetField(ref _meanCycleTime, value);
    }

    /// <inheritdoc />
    public TimeSpan MaxCycleTime
    {
        get => _maxCycleTime;
        set => SetField(ref _maxCycleTime, value);
    }

    /// <inheritdoc />
    public TimeSpan LastExecutionTime
    {
        get => _lastExecutionTime;
        set => SetField(ref _lastExecutionTime, value);
    }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Abort method.
    /// </summary>
    public void Abort()
    {
        foreach (ICyclicComponent component in Components)
        {
            component.Abort();
        }

        State = CycleState.Stopped;
    }

    /// <inheritdoc />
    public void Pause()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Running)
            {
                State = CycleState.Paused;
            }
        }
    }

    /// <inheritdoc />
    public void Resume()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Paused)
            {
                State = CycleState.Running;
            }
            else if (State == CycleState.Stopped)
            {
                InternalStart();
            }
        }
    }

    /// <inheritdoc />
    public void Start()
    {
        lock (_stateLock)
        {
            InternalStart();
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        lock (_stateLock)
        {
            if (State is CycleState.Stopped or CycleState.Stopping)
            {
                return;
            }

            State = CycleState.Stopping;
            _cancellationTokenSource.Cancel();

            while (State != CycleState.Stopped && !_executorTask.IsCompleted)
            {
                Thread.Sleep(10);
            }

            State = CycleState.Stopped;
        }
    }

    #endregion

    #region others methods

    /// <summary>
    ///     Main execution loop that handles the cyclic execution of components.
    ///     It checks the state of the service and executes components based on their schedule.
    /// </summary>
    private async Task Execute()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            if (State == CycleState.Paused)
            {
                await Task.Delay(100, _cancellationTokenSource.Token); // Sleep briefly while paused
                continue;
            }

            try
            {
                _nCycle++;
                CheckComponentsTimeout();
                _cycleStopwatch.Restart();
                int nbExecutedComponents = ExecuteComponents(_cancellationTokenSource.Token);
                _cycleStopwatch.Stop();
                ManageTimeMeasurement(nbExecutedComponents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Abort();
                LogService.LogApp(LogSeverity.Warning, $"Exception thrown during execution {e.Message}");
            }

            await Task.Delay(10, _cancellationTokenSource.Token); // Delay between cycles
        }
    }

    private void CheckComponentsTimeout()
    {
        var timedOutComponentIds
            = (from kvp in _componentsTimeout
               where DateTime.UtcNow > kvp.Value
               select kvp.Key).ToList();

        IEnumerable<ICyclicComponent> componentsTimedOut
            = Components.Where(x => timedOutComponentIds.Contains(x.Id));

        foreach (ICyclicComponent component in componentsTimedOut)
        {
            component.Abort();
            _componentsTimeout.Remove(component.Id, out _);

            LogService.LogApp(
                LogSeverity.Warning,
                $"Component {component.InstanceName} timed out and was aborted");
        }
    }

    private void ManageTimeMeasurement(int nbExecutedComponents)
    {
        LastExecutionTime = _cycleStopwatch.Elapsed;

        if (LastExecutionTime > MaxCycleTime)
            MaxCycleTime = LastExecutionTime;

        // do not include first cycle in the mean cycle time
        if (_nCycle > 10 && nbExecutedComponents > 0)
        {
            if (MeanCycleTime == TimeSpan.Zero)
            {
                MeanCycleTime = LastExecutionTime;
            }
            else
            {
                MeanCycleTime = (LastExecutionTime + (MeanCycleTime * (_nCycle - 1))) / _nCycle;
            }
        }
    }

    /// <summary>
    ///     Executes a single cyclic component, handling cancellation and errors.
    ///     If the execution fails or times out, the component is aborted.
    /// </summary>
    /// <param name="component">The cyclic component to execute.</param>
    /// <param name="token">A combined cancellation token that includes the service's token.</param>
    private Task? StartComponent(ICyclicComponent component, CancellationToken token)
    {
        try
        {
            var task = new Task(component.Execute, token);

            task.ContinueWith(
                t =>
                {
                    if (t is { IsFaulted: true, Exception: not null })
                    {
                        component.Abort();
                        LogService.LogApp(
                            LogSeverity.Error,
                            $"{component.InstanceName} failed during execution : {t.Exception.InnerException?.Message}");
                    }

                    _componentsTimeout.Remove(component.Id, out _);
                },
                TaskScheduler.Default);

            return task;
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            component.Abort(); // Abort the component if an exception occurs
            _componentsTimeout.Remove(component.Id, out _);
            LogService.LogApp(LogSeverity.Error, $"{component.InstanceName} failed during execution: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    ///     Iterates through all cyclic components and executes those that are ready and not in error.
    /// </summary>
    /// <param name="token">A cancellation token that comes from the service.</param>
    private int ExecuteComponents(CancellationToken token)
    {
        int counter = 0;

        foreach (ICyclicComponent component in Components)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                break;
            }

            if (IsComponentReadyForNewExecution(component))
            {
                counter++;
                Task? task = StartComponent(component, token);

                if (task is not null)
                    _componentsTimeout.TryAdd(component.Id, DateTime.Now + component.Timeout);

                task?.Start();
            }
        }

        return counter;
    }

    private bool IsComponentReadyForNewExecution(ICyclicComponent component) =>
        component.NextExecution <= DateTime.Now
        && component is { CanExecute: true, IsInError: false }
        && !_componentsTimeout.ContainsKey(component.Id)
        && !component.IsRunning;

    private void InternalStart()
    {
        if (State == CycleState.Running)
        {
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        State = CycleState.Running;
        _executorTask = Task.Run(Execute, _cancellationTokenSource.Token);
    }

    #endregion
}
