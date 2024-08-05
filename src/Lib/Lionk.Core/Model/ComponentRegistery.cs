// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;

namespace Lionk.Core.TypeRegistery;

/// <summary>
/// .
/// </summary>
public class ComponentRegistery
{
    private readonly Dictionary<Type, Factory> _typesRegistery;

    /// <summary>
    /// Gets r.
    /// </summary>
    public ReadOnlyDictionary<Type, Factory> TypesRegistery
        => _typesRegistery.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegistery"/> class.
    /// </summary>
    public ComponentRegistery()
        => _typesRegistery = [];

    private static readonly Type _type = typeof(Factory);
}
