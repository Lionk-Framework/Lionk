// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.Component.Cyclic;
using Lionk.Notification;
using Moq;

namespace LionkTest.Core.Component.Cyclic;

/// <summary>
///     Test class for the <see cref="CyclicExecutorService" /> class.
/// </summary>
[TestFixture]
public class CyclicExecutorServiceTests
{
    #region fields

    private Mock<IComponentService> _componentServiceMock;

    private Mock<ICyclicComponent> _cyclicComponentMock;

    private Mock<INotifier> _notifierMock;

    private CyclicExecutorService _service;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Tests that if a component fails during execution, it is aborted and a notification is sent.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
    [Test]
    public async Task ExecuteComponent_WhenComponentFails_AbortsAndSendsNotification()
    {
        var component = new ComponentWhichThrow();
        _componentServiceMock.Setup(s => s.GetInstancesOfType<ICyclicComponent>()).Returns(new List<ICyclicComponent> { component });

        _service.Start();

        await Task.Delay(1000); // Give some time for the execution to happen

        Assert.That(component.IsInError, Is.True);
    }

    /// <summary>
    ///     Tests that if a component execution times out, it is aborted.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
    [Test]
    public async Task ExecuteComponent_WhenExecutionTimesOut_AbortsComponent()
    {
        _service = new CyclicExecutorService(_componentServiceMock.Object)
                   {
                       WatchDogTimeout = TimeSpan.FromMilliseconds(500), // Set a short timeout
                   };

        _cyclicComponentMock.Setup(c => c.CanExecute).Returns(true);
        _cyclicComponentMock.Setup(c => c.IsInError).Returns(false);
        _cyclicComponentMock.Setup(c => c.NextExecution).Returns(DateTime.UtcNow.AddSeconds(-1));
        _cyclicComponentMock.Setup(c => c.Execute()).Callback(() => Thread.Sleep(2000)); // Simulate long running task
        _componentServiceMock.Setup(s => s.GetInstancesOfType<ICyclicComponent>())
            .Returns(new List<ICyclicComponent> { _cyclicComponentMock.Object });

        _service.Start();

        await Task.Delay(3000); // Wait for more than the watchdog timeout

        _cyclicComponentMock.Verify(c => c.Abort(), Times.AtLeastOnce);
    }

    /// <summary>
    ///     Tests that cyclic components are not executed if they are in error state.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
    [Test]
    public async Task ExecuteComponents_WhenComponentInErrorState_DoesNotExecute()
    {
        _cyclicComponentMock.Setup(c => c.CanExecute).Returns(true);
        _cyclicComponentMock.Setup(c => c.IsInError).Returns(true);
        _componentServiceMock.Setup(s => s.GetInstancesOfType<ICyclicComponent>())
            .Returns(new List<ICyclicComponent> { _cyclicComponentMock.Object });

        _service.Start();

        await Task.Delay(50); // Give some time for the execution to happen

        _cyclicComponentMock.Verify(c => c.Execute(), Times.Never);
    }

    /// <summary>
    ///     Tests that cyclic components are executed if they are ready.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous unit test.</returns>
    [Test]
    public async Task ExecuteComponents_WhenComponentsAreReady_ExecutesThem()
    {
        _cyclicComponentMock.Setup(c => c.CanExecute).Returns(true);
        _cyclicComponentMock.Setup(c => c.IsInError).Returns(false);
        _cyclicComponentMock.Setup(c => c.NextExecution).Returns(DateTime.UtcNow.AddSeconds(-1));
        _componentServiceMock.Setup(s => s.GetInstancesOfType<ICyclicComponent>())
            .Returns(new List<ICyclicComponent> { _cyclicComponentMock.Object });

        _service.Start();

        await Task.Delay(1000); // Give some time for the execution to happen

        _cyclicComponentMock.Verify(c => c.Execute(), Times.AtLeastOnce);
    }

    /// <summary>
    ///     Tests the Pause method to ensure it transitions to the Paused state.
    /// </summary>
    [Test]
    public void Pause_TransitionsToPausedState()
    {
        _service.Start();
        _service.Pause();

        Assert.That(_service.State, Is.EqualTo(CycleState.Paused));
    }

    /// <summary>
    ///     Tests the Resume method to ensure it transitions from Paused to Running state.
    /// </summary>
    [Test]
    public void Resume_TransitionsToRunningState()
    {
        _service.Start();
        _service.Pause();
        _service.Resume();

        Assert.That(_service.State, Is.EqualTo(CycleState.Running));
    }

    /// <summary>
    ///     Setup for each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _cyclicComponentMock = new Mock<ICyclicComponent>();
        _notifierMock = new Mock<INotifier>();

        _componentServiceMock = new Mock<IComponentService>();
        _service = new CyclicExecutorService(_componentServiceMock.Object);
    }

    /// <summary>
    ///     Tests the Start method to ensure it transitions to the Running state.
    /// </summary>
    [Test]
    public void Start_TransitionsToRunningState()
    {
        _service.Start();

        Assert.That(_service.State, Is.EqualTo(CycleState.Running));
    }

    /// <summary>
    ///     Tests that stopping the service cancels ongoing execution and transitions to stopped state.
    /// </summary>
    [Test]
    public void Stop_CancelsOngoingExecutionAndTransitionsToStopped()
    {
        _cyclicComponentMock.Setup(c => c.CanExecute).Returns(true);
        _cyclicComponentMock.Setup(c => c.IsInError).Returns(false);
        _cyclicComponentMock.Setup(c => c.NextExecution).Returns(DateTime.UtcNow.AddSeconds(-1));
        _cyclicComponentMock.Setup(c => c.Execute()).Callback(() => Thread.Sleep(1000)); // Simulate long running task
        _componentServiceMock.Setup(s => s.GetInstancesOfType<ICyclicComponent>())
            .Returns(new List<ICyclicComponent> { _cyclicComponentMock.Object });

        _service.Start();

        _service.Stop();

        Assert.That(_service.State, Is.EqualTo(CycleState.Stopped));
    }

    /// <summary>
    ///     Tests the Stop method to ensure it transitions to the Stopped state.
    /// </summary>
    [Test]
    public void Stop_TransitionsToStoppedState()
    {
        _service.Start();
        _service.Stop();

        Assert.That(_service.State, Is.EqualTo(CycleState.Stopped));
    }

    #endregion

    private class ComponentWhichThrow : BaseCyclicComponent
    {
        #region properties

        public override bool CanExecute => true;

        #endregion

        #region others methods

        protected override void OnExecute(CancellationToken cancellationToken)
        {
            base.OnExecute(cancellationToken);
            throw new Exception("Test Exception");
        }

        #endregion
    }
}
