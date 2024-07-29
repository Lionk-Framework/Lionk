// Copyright © 2024 Lionk Project

namespace LionkTest;

/// <summary>
/// This class will be used for testing the Lionk project.
/// </summary>
public class Tests
{
    /// <summary>
    /// This methode is called immediately before each test is run.
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
