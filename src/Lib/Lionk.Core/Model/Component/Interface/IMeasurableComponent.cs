// Copyright © 2024 Lionk Project

using Lionk.Core.DataModel;

namespace Lionk.Core.Component;

/// <summary>
///     This interface defines a measurable component.
/// </summary>
/// <typeparam name="T"> The type of the value. </typeparam>
public interface IMeasurableComponent<T> : IComponent
{
    #region delegate and events

    /// <summary>
    ///     Raised when a new value is available.
    /// </summary>
    event EventHandler<MeasureEventArgs<T>>? NewValueAvailable;

    #endregion

    #region properties

    /// <summary>
    ///     Gets the measures of the component.
    /// </summary>
    List<Measure<T>> Measures { get; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     This method processes the measures.
    /// </summary>
    void Measure();

    #endregion
}
