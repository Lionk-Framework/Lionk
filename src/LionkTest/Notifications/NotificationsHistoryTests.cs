// Copyright © 2024 Lionk Project
using Lionk.Notification;

namespace LionkTest.NotificationsTests;

internal class NotificationsHistoryTests
{
    public class MockRecipient : IRecipient
    {
        public string Name { get; }

        public MockRecipient(string name) => Name = name;
    }

    public class MockNotifyer : INotifyer
    {
        public string Name => "TestNotifyer";
    }

    public class MockChannel : IChannel
    {
        public string Name { get; } = $"Channel";

        public List<IRecipient> Recipients { get; } = new() { new MockRecipient($"Recipient1"), new MockRecipient($"Recipient2") };

        public bool IsInitialized => true;

        public void Initialize()
        {
            // do nothing
        }

        public void Send(INotifyer notifyer, Content content)
        {
            // do nothing
        }

        public void AddRecipient(IRecipient recipient)
        {
            // do nothing
        }
    }

    private MockChannel _mockChannel1;
    private MockChannel _mockChannel2;
    private MockNotifyer _mockNotifyer;
    private Content _content;
    private Notification _notification;
    private List<IChannel> _channels;

    [OneTimeSetUp]
    public void Initialize()
    {
        // Arrange
        _mockNotifyer = new MockNotifyer();
        _mockChannel1 = new MockChannel();
        _mockChannel2 = new MockChannel();
        _content = new Content(Severity.Information, "Title", "Message");

        INotifyer notifyer = _mockNotifyer;
        _channels = new() { _mockChannel1, _mockChannel2 };
        _notification = new Notification(_content, notifyer);
        NotificationService.SaveNotificationHistory(_notification, _channels);
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
        NotificationHistory notificationHistory;

        // Act
        notificationHistory = NotificationService.GetNotifications().Last();

        // Assert
        Assert.That(notificationHistory.Notification.Id, Is.EqualTo(_notification.Id), "The notification ID is not the same.");
        Assert.That(notificationHistory.Notification.Notifyer.Name, Is.EqualTo(_notification.Notifyer.Name), "The notifyer name is not the same.");
        Assert.That(notificationHistory.Channels.Count, Is.EqualTo(_channels.Count), "The number of channels is not the same.");
        Assert.That(notificationHistory.Channels[0].Name, Is.EqualTo(_channels[0].Name), "The channel name is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients.Count, Is.EqualTo(_channels[0].Recipients.Count), "The number of recipients is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients[0].Name, Is.EqualTo(_channels[0].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[0].Recipients[1].Name, Is.EqualTo(_channels[0].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[1].Name, Is.EqualTo(_channels[1].Name), "The channel name is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients.Count, Is.EqualTo(_channels[1].Recipients.Count), "The number of recipients is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients[0].Name, Is.EqualTo(_channels[1].Recipients[0].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Channels[1].Recipients[1].Name, Is.EqualTo(_channels[1].Recipients[1].Name), "The recipient name is not the same.");
        Assert.That(notificationHistory.Notification.Content.Level, Is.EqualTo(_notification.Content.Level), "The level is not the same.");
        Assert.That(notificationHistory.Notification.Content.Title, Is.EqualTo(_notification.Content.Title), "The title is not the same.");
        Assert.That(notificationHistory.Notification.Content.Message, Is.EqualTo(_notification.Content.Message), "The message is not the same.");
    }
}
