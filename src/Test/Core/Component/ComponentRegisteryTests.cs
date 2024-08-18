// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.TypeRegistery;
using Moq;

namespace LionkTest.Core;

/// <summary>
/// Test class for <see cref="ComponentRegistery"/>.
/// </summary>
[TestFixture]
public class ComponentRegisteryTests
{
    private Mock<IComponentService> _mockComponentService;
    private Mock<ITypesProvider> _mockTypesProvider;
    private ComponentRegistery _componentRegistery;

    /// <summary>
    /// Method used to set up before each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockComponentService = new Mock<IComponentService>();
        _mockTypesProvider = new Mock<ITypesProvider>();
        _componentRegistery = new ComponentRegistery(_mockTypesProvider.Object, _mockComponentService.Object);
    }

    /// <summary>
    /// Method used to clean up after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
        => _componentRegistery.Dispose();

    /// <summary>
    /// Test for <see cref="ITypesProvider.NewTypesAvailable"/>.
    /// </summary>
    [Test]
    public void OnNewTypesAvailable_ShouldRegisterNewTypes_WhenNewTypesAreAvailable()
    {
        var types = new List<Type> { typeof(MockComponent) };
        _mockTypesProvider.Setup(tp => tp.GetTypes()).Returns(types);

        var eventArgs = new TypesEventArgs(types);
        _mockTypesProvider.Raise(tp => tp.NewTypesAvailable += null, eventArgs);

        System.Collections.ObjectModel
            .ReadOnlyDictionary<ComponentTypeDescription, Factory>
            registeredTypes = _componentRegistery.TypesRegistery;

        Assert.That(registeredTypes, Has.Count.EqualTo(1));
        Assert.That(registeredTypes.Keys.Any(td => td.Type == typeof(MockComponent)), Is.True);
    }

    /// <summary>
    /// Test for <see cref="ComponentRegistery.AddProvider(ITypesProvider)"/>.
    /// </summary>
    [Test]
    public void AddProvider_ShouldAddNewProvider_WhenCalled()
    {
        var newProvider = new Mock<ITypesProvider>();
        _componentRegistery.AddProvider(newProvider.Object);
        newProvider.VerifyAdd(
            p => p.NewTypesAvailable
                += It.IsAny<EventHandler<TypesEventArgs>>(),
            Times.Once);
    }

    /// <summary>
    /// Test for <see cref="ComponentRegistery.DeleteProvider(ITypesProvider)"/>.
    /// </summary>
    [Test]
    public void DeleteProvider_ShouldRemoveProvider_WhenCalled()
    {
        var newProvider = new Mock<ITypesProvider>();
        _componentRegistery.AddProvider(newProvider.Object);

        _componentRegistery.DeleteProvider(newProvider.Object);

        newProvider.VerifyRemove(
            p => p.NewTypesAvailable
                -= It.IsAny<EventHandler<TypesEventArgs>>(),
            Times.Once);
    }

    /// <summary>
    /// Test for <see cref="ComponentRegistery.Dispose"/>.
    /// </summary>
    [Test]
    public void Dispose_ShouldUnsubscribeFromAllProviders_WhenCalled()
    {
        var newProvider = new Mock<ITypesProvider>();
        _componentRegistery.AddProvider(newProvider.Object);

        _componentRegistery.Dispose();

        newProvider.VerifyRemove(
            p => p.NewTypesAvailable
                -= It.IsAny<EventHandler<TypesEventArgs>>(),
            Times.Once);
    }

    private class MockComponent : IComponent
    {
        public string InstanceName { get; set; } = string.Empty;
    }
}
