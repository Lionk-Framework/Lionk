# User Authentication library for Blazor Server

To use this library, add the following NuGet package to your Blazor Server project:

```bash
dotnet add package Lionk.Auth
dotnet add package Lionk.Auth.Razor
```

It's simple to use, just add a `List<string> roles` when you call the `user` constructor.

```csharp
var user = new User("MyUsername", "MyPasswordHash", "My@email.com","MySalt", new List<string> { "admin" });
```

Then, you can use the `AuthorizeView` component to show content based on the user's role. 

```html
<AuthorizeView Roles="admin, super admin">
    <Authorized>
        <p>Admin content</p>
    </Authorized>
    <NotAuthorized>
        <p>Not authorized</p>
    </NotAuthorized>
</AuthorizeView>
```

Now, if you are authenticated, you can access the authorized view.

## `UserServiceRazor` Class

The `UserServiceRazor` class is designed to manage user data in the context of a Blazor application. It interacts with
the browser's protected local storage to persist user information securely.

- **PersistUserToBrowserAsync(User user)**: Saves the user's data to the browser's local storage.
- **FetchUserFromBrowserAsync()**: Retrieves the user data stored in the browser, if available.
- **ClearBrowserUserDataAsync()**: Clears the user data from the browser's local storage.

## `UserAuthenticationStateProvider` Class

The `UserAuthenticationStateProvider` class extends `AuthenticationStateProvider` to manage the authentication state of
users in a Blazor application. It uses `UserServiceRazor` to persist and retrieve user authentication data.

- **LoginAsync(string username, string passwordHash)**: Authenticates the user and updates the authentication state.
- **LogoutAsync()**: Logs the user out and clears their authentication data from the browser.
- **GetAuthenticationStateAsync()**: Retrieves the current authentication state, checking both the browser storage and the database.

## Data Persistence

The `UserServiceRazor` class is responsible for data persistence by storing user information in the browser's local storage.
This allows for maintaining user sessions across page reloads or browser restarts. The `UserAuthenticationStateProvider`
relies on this service to keep the user's authentication state consistent across the application.