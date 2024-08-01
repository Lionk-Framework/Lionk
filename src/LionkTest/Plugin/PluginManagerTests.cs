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
        // Arrange
        string currentDir = Directory.GetCurrentDirectory();
        string path = Path.Combine(currentDir, "Resources", "Plugins");
        var pluginPaths = new List<string>
        {
            Path.Combine(path, "plugin1.dll"),
            Path.Combine(path, "plugin2.dll"),
        };

        string json = JsonConvert.SerializeObject(pluginPaths, Formatting.Indented);

        File.WriteAllText("plugin_paths.json", json);

        // Act
        var pluginManager = new PluginManager();

        // Assert
        Assert.That(pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.GetAllPlugins"/>.
    /// </summary>
    [Test]
    public void AddPlugin_WithValidPath_ShouldAddPlugin()
    {
        // Arrange
        string path = Assembly.GetExecutingAssembly().Location;

        // Act
        _pluginManager.AddPlugin(path);

        // Assert
        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(1));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.AddPlugin(string)"/>.
    /// </summary>
    [Test]
    public void AddPlugin_WithInvalidPath_ShouldNotAddPlugin()
    {
        // Arrange
        string invalidPath = "invalid/path/to/plugin.dll";

        // Act
        _pluginManager.AddPlugin(invalidPath);

        // Assert
        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.RemovePlugin(Plugin)"/>.
    /// </summary>
    [Test]
    public void RemovePlugin_ShouldRemovePlugin()
    {
        // Arrange
        string path = Assembly.GetExecutingAssembly().Location;
        _pluginManager.AddPlugin(path);
        Plugin plugin = _pluginManager.GetAllPlugins().First();

        // Act
        _pluginManager.RemovePlugin(plugin);

        // Assert
        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(0));
    }

    /// <summary>
    /// Test for <see cref="PluginManager.GetTypesFromPlugins"/>.
    /// </summary>
    [Test]
    public void GetTypesFromPlugins_ShouldReturnAllTypes()
    {
        // Arrange
        string path = Assembly.GetExecutingAssembly().Location;
        _pluginManager.AddPlugin(path);

        // Act
        IEnumerable<Type> types = _pluginManager.GetTypesFromPlugins();

        // Assert
        Assert.That(types.Any(), Is.True);
    }

    /// <summary>
    /// Test for bad format configuration file.
    /// </summary>
    [Test]
    public void LoadPluginPaths_WithInvalidFile_ShouldLogError()
    {
        // Arrange
        File.WriteAllText("plugin_paths.json", "invalid json");

        // Act & Assert
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
        // Arrange
        string currentDir = Directory.GetCurrentDirectory();
        string path = Path.Combine(currentDir, "Resources", "Plugins");
        var pluginPaths = new List<string>
        {
            Path.Combine(path, "plugin1.dll"),
            Path.Combine(path, "plugin2.dll"),
        };

        // Act
        _pluginManager = new PluginManager();
        _pluginManager.AddPlugin(pluginPaths[0]);
        _pluginManager.AddPlugin(pluginPaths[1]);

        // Assert
        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));

        _pluginManager = new PluginManager();

        Assert.That(_pluginManager.GetAllPlugins().Count(), Is.EqualTo(2));
    }
}
