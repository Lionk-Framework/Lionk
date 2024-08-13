// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace LionkTest.Core.Component;

/// <summary>
/// This class is used to test the cyclic component.
/// </summary>
public class CyclicComponentTests : CyclicComponentBase, IMeasurableComponent
{
    /// <inheritdoc/>
    public string? Unit => "x";

    /// <inheritdoc/>
    public double Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentTests"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="action"> The action to execute. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    /// <param name="args"> The arguments of the action. </param>
    public CyclicComponentTests(string componentName, Action<object?[]?> action, TimeSpan cycleTime, params object?[]? args)
        : base(componentName, cycleTime, action, args) => Value = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentTests"/> class.
    /// </summary>
    /// <param name="componentName"> The name of the component. </param>
    /// <param name="func"> The function to execute. </param>
    /// <param name="cycleTime"> The cycle time of the component. </param>
    /// <param name="args"> The arguments of the function. </param>
    public CyclicComponentTests(string componentName, Func<object?[]?, Task> func, TimeSpan cycleTime, params object?[]? args)
    : base(componentName, cycleTime, func, args) => Value = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclicComponentTests"/> class.
    /// </summary>
    public CyclicComponentTests()
        : base("TestName", TimeSpan.FromSeconds(1), args => { }, null) => Value = 0;

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
        int cycle = hypoteticNbExecution > nbCycle ? nbCycle : hypoteticNbExecution;

        // Assert
        Assert.That(component.NbCycle == cycle);
        Assert.That(testWords.Count == component.NbCycle);
        foreach (string? word in testWords)
        {
            Assert.That(word, Is.EqualTo(arg));
        }
    }

    /// <summary>
    /// Test for <see cref="CyclicComponentBase.ExecuteAsync"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ExecuteAsync_ShouldExecuteFunc()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        List<string?> testWords = new();
        string arg = "Hello CyclicComponent";
        Func<object?[]?, Task> func = async args =>
        {
            await Task.Delay(1);
            testWords.Add((string?)args?[0]);
        };
        CyclicComponentTests component = new("ComponentTest", func, TimeSpan.FromMilliseconds(cycleTimeMilliseconds), arg);

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            await component.ExecuteAsync();
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
    /// Test for <see cref="CyclicComponentBase.ExecuteAsync"/>.
    /// </summary>
    /// <returns> A <see cref="Task"/> representing the asynchronous operation. </returns>
    [Test]
    public async Task ExecuteAsync_ShouldExecuteActionOnlyWhenTimeIsElapsed()
    {
        // Arrange
        int nbCycle = 5;
        int cycleTimeMilliseconds = 1;
        List<string?> testWords = new();
        string arg = "Hello CyclicComponent";
        Func<object?[]?, Task> func = async args =>
        {
            await Task.Delay(1);
            testWords.Add((string?)args?[0]);
        };
        CyclicComponentTests component = new("ComponentTest", func, TimeSpan.FromMilliseconds(cycleTimeMilliseconds), arg);

        // Act
        DateTime start = DateTime.UtcNow;
        for (int i = 0; i < nbCycle; i++)
        {
            await component.ExecuteAsync();
        }

        TimeSpan executionDuration = DateTime.UtcNow - start;
        int hypoteticNbExecution = ((int)executionDuration.TotalMilliseconds / cycleTimeMilliseconds) + 1;

        // Assert
        int cycle = hypoteticNbExecution > nbCycle ? nbCycle : hypoteticNbExecution;
        Assert.That(component.NbCycle == cycle);
        Assert.That(testWords.Count == component.NbCycle);
        foreach (string? word in testWords)
        {
            Assert.That(word, Is.EqualTo(arg));
        }
    }
}
