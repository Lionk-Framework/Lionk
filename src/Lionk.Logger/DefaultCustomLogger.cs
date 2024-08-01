// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Events;

namespace Lionk.Logger;

/// <summary>
/// Default Logger class.
/// </summary>
public class DefaultCustomLogger : ICustomLogger
{
    private const string AppLogFilename = "app";
    private const string DebugLogFilename = "debug";

    private readonly ILogger? _appLogger;
    private readonly ILogger? _debugLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCustomLogger"/> class.
    /// </summary>
    public DefaultCustomLogger()
    {
        string appLogFilePath = Path.Combine(Utils.DirectoryPath, $"{AppLogFilename}{Utils.LogExtension}");
        string debugLogFilePath = Path.Combine(Utils.DirectoryPath, $"{DebugLogFilename}{Utils.LogExtension}");
        appLogFilePath = Path.GetFullPath(appLogFilePath);

        _appLogger = CreateLogger(appLogFilePath);
        _debugLogger = CreateLogger(debugLogFilePath);
    }

    /// <inheritdoc/>
    public void LogDebug(string message)
        => _debugLogger?.Debug(message);

    /// <inheritdoc/>
    public void LogApp(LogEventLevel severity, string message)
        => _appLogger?.Write(severity, message);

    /// <inheritdoc/>
    public ILogger CreateLogger(string logName)
        => new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logName, rollingInterval: RollingInterval.Day)
            .CreateLogger();
}
