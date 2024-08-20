// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Cyclic;

/// <summary>
/// Interface for a service that manages the execution of cyclic components.
/// </summary>
public interface ICyclicExecutorService
{
    /// <summary>
    /// Starts the cyclic execution service.
    /// </summary>
    void Start();

    /// <summary>
    /// Pauses the cyclic execution service, halting the execution of components without stopping the service.
    /// </summary>
    void Pause();

    /// <summary>
    /// Resumes the cyclic execution service from a paused state.
    /// </summary>
    void Resume();

    /// <summary>
    /// Stops the cyclic execution service, including all ongoing component executions.
    /// </summary>
    void Stop();

    /// <summary>
    /// Gets the current state of the service.
    /// </summary>
    CycleState State { get; }

    /// <summary>
    /// Gets or sets the watchdog timeout. If a component's execution exceeds this duration, the service will take appropriate action.
    /// </summary>
    TimeSpan WatchDogTimeout { get; set; }

    /// <summary>
    /// Gets the collection of cyclic components managed by the service.
    /// </summary>
    IEnumerable<ICyclicComponent> Components { get; }
}
