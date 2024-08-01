// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Events;

namespace Lionk.Logger;

/// <summary>
/// Default Logger class.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SerilogLogger"/> class.
/// </remarks>
/// <param name="path">The path of the log file.</param>
public class SerilogLogger(string path) : IStandardLogger
{
    private readonly ILogger? _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(path, rollingInterval: RollingInterval.Day)
            .CreateLogger();

    /// <inheritdoc/>
    public void Log(LogSeverity severity, string message)
    {
        if (_logger == null)
            return;

        _logger.Write(GetLogEventLevel(severity), message);
    }

    private static LogEventLevel GetLogEventLevel(LogSeverity severity)
        => severity switch
        {
            LogSeverity.Trace => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            LogSeverity.Information => LogEventLevel.Information,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Critical => LogEventLevel.Fatal,
            _ => LogEventLevel.Information,
        };
}
