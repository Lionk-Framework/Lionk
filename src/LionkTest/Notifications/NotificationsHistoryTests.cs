// Copyright © 2024 Lionk Project
using Notifications.Model.Abstraction;
using Notifications.Model.Classes;
using Notifications.Model.Enums;

namespace LionkTest.NotificationsTests;

internal class NotificationsHistoryTests
{
    public class MockNotifyer : INotifyer
    {
        public string Name => "TestNotifyer";
    }

    public class MockChannel : IChannel
    {
        public string Name { get; } = $"Channel";

        public List<NotificationRecipient> Recipients { get; } = new() { new($"Recipient1"), new($"Recipient2") };

        public bool IsInitialized => true;

        public void Initialize()
        {
            // do nothing
        }

        public void Send(NotificationContent content)
        {
            // do nothing
        }

        void IChannel.TestChannel()
        {
            // do nothing
        }
    }

    private MockChannel _mockChannel1;
    private MockChannel _mockChannel2;
    private MockNotifyer _mockNotifyer;
    private NotificationContent _content;
    private Notification _notification;

    [OneTimeSetUp]
    public void Initialize()
    {
        // Arrange
        _mockNotifyer = new MockNotifyer();
        _mockChannel1 = new MockChannel();
        _mockChannel2 = new MockChannel();
        _content = new NotificationContent(Severity.Information, "Title", "Message");

        INotifyer notifyer = _mockNotifyer;
        List<IChannel> channels = new() { _mockChannel1, _mockChannel2 };
        _notification = new Notification(notifyer, channels, _content);

        NotificationService.SaveNotification(_notification);
    }

    [Test]
    public void FileExists()
    {
        // Arrange
        // Nothing to arrange

        // Act
        bool fileExists = File.Exists(NotificationFileHandler.FilePath);

        // Assert
        Assert.IsTrue(fileExists, "The history file doesn't exist.");
    }

    [Test]
    public void TestDeserializationHistory()
    {
        // Arrange
        Notification notification;

        // Act
        notification = NotificationService.GetNotifications().Last();

        // Assert
        Assert.That(notification.Id, Is.EqualTo(_notification.Id), "The notification ID is not the same.");
        Assert.That(notification.Notifyer.Name, Is.EqualTo(_notification.Notifyer.Name), "The notifyer name is not the same.");
        Assert.That(notification.Channels.Count, Is.EqualTo(_notification.Channels.Count), "The number of channels is not the same.");
        Assert.That(notification.Channels[0].Name, Is.EqualTo(_notification.Channels[0].Name), "The channel name is not the same.");
        Assert.That(notification.Channels[0].Recipients.Count, Is.EqualTo(_notification.Channels[0].Recipients.Count), "The number of recipients is not the same.");
        Assert.That(notification.Channels[0].Recipients[0].Name, Is.EqualTo(_notification.Channels[0].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notification.Channels[0].Recipients[1].Name, Is.EqualTo(_notification.Channels[0].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notification.Channels[1].Name, Is.EqualTo(_notification.Channels[1].Name), "The channel name is not the same.");
        Assert.That(notification.Channels[1].Recipients.Count, Is.EqualTo(_notification.Channels[1].Recipients.Count), "The number of recipients is not the same.");
        Assert.That(notification.Channels[1].Recipients[0].Name, Is.EqualTo(_notification.Channels[1].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notification.Channels[1].Recipients[1].Name, Is.EqualTo(_notification.Channels[1].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notification.Content.Level, Is.EqualTo(_notification.Content.Level), "The level is not the same.");
        Assert.That(notification.Content.Title, Is.EqualTo(_notification.Content.Title), "The title is not the same.");
        Assert.That(notification.Content.Message, Is.EqualTo(_notification.Content.Message), "The message is not the same.");
    }
}
