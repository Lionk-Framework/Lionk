﻿// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
///     Service which register the active views.
/// </summary>
public class ViewRegistryService : IViewRegistryService
{
    #region fields

    private readonly HashSet<object> _activeViews = [];

    #endregion

    #region public and override methods

    /// <inheritdoc cref="IViewRegistryService" />
    public bool HasActiveViews(Type t) => _activeViews.Any(view => view.GetType() == t);

    /// <inheritdoc cref="IViewRegistryService" />
    public void Register(object viewInstance) => _activeViews.Add(viewInstance);

    /// <inheritdoc cref="IViewRegistryService" />
    public void Unregister(object viewInstance) => _activeViews.Remove(viewInstance);

    #endregion
}
