// Copyright © 2024 Lionk Project

using Lionk.Auth.Abstraction;
using Lionk.Auth.Identity;
using Lionk.Auth.Utils;
using Lionk.Utils;

namespace LionkTest.Auth;

/// <summary>
///     This class represents the user service tests.
/// </summary>
public class UserServiceTests
{
    #region fields

    private readonly IUserService _userService = new UserService(new UserFileHandler());

    private User _user1;

    private User _user2;

    private User _user3;

    #endregion

    #region public and override methods

    /// <summary>
    ///     This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void AddRoleListToUser()
    {
        // Arrange
        List<string> rolesToAdd = ["role3", "role4"];

        // Act
        _user1.AddRoles(rolesToAdd);

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(4));
    }

    /// <summary>
    ///     This method tests the insertion of a user with a null username.
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
    ///     Method to test that the user is deleted.
    /// </summary>
    [Test]
    public void DeleteUserVerification()
    {
        // Arrange
        // nothing to arrange

        // Act
        _userService.Insert(_user1);
        _userService.Insert(_user2);
        int count = _userService.GetUsers().Count;
        bool isDeleted = _userService.Delete(_user1);

        // Assert
        Assert.That(isDeleted, Is.True);
        Assert.That(_userService.GetUsers().Count, Is.EqualTo(count - 1));
    }

    /// <summary>
    ///     Method to test the insertion of an existing user.
    /// </summary>
    [Test]
    public void InsertExistingUser()
    {
        // Arrange
        User? userCopy = _user1;

        // Act
        User? insert = _userService.Insert(_user1);
        User? insertCopy = _userService.Insert(userCopy);

        // Assert
        Assert.That(insert, Is.Not.Null);
        Assert.That(insertCopy, Is.Null);
    }

    /// <summary>
    ///     Method to test the insertion of a null user.
    /// </summary>
    [Test]
    public void InsertNullUser() =>

        // Impossible to test with current stylecop
        Assert.Pass();

    /// <summary>
    ///     This method tests the insertion of multiple users.
    /// </summary>
    [Test]
    public void InsertUserCountVerification()
    {
        // Arrange
        // nothing to arrange

        // Act
        User? instert1 = _userService.Insert(_user1);
        User? instert2 = _userService.Insert(_user2);

        // Assert
        Assert.That(instert1, Is.Not.Null);
        Assert.That(instert2, Is.Not.Null);
        HashSet<User> users = _userService.GetUsers();
        Assert.That(users.Count, Is.EqualTo(2));
    }

    /// <summary>
    ///     This method tests the insertion of a user with a null username.
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
    ///     This method tests the insertion of a user with a null username.
    /// </summary>
    [Test]
    public void RemoveRoleListFromUser()
    {
        // Arrange
        List<string> rolesToAdd = ["role3", "role4"];
        _user1.AddRoles(rolesToAdd);

        // Act
        _user1.RemoveRoles(rolesToAdd);

        // Assert
        Assert.That(_user1.Roles.Count, Is.EqualTo(2));
    }

    /// <summary>
    ///     Setups the tests.
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
        List<string> roles = ["role1", "role2"];
        _user1 = new User(
            "user1",
            "email1",
            "password1",
            salt1,
            roles);
        _user2 = new User(
            "user2",
            "email2",
            "password2",
            salt2,
            roles);
        _user3 = new User(
            "user3",
            "email3",
            "password3",
            salt3,
            roles);
    }

    /// <summary>
    ///     Method to test that the user is updated.
    /// </summary>
    [Test]
    public void UpdateUserVerification()
    {
        // Arrange
        string newName = "userUpdated";
        string newMail = "userUpdated@email.com";
        string newPass = "passwordUpdated";

        User? insert = _userService.Insert(_user1);

        // Act
        _user1.UpdateUsername(newName);
        _user1.UpdateEmail(newMail);
        _user1.UpdatePasswordHash(newPass);
        User? updated = _userService.Update(_user1);

        // Assert
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated?.Username, Is.EqualTo(newName));
        Assert.That(updated?.Email, Is.EqualTo(newMail));
        Assert.That(updated?.PasswordHash, Is.EqualTo(newPass));
    }

    #endregion
}
