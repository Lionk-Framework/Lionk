// Copyright © 2024 Lionk Project

namespace Notifications.Model.Classes;

/// <summary>
/// this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    /// <summary>
    /// Save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(Notification notification) => NotificationFileHandler.SaveNotification(notification);

    /// <summary>
    /// Get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    public static List<Notification> GetNotifications() => NotificationFileHandler.GetNotifications();

    /// <summary>
    /// Get a notification by its unique identifier.
    /// </summary>
    /// <param name="guid"> the unique identifier of the notification.</param>
    /// <returns> The notification with the specified unique identifier.</returns>
    public static Notification? GetNotificationByGuid(Guid guid) => NotificationFileHandler.GetNotificationByGuid(guid);
}
