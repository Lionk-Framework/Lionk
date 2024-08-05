// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Plugin;
using Newtonsoft.Json;

namespace LionkTest;

/// <summary>
/// Test class for <see cref="PluginManager"/>.
/// </summary>
[TestFixture]
public class PluginManagerTests
{
    private PluginManager _pluginManager;

    /// <summary>
    /// Set up for tests.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        if (File.Exists("plugin_paths.json"))
        {
            File.Delete("plugin_paths.json");
        }

        _pluginManager = new PluginManager();
    }

    /// <summary>
    /// Test for <see cref="PluginManager"/> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldLoadPluginsFromFile()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string path = Path.Combine(currentDir, "Resources", "Plugins");
        var pluginPaths = new List<string>
        {
            Path.Combine(path, "plugin1.dll"),
            Path.Combine(path, "plugin2.dll"),
        };

        string json = JsonConvert.SerializeObject(pluginPaths, Formatting.Indented);

        File.WriteAllText("plugin_paths.json", json);

        var pluginManager = new PluginManager();

        Assert.That(pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.GetAllPlugins"/>.
    /// </summary>
    [Test]
    public void AddPlugin_WithValidPath_ShouldAddPlugin()
    {
        string path = Assembly.GetExecutingAssembly().Location;

        _pluginManager.AddPlugin(path);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(1));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.AddPlugin(string)"/>.
    /// </summary>
    [Test]
    public void AddPlugin_WithInvalidPath_ShouldNotAddPlugin()
    {
        string invalidPath = "invalid/path/to/plugin.dll";

        _pluginManager.AddPlugin(invalidPath);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.RemovePlugin(Plugin)"/>.
    /// </summary>
    [Test]
    public void RemovePlugin_ShouldRemovePlugin()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        _pluginManager.AddPlugin(path);
        Plugin plugin = _pluginManager.GetAllPlugins().First();

        _pluginManager.RemovePlugin(plugin);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.GetTypes"/>.
    /// </summary>
    [Test]
    public void GetTypesFromPlugins_ShouldReturnAllTypes()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        _pluginManager.AddPlugin(path);

        IEnumerable<Type> types = _pluginManager.GetTypes();

        Assert.That(types.Any(), Is.True);
    }

    /// <summary>
    /// Test for bad format configuration file.
    /// </summary>
    [Test]
    public void LoadPluginPaths_WithInvalidFile_ShouldLogError()
    {
        File.WriteAllText("plugin_paths.json", "invalid json");

        Assert.Throws<FormatException>(
            () =>
            _pluginManager = new PluginManager());
    }

    /// <summary>
    /// Test for <see cref="PluginManager.AddPlugin(string)"/>.
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
    /// Test for <see cref="PluginManager"/> constructor.
    /// </summary>
    [Test]
    public void LoadConfigurationFile_WithCreation_ShouldLoadUsedPlugins()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string path = Path.Combine(currentDir, "Resources", "Plugins");
        var pluginPaths = new List<string>
        {
            Path.Combine(path, "plugin1.dll"),
            Path.Combine(path, "plugin2.dll"),
        };

        _pluginManager = new PluginManager();
        _pluginManager.AddPlugin(pluginPaths[0]);
        _pluginManager.AddPlugin(pluginPaths[1]);

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));

        _pluginManager = new PluginManager();

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.NewTypesAvailable"/>.
    /// </summary>
    [Test]
    public void AddPlugin_ShouldTriggerNewTypesAvailableEvent()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        bool eventTriggered = false;

        _pluginManager.NewTypesAvailable +=
            (sender, args) => eventTriggered = true;

        _pluginManager.AddPlugin(path);

        Assert.That(eventTriggered, Is.True);
    }

    /// <summary>
    /// Test for <see cref="PluginManager.NewTypesAvailable"/>.
    /// </summary>
    [Test]
    public void AddPlugin_ShouldProvideTypesInNewTypesAvailableEvent()
    {
        string path = Assembly.GetExecutingAssembly().Location;
        List<Type>? eventTypes = null;

        _pluginManager.NewTypesAvailable +=
            (sender, args) =>
                eventTypes = args.Types.ToList();

        _pluginManager.AddPlugin(path);

        Assert.That(eventTypes, Is.Not.Null);
        Assert.That(eventTypes.Count, Is.GreaterThan(0));
    }
}
