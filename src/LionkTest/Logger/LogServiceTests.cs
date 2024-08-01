// Copyright © 2024 Lionk Project

using Lionk.Log;
using Moq;

namespace LionkTest;

/// <summary>
/// Test class for <see cref="LogService"/>.
/// </summary>
public class LogServiceTests
{
    /// <summary>
    /// Test for <see cref="LogService.Configure(ILoggerFactory)"/>.
    /// </summary>
    [Test]
    public void Configure_ShouldInitializeLoggers()
    {
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockAppLogger = new Mock<IStandardLogger>();
        var mockDebugLogger = new Mock<IStandardLogger>();

        mockLoggerFactory.Setup(factory => factory.CreateLogger("app")).Returns(mockAppLogger.Object);
        mockLoggerFactory.Setup(factory => factory.CreateLogger("debug")).Returns(mockDebugLogger.Object);

        LogService.Configure(mockLoggerFactory.Object);

        Assert.That(LogService.CreateLogger("app"), Is.Not.Null);
        Assert.That(LogService.CreateLogger("debug"), Is.Not.Null);
    }

    /// <summary>
    /// Test for <see cref="LogService.LogDebug(string)"/>.
    /// </summary>
    [Test]
    public void LogDebug_ShouldCallDebugLogger()
    {
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockDebugLogger = new Mock<IStandardLogger>();

        mockLoggerFactory.Setup(factory => factory.CreateLogger("debug")).Returns(mockDebugLogger.Object);
        LogService.Configure(mockLoggerFactory.Object);

        string message = "Debug message";

        LogService.LogDebug(message);

        mockDebugLogger.Verify(logger => logger.Log(LogSeverity.Debug, message), Times.Once);
    }

    /// <summary>
    /// Test for <see cref="LogService.LogApp(LogSeverity, string)"/>.
    /// </summary>
    [Test]
    public void LogApp_ShouldCallAppLogger()
    {
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockAppLogger = new Mock<IStandardLogger>();

        mockLoggerFactory.Setup(factory => factory.CreateLogger("app")).Returns(mockAppLogger.Object);
        LogService.Configure(mockLoggerFactory.Object);

        LogSeverity severity = LogSeverity.Information;
        string message = "App message";

        LogService.LogApp(severity, message);

        mockAppLogger.Verify(logger => logger.Log(severity, message), Times.Once);
    }
}
