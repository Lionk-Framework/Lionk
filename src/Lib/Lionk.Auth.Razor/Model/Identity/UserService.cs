// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a user service.
/// </summary>
public class UserService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="protectedLocalStorage"> The protected local storage.</param>
    public UserService(ProtectedLocalStorage protectedLocalStorage)
    {
        if (protectedLocalStorage is null) throw new ArgumentNullException(nameof(protectedLocalStorage));
        _protectedLocalStorage = protectedLocalStorage;
    }
}
