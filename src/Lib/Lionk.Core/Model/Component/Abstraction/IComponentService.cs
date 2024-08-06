﻿// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegistery;

namespace Lionk.Core.Component;

/// <summary>
/// Interface for component service.
/// </summary>
public interface IComponentService
{
    /// <summary>
    /// Register a component.
    /// </summary>
    /// <param name="component"> the component to register.</param>
    public void RegisterComponentInstance(IComponent component);

    /// <summary>
    /// Unregister a component.
    /// </summary>
    /// <param name="component">Unregister the instance.</param>
    public void UnregisterComponentInstance(IComponent component);

    /// <summary>
    /// Get all instances of components of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the components.</typeparam>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all registered instance of T.</returns>
    public IEnumerable<T> GetInstancesOfType<T>();

    /// <summary>
    /// Get all instances registered.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all registered instace.</returns>
    public IEnumerable<IComponent> GetInstances();

    /// <summary>
    /// Get all registered types from <see cref="ITypesProvider"/>.
    /// </summary>
    /// <returns>A <see cref="IDictionary{TKey, TValue}"/>
    /// which contains information about registered components."/>.</returns>
    public IDictionary<ComponentTypeDescription, Factory> GetRegisteredTypeDictionnary();

    /// <summary>
    /// Get an instance by <see cref="IComponent.InstanceName"/>.
    /// </summary>
    /// <param name="name">The instance name.</param>
    /// <returns>The <see cref="IComponent"/> define by its name, null if not registered.</returns>
    public IComponent? GetInstanceByName(string name);
}