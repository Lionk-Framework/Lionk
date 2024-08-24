// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
///     This interface defines a cyclic component.
/// </summary>
public interface IExecutableComponent : IComponent
{
    #region properties

    /// <summary>
    ///     Gets  a value indicating whether the component can be executed.
    /// </summary>
    bool CanExecute { get; }

    /// <summary>
    ///     Gets a value indicating whether gets a value indicating wether the component is in error.
    /// </summary>
    bool IsInError { get; }

    /// <summary>
    ///     Gets a value indicating whether the component is running.
    /// </summary>
    bool IsRunning { get; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Abort the current execution.
    /// </summary>
    void Abort();

    /// <summary>
    ///     Executes the component.
    /// </summary>
    void Execute();

    /// <summary>
    ///     Used to reset the component.
    /// </summary>
    void Reset();

    #endregion
}
