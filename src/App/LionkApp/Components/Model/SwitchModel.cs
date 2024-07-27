// Copyright © 2024 Lionk Project

namespace LionkApp.Components.Model;

/// <summary>
/// Model class for a switch component.
/// </summary>
public class SwitchModel
{
    /// <summary>
    /// Gets or sets the switch label.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the switch is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;
}
