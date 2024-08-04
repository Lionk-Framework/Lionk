// Copyright © 2024 Lionk Project
namespace Lionk.Notification;

/// <summary>
/// this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    /// <summary>
    /// Gets get the list of all the notifyers.
    /// </summary>
    public static List<INotifyer> Notifyers { get; } = new List<INotifyer>();

    /// <summary>
    /// Gets the list of all the channels.
    /// </summary>
    public static List<IChannel> Channels { get; } = new List<IChannel>();

    /// <summary>
    /// Gets a Dictionary that maps the Notifyer with multiple channels.
    /// </summary>
    public static Dictionary<INotifyer, List<IChannel>> NotifyerChannels { get; } = new Dictionary<INotifyer, List<IChannel>>();

    /// <summary>
    /// This method sends a notification and raises the event.
    /// </summary>
    /// <param name="notification">the notification to send.</param>
    public static void Send(Notification notification)
    {
        // TODO : Raise the event.
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));
        List<IChannel> channels = NotifyerChannels[notification.Notifyer];
        foreach (IChannel channel in channels)
        {
            channel.Send(notification.Notifyer, notification.Content);
        }
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
}
