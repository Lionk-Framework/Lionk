// Copyright © 2024 Lionk Project
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Notifications.Classes;

/// <summary>
/// This class is used to log notifications and create a history of them.
/// </summary>
public static class NotificationLogger
{
    private const string MESSAGE = "Notification sent";

    /// <summary>
    /// The lock object.
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// Gets the history file path.
    /// </summary>
    public static string HistoryFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notifications", "history.log");

    /// <summary>
    /// Gets the notification logger.
    /// </summary>
    private static Logger? _logger;

    /// <summary>
    /// Initializes static members of the <see cref="NotificationLogger"/> class.
    /// </summary>
    static NotificationLogger()
    {
        // Check if the Notifications directory exists, if not create it.
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notifications")))
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notifications"));

        // Initialize the logger.
        InitializeLogger();
    }

    /// <summary>
    /// Method that initialize the logger.
    /// </summary>
    /// <exception cref="InvalidOperationException"> If the logger is not correctly initialized.</exception>
    private static void InitializeLogger()
    {
        NotificationFormatter formatter = new();

        // Initialize the logger and verify if it's not null.
        _logger = new LoggerConfiguration()
            .WriteTo.File(formatter, HistoryFilePath, encoding: System.Text.Encoding.UTF8)
            .CreateLogger() ?? throw new InvalidOperationException("The logger is not correctly initialized.");
    }

    /// <summary>
    /// This method close the logger.
    /// </summary>
    public static void CloseLogger() => Log.CloseAndFlush();

    /// <summary>
    /// Method that log a notification.
    /// </summary>
    /// <param name="notification">The notification to log.</param>
    public static void LogNotification(Notification notification)
    {
        if (_logger is null) throw new InvalidOperationException("The logger must be initialized before logging a notification.");

        LogEventLevel level = notification.Content.Level;
        _logger.ForContext("Notification", notification, destructureObjects: true)
               .Write(level, MESSAGE);
    }
}
