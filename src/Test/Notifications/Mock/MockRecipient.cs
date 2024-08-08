// Copyright © 2024 Lionk Project

using Lionk.Notification;

namespace LionkTest.Notifications.Mock;

/// <summary>
/// This class is a mock for IRecipient.
/// </summary>
public class MockRecipient : IRecipient
{
    /// <summary>
    /// Gets the name of the recipient.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockRecipient"/> class.
    /// </summary>
    /// <param name="name"> The name of the recipient.</param>
    public MockRecipient(string name) => Name = name;
}
