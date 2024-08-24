// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Plugin;
using Lionk.Utils;
using Newtonsoft.Json;

namespace LionkTest.Plugin;

/// <summary>
///     Test class for <see cref="PluginManager" />.
/// </summary>
[TestFixture]
public class PluginManagerTests
{
    #region fields

    private PluginManager _pluginManager;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Test for <see cref="PluginManager.NewTypesAvailable" />.
    /// </summary>
    [Test]
    public void AddPlugin_ShouldProvideTypesInNewTypesAvailableEvent()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        List<Type>? eventTypes = null;

        _pluginManager.NewTypesAvailable += (sender, args) => eventTypes = args.Types.ToList();

        _pluginManager.AddPlugin(path);

        Assert.That(eventTypes, Is.Not.Null);
        Assert.That(eventTypes.Count, Is.GreaterThan(0));
    }

    /// <summary>
    ///     Test for <see cref="PluginManager.NewTypesAvailable" />.
    /// </summary>
    [Test]
    public void AddPlugin_ShouldTriggerNewTypesAvailableEvent()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        bool eventTriggered = false;

        _pluginManager.NewTypesAvailable += (sender, args) => eventTriggered = true;

        _pluginManager.AddPlugin(path);

        Assert.That(eventTriggered, Is.True);
    }

    /// <summary>
    ///     Test for <see cref="PluginManager.AddPlugin(string)" />.
    /// </summary>
    [Test]
    public void AddPlugin_WithInvalidPath_ShouldNotAddPlugin()
    {
        string invalidPath = "invalid/path/to/plugin.dll";

        _pluginManager.AddPlugin(invalidPath);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(0));
    }

    /// <summary>
    ///     Test for <see cref="PluginManager.GetAllPlugins" />.
    /// </summary>
    [Test]
    public void AddPlugin_WithValidPath_ShouldAddPlugin()
    {
        string path = Assembly.GetExecutingAssembly().Location;

        _pluginManager.AddPlugin(path);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(1));
    }

    /// <summary>
    ///     Test for <see cref="PluginManager.AddPlugin(string)" />.
    /// </summary>
    [Test]
    public void AddPlugins_AddingSamePluginTwice_ShouldNotAddPluginTwice()
    {
        string path = Assembly.GetExecutingAssembly().Location;

        _pluginManager.AddPlugin(path);
        _pluginManager.AddPlugin(path);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(1));
    }

    /// <summary>
    ///     Test for <see cref="PluginManager" /> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldLoadPluginsFromFile()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string path = Path.Combine(currentDir, "Resources", "Plugins");
        var pluginPaths = new List<string> { Path.Combine(path, "plugin1.dll"), Path.Combine(path, "plugin2.dll") };

        string json = JsonConvert.SerializeObject(pluginPaths, Formatting.Indented);

        ConfigurationUtils.SaveFile("plugin_paths.json", json, FolderType.Config);

        var pluginManager = new PluginManager();

        Assert.That(pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));
    }

    /// <summary>
    ///     Set up for tests.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        string configFilename = "plugin_paths.json";
        if (ConfigurationUtils.FileExists(configFilename, FolderType.Config))
        {
            ConfigurationUtils.TryDeleteFile(configFilename, FolderType.Config);
        }

        _pluginManager = new PluginManager();
    }

    #endregion
}
