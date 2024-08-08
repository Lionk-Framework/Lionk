// Copyright © 2024 Lionk Project

using System.ComponentModel;

namespace Lionk.Auth.Privilege;

/// <summary>
/// This enum represents the permissions that can be assigned to a user.
/// </summary>
public enum Permission
{
    /// <summary>
    /// Value that represents full access to the application.
    /// </summary>
    [Description("Full Access to application")]
    Full = 0,

    /// <summary>
    /// Value that represents the ability to manage users.
    /// </summary>
    [Description("Can manage users")]
    HumanResources = 1,

    /// <summary>
    /// Value that represents the ability to manage roles.
    /// </summary>
    [Description("Can manage roles")]
    RoleManager = 2,
}
