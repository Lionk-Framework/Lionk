# LionkTest Documentation

## Overview

This documentation is for the `LionkTest` project, which is intended to test the `Lionk` project, plugins and libraries. Currently, all tests are contained within a single file, `Tests.cs`. As our knowledge and requirements grow, we plan to separate tests into individual files.

## Tests.cs Content

The `Tests.cs` file contains the following code:

```csharp
// Copyright © 2024 Lionk Project

namespace LionkTest;

/// <summary>
/// This class will be used for testing the Lionk project.
/// </summary>
public class Tests
{
    /// <summary>
    /// This method is called immediately before each test is run.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// First Test of Lionk Project.
    /// </summary>
    [Test]
    public void FirstTest()
    {
        bool test = true;
        Assert.IsTrue(test);
    }
}
```

## Adding More Tests

To add more tests to the `Tests.cs` file, follow the example below:

### Example: Adding a Second Test

To add a second test, create a new method in the `Tests` class and annotate it with `[Test]`.

```csharp
/// <summary>
/// Second Test of Lionk Project.
/// </summary>
[Test]
public void SecondTest()
{
    int expectedValue = 5;
    int actualValue = 2 + 3;
    Assert.AreEqual(expectedValue, actualValue);
}
```

### Example: Adding a Test with Setup

If you need to initialize some data before running a test, use the `Setup()` method.

```csharp
private int _value;

/// <summary>
/// This method is called immediately before each test is run.
/// </summary>
[SetUp]
public void Setup()
{
    _value = 10;
}

/// <summary>
/// Test that uses the setup value.
/// </summary>
[Test]
public void TestWithSetup()
{
    int expectedValue = 10;
    Assert.AreEqual(expectedValue, _value);
}
```

## Future Plans

Currently, all tests are contained in `Tests.cs`. As we gain more experience and the number of tests grows, we plan to organize tests into separate files for better maintainability and clarity.

For example:

- `LoginTests.cs` for login-related tests
- `DatabaseTests.cs` for database-related tests
- `UITests.cs` for user interface tests

## Conclusion

This document provides an overview of the current test structure for the `LionkTest` project and examples for adding new tests. By following these guidelines, we can ensure that our tests are organized and scalable.