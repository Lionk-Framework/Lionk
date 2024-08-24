// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Lionk.Log;

/// <summary>
///     Default Logger class.
/// </summary>
internal class SerilogLogger : IStandardLogger
{
    #region fields

    private readonly Logger _logger;

    #endregion

    #region constructors

    internal SerilogLogger(string path) =>
        _logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(path, rollingInterval: RollingInterval.Day).CreateLogger();

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public void Dispose()
    {
        _logger.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public void Log(LogSeverity severity, string message)
        => _logger.Write(GetLogEventLevel(severity), message);

    #endregion

    #region others methods

    private static LogEventLevel GetLogEventLevel(LogSeverity severity) =>
        severity switch
        {
            LogSeverity.Trace => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            LogSeverity.Information => LogEventLevel.Information,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Critical => LogEventLevel.Fatal,
            _ => LogEventLevel.Information,
        };

    #endregion
}
