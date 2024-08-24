// Copyright © 2024 Lionk Project

namespace Lionk.Notification;

/// <summary>
///     This enum define the level of a notification.
/// </summary>
public enum Severity
{
    /// <summary>
    ///     The lowest log level, enable detailed trace logging mainly for application troubleshooting.
    /// </summary>
    Verbose,

    /// <summary>
    ///     Used for application debugging purposes and to inspect run-time outcomes in development environments.
    /// </summary>
    Debug,

    /// <summary>
    ///     Used for application monitoring and to track request and response details or specific operation results.
    /// </summary>
    Information,

    /// <summary>
    ///     Used to review potential non-critical, non-friendly operation outcomes.
    /// </summary>
    Warning,

    /// <summary>
    ///     The most helpful, and yet the most unwanted, log level. Enables detailed error tracking and helps to write error-free applications.
    /// </summary>
    Error,

    /// <summary>
    ///     The most important log level, used to log critical system operations or outcomes that require urgent attention.
    /// </summary>
    Fatal,
}
