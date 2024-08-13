// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace LionkTest.Core.Component;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
public class CyclicComponentTests : CyclicComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentTests"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="action"> The action to execute. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    /// <param name="args"> The arguments of the action. </param>
    public CyclicComponentTests(string componentName, Action<object?[]?> action, TimeSpan cycleTime, params object?[] args)
        : base(componentName, cycleTime, action, args)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentTests"/> class.
    /// </summary>
    public CyclicComponentTests()
        : base("TestName", TimeSpan.FromSeconds(1), args => { }, null)
    {
    }

    /// <summary>
    /// Test for <see cref="CyclicComponentBase.Execute"/>.
    /// </summary>
    [Test]
    public void ExecuteShouldExecuteAction()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        List<string?> testWords = new();
        string arg = "Hello CyclicComponent";
        Action<object?[]?> action = args => testWords.Add((string?)args?[0]);
        CyclicComponentTests component = new("ComponentTest", action, TimeSpan.FromMilliseconds(cycleTimeMilliseconds), arg);

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            component.Execute();
            Thread.Sleep(cycleTimeMilliseconds);
        }

        // Assert
        Assert.That(component.NbCycle == nbCycle);
        Assert.That(testWords.Count == component.NbCycle);
        foreach (string? word in testWords)
        {
            Assert.That(word, Is.EqualTo(arg));
        }
    }

    /// <summary>
    /// Test for <see cref="CyclicComponentBase.Execute"/>.
    /// </summary>
    [Test]
    public void Execute_ShouldExecuteActionOnlyWhenTimeIsElapsed()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        List<string?> testWords = new();
        string arg = "Hello CyclicComponent";
        Action<object?[]?> action = args => testWords.Add((string?)args?[0]);
        CyclicComponentTests component = new("ComponentTest", action, TimeSpan.FromMilliseconds(cycleTimeMilliseconds), arg);

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            component.Execute();
        }

        TimeSpan executionDuration = DateTime.UtcNow - start;
        int hypoteticNbExecution = ((int)executionDuration.TotalMilliseconds / cycleTimeMilliseconds) + 1;

        // Assert
        Assert.That(component.NbCycle == hypoteticNbExecution);
        Assert.That(testWords.Count == component.NbCycle);
        foreach (string? word in testWords)
        {
            Assert.That(word, Is.EqualTo(arg));
        }
    }
}
