// Copyright © 2024 Lionk Project
namespace Notifications.Model.Abstraction;

/// <summary>
/// Interface that define a Notifyer.
/// </summary>
public interface INotifyer
{
    /// <summary>
    /// Gets the name of the notifyer.
    /// </summary>
    string Name { get; }
}
