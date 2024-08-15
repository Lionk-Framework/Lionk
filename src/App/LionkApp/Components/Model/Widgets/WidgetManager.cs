// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.Service;

// Types Aliases définition
using Parameters = System.Collections.Generic.Dictionary<string, object>;

namespace LionkApp.Components.Widgets;

/// <summary>
/// This class is responsible for managing widgets.
/// </summary>
public static class WidgetManager
{
    private static readonly ComponentService? _componentService;
    private static readonly List<IWidgetable> _widgets = new();
    private static readonly List<Dictionary<IWidgetable, Parameters>> _dashboardedWidgets = new();

    /// <summary>
    /// Initializes static members of the <see cref="WidgetManager"/> class.
    /// </summary>
    static WidgetManager() => _componentService = ServiceProviderAccessor.ServiceProvider?.GetService<ComponentService>();

    /// <summary>
    /// This method lists all instancied widgets.
    /// </summary>
    public static void GetInstanciedWidgets() => _widgets.AddRange(_componentService?.GetInstancesOfType<IWidgetable>() ?? Enumerable.Empty<IWidgetable>());
}
