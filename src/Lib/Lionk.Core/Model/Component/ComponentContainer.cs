// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.Observable;

namespace Lionk.Core.Model.Component;

/// <summary>
/// Container for component instance.
/// </summary>
public class ComponentContainer : ObservableElement
{
    private readonly IComponentService _componentService;

    /// <summary>
    /// Occurs when a new component is available.
    /// </summary>
    public event EventHandler? NewComponentAvailable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentContainer"/> class.
    /// </summary>
    /// <param name="componentService">component service.</param>
    /// <param name="componentId">component instance unique ID.</param>
    public ComponentContainer(IComponentService componentService, Guid componentId)
    {
        _componentService = componentService;
        _componentId = componentId;
        IComponent? component = _componentService.GetInstanceByID(ComponentId);

        if (component is not null)
        {
            Component = component;
        }
        else
        {
            componentService.NewComponentAvailable += (s, e) => OnNewComponentAvailable();
        }
    }

    private Guid _componentId;

    /// <summary>
    /// Gets or sets the component instance unique ID.
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

    private IComponent? _component;

    /// <summary>
    /// Gets the component instance.
    /// </summary>
    public IComponent? Component
    {
        get => _component;
        private set => SetField(ref _component, value);
    }

    private void OnNewComponentAvailable()
    {
        IComponent? component = _componentService.GetInstanceByID(ComponentId);

        if (component != null)
        {
            Component = component;
            _componentService.NewComponentAvailable -= (s, e) => OnNewComponentAvailable();
            NewComponentAvailable?.Invoke(this, EventArgs.Empty);
        }
    }
}
