// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.Core.TypeRegistery;

/// <summary>
/// Provides data for the TypeAvailable event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TypesEventArgs"/> class.
/// </remarks>
/// <param name="type">The new types that is available.</param>
public class MeasureEventArgs(IEnumerable<Measure> measurableComponent) : EventArgs
{
    /// <summary>
    /// Gets the new types that is available.
    /// </summary>
    public IEnumerable<Type> Types { get; } = type ?? throw new ArgumentNullException(nameof(type));
}
