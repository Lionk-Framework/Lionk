// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Specifies the property name of the ID for a component to enable linking after deserialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IdIsAttribute(string idPropertyName) : Attribute
{
    /// <summary>
    ///     Gets the name of the property that represents the ID of the component.
    /// </summary>
    public string IdPropertyName { get; } = idPropertyName;
}
