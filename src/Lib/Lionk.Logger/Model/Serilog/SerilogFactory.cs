// Copyright © 2024 Lionk Project

using Lionk.Utils;

namespace Lionk.Log.Serilog;

/// <summary>
/// A factory for creating Serilog loggers.
/// </summary>
public class SerilogFactory : ILoggerFactory
{
    /// <inheritdoc/>
    public IStandardLogger CreateLogger(string loggerName)
    {
        string logFilePath =
            Path.Combine(
                ConfigurationUtils.GetFolderPath(FolderType.Logs),
                $"{loggerName}{Utils.LogExtension}");

        logFilePath = Path.GetFullPath(logFilePath);

        return new SerilogLogger(logFilePath);
    }
}
