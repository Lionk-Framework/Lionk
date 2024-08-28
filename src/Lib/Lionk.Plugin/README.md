# Lionk Plugin Library

## Overview

The Lionk Plugin Library provides a comprehensive framework for managing plugins within your applications. It enables dynamic loading, managing dependencies, and controlling plugin lifecycles with ease. The library is designed to enhance application modularity by allowing plugins to be added, removed, or updated without modifying the core application. It supports file-based plugin management using .NET assemblies, offering flexibility for both developers and end-users.

## Features

- **Plugin Management**: Load, manage, and remove plugins dynamically with the `PluginManager`.
- **Dependency Handling**: Automatically manage dependencies between plugins and their assemblies.
- **Plugin Metadata**: Access plugin metadata including name, version, author, and description.
- **Persistence**: Maintain plugin paths and states across application restarts using JSON configuration files.
- **Type Discovery**: Integrate with the application’s type system, providing new types and services as plugins are loaded.

## Installation

To install the Lionk Plugin Library, use the NuGet package manager console:

```bash
Install-Package Lionk.Plugin
```

## Getting Started

### 1. Setting Up the PluginManager

The core of the library is the `PluginManager` class, which handles all plugin-related operations. To set up the `PluginManager` in your application, create an instance of it and initialize it as needed:

```csharp
using Lionk.Plugin;

// Initialize PluginManager
var pluginManager = new PluginManager();

// Optionally, subscribe to new types available event
pluginManager.NewTypesAvailable += (sender, args) => 
{
    Console.WriteLine("New types available:");
    foreach (var type in args.Types)
    {
        Console.WriteLine(type.FullName);
    }
};
```

### 2. Managing Plugins

With the `PluginManager` initialized, you can start managing plugins within your application. Here are some common operations:

#### Adding a Plugin

To add a new plugin, use the `AddPlugin` method and provide the path to the plugin assembly (DLL). The method will validate the plugin, load it, and manage its dependencies.

```csharp
pluginManager.AddPlugin("path/to/your/plugin.dll");
```

#### Retrieving Loaded Plugins

You can retrieve all currently loaded plugins using the `GetAllPlugins` method, which returns a collection of `Plugin` objects containing metadata and status information:

```csharp
foreach (var plugin in pluginManager.GetAllPlugins())
{
    Console.WriteLine($"Loaded Plugin: {plugin.Name}, Version: {plugin.Version}, Author: {plugin.Author}");
}
```

#### Checking If Restart is Needed

Some plugin operations might require an application restart, such as removing a plugin. Use the `DoNeedARestart` method to check if a restart is necessary:

```csharp
if (pluginManager.DoNeedARestart())
{
    Console.WriteLine("A restart is required for changes to take effect.");
}
```

#### Removing a Plugin

To remove a plugin, use the `RemovePlugin` method with the specific `Plugin` object you want to remove. Note that the actual removal will take effect only after the application restarts:

```csharp
var pluginToRemove = pluginManager.GetAllPlugins().FirstOrDefault(p => p.Name == "PluginName");
if (pluginToRemove != null)
{
    pluginManager.RemovePlugin(pluginToRemove);
    Console.WriteLine("Plugin scheduled for removal. A restart is required.");
}
```

### 3. Handling Plugin Dependencies

The `PluginManager` automatically handles dependencies between plugins. When loading a plugin, it checks for any required assemblies and attempts to load them. Dependencies are tracked using the `Dependency` class, which stores information about each required assembly.

### Example Usage

Here's an example demonstrating how to add, list, and remove plugins:

```csharp
using Lionk.Plugin;

// Initialize the PluginManager
var pluginManager = new PluginManager();

// Add a plugin
pluginManager.AddPlugin("path/to/plugin.dll");

// List all plugins
Console.WriteLine("Loaded Plugins:");
foreach (var plugin in pluginManager.GetAllPlugins())
{
    Console.WriteLine($"Name: {plugin.Name}, Version: {plugin.Version}, Loaded: {plugin.IsLoaded}");
}

// Remove a plugin (requires restart to take effect)
var plugin = pluginManager.GetAllPlugins().FirstOrDefault(p => p.Name == "SomePlugin");
if (plugin != null)
{
    pluginManager.RemovePlugin(plugin);
    Console.WriteLine("Plugin removed. Restart required to complete the removal.");
}

// Check if restart is required
if (pluginManager.DoNeedARestart())
{
    Console.WriteLine("Restart is needed to apply changes.");
}
```

## Plugin Structure and Metadata

Each plugin is represented by the `Plugin` class, which encapsulates the following properties:

- **Assembly**: The assembly associated with the plugin.
- **Name**: The name of the plugin, extracted from the assembly’s metadata.
- **Version**: The version of the plugin.
- **Author**: The author of the plugin.
- **Description**: A brief description of the plugin.
- **Dependencies**: A list of dependencies required by the plugin, managed through the `Dependency` class.

## Extending Plugin Functionality

To extend the plugin management functionality, you can implement custom logic for loading plugins, handling dependencies, or managing plugin configurations by subclassing or modifying the `PluginManager`.

### Custom Plugin Manager Example

```csharp
using Lionk.Plugin;

public class CustomPluginManager : PluginManager
{
    // Override methods or add new functionalities here
    protected override void LoadPlugin(string path)
    {
        // Custom loading logic
        base.LoadPlugin(path);
    }
}
```

## Security Considerations

- **Plugin Validation**: Always validate plugins before loading to ensure they are from trusted sources.
- **Dependency Management**: Handle dependencies carefully to avoid version conflicts and security vulnerabilities.
- **Restart Requirements**: Be aware that some changes require an application restart to fully apply, especially when dealing with assembly loading and unloading.

## Contribution

Contributions to the Lionk Plugin Library are welcome! Please fork the repository, create a branch, and submit a pull request with your enhancements. For detailed contribution guidelines, please refer to the project’s repository.
