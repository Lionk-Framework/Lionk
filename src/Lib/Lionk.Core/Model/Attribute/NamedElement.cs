// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Attribute to define the name of a viewable component.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NamedElement"/> class.
/// </remarks>
/// <param name="name">The name.</param>
/// <param name="description">The description.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
public class NamedElement(string name, string description) : Attribute
{
    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the description of the item.
    /// </summary>
    public string Description { get; } = description;
}
