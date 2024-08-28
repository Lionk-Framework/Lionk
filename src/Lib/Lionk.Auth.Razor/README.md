# Lionk User Authentication Library for Blazor Server

## Overview

The Lionk User Authentication Library provides a robust and flexible authentication solution for Blazor Server applications. This library allows developers to manage user authentication, handle roles, and persist user data securely within the browser. With easy-to-use components and services, it integrates seamlessly into Blazor applications, supporting both basic authentication and role-based access control.

## Features

- **User Management**: Create, update, retrieve, and delete users using `UserService`.
- **Role-Based Access Control**: Display content conditionally based on user roles using Blazor's `AuthorizeView` component.
- **Persistent Authentication State**: Maintain user sessions across page reloads or browser restarts using local storage with `UserServiceRazor`.
- **Secure Data Handling**: Use hashing and salting techniques for password security.
- **Extensible**: Easily extend or replace storage mechanisms for user data persistence.

## Installation

To use this library, add the following NuGet packages to your Blazor Server project:

```bash
dotnet add package Lionk.Auth
dotnet add package Lionk.Auth.Razor
```

## Getting Started

### 1. Setting Up the Services

Register the necessary services in your `Startup.cs` or `Program.cs` file to integrate the authentication library:

```csharp
using Lionk.Auth.Abstraction;
using Lionk.Auth.Identity;

public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorComponents().AddInteractiveServerComponents();
    services.AddMudServices();

    // Register the authentication services
    services.AddScoped<UserService>();
    services.AddScoped<UserServiceRazor>();
    services.AddSingleton<IUserRepository, UserFileHandler>();
    services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>()));
    services.AddScoped<UserAuthenticationStateProvider>();
    services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserAuthenticationStateProvider>());
}
```

### 2. Creating a User

To create a user, instantiate the `User` class with the necessary properties, including roles, and then use the `UserService` to insert the user:

```csharp
using Lionk.Auth.Identity;
using Lionk.Auth.Utils;

var salt = PasswordUtils.GenerateSalt(32);
var passwordHash = PasswordUtils.HashPassword("MyPassword", salt);
var user = new User("MyUsername", "myemail@example.com", passwordHash, salt, new List<string> { "admin" });

var userService = serviceProvider.GetRequiredService<IUserService>();
var result = userService.Insert(user);

if (result == null)
{
    Console.WriteLine("User creation failed: Username or email already exists.");
}
else
{
    Console.WriteLine("User created successfully.");
}
```

### 3. Using Role-Based Authorization

Use the `AuthorizeView` component to control access to specific parts of your application based on user roles:

```razor
<AuthorizeView Roles="admin, super admin">
    <Authorized>
        <p>Admin content visible only to users with 'admin' or 'super admin' roles.</p>
    </Authorized>
    <NotAuthorized>
        <p>You do not have permission to view this content.</p>
    </NotAuthorized>
</AuthorizeView>
```

### 4. Managing User Sessions

The `UserServiceRazor` class handles user data persistence within the browser's local storage, ensuring the user's authentication state is preserved across sessions:

- **Persist User to Browser**: Saves the user's data securely in the browser's local storage.
- **Fetch User from Browser**: Retrieves the user's data from the local storage.
- **Clear User Data**: Clears the user's data from the browser, typically used during logout.

### 5. Authentication State Management

The `UserAuthenticationStateProvider` class extends Blazor's `AuthenticationStateProvider` to manage the authentication state of users within the application. It interacts with `UserServiceRazor` for handling user data persistence in the browser.

#### Example: Logging In a User

```csharp
var authProvider = serviceProvider.GetRequiredService<UserAuthenticationStateProvider>();
await authProvider.LoginAsync("MyUsername", "MyPasswordHash");
```

#### Example: Logging Out a User

```csharp
await authProvider.LogoutAsync();
```

## Security Considerations

- **Password Security**: The library uses HMACSHA256 for hashing passwords with unique salts per user, providing a secure method of storing credentials.
- **Protected Local Storage**: User data is stored in the browser's protected local storage, offering security enhancements compared to regular local storage.

## Extending Data Persistence

By default, user data is stored in JSON files using `UserFileHandler`. To use a different storage mechanism, implement the `IUserRepository` interface and register your implementation:

```csharp
public class CustomUserRepository : IUserRepository
{
    // Implement methods for Save, Update, Delete, and Get users
}

services.AddSingleton<IUserRepository, CustomUserRepository>();
```

## Contribution

Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request with your changes.