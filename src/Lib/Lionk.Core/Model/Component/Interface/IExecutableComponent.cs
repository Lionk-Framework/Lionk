// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface defines a cyclic component.
/// </summary>
public interface IExecutableComponent : IComponent
{
    /// <summary>
    /// Executes the component.
    /// </summary>
    void Execute();

    /// <summary>
    /// Gets a value indicating whether the component can be executed.
    /// </summary>
    public bool CanExecute { get; }
}
