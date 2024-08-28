// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Cyclic;

/// <summary>
///     Interface for a service that manages the execution of cyclic components.
/// </summary>
public interface ICyclicExecutorService
{
    #region properties

    /// <summary>
    ///     Gets the collection of cyclic components managed by the service.
    /// </summary>
    IEnumerable<ICyclicComponent> Components { get; }

    /// <summary>
    ///     Gets the current state of the service.
    /// </summary>
    CycleState State { get; }

    /// <summary>
    ///     Gets or sets the watchdog timeout. If a component's execution exceeds this duration, the service will take appropriate action.
    /// </summary>
    TimeSpan WatchDogTimeout { get; set; }

    /// <summary>
    ///     Gets or sets the mean cycle time. This is the average time between the start of two consecutive cycles.
    /// </summary>
    TimeSpan MeanCycleTime { get; set; }

    /// <summary>
    ///    Gets or sets the maximum cycle time. This is the maximum time between the start of two consecutive cycles.
    /// </summary>
    TimeSpan MaxCycleTime { get; set; }

    /// <summary>
    /// Gets or sets the last execution time of the service.
    /// </summary>
    TimeSpan LastExecutionTime { get; set; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Pauses the cyclic execution service, halting the execution of components without stopping the service.
    /// </summary>
    void Pause();

    /// <summary>
    ///     Resumes the cyclic execution service from a paused state.
    /// </summary>
    void Resume();

    /// <summary>
    ///     Starts the cyclic execution service.
    /// </summary>
    void Start();

    /// <summary>
    ///     Stops the cyclic execution service, including all ongoing component executions.
    /// </summary>
    void Stop();

    #endregion
}
