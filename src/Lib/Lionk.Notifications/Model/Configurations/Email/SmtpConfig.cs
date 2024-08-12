// Copyright © 2024 Lionk Project

namespace Lionk.Notification.Email;

/// <summary>
/// This class define the configuration of the SMTP server.
/// </summary>
public class SmtpConfig
{
    /// <summary>
    /// Gets or sets The SMTP server.
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the port of the SMTP port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the SSL is enabled.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Gets or sets the username used to authenticate to the SMTP server.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password used to authenticate to the SMTP server.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
