# Lionk Authentication and Identity Library

## Overview

The Lionk Authentication and Identity Library provides a comprehensive set of tools for managing user authentication and identity in applications. This library is designed to offer flexible and secure user management, including features for creating, updating, retrieving, and deleting user records, as well as handling password security through hashing and salting mechanisms. The library leverages JSON file-based persistence but can be easily extended to support more robust storage solutions such as databases.

## Features

- **User Management**: Create, update, retrieve, and delete users with the `UserService`.
- **Password Security**: Securely hash passwords and generate salt values using `HMACSHA256`.
- **Flexible Data Persistence**: Use JSON files for storage with the `UserFileHandler`, which can be replaced by other data handlers without affecting the rest of the application.
- **Role Management**: Support for user roles, including assigning and managing roles per user.
- **Claims-Based Authentication**: Convert users to and from `ClaimsPrincipal` for integration with .NET identity and authentication systems.

## Installation

To install the Lionk Authentication and Identity Library, use the NuGet package manager console:

```bash
Install-Package Lionk.Auth
```

## Getting Started

### 1. Setting Up the UserService

The core of the library is the `UserService` class, which manages all operations related to users. To set up the service in your application, you need to register the required services in the `Startup.cs` or `Program.cs` file of your application:

```csharp
using Lionk.Auth.Abstraction;
using Lionk.Auth.Identity;

// Register services in the container
services.AddSingleton<IUserRepository, UserFileHandler>();
services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>()));
```

### 2. Managing Users

With the `UserService` registered, you can now manage users within your application. Below are some common operations:

#### Creating a User

To create a new user, first, hash the password using the `PasswordUtils` class, then call the `Insert` method of the `UserService`:

```csharp
using Lionk.Auth.Identity;
using Lionk.Auth.Utils;

var salt = PasswordUtils.GenerateSalt(32);
var passwordHash = PasswordUtils.HashPassword("yourPassword", salt);
var user = new User("username", "user@example.com", passwordHash, salt, new List<string> { "User" });

var userService = serviceProvider.GetRequiredService<IUserService>();
var insertedUser = userService.Insert(user);

if (insertedUser == null)
{
    Console.WriteLine("User could not be created. The username or email might already exist.");
}
else
{
    Console.WriteLine("User created successfully.");
}
```

#### Retrieving a User

To retrieve a user by their username or email:

```csharp
var user = userService.GetUserByUsername("username");

if (user != null)
{
    Console.WriteLine($"User found: {user.Username}");
}
else
{
    Console.WriteLine("User not found.");
}
```

#### Updating a User

Updating a user's details, such as their email or password, can be done by first retrieving the user, modifying the properties, and then calling the `Update` method:

```csharp
var user = userService.GetUserById(userId);

if (user != null)
{
    user.UpdateEmail("newemail@example.com");
    userService.Update(user);
    Console.WriteLine("User updated successfully.");
}
else
{
    Console.WriteLine("User not found.");
}
```

#### Deleting a User

To delete a user, simply call the `Delete` method with the user's ID:

```csharp
var user = userService.GetUserByUsername("username");
if (userService.Delete(user))
{
    Console.WriteLine("User deleted successfully.");
}
else
{
    Console.WriteLine("User deletion failed. User might not exist.");
}
```

## Extending Data Persistence

By default, the library uses `UserFileHandler` for JSON file-based storage, but you can implement your own data handler by implementing the `IUserRepository` interface:

```csharp
public class CustomUserRepository : IUserRepository
{
    public void DeleteUser(User id) { /* Custom implementation */ }
    public HashSet<User> GetUsers() { /* Custom implementation */ }
    public void SaveUser(User user) { /* Custom implementation */ }
    public void UpdateUser(User user) { /* Custom implementation */ }
}
```

Register your custom repository in the service container:

```csharp
services.AddSingleton<IUserRepository, CustomUserRepository>();
services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>()));
```

## Security Considerations

- **Password Hashing**: The library uses `HMACSHA256` for password hashing, combined with a unique salt per user to enhance security against rainbow table attacks.
- **Role Management**: Roles are managed within the user object, allowing easy assignment and validation of permissions.

## Contribution

Contributions are welcome! Please fork the repository, create a branch, and submit a pull request with your enhancements.