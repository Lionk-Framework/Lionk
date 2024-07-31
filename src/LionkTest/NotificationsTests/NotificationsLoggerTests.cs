// Copyright © 2024 Lionk Project

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Classes;
using Notifications.Interfaces;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

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

    private readonly List<string> _jsonNotifications = new();

    [OneTimeSetUp]
    public void InitializeHistoryFile()
    {
        NotificationLogger.LogNotification(Notification);
        NotificationLogger.CloseLogger();

        // Assert
        if (!File.Exists(NotificationLogger.HistoryFilePath))
        {
            Assert.Fail("The history file was not created.");
        }

        // Finaly read the file
        using (var fileStream = new FileStream(NotificationLogger.HistoryFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(fileStream))
        {
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                _jsonNotifications.Add(line);
            }
        }
    }

    [Test]
    public void TestContent()
    {
        // Arrange
        // Nothing to arrange

        // Act
        // nothing to act

        // Assert
        if (_jsonNotifications.Count() < 1)
        {
            Assert.Fail("The history file is empty or not correctly written.");
        }
    }

    [Test]
    public void TestDeserializationHistory()
    {
        // Arrange
        // Nothing to arrange

        // Act
        dynamic? logEntry = JsonConvert.DeserializeObject(_jsonNotifications.Last());

        // Assert
        if (logEntry is null)
        {
            Assert.Fail("The last notification in the history file can't be deserialized.");
        }
    }

    [Test]
    public void TestDeserializationNotification()
    {
        // Arrange
        // Nothing to arrange

        // Act
        dynamic? logEntry = JsonConvert.DeserializeObject<dynamic>(_jsonNotifications.Last());
        string jsonLog = logEntry?.ToString() ?? string.Empty;

        LogEvent? notificationLog = LogEventReader.ReadFromJObject(JObject.Parse(jsonLog));

        // Assert
        Assert.NotNull(notificationLog, "The notification log is empty.");
    }

    public void TestDeserializationNotification2()
    {
        // Arrange
        // Nothing to arrange

        // Act
        dynamic? logEntry = JsonConvert.DeserializeObject(_jsonNotifications.Last());
        JToken? jToken = logEntry?.Notification;
        string jsonNotification = jToken?.Value<string>() ?? string.Empty;

        Notification? notification = JsonConvert.DeserializeObject<Notification>(jsonNotification);

        // Assert
        if (notification is null)
        {
            Assert.Fail("The notification file is empty.");
        }
    }
}
