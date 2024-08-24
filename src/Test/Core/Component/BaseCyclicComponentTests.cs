// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace LionkTest.Core;

/// <summary>
///     Test class for the <see cref="BaseCyclicComponent" /> class.
/// </summary>
[TestFixture]
public class BaseCyclicComponentTests
{
    #region fields

    private TestableCyclicComponent _cyclicComponent;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Tests that the <see cref="BaseCyclicComponent.OnExecute(CancellationToken)" /> method increments the
    ///     <see cref="BaseCyclicComponent.NbCycle" /> property.
    /// </summary>
    [Test]
    public void OnExecute_IncrementsNbCycle()
    {
        _cyclicComponent.InvokeOnInitialize();
        int initialNbCycle = _cyclicComponent.NbCycle;

        _cyclicComponent.InvokeOnExecute(CancellationToken.None);

        Assert.That(_cyclicComponent.NbCycle, Is.EqualTo(initialNbCycle + 1));
    }

    /// <summary>
    ///     Tests that the <see cref="BaseCyclicComponent.OnInitialize" /> method sets the <see cref="BaseCyclicComponent.StartedDate" /> property
    ///     correctly.
    /// </summary>
    [Test]
    public void OnInitialize_SetsStartedDate()
    {
        DateTime beforeInitialization = DateTime.Now;

        _cyclicComponent.InvokeOnInitialize();

        Assert.That(_cyclicComponent.StartedDate, Is.GreaterThanOrEqualTo(beforeInitialization));
    }

    /// <summary>
    ///     Tests that the <see cref="BaseCyclicComponent.OnTerminate" /> method sets the <see cref="BaseCyclicComponent.LastExecution" /> property
    ///     to the current date and time.
    /// </summary>
    [Test]
    public void OnTerminate_SetsLastExecutionToNow()
    {
        _cyclicComponent.InvokeOnInitialize();

        _cyclicComponent.InvokeOnTerminate();

        Assert.That(_cyclicComponent.LastExecution, Is.EqualTo(DateTime.Now).Within(1).Seconds);
    }

    /// <summary>
    ///     Setups the test environment.
    /// </summary>
    [SetUp]
    public void Setup() => _cyclicComponent = new TestableCyclicComponent();

    /// <summary>
    ///     Tears down the test environment.
    /// </summary>
    [TearDown]
    public void TearDown() => _cyclicComponent.Dispose();

    #endregion

    /// <summary>
    ///     A testable version of <see cref="BaseCyclicComponent" /> to expose protected methods for testing.
    /// </summary>
    private class TestableCyclicComponent : BaseCyclicComponent
    {
        #region properties

        // Implement the abstract members from the base class
        public override bool CanExecute => true;

        #endregion

        #region public and override methods

        public void InvokeOnExecute(CancellationToken cancellationToken) => OnExecute(cancellationToken);

        public void InvokeOnInitialize() => OnInitialize();

        public void InvokeOnTerminate() => OnTerminate();

        #endregion
    }
}
