// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This abstract class defines a cyclic component.
/// </summary>
public abstract class CyclicComponentBase : IComponent
{
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
    /// Gets the asynchronous function to execute.
    /// </summary>
    public Func<object?[]?, Task>? AsyncTask { get; private set; }

    /// <summary>
    /// Gets the synchronous action to execute.
    /// </summary>
    public Action<object?[]?>? SyncTask { get; private set; }

    /// <summary>
    /// Gets the arguments of the action.
    /// </summary>
    public object?[]? Args { get; private set; }

    /// <summary>
    /// Gets or sets the number of cycles executed.
    /// </summary>
    public long NbCycle { get; protected set; }

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
            LastExecution = DateTime.UtcNow;
            DateTime start = DateTime.UtcNow;
            await AsyncTask(Args);
            ExecutionDuration = DateTime.UtcNow - start;
            NbCycle++;
            NextExecution = StartingDate.AddMilliseconds(CycleTime.TotalMilliseconds * NbCycle);
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
            LastExecution = DateTime.UtcNow;
            DateTime start = DateTime.UtcNow;
            SyncTask(Args);
            ExecutionDuration = DateTime.UtcNow - start;
            NbCycle++;
            NextExecution = StartingDate.AddMilliseconds(CycleTime.TotalMilliseconds * NbCycle);
        }

        return diff;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentBase"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    /// <param name="asyncTask"> The asynchronous function to execute. </param>
    /// <param name="args"> The arguments of the action. </param>
    protected CyclicComponentBase(string componentName, TimeSpan cycleTime, Func<object?[]?, Task> asyncTask, object?[]? args)
        : this(componentName, cycleTime)
    {
        AsyncTask = asyncTask;
        Args = args;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentBase"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    /// <param name="syncTask"> The synchronous action to execute. </param>
    /// <param name="args"> The arguments of the action. </param>
    protected CyclicComponentBase(string componentName, TimeSpan cycleTime, Action<object?[]?> syncTask, object?[]? args)
        : this(componentName, cycleTime)
    {
        SyncTask = syncTask;
        Args = args;
    }

    private CyclicComponentBase(string componentName, TimeSpan cycleTime)
    {
        LastExecution = DateTime.MinValue;
        StartingDate = DateTime.UtcNow;
        InstanceName = componentName;
        CycleTime = cycleTime;
    }
}
