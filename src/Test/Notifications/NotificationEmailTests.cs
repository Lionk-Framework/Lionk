// Copyright © 2024 Lionk Project

using Lionk.Notification;
using Lionk.Notification.Email;

namespace LionkTest.Notifications;

/// <summary>
/// This class is used to test the email notification.
/// </summary>
public class NotificationEmailTests
{
    private class MockNotifyer : INotifyer
    {
        public string Name => "TestNotifyer";
    }

    private Content _content;
    private EmailChannel _emailChannel;
    private Notification _notification;
    private MockNotifyer _notifyer;

    /// <summary>
    /// Initializes datas for the test.
    /// </summary>
    [OneTimeSetUp]
    public void Initialize()
    {
        // Arrange
        _notifyer = new MockNotifyer();
        _emailChannel = new EmailChannel("Email Channel");
        _content = new Content(Severity.Information, "Title", "Message");
        _notification = new Notification(_content, _notifyer);
        NotificationService.Channels.Add(_emailChannel);
        NotificationService.Notifyers.Add(_notifyer);
        NotificationService.MapNotifyerToChannel(_notifyer, _emailChannel);
        SmtpServerTest.Start();
    }

    /// <summary>
    /// Clean up the test.
    /// </summary>
    [OneTimeTearDown]
    public void Cleanup() => SmtpServerTest.Stop();

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
        Notification notification = new(_content, _notifyer);

        // Act
        _emailChannel.CreatSmtpConfigurationFile(smtpServer, port, enableSsl, username, password);
        _emailChannel.Initialize();
        _emailChannel.AddRecipient(new EmailRecipients("Recipient", "recipient@email.test"));
        NotificationService.Send(notification);

        // Assert
        Assert.That(SmtpServerTest.Mailbox.Count, Is.EqualTo(1));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("Title"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("Message"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("notifyer@email.test"));
        Assert.That(SmtpServerTest.Mailbox.First(), Does.Contain("recipient@email.test"));
    }
}
