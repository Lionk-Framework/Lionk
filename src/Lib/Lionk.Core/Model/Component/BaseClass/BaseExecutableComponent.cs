// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Core.Component;

/// <summary>
/// Base class that implements the <see cref="IExecutableComponent"/> interface.
/// Provides a template for components that can be executed with a defined life cycle,
/// including initialization, execution, termination phases, and error handling.
/// </summary>
public abstract class BaseExecutableComponent : BaseComponent, IExecutableComponent
{
    private CancellationTokenSource _cancellationTokenSource = new();

    private bool _isRunning;
    private bool _isInitialized;
    private bool _isInError;

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
    [JsonIgnore]
    public bool IsRunning
    {
        get => _isRunning;
        private set => SetField(ref _isRunning, value);
    }

    /// <summary>
    /// Gets a value indicating whether the component is currently in an error state.
    /// This value is set to true if the component encounters an error during execution,
    /// or if it is aborted. A component in an error state cannot be executed until it is reset.
    /// </summary>
    [JsonIgnore]
    public bool IsInError
    {
        get => _isInError;
        private set => SetField(ref _isInError, value);
    }

    /// <summary>
    /// Executes the component by initializing it (if not already initialized),
    /// then running the execution logic, and finally terminating the execution.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the component cannot be executed because it is either in an error state
    /// (as indicated by <see cref="IsInError"/>), is not ready to execute (as determined by
    /// <see cref="CanExecute"/>), or is already running.
    /// </exception>
    public void Execute()
    {
        if (IsInError)
            throw new InvalidOperationException("The component is in error and cannot be executed.");

        if (!CanExecute)
            throw new InvalidOperationException("The component cannot be executed because it is not ready.");

        if (IsRunning)
            throw new InvalidOperationException("The component is already running.");

        if (!_isInitialized) OnInitialize();

        _cancellationTokenSource = new CancellationTokenSource();  // Reset the token source for each execution

        try
        {
            OnExecute(_cancellationTokenSource.Token);
        }
        catch (Exception)
        {
            Abort(); // Abort the execution and mark as in error
            throw; // Re-throw the exception to propagate the error
        }
        finally
        {
            OnTerminate(); // Ensure that termination logic is executed
        }
    }

    /// <summary>
    /// Aborts the component's execution by canceling the ongoing operation.
    /// This method sets the <see cref="IsInError"/> flag to true, preventing further executions
    /// until the component is reset. Can be overridden by derived classes to implement custom abort logic.
    /// </summary>
    public virtual void Abort()
    {
        IsInError = true;
        IsRunning = false;
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Resets the component, clearing the error state and allowing it to be executed again.
    /// This method cannot be called while the component is running.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the component is running when an attempt is made to reset it.
    /// </exception>
    public void Reset()
    {
        if (IsRunning)
            throw new InvalidOperationException("Cannot reset the component while it is running.");

        _isInitialized = false;
        IsInError = false;
    }

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
    /// Disposes of the component, ensuring that any running execution is aborted before disposal.
    /// This method should be called when the component is no longer needed to release resources.
    /// </summary>
    public override void Dispose()
    {
        if (IsRunning)
            Abort();

        base.Dispose();
    }
}
