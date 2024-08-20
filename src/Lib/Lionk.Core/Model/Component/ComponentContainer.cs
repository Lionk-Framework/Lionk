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
    /// Initializes a new instance of the <see cref="ComponentContainer"/> class.
    /// </summary>
    /// <param name="componentService">component service.</param>
    public ComponentContainer(IComponentService componentService)
    {
        _componentService = componentService;

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

    private Guid _componentID;

    /// <summary>
    /// Gets or sets the component instance unique ID.
    /// </summary>
    public Guid ComponentId
    {
        get => _componentID;
        set
        {
            _componentID = value;
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
        }
    }
}
