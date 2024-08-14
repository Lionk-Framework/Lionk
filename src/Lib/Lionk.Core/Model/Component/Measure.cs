// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This class represents a measure.
/// </summary>
/// <typeparam name="T"> The type of the value. </typeparam>
public class Measure<T>
{
    private T? _value;

    /// <summary>
    /// Gets the name of the measure.
    /// </summary>
    public string MeasureName { get; private set; }

    /// <summary>
    /// Gets the time of the measure.
    /// </summary>
    public DateTime? Time { get; private set; }

    /// <summary>
    /// Gets the unit of the value.
    /// </summary>
    public string Unit { get; private set; }

    /// <summary>
    /// Gets or sets the value of the measure.
    /// </summary>
    public T? Value
    {
        get => _value;
        set
        {
            Time = DateTime.UtcNow;
            _value = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Measure{T}"/> class.
    /// </summary>
    /// <param name="measureName"> The name of the measure. </param>
    /// <param name="unit"> The unit of the value. </param>
    public Measure(string measureName, string unit)
    {
        MeasureName = measureName;
        Unit = unit;
    }
}
