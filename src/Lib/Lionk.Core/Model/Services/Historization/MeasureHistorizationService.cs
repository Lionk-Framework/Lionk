// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using Lionk.Core.Component;
using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.Core;

/// <summary>
/// Service responsible for monitoring measurable components and storing their measures using a data storage service.
/// This implementation only handles IMeasurableComponent&lt;double&gt; components.
/// </summary>
public class MeasureHistorizationService : IMeasureHistorizationService
{
    #region fields

    private readonly IComponentService _componentService;
    private readonly IDataStorageService _dataStorageService;
    private readonly ConcurrentDictionary<Guid, WeakReference<IMeasurableComponent<double>>> _subscribedComponents = new();

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureHistorizationService"/> class.
    /// </summary>
    /// <param name="componentService">The component service.</param>
    /// <param name="dataStorageService">The data storage service.</param>
    public MeasureHistorizationService(IComponentService componentService, IDataStorageService dataStorageService)
    {
        _componentService = componentService ?? throw new ArgumentNullException(nameof(componentService));
        _dataStorageService = dataStorageService ?? throw new ArgumentNullException(nameof(dataStorageService));

        // Subscribe to component registration events
        _componentService.NewInstanceRegistered += OnNewComponentRegistered;
    }

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public void SubscribeToComponents()
    {
        // Subscribe to existing measurable components
        IEnumerable<IMeasurableComponent<double>> components = _componentService.GetInstances().OfType<IMeasurableComponent<double>>();
        foreach (IMeasurableComponent<double> component in components)
        {
            SubscribeComponent(component);
        }
    }

    /// <inheritdoc />
    public void UnsubscribeFromComponents()
    {
        foreach (KeyValuePair<Guid, WeakReference<IMeasurableComponent<double>>> componentEntry
                 in _subscribedComponents.ToArray())
        {
            if (componentEntry.Value.TryGetTarget(out IMeasurableComponent<double>? measurable))
            {
                UnsubscribeFromComponent(measurable);
            }

            _subscribedComponents.TryRemove(componentEntry.Key, out _);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _componentService.NewInstanceRegistered -= OnNewComponentRegistered;
        UnsubscribeFromComponents();
        GC.SuppressFinalize(this);
    }

    #endregion

    #region others methods

    private void OnNewComponentRegistered(object? sender, EventArgs e)
    {
        IEnumerable<IMeasurableComponent<double>> components = _componentService.GetInstances().OfType<IMeasurableComponent<double>>();
        foreach (IMeasurableComponent<double> component in components)
        {
            // Only subscribe to components we haven't already subscribed to
            if (!_subscribedComponents.ContainsKey(component.Id))
            {
                SubscribeComponent(component);
            }
        }
    }

    private void SubscribeComponent(IMeasurableComponent<double> component)
    {
        // Keep track of the subscribed component with a weak reference
        _subscribedComponents[component.Id] = new WeakReference<IMeasurableComponent<double>>(component);

        component.NewValueAvailable += OnNewMeasureAvailable;

        LogService.LogApp(LogSeverity.Information,
            $"Subscribed to measurable component: {component.InstanceName} (ID: {component.Id})");
    }

    private void UnsubscribeFromComponent(IMeasurableComponent<double> measurable)
    {
        // Unsubscribe from the NewValueAvailable event
        measurable.NewValueAvailable -= OnNewMeasureAvailable;

        LogService.LogApp(LogSeverity.Information,
            $"Unsubscribed from measurable component: {measurable.InstanceName} (ID: {measurable.Id})");

        // Remove the component from tracked components
        _subscribedComponents.TryRemove(measurable.Id, out _);
    }

    // Event handler for measurable components
    private void OnNewMeasureAvailable(object? sender, MeasureEventArgs<double> e)
    {
        if (sender is not IMeasurableComponent<double> component)
            return;

        try
        {
            // Store the measures using the data storage service
            foreach (Measure<double> measure in e.Measures)
            {
                _dataStorageService.StoreMeasure(component.InstanceName, measure);
            }

            LogService.LogApp(LogSeverity.Debug,
                $"Stored {e.Measures.Count()} measures from component {component.InstanceName} (ID: {component.Id})");
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error,
                $"Error storing measures from component {component?.InstanceName}: {ex.Message}");
        }
    }

    #endregion
}
