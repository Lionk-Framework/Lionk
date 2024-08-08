// Copyright © 2024 Lionk Project

using System.Security.Cryptography;
using System.Text;

namespace Lionk.Auth.Utils;

/// <summary>
/// This class contains methods to manage users authentication.
/// </summary>
public static class PasswordUtils
{
    /// <summary>
    /// Method to generate a salt with a specific size.
    /// </summary>
    /// <param name="size"> The size of the salt to generate.</param>
    /// <returns> The generated salt.</returns>
    public static string GenerateSalt(int size)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] buffer = new byte[size];
        rng.GetBytes(buffer);
        return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// Method to hash a password with a specific salt.
    /// </summary>
    /// <param name="password"> The password to hash.</param>
    /// <param name="salt"> The salt to use to hash the password.</param>
    /// <returns> The hashed password.</returns>
    public static string HashPassword(string password, string salt)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = hmac.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
