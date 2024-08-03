// Copyright © 2024 Lionk Project
using Notifications.Model.Classes;

namespace Notifications.Model.Abstraction;

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
    List<NotificationRecipient> Recipients { get; }

    /// <summary>
    /// Gets a value indicating whether the channel is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initialize the channel to send notifications.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Test the channel to check if it's working.
    /// </summary>
    void TestChannel();

    /// <summary>
    /// Send a notification to the specified recipients.
    /// </summary>
    /// <param name="content"> The content of the notification.</param>
    void Send(NotificationContent content);
}
