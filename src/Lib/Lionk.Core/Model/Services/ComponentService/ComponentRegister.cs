// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;
using Lionk.Core.TypeRegistery;

namespace Lionk.Core.Component;

/// <summary>
/// Class that stores elements implementing <see cref="IComponent"/>
/// Elements can be extended using the <see cref="ITypesProvider"/> to provide new types.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ComponentRegister"/> class.
/// </remarks>
/// <param name="service">The component service.</param>
public class ComponentRegister(IComponentService service) : IDisposable
{
    /// <summary>
    /// Gets a dictionnary containing all registered Type and theirs factory.
    /// </summary>
    public ReadOnlyDictionary<ComponentTypeDescription, Factory> TypesRegistery
        => _typesRegister.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegister"/> class.
    /// </summary>
    /// <param name="providers">A list of component providers.</param>
    /// <param name="service">The component service.</param>
    public ComponentRegister(IEnumerable<ITypesProvider> providers, IComponentService service)
        : this(service)
    {
        _providers = providers.ToList();
        _providers.ForEach(p => p.NewTypesAvailable += OnNewTypesAvailable);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentRegister"/> class.
    /// </summary>
    /// <param name="provider">A <see cref="ITypesProvider"/> to poll for new types.</param>
    /// <param name="service">The component service.</param>
    public ComponentRegister(ITypesProvider provider, IComponentService service)
        : this(new List<ITypesProvider> { provider }, service)
        => AddTypesToRegistery(provider.GetTypes());

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
        => AddTypesToRegistery(e.Types);

    private void AddTypesToRegistery(IEnumerable<Type> types)
    {
        bool newTypeAvailabe = false;

        foreach (Type type in types)
        {
            if (type.GetInterfaces().Contains(typeof(IComponent)) &&
                !_registeredTypes.Contains(type))
            {
                var factory = new ComponentFactory(type, _componentService);
                var description = new ComponentTypeDescription(type);

                _typesRegister.Add(description, factory);
                _registeredTypes.Add(type);
                newTypeAvailabe = true;
            }
        }

        if (newTypeAvailabe)
            NewComponentAvailable?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Event raised when a new type is available.
    /// </summary>
    public event EventHandler<EventArgs>? NewComponentAvailable;

    private readonly IComponentService _componentService = service;
    private readonly Dictionary<ComponentTypeDescription, Factory> _typesRegister = [];
    private readonly HashSet<Type> _registeredTypes = [];
    private readonly List<ITypesProvider> _providers = [];
}
