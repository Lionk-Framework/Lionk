// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;
using Lionk.Core.DataModel;

namespace Lionk.TestComponent;

/// <summary>
///     Counter test component that also implements IMeasurableComponent to demonstrate measure historization.
/// </summary>
[NamedElement("Counter test", "test cyclic element with measurements")]
public class Counter : BaseCyclicComponent, IMeasurableComponent<double>
{
    #region fields

    private int _counter;
    private TimeSpan _historyDuration = TimeSpan.FromMinutes(10); // Default to 10 minutes of history

    #endregion

    #region properties

    /// <inheritdoc />
    public override bool CanExecute => true;

    /// <summary>
    ///     Gets or sets counter value.
    /// </summary>
    public int CounterValue
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    /// <inheritdoc />
    public TimeSpan HistoryDuration
    {
        get => _historyDuration;
        set => SetField(ref _historyDuration, value);
    }

    #endregion

    #region delegate and events

    /// <inheritdoc />
    public event EventHandler<MeasureEventArgs<double>>? NewValueAvailable;

    public List<Measure<double>> Measures { get; set; } = new();

    #endregion

    #region public and override methods

    /// <inheritdoc />
    /// abort behavior, terminates execution via cancellationToken
    /// and sets the component to error
    public override void Abort() => base.Abort();

    /// <inheritdoc />
    public void Measure()
    {
        // Create new measures with the current counter value and a calculated value
        double currentValue = (double)CounterValue;
        double squaredValue = Math.Pow(currentValue, 2);

        // Create new measures with hierarchical names
        DateTime currentTime = DateTime.Now;
        var counterMeasure = new Measure<double>(
            "CounterValue",
            currentTime,
            "count",
            currentValue);
        var squaredMeasure = new Measure<double>(
            "SquaredValue",
            currentTime,
            "count²",
            squaredValue);

        Measures = [counterMeasure, squaredMeasure];

        // Raise the event with these new measures directly
        OnNewValueAvailable(new[] { counterMeasure, squaredMeasure });
    }

    #endregion

    #region others methods

    /// <inheritdoc />
    protected override void OnExecute(CancellationToken ct)
    {
        // behavior that occurs at each execution, the cancellation token is cancelled on abort
        // it's up to you to manage it the way you want if you need it
        base.OnExecute(ct);
        CounterValue++;

        // Take measurements after updating the counter value
        Measure();
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        // behavior that occurs only once at the first start
        Period = TimeSpan.FromSeconds(1);
        base.OnInitialize();
    }

    /// <summary>
    /// Raises the NewValueAvailable event
    /// </summary>
    /// <param name="measures">The measures to include in the event</param>
    private void OnNewValueAvailable(IEnumerable<Measure<double>> measures)
        => NewValueAvailable?.Invoke(this, new MeasureEventArgs<double>(measures));

    #endregion
}
