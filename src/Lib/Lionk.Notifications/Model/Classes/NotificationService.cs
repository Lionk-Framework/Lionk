// Copyright © 2024 Lionk Project
using System.Collections.ObjectModel;
using Lionk.Notification.Event;

namespace Lionk.Notification;

/// <summary>
/// this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    private static readonly List<INotifyer> _notifyers = new();
    private static readonly List<IChannel> _channels = new();
    private static readonly Dictionary<INotifyer, List<IChannel>> _notifyerChannels = new();

    /// <summary>
    /// This event is raised when a notification is sent.
    /// </summary>
    public static event EventHandler<NotificationEventArgs>? NotificationSent;

    /// <summary>
    /// Gets get the list of all the notifyers.
    /// </summary>
    public static ReadOnlyCollection<INotifyer> Notifyers => _notifyers.AsReadOnly();

    /// <summary>
    /// Gets the list of all the channels.
    /// </summary>
    public static ReadOnlyCollection<IChannel> Channels => _channels.AsReadOnly();

    /// <summary>
    /// Gets a Dictionary that maps the Notifyer with multiple channels.
    /// </summary>
    public static ReadOnlyDictionary<INotifyer, List<IChannel>> NotifyerChannels => _notifyerChannels.AsReadOnly();

    /// <summary>
    /// This method sends a notification and raises the event.
    /// </summary>
    /// <param name="notification">the notification to send.</param>
    public static void Send(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));
        List<IChannel> channels = NotifyerChannels[notification.Notifyer];
        foreach (IChannel channel in channels)
        {
            channel.Send(notification.Notifyer, notification.Content);
        }

        SaveNotificationHistory(notification, channels);
        NotificationSent?.Invoke(null, new NotificationEventArgs(notification, channels));
    }

    /// <summary>
    /// Save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    /// <param name="channels"> The list of channels where the notification was sent.</param>
    public static void SaveNotificationHistory(Notification notification, List<IChannel> channels)
    {
        var notificationHistory = new NotificationHistory(notification, channels);
        NotificationFileHandler.SaveNotification(notificationHistory);
    }

    /// <summary>
    /// Edit a notification in history.
    /// </summary>
    /// <param name="notification">The notification to edit.</param>
    public static void EditNotificationHistory(Notification notification)
    {
        NotificationHistory? notificationHistory = NotificationFileHandler.GetNotificationByGuid(notification.Id);
        if (notificationHistory is null) return;
        notificationHistory.Notification = notification;
        NotificationFileHandler.EditNotification(notificationHistory);
    }

    /// <summary>
    /// Get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    public static List<NotificationHistory> GetNotifications() => NotificationFileHandler.GetNotifications();

    /// <summary>
    /// Get a notification by its unique identifier.
    /// </summary>
    /// <param name="guid"> the unique identifier of the notification.</param>
    /// <returns> The notification with the specified unique identifier.</returns>
    public static NotificationHistory? GetNotificationByGuid(Guid guid) => NotificationFileHandler.GetNotificationByGuid(guid);

    /// <summary>
    /// Map a notifyer to a channel.
    /// </summary>
    /// <param name="notifyer"> The notifyer to map.</param>
    /// <param name="channels"> The list of channels to map.</param>
    public static void MapNotifyerToChannel(INotifyer notifyer, params IChannel[] channels)
    {
        if (channels is null || notifyer is null) return;
        foreach (IChannel channel in channels)
        {
            if (!_channels.Contains(channel)) _channels.Add(channel);
        }

        if (!_notifyers.Contains(notifyer)) _notifyers.Add(notifyer);
        if (!_notifyerChannels.ContainsKey(notifyer)) _notifyerChannels.Add(notifyer, new List<IChannel>());

        foreach (IChannel channel in channels)
        {
            _notifyerChannels[notifyer].Add(channel);
        }
    }

    /// <summary>
    /// This method adds channels to the list of channels.
    /// </summary>
    /// <param name="channels"> The channels to add.</param>
    public static void AddChannel(params IChannel[] channels)
    {
        ArgumentNullException.ThrowIfNull(channels, nameof(channels));
        foreach (IChannel item in channels)
        {
            if (!_channels.Contains(item)) _channels.Add(item);
        }
    }

    /// <summary>
    /// This method adds notifyers to the list of notifyers.
    /// </summary>
    /// <param name="notifyers"> The notifyers to add.</param>
    public static void AddNotifyer(params INotifyer[] notifyers)
    {
        ArgumentNullException.ThrowIfNull(notifyers, nameof(notifyers));
        foreach (INotifyer item in notifyers)
        {
            if (!_notifyers.Contains(item)) _notifyers.Add(item);
        }
    }
}
