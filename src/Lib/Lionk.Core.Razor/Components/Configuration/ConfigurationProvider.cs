// Copyright © 2024 Lionk Project

using System.ComponentModel;

namespace Lionk.Core.Components.Configuration;

/// <summary>
/// This interface is used to define a configuration provider.
/// </summary>
public static class ConfigurationProvider
{
    /// <summary>
    /// This method returns the configuration type of the component.
    /// </summary>
    /// <param name="component"> The component.</param>
    /// <returns> The configuration type.</returns>
    public static Type? GetConfigurationType(this IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);
        return null;
    }
}
