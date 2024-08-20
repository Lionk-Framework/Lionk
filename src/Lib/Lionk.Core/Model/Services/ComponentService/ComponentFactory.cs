// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegister;
using Lionk.Log;

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
    {
        if (instance is IComponent component)
        {
            _componentService?.RegisterComponentInstance(component);
        }
        else
        {
            LogService.LogApp(
                LogSeverity.Error,
                $"Error creating instance of type {Type.Name}. " + $"The instance is not a component.");
        }
    }
}
