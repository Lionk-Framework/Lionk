// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components;

namespace Lionk.Core.Component;

/// <summary>
/// This interface is used to define an element that can be used as a widget.
/// </summary>
public abstract class Widget : ComponentBase, IComponent
{
    /// <summary>
    /// Gets or sets the name of the widget.
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// Gets or sets the component to display in the widget.
    /// </summary>
    public IComponent? Component
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the component to display in the widget.
    /// </summary>
    /// <remarks> This property must have [Parameter] attribute. </remarks>
    [Parameter]
    public IComponent? ComponentParameter { get; set; }

    /// <summary>
    /// Gets or sets the name of the widget.
    /// </summary>
    /// <remarks> This property must have [Parameter] attribute. </remarks>
    [Parameter]
    public IComponent? InstanceNameParameter
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the type of the component that will be displayed in the widget.
    /// </summary>
    public abstract Type? ComponentType { get; }

    /// <summary>
    /// This method is called when the component is initialized.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (ComponentParameter != null) Component = ComponentParameter;
        if (InstanceNameParameter != null) InstanceName = InstanceNameParameter?.InstanceName;
    }
}
