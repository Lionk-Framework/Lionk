// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface defines a cyclic component.
/// </summary>
public interface ICyclicComponent : IExecutableComponent
{
    /// <summary>
    /// Gets the execution frequency of the component.
    /// </summary>
    TimeSpan ExecutionFrequency { get; }

    /// <summary>
    /// Gets the last execution time of the component.
    /// </summary>
    DateTime LastExecution { get; }

    DateTime NextExecution => StartedDate.AddMilliseconds(ExecutionFrequency.TotalMilliseconds * NbCycle);

    /// <summary>
    /// Gets or Sets the number of cycles executed.
    /// </summary>
    int NbCycle { get; set; }

    /// <summary>
    /// Gets or sets the starting date of the component.
    /// </summary>
    DateTime StartedDate { get; set; }
}
