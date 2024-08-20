// Copyright © 2024 Lionk Project

using Lionk.Core.Observable;
using Lionk.Log;
using Lionk.Notification;

namespace Lionk.Core.Component.Cyclic;

/// <summary>
/// Service responsible for executing cyclic components and managing their execution cycle.
/// </summary>
public class CyclicExecutorService : ObservableElement, ICyclicExecutorService
{
    private readonly object _stateLock = new();
    private readonly IComponentService _componentService;
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _executionTask;

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicExecutorService"/> class.
    /// </summary>
    /// <param name="componentService">The service that provides access to components.</param>
    public CyclicExecutorService(IComponentService componentService)
    {
        _componentService = componentService;
        WatchDogTimeout = TimeSpan.FromSeconds(10);

        State = CycleState.Stopped;
    }

    private CycleState _cycleState;

    /// <inheritdoc/>
    public CycleState State
    {
        get => _cycleState;
        set => SetField(ref _cycleState, value);
    }

    private TimeSpan _watchdogTimeout;

    /// <inheritdoc/>
    public TimeSpan WatchDogTimeout
    {
        get => _watchdogTimeout;
        set => SetField(ref _watchdogTimeout, value);
    }

    /// <inheritdoc/>
    public IEnumerable<ICyclicComponent> Components
        => _componentService.GetInstancesOfType<ICyclicComponent>();

    /// <inheritdoc/>
    public void Start()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Running) return;

            _cancellationTokenSource = new CancellationTokenSource();
            State = CycleState.Running;

            _executionTask = Task.Run(Execute, _cancellationTokenSource.Token);
        }
    }

    /// <inheritdoc/>
    public void Pause()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Running)
                State = CycleState.Paused;
        }
    }

    /// <inheritdoc/>
    public void Resume()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Paused)
                State = CycleState.Running;
        }
    }

    /// <inheritdoc/>
    public void Stop()
    {
        lock (_stateLock)
        {
            if (State == CycleState.Stopped || State == CycleState.Stopping) return;

            State = CycleState.Stopping;
            _cancellationTokenSource?.Cancel();

            foreach (ICyclicComponent component in Components)
                component.Abort();

            State = CycleState.Stopped;
        }
    }

    /// <summary>
    /// Main execution loop that handles the cyclic execution of components.
    /// It checks the state of the service and executes components based on their schedule.
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

            var watchdogCancellationSource = new CancellationTokenSource(WatchDogTimeout);
            var combinedCancellation =
                CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token,
                    watchdogCancellationSource.Token);

            try
            {
                await ExecuteComponents(combinedCancellation.Token);
            }
            catch (TaskCanceledException)
            {
                if (watchdogCancellationSource.Token.IsCancellationRequested && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Stop();

                    Content content = new(
                        Severity.Warning,
                        "Watchdog timeout",
                        "something caused a watchdog timeout.");

                    var notification = new Notification.Notification(content, _notifyer);

                    // TODO CJS -> Uncomment when notification work
                    // NotificationService.Send(notification);
                    LogService.LogApp(LogSeverity.Warning, $"Watchdog timeout exceeded");
                }
            }

            await Task.Delay(10, _cancellationTokenSource.Token); // Delay between cycles
        }
    }

    /// <summary>
    /// Iterates through all cyclic components and executes those that are ready and not in error.
    /// </summary>
    /// <param name="combinedToken">A combined cancellation token that includes both the service's and the watchdog's cancellation tokens.</param>
    private async Task ExecuteComponents(CancellationToken combinedToken)
    {
        foreach (ICyclicComponent component in Components)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
                break;

            if (component.NextExecution <= DateTime.Now && component.CanExecute && !component.IsInError)
            {
                await ExecuteComponent(component, combinedToken);
            }
        }
    }

    /// <summary>
    /// Executes a single cyclic component, handling cancellation and errors.
    /// If the execution fails or times out, the component is aborted.
    /// </summary>
    /// <param name="component">The cyclic component to execute.</param>
    /// <param name="combinedToken">A combined cancellation token that includes both the service's and the watchdog's cancellation tokens.</param>
    private async Task ExecuteComponent(ICyclicComponent component, CancellationToken combinedToken)
    {
        try
        {
            var task = Task.Run(component.Execute, combinedToken);

            while (!task.IsCompleted)
            {
                if (combinedToken.IsCancellationRequested)
                    throw new TaskCanceledException("Watchdog timeout exceeded");

                await Task.Delay(1); // Petit délai pour ne pas saturer le CPU
            }

            if (task.IsCanceled && !combinedToken.IsCancellationRequested)
                throw new TaskCanceledException("Watchdog timeout exceeded");
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            component.Abort(); // Abort the component if an exception occurs

            Content content = new(
                    Severity.Warning,
                    "Device crash during cycle execution",
                    $"The device : {component.InstanceName} has been aborted during the cycle execution, error message : {ex.Message}");

            var notification = new Notification.Notification(content, _notifyer);
            NotificationService.Send(notification);

            LogService.LogApp(LogSeverity.Error, $"{component.InstanceName} failed during execution: {ex.Message}");
        }
    }

    private readonly INotifyer _notifyer = new ServiceNotifyer();

    private class ServiceNotifyer : INotifyer
    {
        public Guid Id => Guid.NewGuid();

        public string Name => "Cyclic executor service";

        public bool Equals(INotifyer? obj) => obj is ServiceNotifyer && obj.Id == Id;
    }
}
