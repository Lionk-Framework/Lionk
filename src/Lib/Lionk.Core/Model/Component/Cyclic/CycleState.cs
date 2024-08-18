// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This enum represents the state of a cycle.
/// </summary>
public enum CycleState
{
    /// <summary>
    /// The cycle is stopped.
    /// </summary>
    Stopped,

    /// <summary>
    /// The cycle is paused.
    /// </summary>
    Paused,

    /// <summary>
    /// The cycle is running.
    /// </summary>
    Running,

    /// <summary>
    /// The cycle is stopping.
    /// </summary>
    Stopping,

    /// <summary>
    /// The cycle is aborted.
    /// </summary>
    Aborted,
}
