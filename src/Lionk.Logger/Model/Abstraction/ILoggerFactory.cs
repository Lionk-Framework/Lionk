// Copyright © 2024 Lionk Project

namespace Lionk.Logger;

/// <summary>
/// Interface for creating loggers.
/// </summary>
public interface ILoggerFactory
{
    /// <summary>
    /// Create a logger.
    /// </summary>
    /// <param name="loggerName">The logger name.</param>
    /// <returns>A new <see cref="IStandardLogger"/>.</returns>
    public IStandardLogger CreateLogger(string loggerName);
}
