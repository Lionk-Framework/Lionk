// Copyright © 2024 Lionk Project

using Lionk.Auth.Identity;

namespace Lionk.Auth.Abstraction;

/// <summary>
/// This interface is used to manage the storage of users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Method to save a user.
    /// </summary>
    /// <param name="user">The user to save.</param>
    public void SaveUser(User user);

    /// <summary>
    /// Method to update a user.
    /// </summary>
    /// <param name="user">The user to update.</param>
    public void UpdateUser(User user);

    /// <summary>
    /// Method to delete a user.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    public void DeleteUser(User id);

    /// <summary>
    /// Method to get a <see cref="HashSet{T}"/> of all the users.
    /// </summary>
    /// <returns>A <see cref="HashSet{T}"/> of all the users.</returns>
    public HashSet<User> GetUsers();
}
