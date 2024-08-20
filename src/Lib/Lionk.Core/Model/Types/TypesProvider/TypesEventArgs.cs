// Copyright © 2024 Lionk Project

namespace Lionk.Core.TypeRegister;

/// <summary>
/// Provides data for the TypeAvailable event.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TypesEventArgs"/> class.
/// </remarks>
/// <param name="type">The new types that is available.</param>
public class TypesEventArgs(IEnumerable<Type> type) : EventArgs
{
    /// <summary>
    /// Gets the new types that is available.
    /// </summary>
    public IEnumerable<Type> Types { get; } = type ?? throw new ArgumentNullException(nameof(type));
}
