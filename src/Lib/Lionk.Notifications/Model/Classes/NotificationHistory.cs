// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
/// This class represents the history of notifications.
/// </summary>
public class NotificationHistory
{
    /// <summary>
    /// Gets the unique identifier of the notification history.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Gets a value indicating whether the notification is active.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// Gets or sets the notification.
    /// </summary>
    public Notification Notification { get; set; }

    /// <summary>
    /// Gets the list of channels.
    /// </summary>
    public List<IChannel> Channels { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationHistory"/> class.
    /// </summary>
    /// <param name="id"> The GUID of the notification history.</param>
    /// <param name="notification"> The notification.</param>
    /// <param name="channels"> The list of channels.</param>
    /// <param name="isRead"> The value indicating whether the notification is read.</param>
    [JsonConstructor]
    public NotificationHistory(Guid id, Notification notification, List<IChannel> channels, bool isRead)
    {
        Id = id;
        Notification = notification;
        Channels = channels;
        IsRead = isRead;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationHistory"/> class.
    /// </summary>
    /// <param name="notification"> The notification.</param>
    /// <param name="channels"> The list of channels.</param>
    public NotificationHistory(Notification notification, List<IChannel> channels)
    {
        Notification = notification;
        Channels = channels;
    }

    /// <summary>
    /// This method marks the notification as read.
    /// </summary>
    public void Read()
    {
        IsRead = true;
        NotificationService.EditNotificationHistory(this);
    }

    /// <summary>
    /// This method marks the notification as unread.
    /// </summary>
    public void Unread()
    {
        IsRead = false;
        NotificationService.EditNotificationHistory(this);
    }
}
