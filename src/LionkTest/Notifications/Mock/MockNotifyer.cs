// Copyright © 2024 Lionk Project

using Lionk.Notification;

namespace LionkTest.Notifications.Mock;

/// <summary>
/// This class is a mock for INotifyer.
/// </summary>
public class MockNotifyer : INotifyer
{
    /// <summary>
    /// Gets the name of the notifyer.
    /// </summary>
    public string Name => "TestNotifyer";
}
