namespace Lionk.Core;

/// <summary>
/// Specifies the property name of the ID for a component to enable linking after deserialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IdIsAttribute(string idPropertyName) : Attribute
{
    public string IdPropertyName { get; } = idPropertyName;
}
