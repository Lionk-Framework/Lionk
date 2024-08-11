// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
/// Interface that define a recipient to send notifications.
/// </summary>
public interface IRecipient
{
    /// <summary>
    /// Gets the Guid of the channel.
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// Gets the name of the recipient.
    /// </summary>
    string Name { get; }
}
