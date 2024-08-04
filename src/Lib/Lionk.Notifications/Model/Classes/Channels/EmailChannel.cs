// Copyright © 2024 Lionk Project

using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;

namespace Lionk.Notification.Email;

/// <summary>
/// This class define an email channel.
/// </summary>
public class EmailChannel : IChannel
{
    private SmtpClient? _smtpClient;
    private SmtpConfig? _smtpConfig;
    private bool _isInitialized;

    /// <inheritdoc/>
    public string Name { get; private set; }

    /// <inheritdoc/>
    public List<IRecipient> Recipients { get; private set; }

    /// <inheritdoc/>
    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailChannel"/> class.
    /// </summary>
    /// <param name="name">The name of the channel.</param>
    public EmailChannel(string name)
    {
        Name = name;
        Recipients = new List<IRecipient>();
        _isInitialized = false;
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        string configFilePath = "path/to/smtpconfig.json";
        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException("The SMTP configuration file was not found.", configFilePath);
        }

        string configJson = File.ReadAllText(configFilePath);
        _smtpConfig = JsonConvert.DeserializeObject<SmtpConfig>(configJson);

        if (_smtpConfig == null)
        {
            throw new InvalidOperationException("The SMTP configuration is invalid.");
        }

        _smtpClient = new SmtpClient(_smtpConfig.SmtpServer, _smtpConfig.Port);
        _smtpClient.EnableSsl = _smtpConfig.EnableSsl;
        _smtpClient.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);

        _isInitialized = true;
    }

    /// <inheritdoc/>
    public void Send(INotifyer notifyer, Content content)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(notifyer);
        if (!_isInitialized) throw new InvalidOperationException("EmailChannel is not initialized.");
        if (_smtpConfig is null) throw new InvalidOperationException("SMTP configuration is not initialized.");
        if (_smtpClient is null) throw new InvalidOperationException("SMTP client is not initialized.");

        foreach (IRecipient recipient in Recipients)
        {
            var emailRecipient = (EmailRecipients)recipient;
            var mailMessage = new MailMessage(_smtpConfig.Username, emailRecipient.Email);
            mailMessage.Subject = content.Title;
            mailMessage.Body = content.Message;
            _smtpClient.Send(mailMessage);
        }
    }

    /// <summary>
    /// Adds a recipient to the channel.
    /// </summary>
    /// <param name="recipient">The recipient to add.</param>
    public void AddRecipient(IRecipient recipient)
    {
        if (recipient is not EmailRecipients) throw new InvalidOperationException("Recipient is not an email recipient.");
        Recipients.Add(recipient);
    }
}
