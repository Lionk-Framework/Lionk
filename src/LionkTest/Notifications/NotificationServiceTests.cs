// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Lionk.Utils;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

/// <summary>
/// This class is used to test the notification service.
/// </summary>
public class NotificationServiceTests
{
    /// <summary>
    /// Initializes datas for the test.
    /// </summary>
    [SetUp]
    public void Initialize()
    {
        string channelFilePath = Path.Combine("notifications", "channels.json");
        string notifyerFilePath = Path.Combine("notifications", "notifyers.json");
        string notifyerChannelFilePath = Path.Combine("notifications", "notifyerChannels.json");
        ConfigurationUtils.TryDeleteFile(channelFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerChannelFilePath, FolderType.Data);
    }

    /// <summary>
    /// This method tests if the notification service can add a channel and avoid adding the same channel multiple times.
    /// </summary>
    [Test]
    public void MultipleIdenicalChannelsFail()
    {
        // Arrange
        MockChannel channel = new("ChannelNotifiationServiceTest");
        MockChannel copiedChannel = channel;

        // Act
        NotificationService.AddChannels(channel, copiedChannel);

        // Assert
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(1), "The number of channels is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service can add multiple different channels.
    /// </summary>
    [Test]
    public void MultipleDifferentChannelsSuccess()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        // Act
        NotificationService.AddChannels(channel1, channel2);

        // Assert
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(2), "The number of channels is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service can add a recipient and avoid adding the same recipient multiple times.
    /// </summary>
    [Test]
    public void GetRecipientsFromChannel()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockRecipient recipient1 = new("RecipientNotifiationServiceTest1");
        MockRecipient recipient2 = new("RecipientNotifiationServiceTest2");
        MockRecipient recipient3 = new("RecipientNotifiationServiceTest3");

        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        // Act
        channel1.AddRecipients(recipient1, recipient2, recipient3);
        NotificationService.AddChannels(channel1);
        NotificationService.AddChannels(channel2);

        // Assert
        Assert.That(NotificationService.Channels.First().Recipients.Count, Is.EqualTo(3), "The number of recipients is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service can add a notifyer and avoid adding the same notifyer multiple times.
    /// </summary>
    [Test]
    public void MultipleIdenticalNotifyerFail()
    {
        // Arrange
        MockNotifyer notifyer = new("NotifyerNotifiationServiceTest");
        MockNotifyer copiedNotifyer = notifyer;

        // Act
        NotificationService.AddNotifyers(notifyer, copiedNotifyer);

        // Assert
        Assert.That(NotificationService.Notifyers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service can add multiple different notifyers.
    /// </summary>
    [Test]
    public void MultipleDifferentNotifyerSuccess()
    {
        // Arrange
        MockNotifyer notifyer1 = new("NotifyerNotifiationServiceTest1");
        MockNotifyer notifyer2 = new("NotifyerNotifiationServiceTest2");

        // Act
        NotificationService.AddNotifyers(notifyer1, notifyer2);

        // Assert
        Assert.That(NotificationService.Notifyers.Count, Is.EqualTo(2), "The number of notifyers is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service map the notifyer to a channel and add them to the list of Notifyers and Channels if they are not already in the list.
    /// </summary>
    [Test]
    public void InsertNewDictionaryValue()
    {
        // Arrange
        MockNotifyer notifyer = new("NotifyerNotifiationServiceTest");
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        // Act
        NotificationService.MapNotifyerToChannel(notifyer, channel1, channel2);

        // Assert
        Assert.That(NotificationService.NotifyerChannels.Count, Is.EqualTo(1), "The number of notifyer channels is not the same.");
        Assert.That(NotificationService.NotifyerChannels[notifyer.Id].Count, Is.EqualTo(2), "The number of channels inside dictionary is not the same.");
        Assert.That(NotificationService.Notifyers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(2), "The number of channels is not the same.");
    }

    /// <summary>
    /// This method tests if the notification service map the notifyer to a channel and add them to the list of Notifyers and Channels if they are not already in the list.
    /// </summary>
    [Test]
    public void InsertMultipleValueWihSameKey()
    {
        // Arrange
        MockNotifyer notifyer = new("NotifyerNotifiationServiceTest");
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");
        MockChannel channel3 = new("ChannelNotifiationServiceTest3");

        // Act
        NotificationService.MapNotifyerToChannel(notifyer, channel1, channel2);
        NotificationService.MapNotifyerToChannel(notifyer, channel3);

        // Assert
        Assert.That(NotificationService.NotifyerChannels.Count, Is.EqualTo(1), "The number of notifyer channels is not the same.");
        Assert.That(NotificationService.NotifyerChannels[notifyer.Id].Count, Is.EqualTo(3), "The number of channels inside dictionary is not the same.");
        Assert.That(NotificationService.Notifyers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(3), "The number of channels is not the same.");
    }
}
