// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
/// Service which register the active views.
/// </summary>
public class ViewRegistryService : IViewRegistry
{
    private readonly HashSet<object> _activeViews = [];

    /// <inheritdoc cref="IViewRegistry"/>
    public void Register(object viewInstance)
        => _activeViews.Add(viewInstance);

    /// <inheritdoc cref="IViewRegistry"/>
    public void Unregister(object viewInstance)
        => _activeViews.Remove(viewInstance);

    /// <inheritdoc cref="IViewRegistry"/>
    public bool HasActiveViews<T>()
        => _activeViews.Any(view => view is T);
}
