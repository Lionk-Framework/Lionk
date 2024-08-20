// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.TypeRegister;
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
        TestUtils.DeleteAllConfigFile();
        _componentService = new ComponentService(_mockTypesProvider.Object);
        var component1 = new TestComponent();
        component1.InstanceName = "TestComponent1";

        var component2 = new TestComponent();
        component2.InstanceName = "TestComponent2";

        _componentService.RegisterComponentInstance(component1);
        _componentService.RegisterComponentInstance(component2);

        IEnumerable<IComponent> result = _componentService.GetInstances();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.ToList(), Does.Contain(component1));
        Assert.That(result.ToList(), Does.Contain(component2));
    }

    /// <summary>
    /// Test for the <see cref="ComponentService.GetInstancesOfType{T}"/> method.
    /// </summary>
    [Test]
    public void GetInstancesOfType_ShouldReturnAllComponentsOfType_WhenTypeIsSpecified()
    {
        TestUtils.DeleteAllConfigFile();
        _componentService = new ComponentService(_mockTypesProvider.Object);
        var component1 = new TestComponent();
        component1.InstanceName = "TestComponent1";

        var component2 = new TestComponent();
        component2.InstanceName = "TestComponent2";

        _componentService.RegisterComponentInstance(component1);
        _componentService.RegisterComponentInstance(component2);

        IEnumerable<IComponent> result = _componentService.GetInstancesOfType<IComponent>();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.ToList(), Does.Contain(component1));
        Assert.That(result.ToList(), Does.Contain(component2));
    }

    /// <summary>
    /// Test for the naming of instances.
    /// </summary>
    [Test]
    public void RegisterComponentInstance_ShouldAssignDefaultName_WhenNameIsNull()
    {
        TestUtils.DeleteAllConfigFile();
        var component = new TestComponent();
        var component2 = new TestComponent();

        _componentService.RegisterComponentInstance(component);
        _componentService.RegisterComponentInstance(component2);

        Assert.That(component.InstanceName, Is.EqualTo("Component"));
        Assert.That(component2.InstanceName, Is.EqualTo("Component_1"));
    }

    private class TestComponent : IComponent
    {
        public string InstanceName { get; set; } = string.Empty;

        public Guid Id { get; } = Guid.NewGuid();

        public void Dispose()
        {
        }
    }
}
