// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
/// Interface which define the service to locate the view of a component.
/// </summary>
public interface IViewLocatorService
{
    /// <summary>
    /// Get the view of a component depending on the context.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <param name="context">The context.</param>
    /// <returns>A <see cref="Type"/> which define the view.</returns>
    public IEnumerable<Type> GetViewOf<T>(ViewContext context);
}
