// Copyright © 2024 Lionk Project

namespace Lionk.Logger;

/// <summary>
/// Interface for custom logger.
/// </summary>
public interface IStandardLogger
{
    /// <summary>
    /// Log a message to the debug file use for debuging purpose, this log is disabled when the
    /// app is compiled in release mode.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogDebug(string message);

    /// <summary>
    /// Log a message with a <see cref="LogSeverity"/> to the default log file.
    /// </summary>
    /// <param name="severity">The severity.</param>
    /// <param name="message">The message to log.</param>
    public void LogApp(LogSeverity severity, string message);

    /// <summary>
    /// Create a logger.
    /// </summary>
    /// <param name="loggerName">The logger name.</param>
    /// <returns>A new <see cref="IStandardLogger"/>.</returns>
    public IStandardLogger CreateLogger(string loggerName);
}
