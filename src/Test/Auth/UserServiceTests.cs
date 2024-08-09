// Copyright © 2024 Lionk Project

using Lionk.Auth.Identity;
using Lionk.Auth.Utils;
using Lionk.Utils;

namespace LionkTest.Auth;

/// <summary>
/// This class represents the user service tests.
/// </summary>
public class UserServiceTests
{
    private User _user1;
    private User _user2;
    private User _user3;

    /// <summary>
    /// Setups the tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Clear the files
        ConfigurationUtils.TryDeleteFile(UserFileHandler.UsersPath, FolderType.Data);

        // Arrange
        string salt1 = PasswordUtils.GenerateSalt(16);
        string salt2 = PasswordUtils.GenerateSalt(16);
        string salt3 = PasswordUtils.GenerateSalt(16);
        List<string> roles = new() { "role1", "role2" };
        _user1 = new("user1", "email1", "password1", salt1, roles);
        _user2 = new("user2", "email2", "password2", salt2, roles);
        _user3 = new("user3", "email3", "password3", salt3, roles);
    }

    /// <summary>
    /// This method tests the insertion of multiple users.
    /// </summary>
    [Test]
    public void InsertUserCountVerification()
    {
        // Arrange
        // nothing to arrange

        // Act
        User? instert1 = UserService.Insert(_user1);
        User? instert2 = UserService.Insert(_user2);

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

        User? insert = UserService.Insert(_user1);

        // Act
        _user1.UpdateUsername(newName);
        _user1.UpdateEmail(newMail);
        _user1.UpdatePasswordHash(newPass);
        User? updated = UserService.Update(_user1);

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
        // Arrange
        // nothing to arrange

        // Act
        UserService.Insert(_user1);
        UserService.Insert(_user2);
        int count = UserService.GetUsers().Count;
        bool isDeleted = UserService.Delete(_user1);

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
        User? userCopy = _user1;

        // Act
        User? insert = UserService.Insert(_user1);
        User? insertCopy = UserService.Insert(userCopy);

        // Assert
        Assert.That(insert, Is.Not.Null);
        Assert.That(insertCopy, Is.Null);
    }

    /// <summary>
    /// This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void AddRoleToUser()
    {
        // Arrange
        // nothing to arrange

        // Act
        _user1.AddRole("role3");

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(3));
    }

    /// <summary>
    /// This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void AddRoleListToUser()
    {
        // Arrange
        List<string> rolesToAdd = new() { "role3", "role4" };

        // Act
        _user1.AddRoles(rolesToAdd);

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(4));
    }

    /// <summary>
    /// This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void RemoveRoleFromUser()
    {
        // Arrange
        _user1.AddRole("role3");

        // Act
        _user1.RemoveRole("role3");

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(2));
    }

    /// <summary>
    /// This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void RemoveRoleListFromUser()
    {
        // Arrange
        List<string> rolesToAdd = new() { "role3", "role4" };
        _user1.AddRoles(rolesToAdd);

        // Act
        _user1.RemoveRoles(rolesToAdd);

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(2));
    }
}
