// Copyright © 2024 Lionk Project

namespace Lionk.Core.TypeRegister;

/// <summary>
///     Interface which define a types provider.
/// </summary>
public interface ITypesProvider
{
    #region delegate and events

    /// <summary>
    ///     Event raised when a new type is available.
    /// </summary>
    event EventHandler<TypesEventArgs> NewTypesAvailable;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Gets all types from the provider.
    /// </summary>
    /// <returns>A collection of types.</returns>
    IEnumerable<Type> GetTypes();

    #endregion
}
