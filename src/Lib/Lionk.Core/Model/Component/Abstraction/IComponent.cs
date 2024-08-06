// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// Interface that define a component.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string? InstanceName { get; set; }
}
