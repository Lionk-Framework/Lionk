﻿// Copyright © 2024 Lionk Project

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
    ///     This event is triggered when the badge content changes.
    /// </summary>
    public event Action? OnBadgeContentChanged;

    private int _badgeContent;

    /// <summary>
    ///     Gets or sets the badge content.
    /// </summary>
    public int BadgeContent
    {
        get => _badgeContent;
        set
        {
            if (_badgeContent != value)
            {
                _badgeContent = value;
                OnBadgeContentChanged?.Invoke();
            }
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotificationStateService"/> class
    ///     and subscribes to the notification sent event.
    /// </summary>
    public NotificationStateService() => NotificationService.NotificationSent += (sender, args) => OnNotificationReceived?.Invoke();

    /// <summary>
    /// Resets the badge content to zero.
    /// </summary>
    public void ResetBadgeContent() => BadgeContent = 0;

    /// <summary>
    /// Increments the badge content by one.
    /// </summary>
    public void IncrementBadgeContent() => BadgeContent++;
}
