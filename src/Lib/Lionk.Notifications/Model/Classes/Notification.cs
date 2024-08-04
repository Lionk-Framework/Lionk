// Copyright © 2024 Lionk Project
using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
/// This class represents a notification that can be displayed to the user.
/// </summary>
public class Notification
{
    /// <summary>
    /// Gets a value indicating whether the notification is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the timestamp when the notification was created.
    /// </summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Gets the content of the notification.
    /// </summary>
    public Content Content { get; private set; }

    /// <summary>
    /// Gets the notifyer that sent the notification.
    /// </summary>
    public INotifyer Notifyer { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="content"> The content of the notification.</param>
    /// <param name="notifyer"> The notifyer that sent the notification.</param>
    public Notification(Content content, INotifyer notifyer)
        : this(Guid.NewGuid(), content, notifyer, DateTime.Now)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="id"> The unique identifier of the notification.</param>
    /// <param name="content"> The content of the notification.</param>
    /// <param name="notifyer"> The notifyer that sent the notification.</param>
    /// <param name="timestamp">The timestamp when the notification was created.</param>
    [JsonConstructor]
    public Notification(Guid id, Content content, INotifyer notifyer, DateTime timestamp)
    {
        Id = id;
        Timestamp = timestamp;
        Content = content;
        Notifyer = notifyer;
    }
}
