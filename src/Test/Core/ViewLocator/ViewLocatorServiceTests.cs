// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Core.Component;
using Lionk.Core.TypeRegister;
using Lionk.Core.View;
using Moq;

namespace LionkTest.Core;

/// <summary>
///     Tests for the <see cref="ViewLocatorService" /> class.
/// </summary>
[TestFixture]
public class ViewLocatorServiceTests
{
    #region fields

    private Mock<ITypesProvider> _mockTypesProvider;

    private ViewLocatorService _viewLocatorService;

    #endregion

    #region public and override methods

    /// <summary>
    ///     Test to ensure that new types are registered when available.
    /// </summary>
    [Test]
    public void OnNewTypesAvailable_ShouldAddNewViews_WhenNewTypesAreProvided()
    {
        var typesEventArgs = new TypesEventArgs(new[] { typeof(TestView) });
        _mockTypesProvider.Raise((ITypesProvider provider) => provider.NewTypesAvailable += null, typesEventArgs);

        IEnumerable<ComponentViewDescription> result = _viewLocatorService.GetViewOf(typeof(TestComponent), ViewContext.Widget);

        Assert.Multiple(
            () =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.First().ViewType, Is.EqualTo(typeof(TestView)));
            });
    }

    /// <summary>
    ///     Test to ensure that views without the correct attribute are not added.
    /// </summary>
    [Test]
    public void OnNewTypesAvailable_ShouldNotAddViews_WhenNoViewOfAttributeIsPresent()
    {
        var typesEventArgs = new TypesEventArgs(new[] { typeof(NoViewAttributeComponent) });
        _mockTypesProvider.Raise((ITypesProvider provider) => provider.NewTypesAvailable += null, typesEventArgs);

        IEnumerable<ComponentViewDescription> result = _viewLocatorService.GetViewOf(typeof(TestComponent), ViewContext.Widget);

        Assert.That(result, Is.Empty);
    }

    /// <summary>
    ///     Setup method.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _mockTypesProvider = new Mock<ITypesProvider>();
        _viewLocatorService = new ViewLocatorService(_mockTypesProvider.Object);
    }

    /// <summary>
    ///     Tear down method.
    /// </summary>
    [TearDown]
    public void TearDown() => _viewLocatorService.Dispose();

    #endregion

    #region others methods

    private ViewLocatorService SetupViewLocatorServiceWithViews(params ComponentViewDescription[] views)
    {
        var typesWithViewAttributes = views.Select(
            (ComponentViewDescription view) =>
            {
                var mockViewType = new Mock<Type>();
                mockViewType.Setup((Type t) => t.GetTypeInfo().GetCustomAttributes(typeof(ViewOfAttribute), false)).Returns(
                    new object[]
                    {
                        new ViewOfAttribute(
                            "test",
                            view.ComponentType,
                            view.ViewType,
                            view.ViewContext),
                    });

                return mockViewType.Object;
            }).ToList();

        _mockTypesProvider.Setup((ITypesProvider p) => p.GetTypes()).Returns(typesWithViewAttributes);
        var service = new ViewLocatorService(_mockTypesProvider.Object);

        return service;
    }

    #endregion

    private class NoViewAttributeComponent : IComponent
    {
        #region properties

        public Guid Id { get; } = Guid.NewGuid();

        public string InstanceName { get; set; } = string.Empty;

        #endregion

        #region public and override methods

        public void Dispose()
        {
        }

        #endregion
    }

    private class TestComponent : IComponent
    {
        #region properties

        public Guid Id { get; } = Guid.NewGuid();

        public string InstanceName { get; set; } = string.Empty;

        #endregion

        #region public and override methods

        public void Dispose()
        {
        }

        #endregion
    }

    [ViewOf(
        "test",
        typeof(TestComponent),
        typeof(TestView),
        ViewContext.Widget)]
    private class TestView
    {
    }
}
