// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
/// This interface represents a notifyer that can send notifications.
/// </summary>
public interface INotifyer
{
    /// <summary>
    /// Gets the name of the notifyer.
    /// </summary>
    public string Name { get; }
}
