// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This class defines a cyclic async component.
/// </summary>
public abstract class CyclicComponentAsync : CyclicComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentAsync"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    protected CyclicComponentAsync(string componentName, TimeSpan cycleTime)
        : base(componentName, cycleTime)
    {
    }

    /// <summary>
    /// Gets or sets the asynchronous action to execute.
    /// </summary>
    public Func<Task>? CyclicTask { get; protected set; }

    /// <summary>
    /// Execute a cycle of the component asynchronously.
    /// </summary>
    /// <returns>The difference between the cycle time and the actual time.</returns>
    public async Task<TimeSpan?> ExecuteAsync()
    {
        ArgumentNullException.ThrowIfNull(CyclicTask);

        TimeSpan? diff = DateTime.UtcNow - LastExecution.AddMilliseconds(CycleTime.TotalMilliseconds);

        if (diff >= TimeSpan.Zero)
        {
            PreExecution();
            await CyclicTask();
            PostExecution();
        }

        return diff;
    }

    /// <summary>
    /// Execute a cycle of the component asynchronously.
    /// </summary>
    /// <returns>The difference between the cycle time and the actual time.</returns>
    public override TimeSpan? Execute()
    {
        ExecuteAsync().Wait();
        return ExecutionDuration;
    }
}
