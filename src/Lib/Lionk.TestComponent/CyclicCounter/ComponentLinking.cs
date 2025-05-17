// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;
using System;

namespace Lionk.TestComponent.CyclicCounter;

/// <summary>
///     Component for testing linking functionality between components.
/// </summary>
[NamedElement("Component Linking", "Test component for demonstrating component linking functionality")]
public class ComponentLinking : BaseComponent
{
    #region fields

    private Guid _linkedComponentId;
    private string _linkedComponentName = string.Empty;

    #endregion

    #region properties

    /// <summary>
    ///     Gets or sets the linked component.
    ///     This property will be populated by the ComponentService using the IdIs attribute.
    /// </summary>
    [IdIs(nameof(LinkedComponentId))]
    public IComponent? LinkedComponent { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the component to link to.
    ///     This property stores the ID reference that will be used to establish the link.
    /// </summary>
    public Guid LinkedComponentId
    {
        get => _linkedComponentId;
        set
        {
            if (SetField(ref _linkedComponentId, value))
            {
                // Notify that this field has changed to trigger any link update
                OnPropertyChanged(nameof(LinkedComponent));
            }
        }
    }

    /// <summary>
    ///     Gets or sets a user-friendly name to show for the linked component.
    ///     This is mainly for display purposes.
    /// </summary>
    public string LinkedComponentName
    {
        get => _linkedComponentName;
        set => SetField(ref _linkedComponentName, value);
    }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Updates the linked component name based on the currently linked component.
    /// </summary>
    public void UpdateLinkedComponentName()
    {
        if (LinkedComponent != null)
        {
            LinkedComponentName = LinkedComponent.InstanceName;
        }
        else
        {
            LinkedComponentName = "No component linked";
        }
    }

    #endregion
}
