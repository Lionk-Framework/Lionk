// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegistery;

namespace Lionk.Plugin;

/// <summary>
/// Interface wich define a plugin manager.
/// </summary>
public interface IPluginManager : ITypesProvider
{
    /// <summary>
    /// Adds a plugin to the manager.
    /// </summary>
    /// <param name="path">The path to the plugin.</param>
    public void AddPlugin(string path);

    /// <summary>
    /// Removes the specified plugin from the manager.
    /// </summary>
    /// <param name="plugin">The plugin to remove.</param>
    public void RemovePlugin(Plugin plugin);

    /// <summary>
    /// Gets all loaded plugins.
    /// </summary>
    /// <returns>A collection of loaded plugins.</returns>
    public IEnumerable<Plugin> GetAllPlugins();

    /// <summary>
    /// Gets the number of loaded plugins.
    /// </summary>
    /// <returns>The count of plugins.</returns>
    public int GetPluginCount();
}
