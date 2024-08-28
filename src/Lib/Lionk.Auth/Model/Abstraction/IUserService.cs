// Copyright © 2024 Lionk Project

using Lionk.Auth.Identity;

namespace Lionk.Auth.Abstraction;

/// <summary>
///     Service to manage users.
/// </summary>
public interface IUserService
{
    #region properties

    /// <summary>
    ///     Gets or sets the user repository.
    /// </summary>
    public IUserRepository UserRepository { get; set; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to delete a user.
    /// </summary>
    /// <param name="id"> The user to delete.</param>
    /// <returns> True if the user was deleted, false otherwise.</returns>
    public bool Delete(User id);

    /// <summary>
    ///     Method to get a registered user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <param name="passwordHash"> The hash of the password of the user.</param>
    /// <returns> The registered user.</returns>
    public User? GetRegisteredUser(string username, string passwordHash);

    /// <summary>
    ///     Method to get a user by its email.
    /// </summary>
    /// <param name="email"> The email of the user.</param>
    /// <returns> The user with the email, null if not found.</returns>
    public User? GetUserByEmail(string email);

    /// <summary>
    ///     Method to get a user by its id.
    /// </summary>
    /// <param name="id"> The id of the user.</param>
    /// <returns> The user with the id, null if not found.</returns>
    public User? GetUserById(Guid id);

    /// <summary>
    ///     Method to get a user by its username.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <returns> The user with the username, null if not found.</returns>
    public User? GetUserByUsername(string username);

    /// <summary>
    ///     Method to get all the users.
    /// </summary>
    /// <returns> The hashSet of all the users.</returns>
    public HashSet<User> GetUsers();

    /// <summary>
    ///     Method to get the salt of a user.
    /// </summary>
    /// <param name="username"> The username of the user.</param>
    /// <returns> The salt of the user.</returns>
    public string GetUserSalt(string username);

    /// <summary>
    ///     Method to insert a user.
    /// </summary>
    /// <param name="user"> The user to insert.</param>
    /// <returns> The inserted user, null the insertion failed.</returns>
    public User? Insert(User user);

    /// <summary>
    ///     Method to check if a username exists.
    /// </summary>
    /// <param name="email"> The user to check.</param>
    /// <returns> True if the username exists, false otherwise.</returns>
    public bool IsEmailExist(string email);

    /// <summary>
    ///     Method to check if an id exists.
    /// </summary>
    /// <param name="id"> The user to check.</param>
    /// <returns> True if the id exists, false otherwise.</returns>
    public bool IsIdExist(Guid id);

    /// <summary>
    ///     Method to check if a username exists.
    /// </summary>
    /// <param name="username"> The user to check.</param>
    /// <returns> True if the username exists, false otherwise.</returns>
    public bool IsUsernameExist(string username);

    /// <summary>
    ///     Method to check if the current user being registered is the first user in the system.
    ///     If true, this indicates that the user should be assigned the Admin role.
    /// </summary>
    /// <returns> True if the current user is the first to be registered, false otherwise.</returns>
    public bool IsFirstUserRegistered();

    /// <summary>
    ///     Method to update a user.
    /// </summary>
    /// <param name="user"> The user to update.</param>
    /// <returns> The updated user, null if the update failed.</returns>
    public User? Update(User user);

    #endregion
}
