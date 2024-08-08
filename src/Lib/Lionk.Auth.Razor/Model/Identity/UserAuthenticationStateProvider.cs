// Copyright © 2024 Lionk Project

using System.Security.Claims;
using Lionk.Auth.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace Lionk.Auth.Razor.Identity;

/// <summary>
/// This class is used to provide the authentication state of the user.
/// </summary>
public class UserAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly UserService _userService;

    /// <summary>
    /// Gets the current user.
    /// </summary>
    public User? CurrentUser { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuthenticationStateProvider"/> class.
    /// </summary>
    /// <param name="userService"> The user service to use.</param>
    public UserAuthenticationStateProvider(UserService userService)
    {
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        _userService = userService;
    }

    /// <summary>
    /// Method to get the authentication state of the user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <param name="passwordHash"> The hash of the password of the user.</param>
    /// <returns> The task.</returns>
    public async Task LoginAsync(string username, string passwordHash)
    {
        var principal = new ClaimsPrincipal();
        User? user = UserService.GetRegisteredUser(username, passwordHash);

        if (user is not null)
        {
            await _userService.PersistUserToBrowserAsync(user);
            principal = user.ToClaimsPrincipal();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// Method to logout the user.
    /// </summary>
    /// <returns> The task.</returns>
    public async Task LogoutAsync()
    {
        await _userService.ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }

    /// <summary>
    /// Method to get the authentication state of the user.
    /// </summary>
    /// <returns> The authentication state of the user.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        {
            var principal = new ClaimsPrincipal();
            User? user = await _userService.FetchUserFromBrowserAsync();

            if (user is not null)
            {
                User? userInDatabase = UserService.GetRegisteredUser(user.Username, user.PasswordHash);

                if (userInDatabase is not null)
                {
                    principal = userInDatabase.ToClaimsPrincipal();
                    CurrentUser = userInDatabase;
                }
            }

            return new(principal);
        }
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        AuthenticationState? authenticationState = await task;

        if (authenticationState is not null && authenticationState.User.Identities.Count() > 0)
        {
            CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        }
    }

    /// <summary>
    /// Method to dispose the user authentication state provider.
    /// </summary>
    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
}
