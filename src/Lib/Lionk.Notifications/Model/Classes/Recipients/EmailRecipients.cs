// Copyright © 2024 Lionk Project

namespace Lionk.Notification.Email;

/// <summary>
/// This class define a email recipients to send notifications.
/// </summary>
public class EmailRecipients : IRecipient
{
    /// <inheritdoc/>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the email of the recipient.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets the subject of the email.
    /// </summary>
    public string Subject { get; private set; }

    /// <summary>
    /// Gets the body of the email.
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailRecipients"/> class with the specified name, email, subject, and body.
    /// </summary>
    /// <param name="name"> The name of the recipient.</param>
    /// <param name="email"> The email of the recipient.</param>
    /// <param name="subject"> The subject of the email.</param>
    /// <param name="body"> The body of the email.</param>
    public EmailRecipients(string name, string email, string subject, string body)
    {
        Name = name;
        Email = email;
        Subject = subject;
        Body = body;
    }
}
