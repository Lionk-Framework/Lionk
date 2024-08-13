// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Mock;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
internal class MockCyclicComponent : CyclicComponent
{
    /// <summary>
    /// Gets the value of the component.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockCyclicComponent"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    public MockCyclicComponent(string componentName, TimeSpan cycleTime)
        : base(componentName, cycleTime) => CyclicTask = MySuperCyclicAction;

    private void MySuperCyclicAction() => Value++;
}
