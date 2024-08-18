// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
/// This attribute is used to specify the view that is used to configure the object.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ViewOfAttribute"/> class.
/// </remarks>
/// <param name="name"> The name of the view. </param>
/// <param name="componentType"> The type of the object that is being configured. </param>
/// <param name="configurableView"> The type of the view that is used to configure the object. </param>
/// <param name="viewMode"> The view mode that is used to configure the object. </param>
[AttributeUsage(AttributeTargets.Class)]
public class ViewOfAttribute(
    string name,
    Type componentType,
    Type configurableView,
    ViewContext viewMode) : Attribute
{
    /// <summary>
    /// Gets the description of the view.
    /// </summary>
    public ComponentViewDescription Description { get; }
        = new ComponentViewDescription(name, componentType, configurableView, viewMode);
}
