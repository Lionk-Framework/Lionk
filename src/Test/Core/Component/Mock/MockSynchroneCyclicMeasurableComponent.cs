// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Mock;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
internal class MockSynchroneCyclicMeasurableComponent : CyclicComponentBase
{
    /// <summary>
    /// Gets the value of the component.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockSynchroneCyclicMeasurableComponent"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    public MockSynchroneCyclicMeasurableComponent(string componentName, TimeSpan cycleTime)
        : base(componentName, cycleTime) => SyncTask = MySuperCyclicAction;

    private void MySuperCyclicAction() => Value++;
}
