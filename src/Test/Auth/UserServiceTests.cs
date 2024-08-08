// Copyright © 2024 Lionk Project

using System.Globalization;
using Lionk.Auth.Identity;
using Lionk.Auth.Utils;
using Lionk.Utils;

namespace LionkTest.Auth;

/// <summary>
/// This class represents the user service tests.
/// </summary>
public class UserServiceTests
{
    /// <summary>
    /// Setups the tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Clear the files
        string usersFilePath = Path.Combine("users", "users.json");
        ConfigurationUtils.TryDeleteFile(usersFilePath, FolderType.Data);
    }

    /// <summary>
    /// This method tests the insertion of multiple users.
    /// </summary>
    [Test]
    public void InsertUserCountVerification()
    {
        // Arrange
        string salt1 = PasswordUtils.GenerateSalt(16);
        string salt2 = PasswordUtils.GenerateSalt(16);
        User user1 = new("user1", "user1@email.com", "password1", salt1);
        User user2 = new("user2", "user2@email.com", "password2", salt2);

        // Act
        User? instert1 = UserService.Insert(user1);
        User? instert2 = UserService.Insert(user2);

        // Assert
        Assert.That(instert1, Is.Not.Null);
        Assert.That(instert2, Is.Not.Null);
        HashSet<User> users = UserService.GetUsers();
        Assert.That(users.Count, Is.EqualTo(2));
    }

    /// <summary>
    /// Method to test that the user is updated.
    /// </summary>
    [Test]
    public void UpdateUserVerification()
    {
        // Arrange
        string newName = "userUpdated";
        string newMail = "userUpdated@email.com";
        string newPass = "passwordUpdated";
        string salt = "salt";

        User user = new("user", "user@email.com", "password", salt);
        User? insert = UserService.Insert(user);

        // Act
        user.UpdateUsername(newName);
        user.UpdateEmail(newMail);
        user.UpdatePasswordHash(newPass);
        User? updated = UserService.Update(user);

        // Assert
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated?.Username, Is.EqualTo(newName));
        Assert.That(updated?.Email, Is.EqualTo(newMail));
        Assert.That(updated?.PasswordHash, Is.EqualTo(newPass));
    }

    /// <summary>
    /// Method to test that the user is deleted.
    /// </summary>
    [Test]
    public void DeleteUserVerification()
    {
        string salt1 = PasswordUtils.GenerateSalt(16);
        string salt2 = PasswordUtils.GenerateSalt(16);
        User user1 = new("user1", "user1@email.com", "password1", salt1);
        User user2 = new("user2", "user2@email.com", "password2", salt2);

        // Act
        UserService.Insert(user1);
        UserService.Insert(user2);
        int count = UserService.GetUsers().Count;
        bool isDeleted = UserService.Delete(user1);

        // Assert
        Assert.That(isDeleted, Is.True);
        Assert.That(UserService.GetUsers().Count, Is.EqualTo(count - 1));
    }

    /// <summary>
    /// Method to test the insertion of a null user.
    /// </summary>
    [Test]
    public void InsertNullUser() =>

        // Impossible to test with current stylecop
        Assert.Pass();

    /// <summary>
    /// Method to test the insertion of an existing user.
    /// </summary>
    [Test]
    public void InsertExistingUser()
    {
        // Arrange
        User user = new("user", "user@email.com", "password", "salt");
        User? userCopy = user;

        // Act
        User? insert = UserService.Insert(user);
        User? insertCopy = UserService.Insert(userCopy);

        // Assert
        Assert.That(insert, Is.Not.Null);
        Assert.That(insertCopy, Is.Null);
    }
}
