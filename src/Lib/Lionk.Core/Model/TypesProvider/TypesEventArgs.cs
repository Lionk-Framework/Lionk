// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Provides data for the TypeAvailable event.
/// </summary>
public class TypesEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypesEventArgs"/> class.
    /// </summary>
    /// <param name="type">The new types that is available.</param>
    public TypesEventArgs(IEnumerable<Type> type)
        => Types = type ?? throw new ArgumentNullException(nameof(type));

    /// <summary>
    /// Gets the new types that is available.
    /// </summary>
    public IEnumerable<Type> Types { get; }
}
