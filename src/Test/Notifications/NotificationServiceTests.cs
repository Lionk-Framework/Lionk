// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Lionk.Utils;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

/// <summary>
///     This class is used to test the notification service.
/// </summary>
public class NotificationServiceTests
{
    #region public and override methods

    /// <summary>
    ///     This method verifies that the channel is used by correct notifyers.
    /// </summary>
    [Test]
    public void CheckWhoUseTheChannel()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        MockNotifier notifyer1 = new("NotifyerNotifiationServiceTest1");
        MockNotifier notifyer2 = new("NotifyerNotifiationServiceTest2");
        MockNotifier notifyer3 = new("NotifyerNotifiationServiceTest3");

        // Act
        NotificationService.AddChannels(channel1);
        NotificationService.AddNotifiers(notifyer1);
        NotificationService.AddNotifiers(notifyer2);
        NotificationService.AddNotifiers(notifyer3);
        NotificationService.MapNotifierToChannel(notifyer1, channel1);
        NotificationService.MapNotifierToChannel(notifyer2, channel1);
        NotificationService.MapNotifierToChannel(notifyer3, channel2);

        List<INotifier> notifyers = NotificationService.WhoUseThisChannel(channel1);
        List<INotifier> notifyers2 = NotificationService.WhoUseThisChannel(channel2);

        // Assert
        Assert.That(notifyers.Count, Is.EqualTo(2), "The number of notifyers is not the same.");
        Assert.That(notifyers2.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service can add a recipient and avoid adding the same recipient multiple times.
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
    ///     This method tests if an inexistant channel called to be removed is ignored.
    /// </summary>
    [Test]
    public void IgnoreRemoveInexistantChannel()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        // Act
        NotificationService.AddChannels(channel1);
        NotificationService.RemoveChannel(channel2);

        // Assert
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(1), "The number of channels is not the same.");
    }

    /// <summary>
    ///     This method tests if an inexistant notifier called to be removed is ignored.
    /// </summary>
    [Test]
    public void IgnoreRemoveInexistantNotifyer()
    {
        // Arrange
        MockNotifier notifyer1 = new("NotifyerNotifiationServiceTest1");
        MockNotifier notifyer2 = new("NotifyerNotifiationServiceTest2");

        // Act
        NotificationService.AddNotifiers(notifyer1);
        NotificationService.RemoveNotifier(notifyer2);

        // Assert
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
    }

    /// <summary>
    ///     Initializes datas for the test.
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
    ///     This method tests if the notification service map the notifier to a channel and add them to the list of Notifiers and Channels if they
    ///     are not already in the list.
    /// </summary>
    [Test]
    public void InsertMultipleValueWihSameKey()
    {
        // Arrange
        MockNotifier notifier = new("NotifyerNotifiationServiceTest");
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");
        MockChannel channel3 = new("ChannelNotifiationServiceTest3");

        // Act
        NotificationService.MapNotifierToChannel(notifier, channel1, channel2);
        NotificationService.MapNotifierToChannel(notifier, channel3);

        // Assert
        Assert.That(NotificationService.NotifierChannels.Count, Is.EqualTo(1), "The number of notifier channels is not the same.");
        Assert.That(
            NotificationService.NotifierChannels[notifier.Id].Count,
            Is.EqualTo(3),
            "The number of channels inside dictionary is not the same.");
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(3), "The number of channels is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service map the notifier to a channel and add them to the list of Notifiers and Channels if they
    ///     are not already in the list.
    /// </summary>
    [Test]
    public void InsertNewDictionaryValue()
    {
        // Arrange
        MockNotifier notifier = new("NotifyerNotifiationServiceTest");
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");

        // Act
        NotificationService.MapNotifierToChannel(notifier, channel1, channel2);

        // Assert
        Assert.That(NotificationService.NotifierChannels.Count, Is.EqualTo(1), "The number of notifier channels is not the same.");
        Assert.That(
            NotificationService.NotifierChannels[notifier.Id].Count,
            Is.EqualTo(2),
            "The number of channels inside dictionary is not the same.");
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(2), "The number of channels is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service can add multiple different channels.
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
    ///     This method tests if the notification service can add multiple different notifyers.
    /// </summary>
    [Test]
    public void MultipleDifferentNotifyerSuccess()
    {
        // Arrange
        MockNotifier notifyer1 = new("NotifyerNotifiationServiceTest1");
        MockNotifier notifyer2 = new("NotifyerNotifiationServiceTest2");

        // Act
        NotificationService.AddNotifiers(notifyer1, notifyer2);

        // Assert
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(2), "The number of notifyers is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service can add a channel and avoid adding the same channel multiple times.
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
    ///     This method tests if the notification service can add a notifier and avoid adding the same notifier multiple times.
    /// </summary>
    [Test]
    public void MultipleIdenticalNotifyerFail()
    {
        // Arrange
        MockNotifier notifier = new("NotifyerNotifiationServiceTest");
        MockNotifier copiedNotifier = notifier;

        // Act
        NotificationService.AddNotifiers(notifier, copiedNotifier);

        // Assert
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(1), "The number of notifyers is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service can remove a channel from the list of channels.
    /// </summary>
    [Test]
    public void RemoveChannelFromListAndFromDictionary()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");
        MockNotifier notifier = new("NotifyerNotifiationServiceTest");

        // Act
        NotificationService.AddChannels(channel1, channel2);
        NotificationService.AddNotifiers(notifier);
        NotificationService.MapNotifierToChannel(notifier, channel1, channel2);
        int currentChannelCount = NotificationService.Channels.Count;
        int currentNotifyerChannelCount = NotificationService.NotifierChannels[notifier.Id].Count;

        NotificationService.RemoveChannel(channel1);

        // Assert
        Assert.That(NotificationService.Channels.Count, Is.EqualTo(currentChannelCount - 1), "The number of channels is not the same.");
        Assert.That(
            NotificationService.NotifierChannels[notifier.Id].Count,
            Is.EqualTo(currentNotifyerChannelCount - 1),
            "The number of channels inside dictionary is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service can remove a channel from many notifyers.
    /// </summary>
    [Test]
    public void RemoveChannelFromManyNotifyers()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockChannel channel2 = new("ChannelNotifiationServiceTest2");
        MockNotifier notifyer1 = new("NotifyerNotifiationServiceTest");
        MockNotifier notifyer2 = new("NotifyerNotifiationServiceTest2");

        // Act
        NotificationService.AddChannels(channel1, channel2);
        NotificationService.AddNotifiers(notifyer1);
        NotificationService.AddNotifiers(notifyer2);
        NotificationService.MapNotifierToChannel(notifyer1, channel1, channel2);
        NotificationService.MapNotifierToChannel(notifyer2, channel1, channel2);
        int notifyerChannelCount1 = NotificationService.NotifierChannels[notifyer1.Id].Count;
        int notifyerChannelCount2 = NotificationService.NotifierChannels[notifyer2.Id].Count;

        NotificationService.RemoveChannel(channel1);

        // Assert
        Assert.That(
            NotificationService.NotifierChannels[notifyer1.Id].Count,
            Is.EqualTo(notifyerChannelCount1 - 1),
            "The number of channels inside dictionary is not the same.");
        Assert.That(
            NotificationService.NotifierChannels[notifyer2.Id].Count,
            Is.EqualTo(notifyerChannelCount2 - 1),
            "The number of channels inside dictionary is not the same.");
    }

    /// <summary>
    ///     This method tests if a notifier can be removed from the list of notifyers and from the dictionary.
    /// </summary>
    [Test]
    public void RemoveNotifyerFromListAndFromDictionary()
    {
        // Arrange
        MockChannel channel1 = new("ChannelNotifiationServiceTest1");
        MockNotifier notifyer1 = new("NotifyerNotifiationServiceTest");
        MockNotifier notifyer2 = new("NotifyerNotifiationServiceTest2");

        // Act
        NotificationService.AddChannels(channel1);
        NotificationService.AddNotifiers(notifyer1);
        NotificationService.AddNotifiers(notifyer2);
        NotificationService.MapNotifierToChannel(notifyer1, channel1);
        NotificationService.MapNotifierToChannel(notifyer2, channel1);
        int currentNotifyerCount = NotificationService.Notifiers.Count;
        int currentNotifyerChannelCount = NotificationService.NotifierChannels.Count;

        NotificationService.RemoveNotifier(notifyer1);

        // Assert
        Assert.That(NotificationService.Notifiers.Count, Is.EqualTo(currentNotifyerCount - 1), "The number of notifyers is not the same.");
        Assert.That(
            NotificationService.NotifierChannels.Count,
            Is.EqualTo(currentNotifyerChannelCount - 1),
            "The number of notifier channels is not the same.");
    }

    /// <summary>
    ///     This method tests if the notification service ignore sending a notification if the channel is null.
    /// </summary>
    [Test]
    public void SendWithNullChannelMustBeIgnored()
    {
        // Arrange
        MockNotifier notifier = new("NotifyerNotifiationServiceTest1");

        Content content = new(Severity.Information, "Test", "Test");
        Notification notification = new(content, notifier);

        // Act
        NotificationService.AddNotifiers(notifier);

        NotificationService.Send(notification);

        // Assert
        Assert.Pass();
    }

    #endregion
}
