// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.TypeRegistery;
using Moq;

namespace LionkTest.Core;

/// <summary>
/// Tests for the <see cref="ComponentService"/> class.
/// </summary>
[TestFixture]
public class ComponentServiceTests
{
    private Mock<ITypesProvider> _mockTypesProvider;
    private IComponentService _componentService;

    /// <summary>
    /// Setup method.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockTypesProvider = new Mock<ITypesProvider>();
        _componentService = new ComponentService(_mockTypesProvider.Object);
    }

    /// <summary>
    /// Tear down method.
    /// </summary>
    [TearDown]
    public void TearDown() =>
        _componentService.Dispose();

    /// <summary>
    /// Test for the <see cref="ComponentService.RegisterComponentInstance"/> method.
    /// </summary>
    [Test]
    public void RegisterComponentInstance_ShouldAssignUniqueName_WhenNameConflictOccurs()
    {
        var component1 = new Mock<IComponent>();
        component1.SetupProperty(c => c.InstanceName, "TestComponent");

        var component2 = new Mock<IComponent>();
        component2.SetupProperty(c => c.InstanceName, "TestComponent");

        _componentService.RegisterComponentInstance(component1.Object);
        _componentService.RegisterComponentInstance(component2.Object);

        Assert.That(component1.Object.InstanceName, Is.EqualTo("TestComponent"));
        Assert.That(component2.Object.InstanceName, Is.EqualTo("TestComponent_1"));
    }

    /// <summary>
    /// Test for the <see cref="ComponentService.GetInstanceByName"/> method.
    /// </summary>
    [Test]
    public void GetInstanceByName_ShouldReturnCorrectComponent_WhenComponentExists()
    {
        var component = new Mock<IComponent>();
        component.SetupProperty(c => c.InstanceName, "TestComponent");

        _componentService.RegisterComponentInstance(component.Object);

        IComponent? result = _componentService.GetInstanceByName("TestComponent");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.InstanceName, Is.EqualTo("TestComponent"));
    }

    /// <summary>
    /// Test for the <see cref="ComponentService.UnregisterComponentInstance(IComponent)"/> method.
    /// </summary>
    [Test]
    public void UnregisterComponentInstance_ShouldRemoveComponent_WhenComponentIsRegistered()
    {
        var component = new Mock<IComponent>();
        component.SetupProperty(c => c.InstanceName, "TestComponent");

        _componentService.RegisterComponentInstance(component.Object);

        _componentService.UnregisterComponentInstance(component.Object);
        IComponent? result = _componentService.GetInstanceByName("TestComponent");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Test for the <see cref="ComponentService.GetInstances"/> method.
    /// </summary>
    [Test]
    public void GetInstances_ShouldReturnAllRegisteredComponents()
    {
        var component1 = new Mock<IComponent>();
        component1.SetupProperty(c => c.InstanceName, "TestComponent1");

        var component2 = new Mock<IComponent>();
        component2.SetupProperty(c => c.InstanceName, "TestComponent2");

        _componentService.RegisterComponentInstance(component1.Object);
        _componentService.RegisterComponentInstance(component2.Object);

        IEnumerable<IComponent> result = _componentService.GetInstances();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.ToList(), Does.Contain(component1.Object));
        Assert.That(result.ToList(), Does.Contain(component2.Object));
    }

    /// <summary>
    /// Test for the <see cref="ComponentService.GetInstancesOfType{T}"/> method.
    /// </summary>
    [Test]
    public void GetInstancesOfType_ShouldReturnAllComponentsOfType_WhenTypeIsSpecified()
    {
        var component1 = new Mock<IComponent>();
        component1.SetupProperty(c => c.InstanceName, "TestComponent1");

        var component2 = new Mock<IComponent>();
        component2.SetupProperty(c => c.InstanceName, "TestComponent2");

        _componentService.RegisterComponentInstance(component1.Object);
        _componentService.RegisterComponentInstance(component2.Object);

        IEnumerable<IComponent> result = _componentService.GetInstancesOfType<IComponent>();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.ToList(), Does.Contain(component1.Object));
        Assert.That(result.ToList(), Does.Contain(component2.Object));
    }

    /// <summary>
    /// Test for the naming of instances.
    /// </summary>
    [Test]
    public void RegisterComponentInstance_ShouldAssignDefaultName_WhenNameIsNull()
    {
        var component = new Mock<IComponent>();
        var component2 = new Mock<IComponent>();

        foreach (Mock<IComponent>? c in new[] { component, component2 })
            c.SetupProperty(c => c.InstanceName, string.Empty);

        _componentService.RegisterComponentInstance(component.Object);
        _componentService.RegisterComponentInstance(component2.Object);

        Assert.That(component.Object.InstanceName, Is.EqualTo("Component"));
        Assert.That(component2.Object.InstanceName, Is.EqualTo("Component_1"));
    }
}
