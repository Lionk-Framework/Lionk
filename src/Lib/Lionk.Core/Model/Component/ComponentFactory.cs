// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegistery;

namespace Lionk.Core.Component;

/// <summary>
/// Factory for components.
/// </summary>
public class ComponentFactory(Type type) : Factory(type)
{
    private readonly IComponentService? _componentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentFactory"/> class.
    /// </summary>
    /// <param name="type"> The type to instanciate.</param>
    /// <param name="service">The service used to register instance.</param>
    public ComponentFactory(Type type, IComponentService service)
        : this(type) => _componentService = service;

    /// <inheritdoc/>
    protected override void OnCreateInstance(object instance)
        => _componentService?.RegisterComponentInstance(instance);
}
