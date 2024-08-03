// Copyright © 2024 Lionk Project

// Copyright © 2024 Lionk Project
using Notifications.Model.Enums;

namespace Notifications.Model.Classes;

/// <summary>
/// This class define a notification content.
/// </summary>
public class NotificationContent
{
    /// <summary>
    /// Gets the level of the notification.
    /// </summary>
    public Severity Level { get; private set; }

    /// <summary>
    /// Gets the title of the notification.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the message of the notification.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationContent"/> class with the specified level, title, and message.
    /// </summary>
    /// <param name="level">The level of the notification.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    public NotificationContent(Severity level, string title, string message)
    {
        Level = level;
        Title = title;
        Message = message;
    }
}
