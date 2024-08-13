// Copyright © 2024 Lionk Project

using Lionk.Core.Model.Component.Cyclic;

namespace LionkTest.Core.Component.Mock;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
public class MockCyclicComponentAsync : CyclicComponentAsync
{
    /// <summary>
    /// Gets the value of the component.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockCyclicComponentAsync"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    public MockCyclicComponentAsync(string componentName, TimeSpan cycleTime)
        : base(componentName, cycleTime) => CyclicTask = MySuperCyclicAction;

    private async Task MySuperCyclicAction()
    {
        Value++;
        await Task.Delay(0);
    }
}
