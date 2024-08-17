// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.Core.Events;

/// <summary>
/// Provides data for the Widget event.
/// </summary>
public class WidgetEventArgs
{
    /// <summary>
    /// Gets or sets the widget.
    /// </summary>
    public Widget? Widget { get; set; }
}
