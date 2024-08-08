// Copyright © 2024 Lionk Project

using System.Diagnostics;

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a user service.
/// </summary>
public static class UserService
{
    /// <summary>
    /// Method to get all the users.
    /// </summary>
    /// <returns> The hashSet of all the users.</returns>
    public static HashSet<User> GetUsers() => UserFileHandler.GetUsers();

    /// <summary>
    /// Method to get a user by its id.
    /// </summary>
    /// <param name="id"> The id of the user.</param>
    /// <returns> The user with the id, null if not found.</returns>
    public static User? GetUserById(Guid id)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => user.Id.Equals(id));
        return user;
    }

    /// <summary>
    /// Method to get a user by its username.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <returns> The user with the username, null if not found.</returns>
    public static User? GetUserByUsername(string username)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username));
        return user;
    }

    /// <summary>
    /// Method to get a user by its email.
    /// </summary>
    /// <param name="email"> The email of the user.</param>
    /// <returns> The user with the email, null if not found.</returns>
    public static User? GetUserByEmail(string email)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Email, email));
        return user;
    }

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
    /// Method to insert a user.
    /// </summary>
    /// <param name="user"> The user to insert.</param>
    /// <returns> The inserted user, null the insertion failed.</returns>
    public static User? Insert(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (UsernameExists(user) || EmailExists(user)) return null;
        UserFileHandler.SaveUser(user);
        return user;
    }

    /// <summary>
    /// Method to check if a username exists.
    /// </summary>
    /// <param name="user"> The user to check.</param>
    /// <returns> True if the username exists, false otherwise.</returns>
    public static bool UsernameExists(User user) => GetUserByUsername(user.Username) != null;

    /// <summary>
    /// Method to check if a username exists.
    /// </summary>
    /// <param name="user"> The user to check.</param>
    /// <returns> True if the username exists, false otherwise.</returns>
    public static bool EmailExists(User user) => GetUserByEmail(user.Email) != null;

    /// <summary>
    /// Method to check if an id exists.
    /// </summary>
    /// <param name="user"> The user to check.</param>
    /// <returns> True if the id exists, false otherwise.</returns>
    public static bool IdExists(User user) => GetUserById(user.Id) != null;

    /// <summary>
    /// Method to update a user.
    /// </summary>
    /// <param name="user"> The user to update.</param>
    /// <returns> The updated user, null if the update failed.</returns>
    public static User? Update(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (!IdExists(user)) return null;
        UserFileHandler.UpdateUser(user);
        return user;
    }

    /// <summary>
    /// Method to delete a user.
    /// </summary>
    /// <param name="user"> The user to delete.</param>
    /// <returns> True if the user was deleted, false otherwise.</returns>
    public static bool Delete(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (!IdExists(user)) return false;
        UserFileHandler.DeleteUser(user);
        return true;
    }
}
