// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This class defines a cyclic component.
/// </summary>
public abstract class CyclicComponent : CyclicComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponent"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    protected CyclicComponent(string componentName, TimeSpan cycleTime)
        : base(componentName, cycleTime)
    {
    }

    /// <summary>
    /// Gets or sets the synchronous action to execute.
    /// </summary>
    public Action? CyclicTask { get; protected set; }

    /// <summary>
    /// Execute a cycle of the component synchronously.
    /// </summary>
    /// <returns>The difference between the cycle time and the actual time.</returns>
    public override TimeSpan? Execute()
    {
        ArgumentNullException.ThrowIfNull(CyclicTask);

        TimeSpan? diff = DateTime.UtcNow - LastExecution.AddMilliseconds(CycleTime.TotalMilliseconds);

        if (diff >= TimeSpan.Zero)
        {
            PreExecution();
            CyclicTask();
            PostExecution();
        }

        return diff;
    }
}
