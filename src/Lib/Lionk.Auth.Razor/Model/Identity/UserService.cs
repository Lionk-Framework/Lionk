// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a user service.
/// </summary>
public class UserService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string _userStorageKey = "UserIdentity";

    /// <summary>
    /// Method to get all the users.
    /// </summary>
    /// <returns> The hashSet of all the users.</returns>
    public static HashSet<User> GetUsers() => UserFileHandler.GetUsers();

    /// <summary>
    /// Method to get a registered user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <param name="passwordHash"> The hash of the password of the user.</param>
    /// <returns> The registered user.</returns>
    public static User? GetRegisteredUser(string username, string passwordHash)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username) && string.Equals(user.PasswordHash, passwordHash));
        return user;
    }

    /// <summary>
    /// Method to get the salt of a user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <returns> The salt of the user.</returns>
    public static string GetUserSalt(string username)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username));
        return user?.Salt ?? string.Empty;
    }

    /// <summary>
    /// Persists the user to the browser.
    /// </summary>
    /// <param name="user"> The user to persist.</param>
    /// <returns> The task.</returns>
    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        await _protectedLocalStorage.SetAsync(_userStorageKey, userJson);
    }

    /// <summary>
    /// Method to fetch the user from the browser.
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
    /// Method to clear the user data from the browser.
    /// </summary>
    /// <returns> The task.</returns>
    public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_userStorageKey);

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
