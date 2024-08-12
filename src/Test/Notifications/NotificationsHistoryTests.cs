// Copyright © 2024 Lionk Project
using Lionk.Notification;
using Lionk.Utils;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

internal class NotificationsHistoryTests
{
    private MockChannel _mockChannel1;
    private MockChannel _mockChannel2;
    private MockNotifyer _mockNotifyer;
    private Content _content;
    private Notification _notification;
    private List<IChannel> _channels;

    /// <summary>
    /// Initializes datas for the test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Clear the files
        string channelFilePath = Path.Combine("notifications", "channels.json");
        string notifyerFilePath = Path.Combine("notifications", "notifyers.json");
        string notifyerChannelFilePath = Path.Combine("notifications", "notifyerChannels.json");
        string notificationFilePath = Path.Combine("notifications", "notifications.json");
        ConfigurationUtils.TryDeleteFile(channelFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerChannelFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notificationFilePath, FolderType.Data);

        // Arrange
        _mockNotifyer = new MockNotifyer("NotifyerHistoryTests");
        _mockChannel1 = new MockChannel("ChannelHistory1");
        _mockChannel2 = new MockChannel("ChannelHistory2");

        _mockChannel1.AddRecipients(new MockRecipient("Recipient1"), new MockRecipient("Recipient2"));
        _mockChannel2.AddRecipients(new MockRecipient("Recipient3"), new MockRecipient("Recipient4"));

        _content = new Content(Severity.Information, "Title", "Message");
        _channels = [_mockChannel1, _mockChannel2];
        _notification = new Notification(_content, _mockNotifyer);
        NotificationService.MapNotifyerToChannel(_mockNotifyer, _channels.ToArray());

        // Act
        NotificationService.SaveNotificationHistory(_notification);
    }

    [Test]
    public void TestDeserializationHistory()
    {
        // Arrange
        NotificationHistory notificationHistory;

        // Act
        notificationHistory = NotificationService.GetNotifications().Last();

        // Assert
        Assert.That(notificationHistory.Notification.Id, Is.EqualTo(_notification.Id), "The notification ID is not the same.");
        Assert.That(notificationHistory.Notification.Notifyer.Name, Is.EqualTo(_notification.Notifyer.Name), "The notifyer name is not the same.");
        Assert.That(notificationHistory.Channels.Count, Is.EqualTo(_channels.Count), "The number of channels is not the same.");
        Assert.That(notificationHistory.Channels[0].Name, Is.EqualTo(_channels[0].Name), "The channel name is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients.Count, Is.EqualTo(2), "The number of recipients is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients[0].Name, Is.EqualTo(_channels[0].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients[1].Name, Is.EqualTo(_channels[0].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[1].Name, Is.EqualTo(_channels[1].Name), "The channel name is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients.Count, Is.EqualTo(2), "The number of recipients is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients[0].Name, Is.EqualTo(_channels[1].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients[1].Name, Is.EqualTo(_channels[1].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Notification.Content.Level, Is.EqualTo(_notification.Content.Level), "The level is not the same.");
        Assert.That(notificationHistory.Notification.Content.Title, Is.EqualTo(_notification.Content.Title), "The title is not the same.");
        Assert.That(notificationHistory.Notification.Content.Message, Is.EqualTo(_notification.Content.Message), "The message is not the same.");
    }

    [Test]
    public void TestEditedNotificationDeserialization()
    {
        // Arrange
        Guid id;

        // Act
        List<NotificationHistory> notifications = NotificationService.GetNotifications();
        NotificationHistory notificationHistory = notifications.Last();
        if (notificationHistory is null)
        {
            Assert.Fail("The notification history is null.");
            return;
        }

        id = notificationHistory.Id;
        notificationHistory.Read();
        NotificationService.EditNotificationHistory(notificationHistory);
        NotificationHistory? editedNotificationHistory = NotificationService.GetNotificationByGuid(id);
        if (editedNotificationHistory is null)
        {
            Assert.Fail("The edited notification history is null.");
            return;
        }

        // Assert
        Assert.That(editedNotificationHistory.IsRead, Is.True, "The notification is not read.");
    }
}
