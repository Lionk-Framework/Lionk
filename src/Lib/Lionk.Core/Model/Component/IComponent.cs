// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Interface that define a component.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Gets the name of the component.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the component.
    /// </summary>
    string Description { get; }
}
