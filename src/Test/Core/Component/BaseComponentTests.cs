// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Moq;

namespace LionkTest.Core;

/// <summary>
/// Test class for the <see cref="BaseComponent"/> class.
/// </summary>
[TestFixture]
public class BaseComponentTests
{
    private Mock<BaseComponent> _baseComponentMock;

    /// <summary>
    /// Setups the test environment.
    /// </summary>
    [SetUp]
    public void Setup()
        => _baseComponentMock = new Mock<BaseComponent> { CallBase = true };

    /// <summary>
    /// Tests the <see cref="BaseComponent.InstanceName"/> property.
    /// </summary>
    [Test]
    public void InstanceName_SetAndGet_ReturnsCorrectValue()
    {
        string expectedName = "TestComponent";

        _baseComponentMock.Object.InstanceName = expectedName;

        Assert.That(_baseComponentMock.Object.InstanceName, Is.EqualTo(expectedName));
    }

    /// <summary>
    /// Tests the <see cref="BaseComponent.Id"/> property.
    /// </summary>
    [Test]
    public void Id_IsGeneratedOnCreation_ReturnsUniqueId()
    {
        Guid id = _baseComponentMock.Object.Id;

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    /// <summary>
    /// Tests the <see cref="BaseComponent.Dispose"/> method.
    /// </summary>
    [Test]
    public void Dispose_WhenCalled_SuppressesFinalize()
    {
        _baseComponentMock.Object.Dispose();

        _baseComponentMock.Verify(m => m.Dispose(), Times.Once);
    }
}
