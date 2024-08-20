// Copyright © 2024 Lionk Project

using Lionk.Core.Component.Cyclic;
using Newtonsoft.Json;

namespace Lionk.Core.Component;

/// <summary>
/// Base class that implements the <see cref="ICyclicComponent"/> interface.
/// Provides default behavior for cyclic components, managing their life cycle,
/// periodic execution, and cycle count.
/// </summary>
public abstract class BaseCyclicComponent : BaseExecutableComponent, ICyclicComponent
{
    #region Observable Properties

    private DateTime _startedDate;

    /// <summary>
    /// Gets the date and time when the component was started.
    /// This property is updated automatically when the component is initialized.
    /// </summary>
    [JsonIgnore]
    public DateTime StartedDate
    {
        get => _startedDate;
        private set => SetField(ref _startedDate, value);
    }

    private TimeSpan _period;

    /// <summary>
    /// Gets or sets the period between each execution of the component.
    /// The period must be greater than zero.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if the period is less than or equal to zero.
    /// </exception>
    public TimeSpan Period
    {
        get => _period;
        set
        {
            if (value <= TimeSpan.Zero)
                value = TimeSpan.FromMilliseconds(10);

            SetField(ref _period, value);
        }
    }

    private DateTime _lastExecution;

    /// <summary>
    /// Gets the date and time of the last execution of the component.
    /// This property is automatically updated each time the component completes its execution.
    /// </summary>
    public DateTime LastExecution
    {
        get => _lastExecution;
        private set => SetField(ref _lastExecution, value);
    }

    private int _nbCycle;

    /// <summary>
    /// Gets the number of execution cycles that have been completed by the component.
    /// This count is incremented automatically with each execution.
    /// </summary>
    [JsonIgnore]
    public int NbCycle
    {
        get => _nbCycle;
        private set => SetField(ref _nbCycle, value);
    }

    private CyclicComputationMethod _computationMethod;

    /// <summary>
    /// Gets or sets the method used to compute the next execution time of the component.
    /// This can be either relative to the last execution or relative to the start time.
    /// </summary>
    public CyclicComputationMethod CyclicComputationMethod
    {
        get => _computationMethod;
        set => SetField(ref _computationMethod, value);
    }

    #endregion

    /// <summary>
    /// Initializes the component by setting the start date to the current date and time.
    /// This method is called automatically during the first execution of the component.
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();
        StartedDate = DateTime.Now;
    }

    /// <summary>
    /// Executes the component, incrementing the cycle count.
    /// This method is called during each execution cycle of the component.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the execution.
    /// </param>
    protected override void OnExecute(CancellationToken cancellationToken)
    {
        NbCycle++;
        base.OnExecute(cancellationToken);
    }

    /// <summary>
    /// Terminates the execution of the component, updating the last execution time.
    /// This method is called automatically after the execution logic has completed.
    /// </summary>
    protected override void OnTerminate()
    {
        base.OnTerminate();
        LastExecution = DateTime.Now;
    }
}
