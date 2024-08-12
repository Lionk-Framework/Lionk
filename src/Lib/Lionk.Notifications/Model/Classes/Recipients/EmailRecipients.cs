// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Notification.Email;

/// <summary>
/// This class define a email recipients to send notifications.
/// </summary>
public class EmailRecipients : IRecipient
{
    /// <inheritdoc/>
    public Guid Guid { get; private set; } = Guid.NewGuid();

    /// <inheritdoc/>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the email of the recipient.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailRecipients"/> class with the specified name, email, subject, and body.
    /// </summary>
    /// <param name="name"> The name of the recipient.</param>
    /// <param name="email"> The email of the recipient.</param>
    public EmailRecipients(string name, string email)
    {
        Name = name;
        Email = email;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailRecipients"/> class with the specified Guid, name, and email.
    /// </summary>
    /// <param name="guid"> The Guid of the recipient.</param>
    /// <param name="name"> The name of the recipient.</param>
    /// <param name="email"> The email of the recipient.</param>
    [JsonConstructor]
    public EmailRecipients(Guid guid, string name, string email)
    {
        Guid = guid;
        Name = name;
        Email = email;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailRecipients"/> class.
    /// </summary>
    public EmailRecipients()
    {
    }
}
