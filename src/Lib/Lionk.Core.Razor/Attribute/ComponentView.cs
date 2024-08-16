﻿// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This attribute is used to specify the view that is used to configure the object.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ComponentView : Attribute
{
    /// <summary>
    /// Gets the views that are used to configure the objects.
    /// </summary>
    public static List<ComponentViewModel> ConfigurationViews { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentView"/> class.
    /// </summary>
    /// <param name="configurableType"> The type of the object that is being configured. </param>
    /// <param name="configurableView"> The type of the view that is used to configure the object. </param>
    /// <param name="viewMode"> The view mode that is used to configure the object. </param>
    public ComponentView(Type configurableType, Type configurableView, ComponentViewMode viewMode) => ConfigurationViews.Add(new ComponentViewModel(configurableType, configurableView, viewMode));

    /// <summary>
    /// Gets the view that is used to configure the object.
    /// </summary>
    /// <param name="configurableType"> The type of the object that is being configured. </param>
    /// <returns> The type of the view that is used to configure the object. </returns>
    public static ComponentViewModel? GetConfigurationView(Type? configurableType)
    {
        if (configurableType is null)
        {
            return null;
        }

        return ConfigurationViews.FirstOrDefault(x => x.ComponentType == configurableType);
    }
}

/// <summary>
/// This enum is used to specify the view that is used to configure the object.
/// </summary>
public enum ComponentViewMode
{
    /// <summary>
    /// Defines the view as a configuration view.
    /// </summary>
    Configuration,

    /// <summary>
    /// Defines the view as a detail view.
    /// </summary>
    Detail,

    /// <summary>
    /// Defines the view as a page view.
    /// </summary>
    Page,
}
