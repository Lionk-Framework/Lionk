// Copyright © 2024 Lionk Project

using System.ComponentModel;
using System.Reflection;

namespace Lionk.Auth.Privilege;

/// <summary>
/// Extension methods for the Permission enum.
/// </summary>
public static class PermissionExtension
{
    /// <summary>
    /// Gets the description of an enum value.
    /// </summary>
    /// <param name="value">The enum value to get the description for.</param>
    /// <returns>The description of the enum or the name of the enum if no description is found.</returns>
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field is null) return value.ToString();
        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}
