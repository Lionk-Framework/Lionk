// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// Provides data for the MeasureAvailable event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MeasureEventArgs{T}"/> class.
/// </remarks>
/// <typeparam name="T"> The type of the value. </typeparam>
public class MeasureEventArgs<T>(IEnumerable<Measure<T>> measurableComponent) : EventArgs
{
    /// <summary>
    /// Gets the new types that is available.
    /// </summary>
    public IEnumerable<Measure<T>> Measures { get; } = measurableComponent ?? throw new ArgumentNullException(nameof(measurableComponent));
}
