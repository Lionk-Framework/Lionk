// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace LionkTest.Core;

/// <summary>
/// Test class for the <see cref="BaseExecutableComponent"/> class.
/// </summary>
[TestFixture]
public class BaseExecutableComponentTests
{
    private TestableExecutableComponent _testableComponent;

    /// <summary>
    /// Tears down the test environment.
    /// </summary>
    [TearDown]
    public void TearDown()
        => _testableComponent.Dispose();

    /// <summary>
    /// Setups the test environment.
    /// </summary>
    [SetUp]
    public void Setup()
        => _testableComponent = new TestableExecutableComponent();

    /// <summary>
    /// Tests the <see cref="BaseExecutableComponent.Execute"/> method when <see cref="BaseExecutableComponent.CanExecute"/> is false.
    /// </summary>
    [Test]
    public void Execute_WhenCanExecuteIsFalse_ThrowsInvalidOperationException()
    {
        _testableComponent.CanExecuteSetter = false;

        Assert.Throws<InvalidOperationException>(() => _testableComponent.Execute());
    }

    /// <summary>
    /// Tests the <see cref="BaseExecutableComponent.Execute"/> method when it can be executed successfully.
    /// </summary>
    [Test]
    public void Execute_WhenValid_ExecutesSuccessfully()
    {
        _testableComponent.CanExecuteSetter = true;
        _testableComponent.IsRunning = false;

        _testableComponent.Execute();

        Assert.IsTrue(_testableComponent.OnExecuteCalled);
        Assert.IsTrue(_testableComponent.OnTerminateCalled);
    }

    /// <summary>
    /// Tests the <see cref="BaseExecutableComponent.Abort"/> method.
    /// </summary>
    [Test]
    public void Abort_WhenCalled_CancelsToken()
    {
        _testableComponent.CanExecuteSetter = true;

        _testableComponent.Abort();

        Assert.IsTrue(_testableComponent.TokenCancelled);
    }

    /// <summary>
    /// A testable version of <see cref="BaseExecutableComponent"/> for exposing protected methods.
    /// </summary>
    private class TestableExecutableComponent : BaseExecutableComponent
    {
        public bool OnExecuteCalled { get; private set; }

        public bool OnTerminateCalled { get; private set; }

        public bool TokenCancelled { get; private set; }

        public new bool IsRunning { get; set; }

        public override bool CanExecute => CanExecuteSetter;

        public bool CanExecuteSetter { get; set; }

        protected override void OnExecute(CancellationToken cancellationToken)
        {
            OnExecuteCalled = true;
            base.OnExecute(cancellationToken);

            Thread.SpinWait(10000);
        }

        protected override void OnTerminate()
        {
            OnTerminateCalled = true;
            base.OnTerminate();
        }

        public override void Abort()
        {
            TokenCancelled = true;
            base.Abort();
        }
    }
}
