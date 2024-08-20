// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// Base class that implements the <see cref="IExecutableComponent"/> interface.
/// Provides a template for components that can be executed with a defined life cycle,
/// including initialization, execution, and termination phases.
/// </summary>
public abstract class BaseExecutableComponent : BaseComponent, IExecutableComponent
{
    private CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Gets a value indicating whether the component can be executed.
    /// This property must be implemented by derived classes to specify the conditions
    /// under which the component is ready to execute.
    /// </summary>
    public abstract bool CanExecute { get; }

    /// <summary>
    /// Gets a value indicating whether the component is currently running.
    /// This value is managed internally and should not be modified directly.
    /// </summary>
    public bool IsRunning
    {
        get => _isRunning;
        private set => SetField(ref _isRunning, value);
    }

    /// <summary>
    /// Executes the component by initializing it (if not already initialized),
    /// then running the execution logic, and finally terminating the execution.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the component cannot be executed because it is either not ready
    /// (as determined by <see cref="CanExecute"/>) or is already running.
    /// </exception>
    public void Execute()
    {
        if (!CanExecute)
            throw new InvalidOperationException("The component can't be executed when it is not ready.");

        if (IsRunning)
            throw new InvalidOperationException("The component is already running.");

        if (!_isInitialized) OnInitialize();

        _cancellationTokenSource = new CancellationTokenSource();  // Reset the token source for each execution

        try
        {
            OnExecute(_cancellationTokenSource.Token);
        }
        finally
        {
            OnTerminate();
        }
    }

    /// <summary>
    /// Aborts the component's execution by canceling the ongoing operation.
    /// Can be overridden by derived classes to implement custom abort logic.
    /// </summary>
    public virtual void Abort()
        => _cancellationTokenSource.Cancel();

    /// <summary>
    /// Called once before the component is executed, if it hasn't been initialized yet.
    /// Derived classes can override this method to implement custom initialization logic.
    /// By default, this method sets the <see cref="_isInitialized"/> flag to true.
    /// </summary>
    protected virtual void OnInitialize()
        => _isInitialized = true;

    /// <summary>
    /// The main execution logic of the component. This method is called when the component
    /// is executed, and it should be overridden by derived classes to implement the specific
    /// behavior of the component. By default, it sets the <see cref="IsRunning"/> flag to true.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the execution.</param>
    protected virtual void OnExecute(CancellationToken cancellationToken)
        => IsRunning = true;

    /// <summary>
    /// Called after the execution logic has completed, regardless of whether it succeeded
    /// or failed. Derived classes can override this method to implement custom termination logic.
    /// By default, this method sets the <see cref="IsRunning"/> flag to false.
    /// </summary>
    protected virtual void OnTerminate()
        => IsRunning = false;

    /// <summary>
    /// Inherited from <see cref="BaseComponent"/> but with additional logic to abort the execution.
    /// </summary>
    public override void Dispose()
    {
        if (IsRunning)
            Abort();

        base.Dispose();
    }

    private bool _isRunning;
    private bool _isInitialized;
}
