// Copyright © 2024 Lionk Project

using Lionk.Notification;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

/// <summary>
///     This class is used to test the notification events.
/// </summary>
public class NotificationEventsTests
{
    #region fields

    private Content _content;

    private MockChannel _mockChannel;

    private MockNotifier _mockNotifier;

    private Notification _notification;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Test if the event is raised when a notification is sent.
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

    /// <summary>
    ///     Initializes datas for the test.
    /// </summary>
    [OneTimeSetUp]
    public void Initialize()
    {
        // Arrange
        _mockNotifier = new MockNotifier("NotifyerEventTests");
        _mockChannel = new MockChannel("ChannelEventTest");
        _content = new Content(Severity.Information, "Title", "Message");
        _notification = new Notification(_content, _mockNotifier);

        NotificationService.MapNotifierToChannel(_mockNotifier, _mockChannel);
    }

    #endregion
}
