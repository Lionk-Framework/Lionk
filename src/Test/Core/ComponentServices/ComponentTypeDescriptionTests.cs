// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace LionkTest.Core;

/// <summary>
///     Test class for <see cref="ComponentTypeDescription" />.
/// </summary>
[TestFixture]
public class ComponentTypeDescriptionTests
{
    #region public and override methods

    /// <summary>
    ///     Test for <see cref="ComponentTypeDescription" /> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldInitializeProperties_WhenAttributesArePresent()
    {
        var description = new ComponentTypeDescription(typeof(MockComponentWithAttributes));

        Assert.That(description.Name, Is.EqualTo("TestComponent"));
        Assert.That(description.Description, Is.EqualTo("Test description."));
    }

    /// <summary>
    ///     Test for <see cref="ComponentTypeDescription" /> constructor.
    /// </summary>
    [Test]
    public void Constructor_ShouldUseDefaultValues_WhenAttributesAreNotPresent()
    {
        var description = new ComponentTypeDescription(typeof(MockComponentWithoutAttributes));

        Assert.That(description.Name, Is.EqualTo("Unamed"));
        Assert.That(description.Description, Is.EqualTo("No description available"));
    }

    #endregion

    /// <summary>
    ///     Mock component with attributes.
    /// </summary>
    [NamedElement("TestComponent", "Test description.")]
    private class MockComponentWithAttributes : IComponent
    {
        #region properties

        public Guid Id { get; } = Guid.NewGuid();

        public string InstanceName { get; set; } = string.Empty;

        #endregion

        #region public and override methods

        public void Dispose()
        {
        }

        #endregion
    }

    private class MockComponentWithoutAttributes : IComponent
    {
        #region properties

        public Guid Id { get; } = Guid.NewGuid();

        public string InstanceName { get; set; } = string.Empty;

        #endregion

        #region public and override methods

        public void Dispose()
        {
        }

        #endregion
    }
}
