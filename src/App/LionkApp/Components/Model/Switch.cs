// Copyright © 2024 Lionk Project

namespace LionkApp.Components.Model;

/// <summary>
///     Class for a switch component.
/// </summary>
public class Switch
{
    #region properties

    /// <summary>
    ///     Gets or sets a value indicating whether the switch is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    ///     Gets or sets the switch label.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    #endregion
}
