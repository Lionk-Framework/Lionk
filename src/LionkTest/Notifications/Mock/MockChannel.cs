// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Newtonsoft.Json;

namespace LionkTest.Notifications.Mock;

/// <summary>
/// This class is a mock for IChannel.
/// </summary>
public class MockChannel : IChannel
{
    /// <inheritdoc/>
    public Guid Guid { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the name of the channel.
    /// </summary>
    public string Name { get; set; } = $"Channel";

    /// <summary>
    /// Gets the list of recipients.
    /// </summary>
    [JsonProperty]
    public List<IRecipient> Recipients { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the channel is initialized.
    /// </summary>
    public bool IsInitialized { get; private set; }

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
    /// <param name="recipients"> The recipient to add.</param>
    public void AddRecipients(params IRecipient[] recipients)
    {
        List<IRecipient> recipientsToAdd = new();
        foreach (IRecipient recipient in recipients)
        {
            if (Recipients.Contains(recipient) || recipient is not MockRecipient) continue;
            else recipientsToAdd.Add(recipient);
        }

        Recipients.AddRange(recipientsToAdd);
    }

    /// <inheritdoc/>
    public new bool Equals(object? obj)
    {
        if (obj is MockChannel channel) return Guid == channel.Guid && Name == channel.Name;
        return false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockChannel"/> class.
    /// </summary>
    /// <param name="name"> The name of the channel.</param>
    public MockChannel(string name)
    {
        Name = name;
        Recipients = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockChannel"/> class.
    /// </summary>
    /// <param name="guid"> The Guid of the channel.</param>
    /// <param name="name"> The name of the channel.</param>
    /// <param name="recipients"> The list of recipients.</param>
    /// <param name="isInitialized"> A value indicating whether the channel is initialized.</param>
    [JsonConstructor]
    public MockChannel(Guid guid, string name, List<IRecipient> recipients, bool isInitialized)
    {
        Guid = guid;
        Name = name;
        IsInitialized = isInitialized;
        Recipients = recipients;
    }
}
