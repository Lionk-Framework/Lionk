// Copyright © 2024 Lionk Project

namespace Lionk.Log;

/// <summary>
///     Interface for creating loggers.
/// </summary>
public interface ILoggerFactory
{
    #region public and override methods

    /// <summary>
    ///     Create a logger.
    /// </summary>
    /// <param name="loggerName">The logger name.</param>
    /// <returns>A new <see cref="IStandardLogger" />.</returns>
    public IStandardLogger CreateLogger(string loggerName);

    #endregion
}
