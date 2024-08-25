// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace Lionk.Auth.Identity;

/// <summary>
///     This class represents a user service.
/// </summary>
public class UserServiceRazor
{
    #region fields

    private readonly ProtectedLocalStorage _protectedLocalStorage;

    private readonly string _userStorageKey = "UserIdentity";

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserServiceRazor" /> class.
    /// </summary>
    /// <param name="protectedLocalStorage"> The protected local storage.</param>
    public UserServiceRazor(ProtectedLocalStorage protectedLocalStorage)
     =>
        _protectedLocalStorage =
            protectedLocalStorage ?? throw new ArgumentNullException(nameof(protectedLocalStorage));

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to clear the user data from the browser.
    /// </summary>
    /// <returns> The task.</returns>
    public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_userStorageKey);

    /// <summary>
    ///     Method to fetch the user from the browser.
    /// </summary>
    /// <returns> The user fetched from the browser.</returns>
    public async Task<User?> FetchUserFromBrowserAsync()
    {
        try
        {
            ProtectedBrowserStorageResult<string> storedUserResult = await _protectedLocalStorage.GetAsync<string>(_userStorageKey);

            if (storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
            {
                User? user = JsonConvert.DeserializeObject<User>(storedUserResult.Value);

                return user;
            }
        }
        catch (InvalidOperationException)
        {
            // The user is not stored in the browser.
        }

        return null;
    }

    /// <summary>
    ///     Persists the user to the browser.
    /// </summary>
    /// <param name="user"> The user to persist.</param>
    /// <returns> The task.</returns>
    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        await _protectedLocalStorage.SetAsync(_userStorageKey, userJson);
    }

    #endregion
}
