// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;
using Lionk.Core.TypeRegister;

namespace Lionk.Core.Component;

/// <summary>
///     Class that stores elements implementing <see cref="IComponent" />
///     Elements can be extended using the <see cref="ITypesProvider" /> to provide new types.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ComponentRegister" /> class.
/// </remarks>
/// <param name="service">The component service.</param>
public class ComponentRegister(IComponentService service) : IDisposable
{
    #region fields

    private readonly List<ITypesProvider> _providers = [];

    private readonly HashSet<Type> _registeredTypes = [];

    private readonly Dictionary<ComponentTypeDescription, Factory> _typesRegister = [];

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentRegister" /> class.
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
    ///     Initializes a new instance of the <see cref="ComponentRegister" /> class.
    /// </summary>
    /// <param name="provider">A <see cref="ITypesProvider" /> to poll for new types.</param>
    /// <param name="service">The component service.</param>
    public ComponentRegister(ITypesProvider provider, IComponentService service)
        : this(new List<ITypesProvider> { provider }, service) =>
        AddTypesToRegister(provider.GetTypes());

    #endregion

    #region delegate and events

    /// <summary>
    ///     Event raised when a new type is available.
    /// </summary>
    public event EventHandler<EventArgs>? NewComponentAvailable;

    #endregion

    #region properties

    /// <summary>
    ///     Gets a dictionary containing all registered Type and theirs factory.
    /// </summary>
    public ReadOnlyDictionary<ComponentTypeDescription, Factory> TypesRegister => _typesRegister.AsReadOnly();

    #endregion

    #region public and override methods

    /// <summary>
    ///     Used to add a provider.
    /// </summary>
    /// <param name="provider">Provider to add.</param>
    public void AddProvider(ITypesProvider provider)
    {
        _providers.Add(provider);
        provider.NewTypesAvailable += OnNewTypesAvailable;
    }

    /// <summary>
    ///     Used to delete a provider.
    /// </summary>
    /// <param name="provider">The provider to delete.</param>
    public void DeleteProvider(ITypesProvider provider)
    {
        _providers.Remove(provider);
        provider.NewTypesAvailable -= OnNewTypesAvailable;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _providers.ForEach(p => p.NewTypesAvailable -= OnNewTypesAvailable);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region others methods

    private void AddTypesToRegister(IEnumerable<Type> types)
    {
        bool newTypeAvailable = false;

        foreach (Type type in types)
        {
            if (type.GetInterfaces().Contains(typeof(IComponent))
                && type is { IsClass: true, IsAbstract: false }
                && !_registeredTypes.Contains(type))
            {
                var factory = new ComponentFactory(type, service);
                var description = new ComponentTypeDescription(type);

                _typesRegister.Add(description, factory);
                _registeredTypes.Add(type);
                newTypeAvailable = true;
            }
        }

        if (newTypeAvailable)
        {
            NewComponentAvailable?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnNewTypesAvailable(object? sender, TypesEventArgs e) => AddTypesToRegister(e.Types);

    #endregion
}
