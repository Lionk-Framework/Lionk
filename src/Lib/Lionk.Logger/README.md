# Lionk Logger Library

## Overview

The Lionk Logger Library provides a robust and flexible logging framework designed to integrate seamlessly into your applications. It offers a standardized approach to logging with varying levels of severity, making it easier to track application behavior, debug issues, and maintain detailed logs. The library supports multiple logging backends and includes a default implementation using Serilog for file-based logging.

## Features

- **Logger Factory**: Easily create loggers with the `ILoggerFactory` interface, allowing for customized logger implementations.
- **Standard Logging Interface**: Use the `IStandardLogger` interface for consistent logging across different loggers and applications.
- **Configurable Logging**: Configure the logging behavior using `LogService`, including defining log levels and output destinations.
- **Severity Levels**: Support for multiple log severity levels, including Trace, Debug, Information, Warning, Error, and Critical.
- **Serilog Integration**: Default implementation with Serilog for robust, file-based logging with daily rolling logs.

## Installation

To install the Lionk Logger Library, use the NuGet package manager console:

```bash
dotnet add package Lionk.Logger
```

## Getting Started

### 1. Setting Up the LogService

The core of the library is the `LogService` class, which manages loggers and provides a straightforward API for logging messages. To set up logging in your application, configure the logger factory in your `Startup.cs` or `Program.cs` file:

```csharp
using Lionk.Log;
using Lionk.Log.Serilog;

// Configure the logger factory
var loggerFactory = new SerilogFactory();
LogService.Configure(loggerFactory);
```

### 2. Creating and Using Loggers

With `LogService` configured, you can create loggers and start logging messages immediately. Here are some common operations:

#### Creating a Logger

To create a new logger, use the `CreateLogger` method provided by `LogService`. This method allows you to specify a logger name, which can help categorize logs.

```csharp
using Lionk.Log;

// Create a logger for a specific category
var logger = LogService.CreateLogger("MyAppLogger");

if (logger != null)
{
    logger.Log(LogSeverity.Information, "Logger created successfully.");
}
else
{
    Console.WriteLine("Failed to create logger.");
}
```

#### Logging Messages

You can log messages at various severity levels using the `Log` method of the `IStandardLogger` interface:

```csharp
logger?.Log(LogSeverity.Debug, "This is a debug message.");
logger?.Log(LogSeverity.Information, "This is an info message.");
logger?.Log(LogSeverity.Warning, "This is a warning message.");
logger?.Log(LogSeverity.Error, "This is an error message.");
logger?.Log(LogSeverity.Critical, "This is a critical message.");
```

#### Logging Application and Debug Messages

The `LogService` also provides specific methods for logging application and debug messages:

```csharp
LogService.LogApp(LogSeverity.Warning, "This is a warning related to the application.");
LogService.LogDebug("This is a debug message specific to debugging.");
```

### 3. Configuring Serilog Logging

By default, the library uses Serilog for file-based logging. You can customize Serilog configuration by modifying the `SerilogFactory` class or extending it as needed.

```csharp
using Lionk.Log.Serilog;

// Create a custom logger factory if needed
var customLoggerFactory = new SerilogFactory();
LogService.Configure(customLoggerFactory);

// Log some messages
LogService.LogApp(LogSeverity.Information, "Application has started.");
LogService.LogDebug("Debugging mode is enabled.");
```

## Extending the Logger

The Lionk Logger Library is designed to be easily extensible. You can create custom loggers by implementing the `ILoggerFactory` and `IStandardLogger` interfaces:

### Implementing a Custom Logger

```csharp
using Lionk.Log;

public class CustomLoggerFactory : ILoggerFactory
{
    public IStandardLogger CreateLogger(string loggerName)
    {
        // Implement custom logger creation logic
        return new CustomLogger();
    }
}

public class CustomLogger : IStandardLogger
{
    public void Dispose()
    {
        // Clean up resources
    }

    public void Log(LogSeverity severity, string message)
    {
        // Implement custom logging logic
        Console.WriteLine($"{severity}: {message}");
    }
}
```

### Registering Your Custom Logger

Replace the default Serilog factory with your custom logger factory:

```csharp
// Use the custom logger factory
var customLoggerFactory = new CustomLoggerFactory();
LogService.Configure(customLoggerFactory);

// Now LogService will use the custom logger
LogService.LogApp(LogSeverity.Information, "Using custom logger now.");
```

## Log Severity Levels

The library supports multiple log severity levels to categorize log messages effectively:

- **Trace**: Detailed information, typically of interest only when diagnosing problems.
- **Debug**: Information useful to developers for debugging the application.
- **Information**: Informational messages that highlight the progress of the application.
- **Warning**: Potentially harmful situations of interest to end users or system managers.
- **Error**: Error events of more considerable significance that might still allow the application to continue running.
- **Critical**: Severe error events that will presumably lead the application to abort.

## Security Considerations

- **Log Sanitization**: Ensure that sensitive information, such as personal data or authentication details, is not logged.
- **Log Access**: Restrict access to log files to maintain confidentiality and integrity.

## Contribution

Contributions are welcome! Please fork the repository, create a branch, and submit a pull request with your enhancements.