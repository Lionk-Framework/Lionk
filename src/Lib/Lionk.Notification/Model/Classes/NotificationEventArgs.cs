// Copyright © 2024 Lionk Project

namespace Lionk.Notification.Event;

/// <summary>
///     This class is used to store the event arguments of a notification.
/// </summary>
public class NotificationEventArgs : EventArgs
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotificationEventArgs" /> class.
    /// </summary>
    /// <param name="notification"> The notification that was sent.</param>
    /// <param name="channels"> The channels where the notification was sent.</param>
    public NotificationEventArgs(Notification notification, List<IChannel> channels)
    {
        Notification = notification;
        Channels = channels;
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets the channels where the notification was sent.
    /// </summary>
    public List<IChannel> Channels { get; }

    /// <summary>
    ///     Gets the notification that was sent.
    /// </summary>
    public Notification Notification { get; }

    #endregion
}
