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
    /// Initialize the test class.
    /// </summary>
    [OneTimeSetUp]
    public void Initialize()
    {
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockAppLogger = new Mock<IStandardLogger>();
        _mockDebugLogger = new Mock<IStandardLogger>();
    }

    /// <summary>
    /// Test for <see cref="LogService.Configure(ILoggerFactory)"/>.
    /// </summary>
    [Test]
    public void Configure_ShouldInitializeLoggers()
    {
        _mockLoggerFactory.Setup(
            factory => factory.CreateLogger("app"))
            .Returns(_mockAppLogger.Object);

        _mockLoggerFactory.Setup(
            factory => factory.CreateLogger("debug"))
            .Returns(_mockDebugLogger.Object);

        LogService.Configure(_mockLoggerFactory.Object);

        Assert.That(LogService.CreateLogger("app"), Is.Not.Null);
        Assert.That(LogService.CreateLogger("debug"), Is.Not.Null);
    }

    /// <summary>
    /// Test for <see cref="LogService.LogDebug(string)"/>.
    /// </summary>
    [Test]
    public void LogDebug_ShouldCallDebugLogger()
    {
        _mockLoggerFactory.Setup(
            factory => factory.CreateLogger("debug"))
            .Returns(_mockDebugLogger.Object);

        LogService.Configure(_mockLoggerFactory.Object);

        string message = "Debug message";

        LogService.LogDebug(message);

        _mockDebugLogger.Verify(logger => logger.Log(LogSeverity.Debug, message), Times.Once);
    }

    /// <summary>
    /// Test for <see cref="LogService.LogApp(LogSeverity, string)"/>.
    /// </summary>
    [Test]
    public void LogApp_ShouldCallAppLogger()
    {
        _mockLoggerFactory.Setup(
            factory => factory.CreateLogger("app"))
            .Returns(_mockAppLogger.Object);

        LogService.Configure(_mockLoggerFactory.Object);

        LogSeverity severity = LogSeverity.Information;
        string message = "App message";

        LogService.LogApp(severity, message);

        _mockAppLogger.Verify(logger => logger.Log(severity, message), Times.Once);
    }

    private Mock<ILoggerFactory> _mockLoggerFactory;
    private Mock<IStandardLogger> _mockAppLogger;
    private Mock<IStandardLogger> _mockDebugLogger;
}
