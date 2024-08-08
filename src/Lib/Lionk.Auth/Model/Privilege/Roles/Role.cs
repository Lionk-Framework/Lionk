// Copyright © 2024 Lionk Project

using System.Collections.ObjectModel;

namespace Lionk.Auth.Privilege;

/// <summary>
/// This class represents a role.
/// </summary>
public class Role
{
    private readonly HashSet<Permission> _permissions;

    /// <summary>
    /// Gets the unique identifier of the role.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the description of the role.
    /// </summary>
    public ReadOnlyCollection<Permission> Permissions => _permissions.ToList().AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name"> The name of the role.</param>
    public Role(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        _permissions = new();
    }

    /// <summary>
    /// Adds a permission to the role.
    /// </summary>
    /// <param name="permission"> The permission to add.</param>
    public void AddPermission(Permission permission) => _permissions.Add(permission);

    /// <summary>
    /// Removes a permission from the role.
    /// </summary>
    /// <param name="permission"> The permission to remove.</param>
    public void RemovePermission(Permission permission) => _permissions.Remove(permission);
}
