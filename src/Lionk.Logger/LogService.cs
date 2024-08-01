// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Events;

namespace Lionk.Logger;

/// <summary>
/// Class for logging.
/// </summary>
public static class LogService
{
    private static ICustomLogger? _logger;

    /// <summary>
    /// Configure the logger.
    /// </summary>
    /// <param name="logger">.</param>
    /// <exception cref="ArgumentNullException">a.</exception>
    public static void Configure(ICustomLogger logger)
        => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// .
    /// </summary>
    /// <param name="message">as.</param>
    public static void LogDebug(string message)
        => _logger?.LogDebug(message);

    /// <summary>
    /// asdada.
    /// </summary>
    /// <param name="severity">asd.</param>
    /// <param name="message">dsa.</param>
    public static void LogApp(LogEventLevel severity, string message)
        => _logger?.LogApp(severity, message);

    /// <summary>
    /// asdasd.
    /// </summary>
    /// <param name="loggerName">asdfasdsdf.</param>
    /// <returns>asdfasdf.</returns>
    /// <exception cref="ArgumentNullException">asdasdfsd.</exception>
    public static ILogger CreateLogger(string loggerName)
    {
        string loggerPath = Path.Combine(Utils.DirectoryPath, $"{loggerName}{Utils.LogExtension}");

        ILogger? logger = _logger?.CreateLogger(loggerPath);

        return logger
            ?? throw new ArgumentNullException(nameof(logger));
    }
}
