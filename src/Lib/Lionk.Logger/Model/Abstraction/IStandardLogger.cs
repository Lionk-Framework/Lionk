// Copyright © 2024 Lionk Project

namespace Lionk.Log;

/// <summary>
///     Interface for custom logger.
/// </summary>
public interface IStandardLogger : IDisposable
{
    #region public and override methods

    /// <summary>
    ///     Log a message with a <see cref="LogSeverity" />.
    /// </summary>
    /// <param name="severity">The severity.</param>
    /// <param name="message">The message to log.</param>
    public void Log(LogSeverity severity, string message);

    #endregion
}
