// Copyright © 2024 Lionk Project

using ConfiguredType = System.Type;
using ViewType = System.Type;

namespace Lionk.Core.Component.Configuration;

/// <summary>
/// This attribute is used to specify the view that is used to configure the object.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationView : Attribute
{
    /// <summary>
    /// Gets the views that are used to configure the objects.
    /// </summary>
    public static Dictionary<ConfiguredType, ViewType> ConfigurationViews { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationView"/> class.
    /// </summary>
    /// <param name="configurableType"> The type of the object that is being configured. </param>
    /// <param name="configurableView"> The type of the view that is used to configure the object. </param>
    public ConfigurationView(Type configurableType, Type configurableView) => ConfigurationViews.Add(configurableType, configurableView);

    /// <summary>
    /// Gets the view that is used to configure the object.
    /// </summary>
    /// <param name="configurableType"> The type of the object that is being configured. </param>
    /// <returns> The type of the view that is used to configure the object. </returns>
    public static Type? GetConfigurationView(Type? configurableType)
    {
        if (configurableType is null)
        {
            return null;
        }

        return ConfigurationViews.TryGetValue(configurableType, out Type? view) ? view : null;
    }
}
