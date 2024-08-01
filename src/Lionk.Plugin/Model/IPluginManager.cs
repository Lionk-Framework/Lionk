// Copyright © 2024 Lionk Project

namespace Lionk.Plugin;

/// <summary>
/// Interface wich define a plugin manager.
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// Adds a plugin to the manager.
    /// </summary>
    /// <param name="path">The path to the plugin.</param>
    void AddPlugin(string path);

    /// <summary>
    /// Removes the specified plugin from the manager.
    /// </summary>
    /// <param name="plugin">The plugin to remove.</param>
    void RemovePlugin(Plugin plugin);

    /// <summary>
    /// Gets all types from the loaded plugins.
    /// </summary>
    /// <returns>A collection of types from all plugins.</returns>
    IEnumerable<Type> GetTypesFromPlugins();

    /// <summary>
    /// Gets all loaded plugins.
    /// </summary>
    /// <returns>A collection of loaded plugins.</returns>
    IEnumerable<Plugin> GetAllPlugins();
}
