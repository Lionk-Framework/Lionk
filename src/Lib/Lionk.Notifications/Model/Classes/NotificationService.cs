// Copyright © 2024 Lionk Project
using System.Collections.ObjectModel;
using Lionk.Notification.Event;

namespace Lionk.Notification;

/// <summary>
///     this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    #region fields

    private static List<IChannel> _channels = [];

    private static Dictionary<Guid, List<IChannel>> _notifierChannels = [];

    private static List<INotifier> _notifiers = [];

    #endregion

    #region delegate and events

    /// <summary>
    ///     This event is raised when a notification is sent.
    /// </summary>
    public static event EventHandler<NotificationEventArgs>? NotificationSent;

    #endregion

    #region properties

    /// <summary>
    ///     Gets the list of all the channels.
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
    ///     Gets a Dictionary that maps the Notifier name with multiple channels.
    /// </summary>
    public static ReadOnlyDictionary<Guid, List<IChannel>> NotifierChannels
    {
        get
        {
            GetNotifierChannels();
            return _notifierChannels.AsReadOnly();
        }
    }

    /// <summary>
    ///     Gets get the list of all the notifiers.
    /// </summary>
    public static ReadOnlyCollection<INotifier> Notifiers
    {
        get
        {
            GetNotifiers();
            return _notifiers.AsReadOnly();
        }
    }

    #endregion

    #region public and override methods

    /// <summary>
    ///     This method adds channels to the list of channels if they are not already in the list.
    /// </summary>
    /// <param name="channels"> The channels to add.</param>
    public static void AddChannels(params IChannel[] channels)
    {
        ArgumentNullException.ThrowIfNull(channels, nameof(channels));
        GetChannels();

        foreach (IChannel item in channels)
        {
            bool exists = _channels.Any(channel => channel.Equals(item));

            if (!exists)
            {
                _channels.Add(item);
            }
        }

        SaveChannels();
    }

    /// <summary>
    ///     This method adds notifiers to the list of notifiers if they are not already in the list.
    /// </summary>
    /// <param name="notifiers"> The notifiers to add.</param>
    public static void AddNotifiers(params INotifier[] notifiers)
    {
        ArgumentNullException.ThrowIfNull(notifiers, nameof(notifiers));
        GetNotifiers();

        foreach (INotifier item in notifiers)
        {
            if (!_notifiers.Contains(item))
            {
                _notifiers.Add(item);
            }
        }

        SaveNotifiers();
    }

    /// <summary>
    ///     Edit a notification in history.
    /// </summary>
    /// <param name="notificationHistory">The notification history to edit.</param>
    public static void EditNotificationHistory(NotificationHistory notificationHistory) =>
        NotificationFileHandler.EditNotificationHistory(notificationHistory);

    /// <summary>
    ///     Get a notification by its unique identifier.
    /// </summary>
    /// <param name="guid"> the unique identifier of the notification.</param>
    /// <returns> The notification with the specified unique identifier.</returns>
    public static NotificationHistory? GetNotificationByGuid(Guid guid) => NotificationFileHandler.GetNotificationByGuid(guid);

    /// <summary>
    ///     Get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    public static List<NotificationHistory> GetNotifications() => NotificationFileHandler.GetNotifications();

    /// <summary>
    ///     Map a notifier to a channel and add them to the list of Notifiers and Channels if they are not already in the list.
    ///     If the notifier is already mapped to the channel, the channels will be added if they are not already in the list.
    /// </summary>
    /// <param name="notifier"> The notifier to map.</param>
    /// <param name="channels"> The list of channels to map.</param>
    public static void MapNotifierToChannel(INotifier notifier, params IChannel[] channels)
    {
        AddChannels(channels);
        AddNotifiers(notifier);
        GetNotifierChannels();
        if (!_notifierChannels.ContainsKey(notifier.Id))
        {
            _notifierChannels.Add(notifier.Id, []);
        }

        foreach (IChannel item in channels)
        {
            if (!_notifierChannels[notifier.Id].Contains(item))
            {
                _notifierChannels[notifier.Id].Add(item);
            }
        }

        SaveNotifierChannels();
    }

    /// <summary>
    ///     Unmap a notifier from a list of channels.
    ///     Removes the channels from the list of channels associated with the notifier.
    /// </summary>
    /// <param name="notifier">The notifier to unmap.</param>
    /// <param name="channels">The list of channels to unmap.</param>
    public static void UnmapNotifierFromChannel(INotifier notifier, params IChannel[] channels)
    {
        if (_notifierChannels.ContainsKey(notifier.Id))
        {
            foreach (IChannel channel in channels)
            {
                _notifierChannels[notifier.Id].Remove(channel);
            }

            if (!_notifierChannels[notifier.Id].Any())
            {
                _notifierChannels.Remove(notifier.Id);
            }

            SaveNotifierChannels();
        }
    }

    /// <summary>
    ///     Methode to remove a channel from the list of channels.
    ///     If the channel is mapped to a notifier, it will be removed from the notifier.
    /// </summary>
    /// <param name="channel"> The channel to remove.</param>
    public static void RemoveChannel(IChannel channel)
    {
        ArgumentNullException.ThrowIfNull(channel, nameof(channel));
        GetChannels();
        GetNotifierChannels();

        // Remove the channel from the list of channels
        int index = _channels.FindIndex(c => c.Equals(channel));
        if (index >= 0)
        {
            _channels.RemoveAt(index);
        }

        // Remove the channel from the dictionary of notifier channels
        foreach (Guid key in _notifierChannels.Keys.ToList())
        {
            _notifierChannels[key].RemoveAll(c => c.Equals(channel));
        }

        SaveChannels();
        SaveNotifierChannels();
    }

    /// <summary>
    ///     This method removes a
    ///     notifier from the list of notifiers.
    /// </summary>
    /// <param name="notifier"> The notifier to remove.</param>
    public static void RemoveNotifier(INotifier notifier)
    {
        ArgumentNullException.ThrowIfNull(notifier, nameof(notifier));
        GetNotifiers();
        GetNotifierChannels();

        // Remove the notifier from the list of notifiers
        int index = _notifiers.FindIndex(n => n.Equals(notifier));
        if (index >= 0)
        {
            _notifiers.RemoveAt(index);
        }

        // Remove the notifier from the dictionary of notifier channels
        _notifierChannels.Remove(notifier.Id);

        SaveNotifiers();
        SaveNotifierChannels();
    }

    /// <summary>
    ///     Save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotificationHistory(Notification notification)
    {
        var notificationHistory = new NotificationHistory(notification, _notifierChannels[notification.Notifier.Id]);
        NotificationFileHandler.SaveNotification(notificationHistory);
    }

    /// <summary>
    ///     This method sends a notification and raises the event.
    /// </summary>
    /// <param name="notification">the notification to send.</param>
    public static void Send(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        if (NotifierChannels.TryGetValue(notification.Notifier.Id, out List<IChannel>? channels))
        {
            foreach (IChannel channel in channels)
            {
                channel.Send(notification.Notifier, notification.Content);
            }

            SaveNotificationHistory(notification);
            NotificationSent?.Invoke(null, new NotificationEventArgs(notification, channels));
        }
    }

    /// <summary>
    ///     Methode that returns the list of notifiers that use a channel.
    /// </summary>
    /// <param name="channel"> The channel to check.</param>
    /// <returns> The list of notifiers that use the channel.</returns>
    public static List<INotifier> WhoUseThisChannel(IChannel channel)
    {
        ArgumentNullException.ThrowIfNull(channel, nameof(channel));
        GetNotifierChannels();
        List<INotifier> notifiers = [];
        foreach (Guid key in _notifierChannels.Keys)
        {
            foreach (IChannel item in _notifierChannels[key])
            {
                if (item.Equals(channel))
                {
                    INotifier? notifier = _notifiers.FirstOrDefault(n => n.Id == key);
                    if (notifier != null)
                    {
                        notifiers.Add(notifier);
                    }
                }
            }
        }

        return notifiers;
    }

    #endregion

    #region others methods

    private static void GetChannels() => _channels = NotificationFileHandler.GetChannelsFromJson();

    private static void GetNotifierChannels() => _notifierChannels = NotificationFileHandler.GetNotifierChannelsFromJSon();

    private static void GetNotifiers() => _notifiers = NotificationFileHandler.GetNotifiersFromJson();

    private static void SaveChannels() => NotificationFileHandler.SaveChannelToJson(_channels);

    private static void SaveNotifierChannels() => NotificationFileHandler.SaveNotifierChannelsToJson(_notifierChannels);

    private static void SaveNotifiers() => NotificationFileHandler.SaveNotifierToJson(_notifiers);

    #endregion
}
