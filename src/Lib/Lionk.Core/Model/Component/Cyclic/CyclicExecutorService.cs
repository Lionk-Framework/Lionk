// Copyright © 2024 Lionk Project

using System.Timers;

using Lionk.Core.Component;
using Lionk.Log;

namespace Lionk.Core.Model.Component.Cyclic;

/// <summary>
/// This class is used to execute cyclic components.
/// </summary>
public class CyclicExecutorService : ICyclicExecutorService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly System.Timers.Timer _timer;
    private Thread _thread;

    private void Execute()
    {
        try
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                switch (State)
                {
                    case CycleState.Paused:
                        continue;
                    case CycleState.Running:
                        Running();
                        break;
                    case CycleState.Stopped:
                        State = CycleState.Stopping;
                        _cancellationTokenSource.Cancel();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        catch (ThreadInterruptedException)
        {
            State = CycleState.Aborted;
        }
    }

    private void Running()
    {
        foreach (CyclicComponentBase component in Components)
        {
            _timer.Start();
            component.Execute();
            _timer.Stop();
        }
    }

    /// <inheritdoc/>
    public CycleState State { get; private set; }

    /// <inheritdoc/>
    public TimeSpan WatchDogTime { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<CyclicComponentBase> Components => ComponentService.GetInstancesOfType<CyclicComponentBase>();

    /// <inheritdoc/>
    public IComponentService ComponentService { get; private set; }

    /// <inheritdoc/>
    public void Start()
    {
        State = CycleState.Running;
        _thread.Start();
    }

    /// <inheritdoc/>
    public void Stop() => _cancellationTokenSource.Cancel();

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicExecutorService"/> class.
    /// </summary>
    /// <param name="componentService"> The component service. </param>
    public CyclicExecutorService(IComponentService componentService)
    {
        ComponentService = componentService;
        WatchDogTime = TimeSpan.FromSeconds(3600);
        _timer = new(WatchDogTime);
        _timer.Elapsed += CallWatchDog;
        State = CycleState.Stopped;
        _thread = new(Execute);
    }

    private void CallWatchDog(object? sender, ElapsedEventArgs e)
    {
        LogService.LogApp(LogSeverity.Critical, "Watch dog has been called.");
        _thread.Interrupt();
        _thread = new(Execute);
    }
}
