// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace LionkTest.Core;

/// <summary>
/// Test class for <see cref="ComponentTypeDescription"/>.
/// </summary>
[TestFixture]
public class ComponentTypeDescriptionTests
{
    /// <summary>
    /// Test for <see cref="ComponentTypeDescription"/> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldInitializeProperties_WhenAttributesArePresent()
    {
        var description = new ComponentTypeDescription(typeof(MockComponentWithAttributes));

        Assert.That(description.Name, Is.EqualTo("TestComponent"));
        Assert.That(description.Description, Is.EqualTo("Test description."));
    }

    /// <summary>
    /// Test for <see cref="ComponentTypeDescription"/> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldUseDefaultValues_WhenAttributesAreNotPresent()
    {
        var description = new ComponentTypeDescription(typeof(MockComponentWithoutAttributes));

        Assert.That(description.Name, Is.EqualTo("Unamed"));
        Assert.That(description.Description, Is.EqualTo("No description available"));
    }

    /// <summary>
    /// Mock component with attributes.
    /// </summary>
    [NamedElement("TestComponent", "Test description.")]
    private class MockComponentWithAttributes : IComponent
    {
        public string InstanceName { get; set; } = string.Empty;

        public Guid Id { get; } = Guid.NewGuid();
    }

    private class MockComponentWithoutAttributes : IComponent
    {
        public string InstanceName { get; set; } = string.Empty;

        public Guid Id { get; } = Guid.NewGuid();
    }
}
