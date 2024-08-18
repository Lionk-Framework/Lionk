// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface is used to define an element that can be configured.
/// </summary>
public interface IWidgetable
{
    /// <summary>
    /// This method is used to open the view of the component.
    /// </summary>
    /// <returns> A type that represents the view of the component. </returns>
    Type? GetView();
}
