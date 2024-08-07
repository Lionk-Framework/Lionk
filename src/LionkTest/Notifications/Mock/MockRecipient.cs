// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Newtonsoft.Json;

namespace LionkTest.Notifications.Mock;

/// <summary>
/// This class is a mock for IRecipient.
/// </summary>
public class MockRecipient : IRecipient
{
    /// <inheritdoc/>
    public Guid Guid { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Gets the name of the recipient.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockRecipient"/> class.
    /// </summary>
    /// <param name="name"> The name of the recipient.</param>
    public MockRecipient(string name) => Name = name;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockRecipient"/> class.
    /// </summary>
    /// <param name="name"> The name of the recipient.</param>
    /// <param name="guid"> The guid of the recipient.</param>
    [JsonConstructor]
    public MockRecipient(string name, Guid guid)
        : this(name) => Guid = guid;
}
