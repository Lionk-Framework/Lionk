// Copyright © 2024 Lionk Project

namespace Notifications.Classes;

/// <summary>
/// this class implements the way notifications are saved.
/// </summary>
public static class NotificationService
{
    /// <summary>
    /// Save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(Notification notification) => NotificationLogger.LogNotification(notification);
}
