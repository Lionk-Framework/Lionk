// Copyright © 2024 Lionk Project
using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
///     This class represents a notification that can be displayed to the user.
/// </summary>
public class Notification
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Notification" /> class.
    /// </summary>
    /// <param name="content"> The content of the notification.</param>
    /// <param name="notifier"> The notifier that sent the notification.</param>
    public Notification(Content content, INotifier notifier)
        : this(
            Guid.NewGuid(),
            content,
            notifier,
            DateTime.Now)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Notification" /> class.
    /// </summary>
    /// <param name="id"> The unique identifier of the notification.</param>
    /// <param name="content"> The content of the notification.</param>
    /// <param name="notifier"> The notifier that sent the notification.</param>
    /// <param name="timestamp">The timestamp when the notification was created.</param>
    [JsonConstructor]
    public Notification(Guid id, Content content, INotifier notifier, DateTime timestamp)
    {
        Id = id;
        Timestamp = timestamp;
        Content = content;
        Notifier = notifier;
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets the content of the notification.
    /// </summary>
    public Content Content { get; private set; }

    /// <summary>
    ///     Gets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the notifier that sent the notification.
    /// </summary>
    public INotifier Notifier { get; private set; }

    /// <summary>
    ///     Gets the timestamp when the notification was created.
    /// </summary>
    public DateTime Timestamp { get; private set; }

    #endregion
}
