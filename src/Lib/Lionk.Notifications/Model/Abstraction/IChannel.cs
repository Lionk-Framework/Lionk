// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
/// Interface that define a Notification Channel.
/// </summary>
public interface IChannel
{
    /// <summary>
    /// Gets the name of the channel.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the recipients of the channel.
    /// </summary>
    List<IRecipient> Recipients { get; }

    /// <summary>
    /// Gets a value indicating whether the channel is initialized.
    /// </summary>
    bool IsInitialized { get; }

    void AddRecipient(IRecipient recipient);

    /// <summary>
    /// Initialize the channel to send notifications.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Send a notification to the specified recipients.
    /// </summary>
    /// <param name="notifyer"> The notifyer that send the notification.</param>
    /// <param name="content"> The content of the notification.</param>
    void Send(INotifyer notifyer, Content content);
}
