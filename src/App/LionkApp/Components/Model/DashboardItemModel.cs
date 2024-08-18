// Copyright © 2024 Lionk Project

namespace LionkApp.Components.Model;

/// <summary>
/// Dashboard item model.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DashboardItemModel"/> class.
/// <param name="componentName"> The component instance name.</param>
/// <param name="viewType">The view type.</param>
/// </remarks>
public class DashboardItemModel(string componentName, Type viewType)
{
    /// <summary>
    /// Gets or sets the component instance name.
    /// </summary>
    public string ComponentInstanceName { get; set; } = componentName;

    /// <summary>
    /// Gets or sets the view type.
    /// </summary>
    public Type ViewType { get; set; } = viewType;
}
