// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.Core.Razor;

/// <summary>
/// Interface for cyclic components.
/// </summary>
public interface IBlazorCyclicComponent : IExecutableComponent
{
    /// <summary>
    /// Gets the cyclic component.
    /// </summary>
    public CyclicComponent? CyclicComponent { get; }
}
