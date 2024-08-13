// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// Interface for measurable component.
/// </summary>
public interface IMeasurableComponent : IComponent
{
    /// <summary>
    /// Gets the unit of the value.
    /// </summary>
    string? Unit { get; }

    /// <summary>
    /// Gets the value of the component.
    /// </summary>
    double Value { get; }
}
