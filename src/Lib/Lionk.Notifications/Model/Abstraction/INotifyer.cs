// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
/// This interface represents a notifyer that can send notifications.
/// </summary>
public interface INotifyer
{
    /// <summary>
    /// Gets the Guid of the notifyer.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the name of the notifyer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> A value indicating whether the objects are equal.</returns>
    bool Equals(INotifyer? obj);
}
