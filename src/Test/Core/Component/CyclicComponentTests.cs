// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.Component.Mock;
using LionkTest.Core.Component.Mock;

namespace LionkTest.Core.Component;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
public class CyclicComponentTests
{
    /// <summary>
    /// Test for <see cref="CyclicComponentBase.Execute"/>.
    /// </summary>
    [Test]
    public void ExecuteShouldExecuteAction()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        MockCyclicComponent component = new("ComponentTest", TimeSpan.FromMilliseconds(cycleTimeMilliseconds));

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            component.Execute();
            Thread.Sleep(cycleTimeMilliseconds);
        }

        // Assert
        Assert.That(component.NbCycle, Is.EqualTo(nbCycle));
        Assert.That(component.Value, Is.EqualTo(nbCycle));
    }

    /// <summary>
    /// Test for <see cref="CyclicComponentBase.Execute"/>.
    /// </summary>
    [Test]
    public void ExecuteShouldExecuteActionAsync()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        MockCyclicComponentAsync component = new("ComponentTest", TimeSpan.FromMilliseconds(cycleTimeMilliseconds));

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            component.Execute();
            Thread.Sleep(cycleTimeMilliseconds);
        }

        // Assert
        Assert.That(component.NbCycle, Is.EqualTo(nbCycle));
        Assert.That(component.Value, Is.EqualTo(nbCycle));
    }
}
