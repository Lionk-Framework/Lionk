// Copyright © 2024 Lionk Project
using System.Collections.ObjectModel;
using System.Threading.Channels;
using Lionk.Notification.Event;

namespace Lionk.Notification;

/// <summary>
/// this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    private static List<INotifyer> _notifyers = new();
    private static List<IChannel> _channels = new();
    private static Dictionary<string, List<IChannel>> _notifyerChannels = new();

    /// <summary>
    /// This event is raised when a notification is sent.
    /// </summary>
    public static event EventHandler<NotificationEventArgs>? NotificationSent;

    /// <summary>
    /// Gets get the list of all the notifyers.
    /// </summary>
    public static ReadOnlyCollection<INotifyer> Notifyers
    {
        get
        {
            GetNotifyers();
            return _notifyers.AsReadOnly();
        }
    }

    /// <summary>
    /// Gets the list of all the channels.
    /// </summary>
    public static ReadOnlyCollection<IChannel> Channels
    {
        get
        {
            GetChannels();
            return _channels.AsReadOnly();
        }
    }

    /// <summary>
    /// Gets a Dictionary that maps the Notifyer name with multiple channels.
    /// </summary>
    public static ReadOnlyDictionary<string, List<IChannel>> NotifyerChannels
    {
        get
        {
            GetNotifyerChannels();
            return _notifyerChannels.AsReadOnly();
        }
    }

    /// <summary>
    /// This method sends a notification and raises the event.
    /// </summary>
    /// <param name="notification">the notification to send.</param>
    public static void Send(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));
        List<IChannel> channels = NotifyerChannels[notification.Notifyer.Name];
        foreach (IChannel channel in channels)
        {
            channel.Send(notification.Notifyer, notification.Content);
        }

        SaveNotificationHistory(notification);
        NotificationSent?.Invoke(null, new NotificationEventArgs(notification, channels));
    }

    /// <summary>
    /// Save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotificationHistory(Notification notification)
    {
        var notificationHistory = new NotificationHistory(notification, _notifyerChannels[notification.Notifyer.Name]);
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
    /// Map a notifyer to a channel and add them to the list of Notifyers and Channels if they are not already in the list.
    /// If the notifyer is already mapped to the channel, the channels will be added if they are not already in the list.
    /// </summary>
    /// <param name="notifyer"> The notifyer to map.</param>
    /// <param name="channels"> The list of channels to map.</param>
    public static void MapNotifyerToChannel(INotifyer notifyer, params IChannel[] channels)
    {
        if (channels is null || notifyer is null) return;
        AddChannels(channels);
        AddNotifyers(notifyer);
        GetNotifyerChannels();
        if (!_notifyerChannels.ContainsKey(notifyer.Name)) _notifyerChannels.Add(notifyer.Name, new List<IChannel>());
        foreach (IChannel item in channels) if (!_notifyerChannels[notifyer.Name].Contains(item)) _notifyerChannels[notifyer.Name].Add(item);
        SaveNotifyerChannels();
    }

    /// <summary>
    /// This method adds channels to the list of channels if they are not already in the list.
    /// </summary>
    /// <param name="channels"> The channels to add.</param>
    public static void AddChannels(params IChannel[] channels)
    {
        ArgumentNullException.ThrowIfNull(channels, nameof(channels));
        GetChannels();

        if (_channels == null)
        {
            _channels = new List<IChannel>();
        }

        foreach (IChannel item in channels)
        {
            bool exists = false;
            foreach (IChannel channel in _channels)
            {
                if (channel.Equals(item))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                _channels.Add(item);
            }
        }

        SaveChannels();
    }

    /// <summary>
    /// This method adds notifyers to the list of notifyers if they are not already in the list.
    /// </summary>
    /// <param name="notifyers"> The notifyers to add.</param>
    public static void AddNotifyers(params INotifyer[] notifyers)
    {
        ArgumentNullException.ThrowIfNull(notifyers, nameof(notifyers));
        GetNotifyers();
        foreach (INotifyer item in notifyers)
        {
            if (_notifyers.Count == 0)
            {
                _notifyers.Add(item);
            }
            else
            {
                List<INotifyer> notifyerToAdd = new();
                foreach (INotifyer notifyer in _notifyers)
                {
                    if (notifyer.Equals(item)) continue;
                    else notifyerToAdd.Add(item);
                }

                _notifyers.AddRange(notifyerToAdd);
            }
        }

        SaveNotifyers();
    }

    private static void SaveNotifyers() => NotificationFileHandler.SaveNotifyerToJson(_notifyers);

    private static void SaveChannels() => NotificationFileHandler.SaveChannelToJson(_channels);

    private static void SaveNotifyerChannels() => NotificationFileHandler.SaveNotifyerChannelsToJson(_notifyerChannels);

    private static void GetChannels() => _channels = NotificationFileHandler.GetChannelsFromJson();

    private static void GetNotifyers() => _notifyers = NotificationFileHandler.GetNotifyersFromJson();

    private static void GetNotifyerChannels() => _notifyerChannels = NotificationFileHandler.GetNotifyerChannelsFromJson();
}
