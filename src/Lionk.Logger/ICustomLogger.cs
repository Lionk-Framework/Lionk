// Copyright © 2024 Lionk Project

using Serilog;
using Serilog.Events;

namespace Lionk.Logger;

/// <summary>
/// Interface for custom logger.
/// </summary>
public interface ICustomLogger
{
    /// <summary>
    /// rtest.
    /// </summary>
    /// <param name="message">asd,.</param>
    public void LogDebug(string message);

    /// <summary>
    /// asd.
    /// </summary>
    /// <param name="severity">assd.</param>
    /// <param name="message">tg.</param>
    public void LogApp(LogEventLevel severity, string message);

    /// <summary>
    /// Create a logger.
    /// </summary>
    /// <param name="filePath">fp.</param>
    /// <returns>asd.</returns>
    public ILogger CreateLogger(string filePath);
}
