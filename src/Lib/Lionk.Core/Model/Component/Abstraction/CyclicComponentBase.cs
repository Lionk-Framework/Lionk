// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This abstract class defines a cyclic component.
/// </summary>
public abstract class CyclicComponentBase : IComponent
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
    /// Gets or sets the asynchronous function to execute.
    /// </summary>
    public Func<Task>? AsyncTask { get; protected set; }

    /// <summary>
    /// Gets or sets the synchronous action to execute.
    /// </summary>
    public Action? SyncTask { get; protected set; }

    /// <summary>
    /// Gets or sets the number of cycles executed.
    /// </summary>
    public long NbCycle { get; protected set; }

    private void PostExecution()
    {
        ArgumentNullException.ThrowIfNull(_start);
        ExecutionDuration = DateTime.UtcNow - (DateTime)_start;
        NbCycle++;
        NextExecution = StartingDate.AddMilliseconds(CycleTime.TotalMilliseconds * NbCycle);
    }

    private void PreExecution()
    {
        LastExecution = DateTime.UtcNow;
        _start = DateTime.UtcNow;
    }

    /// <summary>
    /// Execute a cycle of the component asynchronously.
    /// </summary>
    /// <returns>The difference between the cycle time and the actual time.</returns>
    public async Task<TimeSpan?> ExecuteAsync()
    {
        ArgumentNullException.ThrowIfNull(AsyncTask);

        // Gets the difference between the last execution and the current time.
        TimeSpan? diff = DateTime.UtcNow - LastExecution;

        if (diff >= TimeSpan.Zero)
        {
            PreExecution();
            await AsyncTask();
            PostExecution();
        }

        return diff;
    }

    /// <summary>
    /// Execute a cycle of the component synchronously.
    /// </summary>
    /// <returns>The difference between the cycle time and the actual time.</returns>
    public TimeSpan? Execute()
    {
        ArgumentNullException.ThrowIfNull(SyncTask);

        // Gets the difference between the last execution and the current time.
        TimeSpan? diff = DateTime.UtcNow - LastExecution.AddMilliseconds(CycleTime.TotalMilliseconds);

        if (diff >= TimeSpan.Zero)
        {
            PreExecution();
            SyncTask();
            PostExecution();
        }

        return diff;
    }

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
