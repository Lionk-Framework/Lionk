// Copyright © 2024 Lionk Project

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
    public void RegisterComponentInstance(object component);

    /// <summary>
    /// Unregister a component.
    /// </summary>
    /// <param name="component">Unregister the instance.</param>
    public void UnregisterComponentInstance(object component);
}
