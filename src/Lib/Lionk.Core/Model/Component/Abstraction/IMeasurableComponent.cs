// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface defines a measurable component.
/// </summary>
/// <typeparam name="T"> The type of the value. </typeparam>
public interface IMeasurableComponent<T> : IComponent
{
    /// <summary>
    /// Raised when a new value is available.
    /// </summary>
    event EventHandler<MeasureEventArgs<T>>? NewValueAvailable;

    /// <summary>
    /// Gets the measures of the component.
    /// </summary>
    List<Measure<T>> Measures { get; }

    /// <summary>
    /// This method processes the measures.
    /// </summary>
    void Measure();
}
