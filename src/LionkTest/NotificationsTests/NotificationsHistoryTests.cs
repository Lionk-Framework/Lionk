// Copyright © 2024 Lionk Project
using Notifications.Classes;
using Notifications.Enums;
using Notifications.Interfaces;

namespace LionkTest.NotificationsTests;

internal class NotificationsHistoryTests
{
    private class MockChannel : IChannel
    {
        public string Name => "Channel";

        public List<NotificationRecipient> Recipients => new()
        {
            new("Recipient1"),
            new("Recipient2"),
        };

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            // Do nothing
        }

        public void TestChannel()
        {
            // Do nothing
        }

        public void Send(NotificationContent content)
        {
            // Do nothing
        }

        public MockChannel(bool isInitialized = true) => IsInitialized = isInitialized;
    }

    private class MockNotifyer : INotifyer
    {
        public string Name { get; private set; }

        public MockNotifyer(string name = "Notifyer") => Name = name;
    }

    private List<IChannel> Channels => new() { new MockChannel(), new MockChannel() };

    private INotifyer Notifyer => new MockNotifyer();

    private NotificationContent Content => new(Severity.Information, "Title", "Message");

    private Notification Notification => new(Notifyer, Channels, Content);

    private readonly List<string> _jsonNotifications = new();

    [OneTimeSetUp]
    public void InitializeHistoryFile() => NotificationService.SaveNotification(Notification);

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
        List<Notification> notifications = new();

        // Act
        notifications = NotificationService.GetNotifications();

        // Assert
        Assert.IsNotEmpty(notifications, "The history file is empty.");
    }
}
