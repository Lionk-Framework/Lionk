// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegister;

namespace Lionk.Core.Component;

/// <summary>
///     Interface for component service.
/// </summary>
public interface IComponentService : IDisposable
{
    #region delegate and events

    /// <summary>
    ///     Event raised when a new type is available.
    /// </summary>
    public event EventHandler<EventArgs> NewComponentAvailable;

    /// <summary>
    ///     Event raised when a new instance is registered.
    /// </summary>
    public event EventHandler<EventArgs> NewInstanceRegistered;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Get an instance by its unique identifier.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>A component with id, or null if no component exist.</returns>
    public IComponent? GetInstanceByID(Guid id);

    /// <summary>
    ///     Get an instance by <see cref="IComponent.InstanceName" />.
    /// </summary>
    /// <param name="name">The instance name.</param>
    /// <returns>The <see cref="IComponent" /> define by its name, null if not registered.</returns>
    public IComponent? GetInstanceByName(string name);

    /// <summary>
    ///     Get all instances registered.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}" /> containing all registered instace.</returns>
    public IEnumerable<IComponent> GetInstances();

    /// <summary>
    ///     Get all instances of components of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the components.</typeparam>
    /// <returns>An <see cref="IEnumerable{T}" /> containing all registered instance of T.</returns>
    public IEnumerable<T> GetInstancesOfType<T>();

    /// <summary>
    ///     Get all registered types from <see cref="ITypesProvider" />.
    /// </summary>
    /// <returns>
    ///     A <see cref="IDictionary{TKey, TValue}" />
    ///     which contains information about registered components."/>.
    /// </returns>
    public IReadOnlyDictionary<ComponentTypeDescription, Factory> GetRegisteredTypeDictionnary();

    /// <summary>
    ///     Register a component.
    /// </summary>
    /// <param name="component"> the component to register.</param>
    public void RegisterComponentInstance(IComponent component);

    /// <summary>
    ///     Unregister a component.
    /// </summary>
    /// <param name="component">Unregister the instance.</param>
    public void UnregisterComponentInstance(IComponent component);

    #endregion
}
