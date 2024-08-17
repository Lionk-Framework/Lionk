// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Core.TypeRegistery;
using Lionk.Log;
using Lionk.Utils;
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
        if (!File.Exists(path) || Path.GetExtension(path) != ".dll")
        {
            LogService.LogApp(LogSeverity.Warning, $"Invalid plugin path: {path}");
            return;
        }

        var assemblyName = AssemblyName.GetAssemblyName(path);

        if (_plugins.Any(x => x.Name == assemblyName.Name))
        {
            LogService.LogApp(LogSeverity.Warning, $"Plugin already loaded: {path}");
            return;
        }

        path = CopyPluginToPluginFolder(path);

        LoadPlugin(path);
        _pluginPaths.Add(path);
        SavePluginPaths();
    }

    /// <inheritdoc/>
    public void RemovePlugin(Plugin plugin)
    {
        if (plugin is null) return;

        _plugins.Remove(plugin);
        _doNeedARestart = true;
        string tempPath = plugin.Assembly.Location;
        string filename = Path.GetFileName(tempPath);
        string pluginPath = Path.Combine(ConfigurationUtils.GetFolderPath(FolderType.Plugin), filename);
        _pluginPaths.Remove(pluginPath);
        SavePluginPaths();
    }

    /// <inheritdoc/>
    public IEnumerable<Type> GetTypes()
    {
        var types = new List<Type>();

        foreach (Plugin plugin in _plugins)
        {
            if (!plugin.IsLoaded) continue;
            types.AddRange(plugin.Assembly.GetTypes());
        }

        return types;
    }

    /// <inheritdoc/>
    public IEnumerable<Plugin> GetAllPlugins()
        => _plugins.AsReadOnly();

    /// <inheritdoc/>
    public bool DoNeedARestart()
        => _doNeedARestart;

    /// <inheritdoc/>
    public int GetPluginCount()
        => _plugins.Count;

    private void LoadPlugin(string path)
    {
        Plugin? plugin = null;

        try
        {
            path = CopyPluginToTempStorage(path);
            var assembly = Assembly.LoadFrom(path);
            plugin = new Plugin(assembly);

            List<Dependency> referencedAssemblies = plugin.Dependencies;
            TryLoadDependencies(referencedAssemblies, plugin, path);

            _plugins.Add(plugin);
            Type[] types = assembly.GetTypes();
            NewTypesAvailable?.Invoke(this, new TypesEventArgs(types));

            plugin.IsLoaded = true;

            LogService.LogApp(LogSeverity.Information, $"Plugin loaded: {plugin.Name}");
        }
        catch (ReflectionTypeLoadException ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to load plugin from path: {path}. Error: {ex.Message}");

            if (plugin is not null)
                plugin.IsLoaded = false;
        }
    }

    private bool TryLoadDependencies(List<Dependency> referencedAssemblies, Plugin plugin, string path)
    {
        foreach (Dependency referencedAssembly in referencedAssemblies)
        {
            try
            {
                Assembly.Load(referencedAssembly.AssemblyName);
                referencedAssembly.IsLoaded = true;
            }
            catch (Exception depEx)
            {
                LogService.LogApp(
                    LogSeverity.Error,
                    $"Failed to load dependency '{referencedAssembly.AssemblyName.FullName}' for plugin '{plugin.Name}' from path: {path}. Error: {depEx.Message}");
                return false;
            }
        }

        return true;
    }

    private void SavePluginPaths()
    {
        try
        {
            string json = JsonConvert.SerializeObject(_pluginPaths, Formatting.Indented);
            ConfigurationUtils.SaveFile(PluginPathsFile, json, FolderType.Config);
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to save plugin paths. Error: {ex.Message}");
        }
    }

    private void LoadPluginPaths()
    {
        if (ConfigurationUtils.FileExists(PluginPathsFile, FolderType.Config))
        {
            string json = ConfigurationUtils.ReadFile(PluginPathsFile, FolderType.Config);

            TryToReadPaths(json, out List<string>? data);

            if (data is null)
                throw new InvalidOperationException("Invalid plugin paths file.");

            _pluginPaths = data;
        }
        else
        {
            LogService.LogApp(LogSeverity.Information, "Plugin paths file not found.");
            _pluginPaths = [];
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
        CleanTempRepo();
        LoadPluginPaths();
        foreach (string path in _pluginPaths)
            LoadPlugin(path);

        DeleteUnloadedPluginFromPluginFolder();
    }

    private void CleanTempRepo()
    {
        foreach (string path in Directory.GetFiles(ConfigurationUtils.GetFolderPath(FolderType.Temp)))
        {
            if (path.EndsWith(".dll"))
                File.Delete(path);
        }
    }

    private void DeleteUnloadedPluginFromPluginFolder()
    {
        foreach (string path in Directory.GetFiles(ConfigurationUtils.GetFolderPath(FolderType.Plugin)))
        {
            if (_pluginPaths.Contains(path)) continue;

            File.Delete(path);
        }
    }

    private string CopyPluginToTempStorage(string pluginPaths)
    {
        ConfigurationUtils.CopyFileToFolder(pluginPaths, FolderType.Temp);
        return Path.Combine(ConfigurationUtils.GetFolderPath(FolderType.Temp), Path.GetFileName(pluginPaths));
    }

    private string CopyPluginToPluginFolder(string pluginPaths)
    {
        ConfigurationUtils.CopyFileToFolder(pluginPaths, FolderType.Plugin);
        return Path.Combine(ConfigurationUtils.GetFolderPath(FolderType.Plugin), Path.GetFileName(pluginPaths));
    }

    private bool _doNeedARestart = false;
}
