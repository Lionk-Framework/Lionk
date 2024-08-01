// Copyright © 2024 Lionk Project

using Lionk.Log;
using Lionk.Log.Serilog;

namespace LionkTest;

/// <summary>
/// Test class for <see cref="SerilogLogger"/>.
/// </summary>
public class SerilogLoggerTests
{
    /// <summary>
    /// Method to test <see cref="SerilogLogger.Log(LogSeverity, string)"/>.
    /// </summary>
    [Test]
    public void Log_ShouldWriteLogToFile()
    {
        string loggerName = "test";

        string path = Path.Combine(Utils.DirectoryPath, loggerName);

        path = Path.GetFullPath(path
            + DateOnly.FromDateTime(DateTime.Now).ToString("yyyyMMdd")
            + $"{Utils.LogExtension}");

        if (File.Exists(path))
            File.Delete(path);

        IStandardLogger logger = new SerilogFactory().CreateLogger(loggerName);
        LogSeverity severity = LogSeverity.Information;
        string message = "Test log message";

        logger.Log(severity, message);
        logger.Dispose();

        Assert.That(File.Exists(path), Is.True);
        string logContents = File.ReadAllText(path);
        Assert.That(logContents.Contains("Test log message"), Is.True);
    }

    /// <summary>
    /// Method to test <see cref="SerilogLogger.Log(LogSeverity, string)"/>.
    /// </summary>
    [Test]
    public void CreateLogger_ShouldCreateFileLogger()
    {
        var factory = new SerilogFactory();
        string loggerName = "custom";

        string path = Path.Combine(Utils.DirectoryPath, loggerName);

        path = Path.GetFullPath(path
            + DateOnly.FromDateTime(DateTime.Now).ToString("yyyyMMdd")
            + $"{Utils.LogExtension}");

        if (File.Exists(path))
            File.Delete(path);

        IStandardLogger? logger = factory.CreateLogger(loggerName);
        logger?.Log(LogSeverity.Information, "Testing custom file logger");

        Assert.That(logger, Is.Not.Null);
        Assert.That(File.Exists(path), Is.True);
    }
}
