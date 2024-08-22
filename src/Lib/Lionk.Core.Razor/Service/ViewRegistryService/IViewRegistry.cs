// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
/// Interface which define the service to register the active views.
/// </summary>
public interface IViewRegistry
{
    /// <summary>
    /// Method used to register a view.
    /// </summary>
    /// <param name="viewInstance">The view instance.</param>
    void Register(object viewInstance);

    /// <summary>
    /// Method used to unregister a view.
    /// </summary>
    /// <param name="viewInstance">The view instance.</param>
    void Unregister(object viewInstance);

    /// <summary>
    /// Method used to check if an instance of the view is active.
    /// </summary>
    /// <typeparam name="T">The type of the view.</typeparam>
    /// <returns>True if an instance of this type exist.</returns>
    bool HasActiveViews<T>();
}
