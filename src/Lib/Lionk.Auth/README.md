# Authentication and Identity

## `User` class
The `User` class represents a user in the system. It contains properties for the user's ID, username, password, and email address.

## `UserService` class
The `UserService` class provides high-level methods for managing users, such as retrieving, inserting, updating, and deleting users. It leverages the UserFileHandler for data persistence, which stores user information in JSON files
The `UserService` class also includes methods for validating user input.

## `PasswordUtils` class
This static class provides utility methods for hashing passwords and generating salt values. It uses the `System.Security.Cryptography` namespace to generate salt values and use `HMACSHA256` to securely hash passwords.

## Data Persistence
The UserFileHandler class has been developed to handle data persistence through JSON files.
However, it can be easily replaced or extended by a more advanced service within the `UserService` if needed.
This design allows for flexibility in how user data is managed and stored. If a more robust or scalable storage solution is required,
such as a database, the `UserService` can be adapted to use a different data handler while keeping the rest of the application code unchanged.

## `UserFileHandler` class
The UserFileHandler is specifically designed for handling user data through file-based storage, but it can be easily replaced with a more advanced solution,
like a database, within the `UserService`. This design ensures that the application can scale or adapt to different storage needs without significant changes to the overall codebase.
