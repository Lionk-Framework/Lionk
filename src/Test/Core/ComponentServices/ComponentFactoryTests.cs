// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Moq;

namespace LionkTest.Core;

/// <summary>
///     Test class for <see cref="ComponentFactory" />.
/// </summary>
[TestFixture]
public class ComponentFactoryTests
{
    #region fields

    private ComponentFactory _componentFactory;

    private Mock<IComponentService> _mockComponentService;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Test for <see cref="Lionk.Core.TypeRegister.Factory.CreateInstance" />.
    /// </summary>
    [Test]
    public void CreateInstance_ShouldLogError_WhenInstanceIsNotComponent()
    {
        var invalidFactory = new ComponentFactory(typeof(string), _mockComponentService.Object);

        object? instance = invalidFactory.CreateInstance();

        _mockComponentService.Verify(cs => cs.RegisterComponentInstance(It.IsAny<IComponent>()), Times.Never);
    }

    /// <summary>
    ///     Test for <see cref="Lionk.Core.TypeRegister.Factory.CreateInstance" />.
    /// </summary>
    [Test]
    public void CreateInstance_ShouldRegisterComponent_WhenInstanceIsCreated()
    {
        object? instance = _componentFactory.CreateInstance();

        _mockComponentService.Verify(cs => cs.RegisterComponentInstance(It.IsAny<IComponent>()), Times.Once);
    }

    /// <summary>
    ///     Setup method.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockComponentService = new Mock<IComponentService>();
        _componentFactory = new ComponentFactory(typeof(MockComponent), _mockComponentService.Object);
    }

    #endregion

    private class MockComponent : IComponent
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
