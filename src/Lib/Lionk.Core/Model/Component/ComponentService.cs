﻿// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using Lionk.Core.TypeRegistery;

namespace Lionk.Core.Component;

/// <summary>
/// Service that manages components.
/// </summary>
public class ComponentService : IComponentService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentService"/> class.
    /// </summary>
    /// <param name="provider">The type provider.</param>
    public ComponentService(ITypesProvider provider)
        => _componentRegistery = new ComponentRegistery(provider, this);

    /// <inheritdoc/>
    public void RegisterComponentInstance(IComponent component)
    {
        string baseName = component.InstanceName ?? DefaultComponentName;
        string uniqueName = GenerateUniqueName(baseName);
        component.InstanceName = uniqueName;
        _componentInstances.TryAdd(uniqueName, component);
    }

    /// <inheritdoc/>
    public void UnregisterComponentInstance(IComponent component)
        => _componentInstances.TryRemove(component.InstanceName ?? DefaultComponentName, out _);

    /// <inheritdoc/>
    public IEnumerable<T> GetInstancesOfType<T>()
        => _componentInstances.Values.OfType<T>();

    /// <inheritdoc/>
    public IEnumerable<IComponent> GetInstances()
        => _componentInstances.Values;

    /// <inheritdoc/>
    public IDictionary<ComponentTypeDescription, Factory> GetRegisteredTypeDictionnary()
        => _componentRegistery.TypesRegistery;

    /// <inheritdoc/>
    public IComponent? GetInstanceByName(string name)
    {
        _componentInstances.TryGetValue(name, out IComponent? component);
        return component;
    }

    /// <summary>
    /// Generates a unique name for the component by adding suffixes if necessary.
    /// </summary>
    /// <param name="baseName">The base name of the component.</param>
    /// <returns>A unique name for the component.</returns>
    private string GenerateUniqueName(string baseName)
    {
        string uniqueName = baseName;
        int suffix = 0;

        while (_componentInstances.ContainsKey(uniqueName))
        {
            suffix++;
            uniqueName = $"{baseName}_{suffix}";
        }

        return uniqueName;
    }

    private const string DefaultComponentName = "Unamed";

    private readonly ConcurrentDictionary<string, IComponent>
        _componentInstances = new();

    private readonly ComponentRegistery _componentRegistery;
}