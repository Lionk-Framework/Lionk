// Copyright © 2024 Lionk Project

using System.Reflection;

namespace Lionk.Plugin;

/// <summary>
///     Class which represents a dependency.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="Dependency" /> class.
/// </remarks>
/// <param name="isLoaded">True if loaded false otherwise.</param>
/// <param name="assembly">The assembly of the dep.</param>
public class Dependency(bool isLoaded, AssemblyName assembly)
{
    #region properties

    /// <summary>
    ///     Gets the assembly of the dependency.
    /// </summary>
    public AssemblyName AssemblyName { get; } = assembly;

    /// <summary>
    ///     Gets or sets a value indicating whether if the dependency is correctly loaded.
    /// </summary>
    public bool IsLoaded { get; set; } = isLoaded;

    #endregion
}
