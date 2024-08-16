// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This class is used to define the view model of a component.
/// </summary>
public class ComponentViewModel
{
    /// <summary>
    /// Gets the type of the object that is being configured.
    /// </summary>
    public Type ComponentType { get; private set; }

    /// <summary>
    /// Gets the type of the view that is used to configure the object.
    /// </summary>
    public Type View { get; private set; }

    /// <summary>
    /// Gets the view mode that is used to  the object.
    /// </summary>
    public ComponentViewMode ViewMode { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentViewModel"/> class.
    /// </summary>
    /// <param name="componentType"> The type of the object that is being configured. </param>
    /// <param name="view"> The type of the view that is used to configure the object. </param>
    /// <param name="viewMode"> The view mode that is used to configure the object. </param>
    public ComponentViewModel(Type componentType, Type view, ComponentViewMode viewMode)
    {
        ComponentType = componentType;
        View = view;
        ViewMode = viewMode;
    }
}
