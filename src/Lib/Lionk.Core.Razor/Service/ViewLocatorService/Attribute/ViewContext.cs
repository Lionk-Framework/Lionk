// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
///     This enum is used to specify the view that is used to configure the object.
/// </summary>
public enum ViewContext
{
    /// <summary>
    ///     Defines the view as a configuration view.
    /// </summary>
    Configuration,

    /// <summary>
    ///     Defines the view as a detail view.
    /// </summary>
    Detail,

    /// <summary>
    ///     Defines the view as a page view.
    /// </summary>
    Page,

    /// <summary>
    ///     Defines the view as a widget view.
    /// </summary>
    Widget,
}
