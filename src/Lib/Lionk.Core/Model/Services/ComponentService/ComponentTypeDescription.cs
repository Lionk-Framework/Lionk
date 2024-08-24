// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
///     Define a component type description.
/// </summary>
public class ComponentTypeDescription
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentTypeDescription" /> class.
    /// </summary>
    /// <param name="type">The type of the component.</param>
    public ComponentTypeDescription(Type type)
    {
        Type = type;
        var attribute = (NamedElement?)Attribute.GetCustomAttribute(type, typeof(NamedElement));
        Name = attribute?.Name ?? "Unnamed";
        Description = attribute?.Description ?? "No description available";
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets the description of the component.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Gets the name of the component.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the type of the component.
    /// </summary>
    public Type Type { get; }

    #endregion
}
