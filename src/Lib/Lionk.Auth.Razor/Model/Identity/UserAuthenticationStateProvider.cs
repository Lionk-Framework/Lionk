// Copyright © 2024 Lionk Project

using System.Security.Claims;
using Lionk.Auth.Abstraction;
using Microsoft.AspNetCore.Components.Authorization;

namespace Lionk.Auth.Identity;

/// <summary>
///     This class is used to provide the authentication state of the user.
/// </summary>
public class UserAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    #region fields

    private readonly UserServiceRazor _userService;

    private readonly IUserService _userServiceImpl;

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserAuthenticationStateProvider" /> class.
    /// </summary>
    /// <param name="userService"> The user service to use.</param>
    /// <param name="userServiceImpl">The local user service implementation.</param>
    public UserAuthenticationStateProvider(UserServiceRazor userService, IUserService userServiceImpl)
    {
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        _userService = userService;
        _userServiceImpl = userServiceImpl;
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets the current user.
    /// </summary>
    public User? CurrentUser { get; private set; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to dispose the user authentication state provider.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }

    /// <summary>
    ///     Method to get the authentication state of the user.
    /// </summary>
    /// <returns> The authentication state of the user.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        {
            var principal = new ClaimsPrincipal();
            User? user = await _userService.FetchUserFromBrowserAsync();

            if (user is null)
            {
                return new AuthenticationState(principal);
            }

            User? userInDatabase = _userServiceImpl.GetRegisteredUser(user.Username, user.PasswordHash);

            if (userInDatabase is null)
            {
                return new AuthenticationState(principal);
            }

            principal = userInDatabase.ToClaimsPrincipal();
            CurrentUser = userInDatabase;

            return new AuthenticationState(principal);
        }
    }

    /// <summary>
    ///     Method to get the authentication state of the user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <param name="passwordHash"> The hash of the password of the user.</param>
    /// <returns> The task.</returns>
    public async Task LoginAsync(string username, string passwordHash)
    {
        var principal = new ClaimsPrincipal();
        User? user = _userServiceImpl.GetRegisteredUser(username, passwordHash);

        if (user is not null)
        {
            await _userService.PersistUserToBrowserAsync(user);
            principal = user.ToClaimsPrincipal();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    ///     Method to log out the user.
    /// </summary>
    /// <returns> The task.</returns>
    public async Task LogoutAsync()
    {
        await _userService.ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    #endregion

    #region others methods

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        AuthenticationState authenticationState = await task;

        if (authenticationState.User.Identities.Any())
        {
            CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        }
    }

    #endregion
}
