// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Newtonsoft.Json;

namespace LionkTest.Notifications.Mock;

/// <summary>
///     This class is a mock for INotifyer.
/// </summary>
public class MockNotifyer : INotifyer
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="MockNotifyer" /> class.
    /// </summary>
    /// <param name="guid"> The Guid of the notifyer.</param>
    /// <param name="name"> The name of the notifyer.</param>
    [JsonConstructor]
    public MockNotifyer(Guid guid, string name)
    {
        Id = guid;
        Name = name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MockNotifyer" /> class.
    /// </summary>
    /// <param name="name"> The name of the notifyer.</param>
    public MockNotifyer(string name) => Name = name;

    #endregion

    #region properties

    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    ///     Gets the name of the notifyer.
    /// </summary>
    public string Name { get; } = "NotifyerTest";

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to compare two objects.
    /// </summary>
    /// <param name="obj"> The object to compare.</param>
    /// <returns> A value indicating whether the objects are equal.</returns>
    public bool Equals(INotifyer? obj) => obj is MockNotifyer notifyer && notifyer.Id == Id;

    #endregion
}
