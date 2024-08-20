// Copyright © 2024 Lionk Project

namespace Lionk.Core.TypeRegister;

/// <summary>
/// Interface wich define a types provider.
/// </summary>
public interface ITypesProvider
{
    /// <summary>
    /// Gets all types from the provider.
    /// </summary>
    /// <returns>A collection of types.</returns>
    IEnumerable<Type> GetTypes();

    /// <summary>
    /// Event raised when a new type is available.
    /// </summary>
    event EventHandler<TypesEventArgs> NewTypesAvailable;
}
