// Copyright © 2024 Lionk Project
using Serilog.Events;
using Serilog.Parsing;

namespace Notifications.Classes;

/// <summary>
/// This class represents a notification log.
/// </summary>
public class NotificationLog
{
    /// <summary>
    /// Gets or sets the notification log timestamp.
    /// </summary>
    public TimeSpan TimeSpan { get; set; }

    /// <summary>
    /// Gets or sets the notification log level.
    /// </summary>
    public LogEventLevel Level { get; set; }

    /// <summary>
    /// Gets or sets the notification log message.
    /// </summary>
    public MessageTemplateParser? Message { get; set; }

    /// <summary>
    /// Gets or sets the notification log.
    /// </summary>
    public Notification? Notification { get; set; }
}
