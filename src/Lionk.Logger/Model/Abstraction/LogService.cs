// Copyright © 2024 Lionk Project

namespace Lionk.Log;

/// <summary>
/// Static class for logging this class must
/// be used to log messages and create loggers.
/// </summary>
public static class LogService
{
    private const string AppLogFilename = "app";
    private const string DebugLogFilename = "debug";

    private static IStandardLogger? _appLogger;
    private static IStandardLogger? _debugLogger;
    private static ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Configure the logger.
    /// </summary>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <exception cref="ArgumentNullException">Throw
    /// an argument null exception if loggerFactory is null.</exception>
    public static void Configure(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _appLogger = _loggerFactory.CreateLogger(AppLogFilename);
        _debugLogger = _loggerFactory.CreateLogger(DebugLogFilename);
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="message">Debug message.</param>
    public static void LogDebug(string message)
        => _debugLogger?.Log(LogSeverity.Debug, message);

    /// <summary>
    /// Log an application message.
    /// </summary>
    /// <param name="severity">Log severity.</param>
    /// <param name="message">Log message.</param>
    public static void LogApp(LogSeverity severity, string message)
        => _appLogger?.Log(severity, message);

    /// <summary>
    /// Create a new <see cref="IStandardLogger"/>.
    /// </summary>
    /// <param name="loggerName">The name of the logger.</param>
    /// <returns>A new <see cref="IStandardLogger"/>.</returns>
    public static IStandardLogger? CreateLogger(string loggerName)
        => _loggerFactory?.CreateLogger(loggerName);
}
