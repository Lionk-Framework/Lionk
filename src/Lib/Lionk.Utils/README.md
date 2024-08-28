# Lionk Configuration Utilities Library

## Overview

The Lionk Configuration Utilities Library provides a set of utility methods to manage file operations within a defined directory structure for applications. This library is designed to simplify file management tasks such as reading, writing, copying, and deleting files across various designated folders such as configuration, logs, data, and temporary directories. It ensures that these directories are created and managed consistently across the application lifecycle.

## Features

- **File Management**: Perform common file operations like saving, reading, copying, appending, and deleting files.
- **Directory Management**: Automatically ensures required directories exist, using predefined folder types.
- **Asynchronous Operations**: Support for asynchronous file saving to improve application responsiveness.
- **Error Handling**: Built-in error handling for safe file deletions.

## Installation

To install the Lionk Configuration Utilities Library, use the NuGet package manager console:

```bash
Install-Package Lionk.Utils
```

## Getting Started

### 1. Folder Types

The library uses the `FolderType` enumeration to identify various folders used by the application. These include:

- **Config**: Stores configuration files.
- **Logs**: Stores log files.
- **Data**: Stores data files.
- **Plugin**: Stores plugin-related files.
- **Temp**: Stores temporary files.

These folders are automatically created at the application's base directory upon the first use of the library.

### 2. Setting Up ConfigurationUtils

The `ConfigurationUtils` class provides static methods for managing files within the predefined folder structure. It uses a dictionary to map each `FolderType` to its corresponding path within the application’s base directory.

#### Example of Initialization

The static constructor ensures that all required directories are created if they do not already exist:

```csharp
using Lionk.Utils;

// Directories are created when the class is first accessed
ConfigurationUtils.AppendFile("example.txt", "Hello, World!", FolderType.Config);
```

### 3. Managing Files

#### Saving a File

You can save content to a file using the `SaveFile` method. If the file already exists, it will be overwritten:

```csharp
ConfigurationUtils.SaveFile("settings.json", "{ 'setting': 'value' }", FolderType.Config);
```

For asynchronous saving, use:

```csharp
await ConfigurationUtils.SaveFileAsync("settings.json", "{ 'setting': 'value' }", FolderType.Config);
```

#### Reading a File

To read the contents of a file, use the `ReadFile` method. If the file does not exist, an empty string is returned:

```csharp
string configContent = ConfigurationUtils.ReadFile("settings.json", FolderType.Config);
Console.WriteLine(configContent);
```

#### Copying a File

Copy a file to one of the predefined folders using the `CopyFileToFolder` method. This method checks if the destination file already exists before attempting to copy:

```csharp
ConfigurationUtils.CopyFileToFolder("sourceFile.txt", FolderType.Data);
```

#### Deleting a File

You can delete a file with the `DeleteFile` method. To safely attempt deletion, use `TryDeleteFile`, which returns a boolean indicating success:

```csharp
ConfigurationUtils.DeleteFile("old-config.json", FolderType.Config);

// Attempting safe deletion
bool wasDeleted = ConfigurationUtils.TryDeleteFile("temp-file.txt", FolderType.Temp);
Console.WriteLine(wasDeleted ? "File deleted." : "Failed to delete file.");
```

#### Checking if a File Exists

Check for the existence of a file using `FileExists`:

```csharp
bool fileExists = ConfigurationUtils.FileExists("log.txt", FolderType.Logs);
Console.WriteLine(fileExists ? "File exists." : "File does not exist.");
```

#### Appending Content to a File

To append content to an existing file or create it if it doesn't exist, use the `AppendFile` method:

```csharp
ConfigurationUtils.AppendFile("log.txt", "New log entry", FolderType.Logs);
```

### 4. Directory Management with FileHelper

The library includes the `FileHelper` class, which provides utility methods for managing directories, such as ensuring directories exist:

```csharp
using Lionk.Utils;

FileHelper.CreateDirectoryIfNotExists(ConfigurationUtils.GetFolderPath(FolderType.Temp));
```

## Best Practices

- **Error Handling**: Use `TryDeleteFile` for operations where file deletions are not critical to avoid exceptions.
- **Asynchronous Methods**: Prefer asynchronous file operations (`SaveFileAsync`) in scenarios where file I/O might block the main thread or affect application performance.

## Security Considerations

- **File Path Validation**: Always validate file paths to prevent security vulnerabilities such as path traversal attacks.
- **Access Permissions**: Ensure the application has appropriate permissions for the file system to perform the required operations in all specified directories.

## Contribution

Contributions are welcome! Please fork the repository, create a branch, and submit a pull request with your improvements or bug fixes. For detailed contribution guidelines, refer to the project’s repository.