// Copyright © 2024 Lionk Project
using Notifications.Enums;

namespace Notifications.Classes;

/// <summary>
/// This class represents a notification that can be displayed to the user.
/// </summary>
public class Notification
{
    /// <summary>
    /// Gets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the timestamp when the notification was created.
    /// </summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Gets the level of the notification.
    /// </summary>
    public NotificationLevel Level { get; private set; }

    /// <summary>
    /// Gets the title of the notification.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the message of the notification.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    /// <param name="level">The level of the notification.</param>
    public Notification(string title, string message, NotificationLevel level)
    {
        Level = Level;
        Timestamp = DateTime.UtcNow;
        Title = title;
        Message = message;
    }
}
