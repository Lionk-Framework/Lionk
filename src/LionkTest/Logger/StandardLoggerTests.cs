// Copyright © 2024 Lionk Project

using Lionk.Log;
using Moq;

namespace LionkTest;

/// <summary>
/// Test class for <see cref="IStandardLogger"/>.
/// </summary>
public class StandardLoggerTests
{
    /// <summary>
    /// Test for <see cref="IStandardLogger.Log(LogSeverity, string)"/>.
    /// </summary>
    [Test]
    public void Log_ShouldCallLogMethodWithCorrectParameters()
    {
        var mockStandardLogger = new Mock<IStandardLogger>();
        LogSeverity severity = LogSeverity.Information;
        string message = "Test message";

        mockStandardLogger.Object.Log(severity, message);

        mockStandardLogger.Verify(logger => logger.Log(severity, message), Times.Once);
    }
}
