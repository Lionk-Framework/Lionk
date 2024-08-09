// Copyright © 2024 Lionk Project

using System.Security.Claims;
using Newtonsoft.Json;

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a user.
/// </summary>
public class User
{
    private readonly HashSet<string> _roles = new();

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// Gets the email of the user.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets the password hash of the user.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Gets the salt used to hash the password.
    /// </summary>
    public string Salt { get; private set; }

    /// <summary>
    /// Gets the roles of the user.
    /// </summary>
    public IReadOnlyCollection<string> Roles => _roles;

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="userName"> The username of the user.</param>
    /// <param name="email"> The email of the user.</param>
    /// <param name="passwordHash"> The password hash of the user.</param>
    /// <param name="salt"> The salt used to hash the password.</param>
    /// <param name="roles"> The roles of the user.</param>
    public User(string userName, string email, string passwordHash, string salt, List<string> roles)
    {
        Id = Guid.NewGuid();
        Username = userName;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        _roles = roles.ToHashSet();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id"> The unique identifier of the user.</param>
    /// <param name="userName"> The username of the user.</param>
    /// <param name="email"> The email of the user.</param>
    /// <param name="passwordHash"> The password hash of the user.</param>
    /// <param name="salt"> The salt used to hash the password.</param>
    /// <param name="roles"> The roles of the user.</param>
    [JsonConstructor]
    public User(Guid id, string userName, string email, string passwordHash, string salt, List<string> roles)
    {
        Id = id;
        Username = userName;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        _roles = roles.ToHashSet();
    }

    /// <summary>
    /// Method to convert a ClaimsPrincipal to a user.
    /// </summary>
    /// <param name="principal"> The ClaimsPrincipal to convert.</param>
    /// <returns> The user from the ClaimsPrincipal.</returns>
    public static User FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        string userName = principal.FindFirst(ClaimTypes.Name)?.Value ?? throw new ArgumentException("The ClaimsPrincipal does not contain a username.");
        string email = principal.FindFirst(ClaimTypes.Email)?.Value ?? throw new ArgumentException("The ClaimsPrincipal does not contain an email.");
        string passwordHash = principal.FindFirst(ClaimTypes.Hash)?.Value ?? throw new ArgumentException("The ClaimsPrincipal does not contain a password hash.");
        string salt = principal.FindFirst("string")?.Value ?? throw new ArgumentException("The ClaimsPrincipal does not contain a salt.");
        var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        User user = new(
            userName,
            email,
            passwordHash,
            salt,
            roles);

        return user;
    }

    /// <summary>
    /// Method to update the username of the user.
    /// </summary>
    /// <param name="newUsername"> The new username.</param>
    /// <exception cref="ArgumentNullException"> If the new username is null.</exception>
    public void UpdateUsername(string newUsername) => Username = newUsername ?? throw new ArgumentNullException(nameof(newUsername));

    /// <summary>
    /// Method to update the email of the user.
    /// </summary>
    /// <param name="newEmail"> The new email.</param>
    /// <exception cref="ArgumentNullException"> If the new email is null.</exception>
    public void UpdateEmail(string newEmail) => Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));

    /// <summary>
    /// Method to update the password hash of the user.
    /// </summary>
    /// <param name="newPasswordHash"> The new password hash.</param>
    /// <exception cref="ArgumentNullException"> If the new password hash is null.</exception>
    public void UpdatePasswordHash(string newPasswordHash) => PasswordHash = newPasswordHash ?? throw new ArgumentNullException(nameof(newPasswordHash));

    /// <summary>
    /// Method to convert the user to a ClaimsPrincipal.
    /// </summary>
    /// <returns> The user as a ClaimsPrincipal.</returns>
    public ClaimsPrincipal ToClaimsPrincipal()
    {
        ClaimsIdentity identity = new("UserClaim");

        Claim userNameClaim = new(ClaimTypes.Name, Username);
        Claim emailClaim = new(ClaimTypes.Email, Email);
        Claim passwordHashClaim = new(ClaimTypes.Hash, PasswordHash);
        Claim saltClaim = new("string", Salt);
        Claim rolesClaim = new(ClaimTypes.Role, string.Join(',', _roles));

        Claim[] claims = [userNameClaim, emailClaim, passwordHashClaim, saltClaim, rolesClaim];

        identity.AddClaims(claims);
        return new ClaimsPrincipal(identity);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is User user && user.Id == Id;
}
