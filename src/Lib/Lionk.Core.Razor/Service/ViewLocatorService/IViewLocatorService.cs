// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
/// Interface which define the service to locate the view of a component.
/// </summary>
public interface IViewLocatorService : IDisposable
{
    /// <summary>
    /// Get the view of a component depending on the context.
    /// </summary>
    /// <param name="type">The type of the component.</param>
    /// <param name="context">The context.</param>
    /// <returns>A <see cref="Type"/> which define the view.</returns>
    public IEnumerable<ComponentViewDescription> GetViewOf(Type type, ViewContext context);
}
