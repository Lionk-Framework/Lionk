// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components;

namespace Lionk.Core.Component;

/// <summary>
/// This interface is used to define an element that can be used as a widget.
/// </summary>
public abstract class Widget : ComponentBase
{
    /// <summary>
    /// Gets or sets the component to display in the widget.
    /// </summary>
    public IComponent? Component { get; set; }

    /// <summary>
    /// Gets or sets the component to display in the widget.
    /// </summary>
    /// <remarks> This property must have [Parameter] attribute. </remarks>
    [Parameter]
    public IComponent? ComponentParameter { get; set; }

    /// <summary>
    /// This method is called when the component is initialized.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (ComponentParameter != null) Component = ComponentParameter;
    }
}
