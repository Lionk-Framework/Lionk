// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This class is used to define the model of the dashboard item.
/// </summary>
public class DashBoardItemModel
{
    /// <summary>
    /// Gets or sets the parameters of the dashboard item.
    /// </summary>
    public Dictionary<string, string>? PropertyAndInstanceName { get; set; }

    /// <summary>
    /// Gets or sets the type of the view.
    /// </summary>
    public Type? View { get; set; }
}
