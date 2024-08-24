// Copyright © 2024 Lionk Project

using Lionk.Auth.Abstraction;

namespace Lionk.Auth.Identity;

/// <summary>
///     This class represents a user service.
/// </summary>
public class UserService : IUserService
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="repository">The user repository.</param>
    public UserService(IUserRepository repository) => UserRepository = repository;

    #endregion

    #region properties

    /// <inheritdoc />
    public IUserRepository UserRepository { get; set; }

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public bool Delete(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (!IsIdExist(user.Id))
        {
            return false;
        }

        UserRepository.DeleteUser(user);
        return true;
    }

    /// <inheritdoc />
    public User? GetRegisteredUser(string username, string passwordHash)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username) && string.Equals(user.PasswordHash, passwordHash));
        return user;
    }

    /// <inheritdoc />
    public User? GetUserByEmail(string email)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Email, email));
        return user;
    }

    /// <inheritdoc />
    public User? GetUserById(Guid id)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => user.Id.Equals(id));
        return user;
    }

    /// <inheritdoc />
    public User? GetUserByUsername(string username)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username));
        return user;
    }

    /// <inheritdoc />
    public HashSet<User> GetUsers() => UserRepository.GetUsers();

    /// <inheritdoc />
    public string GetUserSalt(string username)
    {
        HashSet<User> users = GetUsers();
        User? user = users.FirstOrDefault(user => string.Equals(user.Username, username));
        return user?.Salt ?? string.Empty;
    }

    /// <inheritdoc />
    public User? Insert(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (IsUsernameExist(user.Username) || IsEmailExist(user.Email))
        {
            return null;
        }

        UserRepository.SaveUser(user);
        return user;
    }

    /// <inheritdoc />
    public bool IsEmailExist(string email) => GetUserByEmail(email) != null;

    /// <inheritdoc />
    public bool IsIdExist(Guid id) => GetUserById(id) != null;

    /// <inheritdoc />
    public bool IsUsernameExist(string username) => GetUserByUsername(username) != null;

    /// <inheritdoc />
    public User? Update(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (!IsIdExist(user.Id))
        {
            return null;
        }

        UserRepository.UpdateUser(user);
        return user;
    }

    #endregion
}
