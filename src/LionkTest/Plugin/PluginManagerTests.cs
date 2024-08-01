// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Plugin;

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
        var pluginPaths = new List<string> { "path/to/plugin1.dll", "path/to/plugin2.dll" };
        File.WriteAllText("plugin_paths.json", $"{{\"pluginPaths\": [\"{pluginPaths[0]}\", \"{pluginPaths[1]}\"]}}");

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
        Assert.Throws<InvalidOperationException>(
            () =>
            _pluginManager = new PluginManager());
    }
}
