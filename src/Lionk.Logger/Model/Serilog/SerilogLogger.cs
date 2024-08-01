// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Lionk.Log;

/// <summary>
/// Default Logger class.
/// </summary>
public class SerilogLogger : IStandardLogger
{
    private readonly Logger _logger;

    internal SerilogLogger(string path) => _logger = new LoggerConfiguration()
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

    /// <summary>
    /// Close the logger.
    /// </summary>
    public void CloseLogger()
        => _logger.Dispose();

    /// <inheritdoc/>
    public void Dispose()
    {
        _logger.Dispose();
        GC.SuppressFinalize(this);
    }
}
