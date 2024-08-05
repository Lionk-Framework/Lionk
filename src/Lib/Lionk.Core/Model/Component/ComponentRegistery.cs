// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;
using Lionk.Core.Model.Component;
using Lionk.Core.TypeRegistery;

namespace Lionk.Core.Component;

/// <summary>
/// Class that stores elements implementing <see cref="IComponent"/>
/// Elements can be extended using the <see cref="ITypesProvider"/> to provide new types.
/// </summary>
public class ComponentRegistery : IDisposable
{
    /// <summary>
    /// Gets a dictionnary containing all registered Type and theirs factory.
    /// </summary>
    public ReadOnlyDictionary<ComponentTypeDescription, Factory> TypesRegistery
        => _typesRegistery.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegistery"/> class.
    /// </summary>
    public ComponentRegistery()
    {
        _typesRegistery = [];
        _registeredTypes = [];
        _providers = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegistery"/> class.
    /// </summary>
    /// <param name="providers">A list of component providers.</param>
    public ComponentRegistery(IEnumerable<ITypesProvider> providers)
        : this()
    {
        _providers = providers.ToList();
        _providers.ForEach(p => p.NewTypesAvailable += OnNewTypesAvailable);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegistery"/> class.
    /// </summary>
    /// <param name="provider">A <see cref="ITypesProvider"/> to poll for new types.</param>
    public ComponentRegistery(ITypesProvider provider)
        : this(new List<ITypesProvider> { provider })
    {
    }

    /// <summary>
    /// Used to add a provider.
    /// </summary>
    /// <param name="provider">Provider to add.</param>
    public void AddProvider(ITypesProvider provider)
    {
        _providers.Add(provider);
        provider.NewTypesAvailable += OnNewTypesAvailable;
    }

    /// <summary>
    /// Used to delete a provider.
    /// </summary>
    /// <param name="provider">The provider to delete.</param>
    public void DeleteProvider(ITypesProvider provider)
    {
        _providers.Remove(provider);
        provider.NewTypesAvailable -= OnNewTypesAvailable;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _providers.ForEach(p => p.NewTypesAvailable -= OnNewTypesAvailable);
        GC.SuppressFinalize(this);
    }

    private void OnNewTypesAvailable(object? sender, TypesEventArgs e)
    {
        foreach (Type type in e.Types)
        {
            if (type.GetInterfaces().Contains(typeof(IComponent)) &&
                !_registeredTypes.Contains(type))
            {
                var factory = new Factory(type);
                var description = new ComponentTypeDescription(type);

                _typesRegistery.Add(description, factory);
                _registeredTypes.Add(type);
            }
        }
    }

    private readonly Dictionary<ComponentTypeDescription, Factory> _typesRegistery;
    private readonly HashSet<Type> _registeredTypes;
    private readonly List<ITypesProvider> _providers;
}
