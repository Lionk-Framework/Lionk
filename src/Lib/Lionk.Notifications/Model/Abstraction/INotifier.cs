// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
///     This interface represents a notifier that can send notifications.
/// </summary>
public interface INotifier : IEquatable<INotifier>
{
    #region properties

    /// <summary>
    ///     Gets the Guid of the notifier.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    ///     Gets the name of the notifier.
    /// </summary>
    public string Name { get; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> A value indicating whether the objects are equal.</returns>
    new bool Equals(INotifier? obj);

    #endregion
}
