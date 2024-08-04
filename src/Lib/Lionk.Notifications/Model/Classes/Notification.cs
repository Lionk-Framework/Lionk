// Copyright © 2024 Lionk Project
using Newtonsoft.Json;

namespace Lionk.Notification;

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
    /// Gets the Notifyer that created the notification.
    /// </summary>
    public INotifyer Notifyer { get; private set; }

    /// <summary>
    /// Gets the content of the notification.
    /// </summary>
    public List<IChannel> Channels { get; private set; }

    /// <summary>
    /// Gets the content of the notification.
    /// </summary>
    public NotificationContent Content { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="notifyer"> The notifyer that created the notification.</param>
    /// <param name="channels"> The channels to send the notification to.</param>
    /// <param name="content"> The content of the notification.</param>
    public Notification(INotifyer notifyer, List<IChannel> channels, NotificationContent content)
        : this(Guid.NewGuid(), notifyer, channels, content, DateTime.Now)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="id"> The unique identifier of the notification.</param>
    /// <param name="notifyer"> The notifyer that created the notification.</param>
    /// <param name="channels"> The channels to send the notification to.</param>
    /// <param name="content"> The content of the notification.</param>
    /// <param name="timestamp">The timestamp when the notification was created.</param>
    [JsonConstructor]
    public Notification(Guid id, INotifyer notifyer, List<IChannel> channels, NotificationContent content, DateTime timestamp)
    {
        Id = id;
        Timestamp = timestamp;
        Channels = channels;
        Content = content;
        Notifyer = notifyer;
    }

    /// <summary>
    /// Method to send the notification to all the channels.
    /// </summary>
    public void SendAll()
    {
        foreach (IChannel channel in Channels)
        {
            SendToChannel(channel);
        }

        NotificationService.SaveNotification(this);
    }

    /// <summary>
    /// Method to send the notification to a specific channel.
    /// </summary>
    /// <param name="channel"> The channel to send the notification to.</param>
    public void SendToChannel(IChannel channel)
    {
        if (channel is null) throw new ArgumentNullException(nameof(channel));
        channel.Send(Content);
    }
}
