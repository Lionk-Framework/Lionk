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
    private const string ConfigFileFolder = "Notifications";
    private const string ConfigFileName = "smtpconfig.json";
    private SmtpClient? _smtpClient;
    private SmtpConfig? _smtpConfig;

    private string ConfigFilePath => Path.Combine(ConfigFileFolder, ConfigFileName);

    /// <inheritdoc/>
    public string Name { get; private set; } = string.Empty;

    /// <inheritdoc/>
    public List<IRecipient> Recipients { get; private set; } = new();

    /// <inheritdoc/>
    [JsonIgnore]
    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailChannel"/> class.
    /// </summary>
    /// <param name="name">The name of the channel.</param>
    public EmailChannel(string name)
    {
        Name = name;
        Recipients = new List<IRecipient>();
        IsInitialized = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailChannel"/> class.
    /// </summary>
    /// <param name="name"> The name of the channel.</param>
    /// <param name="recipients"> The list of recipients.</param>
    public EmailChannel(string name, LinkedList<IRecipient> recipients)
    {
        Name = name;
        Recipients = new List<IRecipient>(recipients);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailChannel"/> class.
    /// </summary>
    public EmailChannel()
    {
    }

    /// <inheritdoc/>
    public void Initialize()
    {
        if (!File.Exists(ConfigFilePath))
        {
            throw new FileNotFoundException("The SMTP configuration file was not found.", ConfigFilePath);
        }

        string configJson = File.ReadAllText(ConfigFilePath);
        _smtpConfig = JsonConvert.DeserializeObject<SmtpConfig>(configJson);

        if (_smtpConfig == null)
        {
            throw new InvalidOperationException("The SMTP configuration is invalid.");
        }

        _smtpClient = new SmtpClient(_smtpConfig.SmtpServer, _smtpConfig.Port);
        _smtpClient.EnableSsl = _smtpConfig.EnableSsl;
        _smtpClient.Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);

        IsInitialized = true;
    }

    /// <inheritdoc/>
    public void Send(INotifyer notifyer, Content content)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(notifyer);
        if (!IsInitialized) throw new InvalidOperationException("EmailChannel is not initialized.");
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

    /// <inheritdoc/>
    public void AddRecipient(IRecipient recipient)
    {
        if (recipient is not EmailRecipients) throw new InvalidOperationException("Recipient is not an email recipient.");
        Recipients.Add(recipient);
    }

    /// <summary>
    /// Method used to create the SMTP configuration file.
    /// </summary>
    /// <param name="smtpServer"> The SMTP server.</param>
    /// <param name="port"> The port of the SMTP server.</param>
    /// <param name="enableSsl"> A value indicating whether the SSL is enabled.</param>
    /// <param name="username"> The username used to authenticate to the SMTP server.</param>
    /// <param name="password"> The password used to authenticate to the SMTP server.</param>
    public void CreatSmtpConfigurationFile(string smtpServer, int port, bool enableSsl, string username, string password)
    {
        if (!Directory.Exists(ConfigFileFolder)) Directory.CreateDirectory(ConfigFileFolder);

        var smtpConfig = new SmtpConfig
        {
            SmtpServer = smtpServer,
            Port = port,
            EnableSsl = enableSsl,
            Username = username,
            Password = password,
        };

        string json = JsonConvert.SerializeObject(smtpConfig);
        File.WriteAllText(ConfigFilePath, json);
    }
}
