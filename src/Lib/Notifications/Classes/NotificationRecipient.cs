// Copyright © 2024 Lionk Project
namespace Notifications.Classes;

/// <summary>
/// This class define a notification recipient.
/// </summary>
public class NotificationRecipient
{
    /// <summary>
    /// Gets the name of the recipient.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRecipient"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the recipient.</param>
    public NotificationRecipient(string name) => Name = name;
}
