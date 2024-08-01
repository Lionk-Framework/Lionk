// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Plugin;

namespace LionkTest;

/// <summary>
/// Test class for <see cref="Plugin"/>.
/// </summary>
[TestFixture]
public class PluginTests
{
    /// <summary>
    /// Test method for <see cref="Plugin(Assembly)"/>.
    /// </summary>
    [Test]
    public void Constructor_WithValidAssembly_ShouldInitializeProperties()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        Version version = assembly.GetName().Version!;

        // Act
        var plugin = new Plugin(assembly);

        // Assert
        Assert.That(plugin.Assembly, Is.EqualTo(assembly));
        Assert.That(plugin.Name, Is.EqualTo("LionkTest"));
        Assert.That(plugin.Version, Is.EqualTo(version.ToString()));
        Assert.That(plugin.Description, Is.EqualTo("No description"));
        Assert.That(plugin.Author, Is.EqualTo("Lionk Project"));
        Assert.IsNotNull(plugin.Dependencies);
    }
}
