// Copyright © 2024 Lionk Project

using Newtonsoft.Json;
using Notifications.Classes;
using Notifications.Interfaces;

namespace LionkTest.NotificationsTests;

internal class NotificationsLoggerTests
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

    private NotificationContent Content => new(Serilog.Events.LogEventLevel.Information, "Title", "Message");

    private Notification Notification => new(Notifyer, Channels, Content);

    private string _fileContent;

    [OneTimeSetUp]
    public void InitializeHistoryFile()
    {
        NotificationLogger.LogNotification(Notification);

        // Assert
        if (!File.Exists(NotificationLogger.HistoryFilePath))
        {
            Assert.Fail("The history file was not created.");
        }

        // Finaly
        _fileContent = File.ReadAllText(NotificationLogger.HistoryFilePath);
    }

    [Test]
    public void TestContent()
    {
        // Arrange
        // Nothing to arrange

        // Act
        // nothing to act

        // Assert
        if (string.IsNullOrEmpty(_fileContent))
        {
            Assert.Fail("The history file is empty.");
        }
    }

    [Test]
    public void TestDeserializationHistory()
    {
        // Arrange
        // Nothing to arrange

        // Act
        dynamic? logEntry = JsonConvert.DeserializeObject(_fileContent);

        // Assert
        if (logEntry is null)
        {
            Assert.Fail("The history file is empty.");
        }
    }

    [Test]
    public void TestDeserializationNotification()
    {
        // Arrange
        // Nothing to arrange

        // Act
        dynamic? logEntry = JsonConvert.DeserializeObject(_fileContent);
        Notification? notification = JsonConvert.DeserializeObject<Notification>(logEntry?.Notification.ToString());

        // Assert
        if (notification is null)
        {
            Assert.Fail("The notification file is empty.");
        }
    }
}
