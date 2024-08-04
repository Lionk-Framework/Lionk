// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
/// This class represents the history of notifications.
/// </summary>
public class NotificationHistory
{
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
    /// <param name="notification"> The notification.</param>
    /// <param name="channels"> The list of channels.</param>
    [JsonConstructor]
    public NotificationHistory(Notification notification, List<IChannel> channels)
    {
        Notification = notification;
        Channels = channels;
    }
}
