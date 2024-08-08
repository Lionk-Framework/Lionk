﻿// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Lionk.Notification.Email;
using Lionk.Utils;
using LionkTest.Notifications.Mock;

namespace LionkTest.Notifications;

/// <summary>
/// This class is used to test the email notification.
/// </summary>
public class NotificationEmailTests
{
    private Content _content;
    private EmailChannel _emailChannel;
    private Notification _notification;
    private MockNotifyer _notifyer;

    /// <summary>
    /// One time setup the test.
    /// </summary>
    [OneTimeSetUp]
    public void Initialize() => SmtpServerTest.Start();

    /// <summary>
    /// Clean up the test.
    /// </summary>
    [OneTimeTearDown]
    public void Cleanup() => SmtpServerTest.Stop();

    /// <summary>
    /// Set up the test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Clear the files
        string channelFilePath = Path.Combine("notifications", "channels.json");
        string notifyerFilePath = Path.Combine("notifications", "notifyers.json");
        string notifyerChannelFilePath = Path.Combine("notifications", "notifyerChannels.json");
        string notificationFilePath = Path.Combine("notifications", "notifications.json");
        string smtpConfigurationFilePath = Path.Combine("notifications", "smtpConfiguration.json");
        ConfigurationUtils.TryDeleteFile(channelFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notifyerChannelFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(notificationFilePath, FolderType.Data);
        ConfigurationUtils.TryDeleteFile(smtpConfigurationFilePath, FolderType.Config);
        _notifyer = new MockNotifyer("EmailTestNotifyer");
        _content = new Content(Severity.Information, "Title", "Message");
        _notification = new Notification(_content, _notifyer);
        _emailChannel = new EmailChannel("Email Channel");
    }

    /// <summary>
    /// Test the email notification.
    /// </summary>
    [Test]
    public void SendEmailNotification()
    {
        // Arrange
        string smtpServer = "localhost";
        int port = 2526;
        bool enableSsl = false;
        string username = "notifyer@email.test";
        string password = "passwordTest";

        // Act
        _emailChannel.SetSmtpConfiguration(smtpServer, port, enableSsl, username, password);
        _emailChannel.Initialize();
        _emailChannel.AddRecipients(new EmailRecipients("Recipient", "recipient@email.test"));
        NotificationService.MapNotifyerToChannel(_notifyer, _emailChannel);
        Notification notification = new(_content, _notifyer);
        NotificationService.Send(notification);

        // Assert
        Assert.That(SmtpServerTest.Mailbox.Count, Is.EqualTo(1));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("Title"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("Message"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("notifyer@email.test"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("recipient@email.test"));
    }
}