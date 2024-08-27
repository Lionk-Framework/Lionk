// Copyright © 2024 Lionk Project

using Lionk.Notification;

/// <summary>
///     This class represents a service that tracks the state of notifications.
/// </summary>
public class NotificationStateService
{
    private int _badgeCounter;

    /// <summary>
    ///     This event is triggered when a notification is received.
    /// </summary>
    public event Action? OnNotificationReceived;

    /// <summary>
    ///     This event is triggered when the badge coounter changes.
    /// </summary>
    public event Action? OnBadgeCounterChanged;

    /// <summary>
    ///     Gets or sets the badge coounter.
    /// </summary>
    public int BadgeCounter
    {
        get => _badgeCounter;
        set
        {
            if (_badgeCounter != value)
            {
                _badgeCounter = value;
                OnBadgeCounterChanged?.Invoke();
            }
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotificationStateService"/> class
    ///     and subscribes to the notification sent event.
    /// </summary>
    public NotificationStateService() => NotificationService.NotificationSent += (sender, args) => OnNotificationReceived?.Invoke();

    /// <summary>
    /// Increments the badge coounter by 1.
    /// </summary>
    public void IncrementBadgeCounter() => BadgeCounter++;

    /// <summary>
    ///     Decrements the badge coounter by 1.
    /// </summary>
    public void DecrementBadgeCounter() => BadgeCounter = Math.Max(BadgeCounter - 1, 0);
}
