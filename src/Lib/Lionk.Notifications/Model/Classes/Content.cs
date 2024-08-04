// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
/// This class represents the content of a notification.
/// </summary>
public class Content
{
    /// <summary>
    /// Gets the severity level of the notification.
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
    /// Initializes a new instance of the <see cref="Content"/> class.
    /// </summary>
    /// <param name="level"> The severity level of the notification.</param>
    /// <param name="title"> The title of the notification.</param>
    /// <param name="message"> The message of the notification.</param>
    public Content(Severity level, string title, string message)
    {
        Level = level;
        Title = title;
        Message = message;
    }
}
