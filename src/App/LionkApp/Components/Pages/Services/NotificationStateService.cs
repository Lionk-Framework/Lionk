// Copyright © 2024 Lionk Project

using Lionk.Notification;

/// <summary>
///     This class represents a service that tracks the state of notifications.
/// </summary>
public class NotificationStateService
{
    /// <summary>
    ///     This event is triggered when a notification is received.
    /// </summary>
    public event Action? OnNotificationReceived;

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotificationStateService"/> class
    ///     and subscribes to the notification sent event.
    /// </summary>
    public NotificationStateService() => NotificationService.NotificationSent += (sender, args) => OnNotificationReceived?.Invoke();
}
