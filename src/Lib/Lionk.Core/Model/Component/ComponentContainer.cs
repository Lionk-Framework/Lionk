// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.Observable;

namespace Lionk.Core.Model.Component;

/// <summary>
///     Container for component instance.
/// </summary>
public class ComponentContainer : ObservableElement
{
    #region fields

    private readonly IComponentService _componentService;

    private IComponent? _component;

    private Guid _componentId;

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentContainer" /> class.
    /// </summary>
    /// <param name="componentService">component service.</param>
    /// <param name="componentId">component instance unique ID.</param>
    public ComponentContainer(IComponentService componentService, Guid componentId)
    {
        _componentService = componentService;
        _componentId = componentId;
        IComponent? component = _componentService.GetInstanceById(ComponentId);

        if (component is not null)
        {
            Component = component;
        }
        else
        {
            componentService.NewComponentAvailable += (s, e) => OnNewComponentAvailable();
        }
    }

    #endregion

    #region delegate and events

    /// <summary>
    ///     Occurs when a new component is available.
    /// </summary>
    public event EventHandler? NewComponentAvailable;

    #endregion

    #region properties

    /// <summary>
    ///     Gets the component instance.
    /// </summary>
    public IComponent? Component
    {
        get => _component;
        private set => SetField(ref _component, value);
    }

    /// <summary>
    ///     Gets or sets the component instance unique ID.
    /// </summary>
    public Guid ComponentId
    {
        get => _componentId;
        set
        {
            _componentId = value;
            OnNewComponentAvailable();
        }
    }

    #endregion

    #region others methods

    private void OnNewComponentAvailable()
    {
        IComponent? component = _componentService.GetInstanceById(ComponentId);

        if (component != null)
        {
            Component = component;
            _componentService.NewComponentAvailable -= (s, e) => OnNewComponentAvailable();
            NewComponentAvailable?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion
}
