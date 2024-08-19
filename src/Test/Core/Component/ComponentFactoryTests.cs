// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Moq;

namespace LionkTest.Core;

/// <summary>
/// Test class for <see cref="ComponentFactory"/>.
/// </summary>
[TestFixture]
public class ComponentFactoryTests
{
    private Mock<IComponentService> _mockComponentService;
    private ComponentFactory _componentFactory;

    /// <summary>
    /// Setup method.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockComponentService = new Mock<IComponentService>();
        _componentFactory = new ComponentFactory(typeof(MockComponent), _mockComponentService.Object);
    }

    /// <summary>
    /// Test for <see cref="Lionk.Core.TypeRegister.Factory.CreateInstance"/>.
    /// </summary>
    [Test]
    public void CreateInstance_ShouldRegisterComponent_WhenInstanceIsCreated()
    {
        object? instance = _componentFactory.CreateInstance();

        _mockComponentService.Verify(
            cs =>
                cs.RegisterComponentInstance(It.IsAny<IComponent>()),
            Times.Once);
    }

    /// <summary>
    /// Test for <see cref="Lionk.Core.TypeRegister.Factory.CreateInstance"/>.
    /// </summary>
    [Test]
    public void CreateInstance_ShouldLogError_WhenInstanceIsNotComponent()
    {
        var invalidFactory = new ComponentFactory(typeof(string), _mockComponentService.Object);

        object? instance = invalidFactory.CreateInstance();

        _mockComponentService.Verify(
            cs =>
                cs.RegisterComponentInstance(It.IsAny<IComponent>()),
            Times.Never);
    }

    private class MockComponent : IComponent
    {
        public string InstanceName { get; set; } = string.Empty;

        public Guid UniqueID { get; } = Guid.NewGuid();
    }
}
