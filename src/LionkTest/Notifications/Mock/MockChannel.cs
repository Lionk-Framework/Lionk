// Copyright © 2024 Lionk Project

using Lionk.Notification;

namespace LionkTest.Notifications.Mock;

/// <summary>
/// This class is a mock for IChannel.
/// </summary>
public class MockChannel : IChannel
{
    /// <summary>
    /// Gets the name of the channel.
    /// </summary>
    public string Name { get; } = $"Channel";

    /// <summary>
    /// Gets the list of recipients.
    /// </summary>
    public List<IRecipient> Recipients { get; } = new() { new MockRecipient($"Recipient1"), new MockRecipient($"Recipient2") };

    /// <summary>
    /// Gets a value indicating whether the channel is initialized.
    /// </summary>
    public bool IsInitialized => true;

    /// <summary>
    /// Initializes the channel.
    /// </summary>
    public void Initialize()
    {
        // do nothing
    }

    /// <summary>
    /// Sends a notification.
    /// </summary>
    /// <param name="notifyer"> The notifyer that sends the notification.</param>
    /// <param name="content"> The content of the notification.</param>
    public void Send(INotifyer notifyer, Content content)
    {
        // do nothing
    }

    /// <summary>
    /// This method adds a recipient to the channel.
    /// </summary>
    /// <param name="recipient"> The recipient to add.</param>
    public void AddRecipient(IRecipient recipient)
    {
        // do nothing
    }
}
