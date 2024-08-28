// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Newtonsoft.Json;

namespace LionkTest.Notifications.Mock;

/// <summary>
///     This class is a mock for INotifier.
/// </summary>
public class MockNotifier : INotifier
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="MockNotifier" /> class.
    /// </summary>
    /// <param name="guid"> The Guid of the notifier.</param>
    /// <param name="name"> The name of the notifier.</param>
    [JsonConstructor]
    public MockNotifier(Guid guid, string name)
    {
        Id = guid;
        Name = name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MockNotifier" /> class.
    /// </summary>
    /// <param name="name"> The name of the notifier.</param>
    public MockNotifier(string name) => Name = name;

    #endregion

    #region properties

    /// <inheritdoc />
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    ///     Gets the name of the notifier.
    /// </summary>
    public string Name { get; private set; } = "NotifyerTest";

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> A value indicating whether the objects are equal.</returns>
    public bool Equals(INotifier? obj) => obj is MockNotifier notifier && notifier.Id == Id;

    #endregion
}
