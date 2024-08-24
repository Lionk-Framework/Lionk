// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
///     This interface represents a notifyer that can send notifications.
/// </summary>
public interface INotifyer : IEquatable<INotifyer>
{
    #region properties

    /// <summary>
    ///     Gets the Guid of the notifyer.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    ///     Gets the name of the notifyer.
    /// </summary>
    public string Name { get; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> A value indicating whether the objects are equal.</returns>
    new bool Equals(INotifyer? obj);

    #endregion
}
