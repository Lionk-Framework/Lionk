// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This abstract class defines a cyclic component.
/// </summary>
public abstract class CyclicComponentBase : ICyclicComponent
{
    private DateTime? _start;

    /// <inheritdoc/>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Gets the cycle time of the component.
    /// </summary>
    public TimeSpan CycleTime { get; private set; }

    /// <summary>
    /// Gets or sets the execution duration of the component.
    /// </summary>
    public TimeSpan ExecutionDuration { get; protected set; }

    /// <summary>
    /// Gets or sets the last execution time of the component.
    /// </summary>
    public DateTime LastExecution { get; protected set; }

    /// <summary>
    /// Gets or sets the next execution time of the component.
    /// </summary>
    public DateTime? NextExecution { get; protected set; }

    /// <summary>
    /// Gets the starting date of the component.
    /// </summary>
    public DateTime StartingDate { get; private set; }

    /// <summary>
    /// Gets or sets the number of cycles executed.
    /// </summary>
    public long NbCycle { get; protected set; }

    /// <summary>
    /// Method called after the execution of the action.
    /// </summary>
    protected void PostExecution()
    {
        ArgumentNullException.ThrowIfNull(_start);
        ExecutionDuration = DateTime.UtcNow - (DateTime)_start;
        NbCycle++;
        NextExecution = StartingDate.AddMilliseconds(CycleTime.TotalMilliseconds * NbCycle);
    }

    /// <summary>
    /// Method called before the execution of the action.
    /// </summary>
    protected void PreExecution()
    {
        LastExecution = DateTime.UtcNow;
        _start = DateTime.UtcNow;
    }

    /// <summary>
    /// Executes the action of the component.
    /// </summary>
    /// <returns> The execution duration of the action. </returns>
    public abstract TimeSpan? Execute();

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentBase"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    protected CyclicComponentBase(string componentName, TimeSpan cycleTime)
    {
        LastExecution = DateTime.MinValue;
        StartingDate = DateTime.UtcNow;
        InstanceName = componentName;
        CycleTime = cycleTime;
    }
}
