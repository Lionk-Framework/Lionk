// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface is used to define an element that can be configured.
/// </summary>
public interface IPageableComponent
{
    /// <summary>
    /// This method is used to open the page of the component.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task OpenComponentPage();
}
