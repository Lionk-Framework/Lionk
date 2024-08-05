// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;
using Lionk.Core.TypeRegistery;

namespace Lionk.Core;

/// <summary>
/// .
/// </summary>
public class ComponentRegistery
{
    private readonly Dictionary<IComponent, Factory> _typesRegistery;

    /// <summary>
    /// Gets r.
    /// </summary>
    public ReadOnlyDictionary<IComponent, Factory> TypesRegistery
        => _typesRegistery.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegistery"/> class.
    /// </summary>
    public ComponentRegistery()
        => _typesRegistery = [];
}
