// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
///     Interface that define a Notification Channel.
/// </summary>
public interface IChannel : IEquatable<IChannel>
{
    #region properties

    /// <summary>
    ///     Gets the Guid of the channel.
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    ///     Gets a value indicating whether the channel is initialized.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    ///     Gets the name of the channel.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Gets the recipients of the channel.
    /// </summary>
    List<IRecipient> Recipients { get; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     This method add one or more recipients to the channel.
    /// </summary>
    /// <param name="recipients"> The recipient to add.</param>
    void AddRecipients(params IRecipient[] recipients);

    /// <summary>
    ///     Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> True if the objects are equals, otherwise false.</returns>
    new bool Equals(IChannel? obj);

    /// <summary>
    ///     Initialize the channel to send notifications.
    /// </summary>
    void Initialize();

    /// <summary>
    ///     Send a notification to the specified recipients.
    /// </summary>
    /// <param name="notifyer"> The notifyer that send the notification.</param>
    /// <param name="content"> The content of the notification.</param>
    void Send(INotifyer notifyer, Content content);

    #endregion
}
