// Copyright © 2024 Lionk Project

using Lionk.Notification;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

/// <summary>
/// This class is used to test the notification events.
/// </summary>
public class NotificationEventsTests
{
    private MockChannel _mockChannel;
    private MockNotifyer _mockNotifyer;
    private Content _content;
    private Notification _notification;

    /// <summary>
    /// Initializes datas for the test.
    /// </summary>
    [OneTimeSetUp]
    public void Initialize()
    {
        // Arrange
        _mockNotifyer = new MockNotifyer();
        _mockChannel = new MockChannel();
        _content = new Content(Severity.Information, "Title", "Message");
        _notification = new Notification(_content, _mockNotifyer);

        NotificationService.Channels.Add(_mockChannel);
        NotificationService.Notifyers.Add(_mockNotifyer);
        NotificationService.MapNotifyerToChannel(_mockNotifyer, _mockChannel);
    }

    /// <summary>
    /// Test if the event is raised when a notification is sent.
    /// </summary>
    [Test]
    public void EventIsRaisedWhenNotificationIsSent()
    {
        // Arrange
        bool eventRaised = false;

        // Act
        NotificationService.NotificationSent += (sender, e) => eventRaised = true;
        NotificationService.Send(_notification);

        // Assert
        Assert.IsTrue(eventRaised, "The event was not raised.");
    }
}
