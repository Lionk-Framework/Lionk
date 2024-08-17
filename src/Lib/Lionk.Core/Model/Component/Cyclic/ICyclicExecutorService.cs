// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface defines a cyclic executor service.
/// </summary>
public interface ICyclicExecutorService
{
    /// <summary>
    /// This method starts the service.
    /// </summary>
    void Start();

    /// <summary>
    /// This method stop the service.
    /// </summary>
    void Stop();

    /// <summary>
    /// Gets the state of the service.
    /// </summary>
    CycleState State { get; }

    /// <summary>
    /// Gets the watch dog of the service.
    /// </summary>
    TimeSpan WatchDogTime { get; }

    /// <summary>
    /// Gets the components of the service.
    /// </summary>
    IEnumerable<ICyclicComponent> Components { get; }

    /// <summary>
    /// Gets the component service.
    /// </summary>
    IComponentService ComponentService { get; }
}
