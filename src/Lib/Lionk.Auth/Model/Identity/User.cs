// Copyright © 2024 Lionk Project

using System.Security.Claims;

namespace Lionk.Auth.Identity;

/// <summary>
/// This class represents a user.
/// </summary>
public class User
{
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
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="userName"> The username of the user.</param>
    /// <param name="email"> The email of the user.</param>
    /// <param name="passwordHash"> The password hash of the user.</param>
    /// <param name="salt"> The salt used to hash the password.</param>
    public User(string userName, string email, string passwordHash, string salt)
    {
        Id = Guid.NewGuid();
        Username = userName;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
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

        User user = new(
            userName,
            email,
            passwordHash,
            salt);

        return user;
    }

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

        Claim[] claims = [userNameClaim, emailClaim, passwordHashClaim, saltClaim];

        identity.AddClaims(claims);
        return new ClaimsPrincipal(identity);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is User user && user.Id == Id;
}
