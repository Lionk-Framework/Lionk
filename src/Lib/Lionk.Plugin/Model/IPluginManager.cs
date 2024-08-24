// Copyright © 2024 Lionk Project

using Lionk.Core.TypeRegister;

namespace Lionk.Plugin;

/// <summary>
///     Interface which define a plugin manager.
/// </summary>
public interface IPluginManager : ITypesProvider
{
    #region public and override methods

    /// <summary>
    ///     Adds a plugin to the manager.
    /// </summary>
    /// <param name="path">The path to the plugin.</param>
    public void AddPlugin(string path);

    /// <summary>
    ///     Gets a boolean indicating if a restart is needed.
    /// </summary>
    /// <returns>True if the application must be restarted false otherwise.</returns>
    public bool DoNeedARestart();

    /// <summary>
    ///     Gets all loaded plugins.
    /// </summary>
    /// <returns>A collection of loaded plugins.</returns>
    public IEnumerable<Plugin> GetAllPlugins();

    /// <summary>
    ///     Removes the specified plugin from the manager.
    ///     As an assembly can't be removed during execution,
    ///     the plugin will be disabled only after a re-run.
    /// </summary>
    /// <param name="plugin">The plugin to remove.</param>
    public void RemovePlugin(Plugin plugin);

    #endregion
}
