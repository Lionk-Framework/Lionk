// Copyright © 2024 Lionk Project

using Lionk.Log;
using Moq;

namespace LionkTest.Logger;

/// <summary>
///     Test class for <see cref="ILoggerFactory" />.
/// </summary>
public class LoggerFactoryTests
{
    #region public and override methods

    /// <summary>
    ///     Test for <see cref="ILoggerFactory.CreateLogger(string)" />.
    /// </summary>
    [Test]
    public void CreateLogger_ShouldReturnIStandardLogger()
    {
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockStandardLogger = new Mock<IStandardLogger>();
        string loggerName = "TestLogger";

        mockLoggerFactory.Setup((ILoggerFactory factory) => factory.CreateLogger(loggerName)).Returns(mockStandardLogger.Object);

        IStandardLogger logger = mockLoggerFactory.Object.CreateLogger(loggerName);

        Assert.That(logger, Is.Not.Null);
        Assert.That(logger, Is.InstanceOf<IStandardLogger>());
    }

    #endregion
}
