// Copyright © 2024 Lionk Project

namespace Lionk.Logger.Serilog;

/// <summary>
/// A factory for creating Serilog loggers.
/// </summary>
public class SerilogFactory : ILoggerFactory
{
    /// <inheritdoc/>
    public IStandardLogger CreateLogger(string loggerName)
    {
        string logFilePath = Path.Combine(Utils.DirectoryPath, $"{loggerName}{Utils.LogExtension}");
        logFilePath = Path.GetFullPath(logFilePath);

        return new SerilogLogger(logFilePath);
    }
}
