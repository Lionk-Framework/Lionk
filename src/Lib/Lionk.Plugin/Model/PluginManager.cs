// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Core.TypeRegistery;
using Lionk.Log;
using Newtonsoft.Json;

namespace Lionk.Plugin;

/// <summary>
/// Class which allows to manage plugins.
/// </summary>
public class PluginManager : IPluginManager
{
    private const string PluginPathsFile = "plugin_paths.json";
    private readonly List<Plugin> _plugins = [];
    private List<string> _pluginPaths = [];

    /// <inheritdoc/>
    public event EventHandler<TypesEventArgs>? NewTypesAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManager"/> class.
    /// </summary>
    public PluginManager()
        => LoadPlugins();

    /// <inheritdoc/>
    public void AddPlugin(string path)
    {
        // TODO copy path to plugin folder
        if (!File.Exists(path) || Path.GetExtension(path) != ".dll")
        {
            LogService.LogApp(LogSeverity.Warning, $"Invalid plugin path: {path}");
            return;
        }

        if (_pluginPaths.Contains(path))
        {
            LogService.LogApp(LogSeverity.Warning, $"Plugin already loaded: {path}");
            return;
        }

        LoadPlugin(path);
        _pluginPaths.Add(path);
        SavePluginPaths();
    }

    /// <inheritdoc/>
    public void RemovePlugin(Plugin plugin)
    {
        if (plugin is null) return;

        _plugins.Remove(plugin);
        _pluginPaths.Remove(plugin.Assembly.Location);
        SavePluginPaths();
    }

    /// <inheritdoc/>
    public IEnumerable<Type> GetTypes()
    {
        var types = new List<Type>();

        foreach (Plugin plugin in _plugins)
            types.AddRange(plugin.Assembly.GetTypes());

        return types;
    }

    /// <inheritdoc/>
    public IEnumerable<Plugin> GetAllPlugins()
        => _plugins.AsReadOnly();

    private void LoadPlugin(string path)
    {
        try
        {
            var assembly = Assembly.LoadFrom(path);
            var plugin = new Plugin(assembly);
            _plugins.Add(plugin);

            NewTypesAvailable?.Invoke(this, new TypesEventArgs(plugin.Assembly.GetTypes()));

            LogService.LogApp(LogSeverity.Information, $"Plugin loaded: {plugin.Name}");
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to load plugin from path: {path}. Error: {ex.Message}");
        }
    }

    private void SavePluginPaths()
    {
        try
        {
            string json = JsonConvert.SerializeObject(_pluginPaths, Formatting.Indented);
            File.WriteAllText(PluginPathsFile, json);
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to save plugin paths. Error: {ex.Message}");
        }
    }

    private void LoadPluginPaths()
    {
        if (File.Exists(PluginPathsFile))
        {
            string json = File.ReadAllText(PluginPathsFile);

            TryToReadPaths(json, out List<string>? data);

            if (data is null)
                throw new InvalidOperationException("Invalid plugin paths file.");

            _pluginPaths = data;
        }
        else
        {
            LogService.LogApp(LogSeverity.Information, "Plugin paths file not found.");
            _pluginPaths = new List<string>();
        }
    }

    private void TryToReadPaths(string json, out List<string>? readedText)
    {
        try
        {
            readedText =
                JsonConvert.DeserializeObject<List<string>>(json);
        }
        catch (JsonReaderException)
        {
            throw new FormatException("The plugin configuration file is not formated.");
        }
    }

    private void LoadPlugins()
    {
        LoadPluginPaths();
        foreach (string path in _pluginPaths)
            LoadPlugin(path);
    }
}
