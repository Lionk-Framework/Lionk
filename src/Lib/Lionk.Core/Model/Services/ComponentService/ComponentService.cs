﻿// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using System.Reflection;
using Lionk.Core.Observable;
using Lionk.Core.TypeRegister;
using Lionk.Log;
using Lionk.Utils;
using Newtonsoft.Json;

namespace Lionk.Core.Component;

/// <summary>
///     Service that manages components.
/// </summary>
public class ComponentService : IComponentService
{
    #region fields

    private const string ConfigurationFileName = "ComponentServiceConfiguration.json";

    private const string DefaultComponentName = "Component";

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly TimeSpan _saveInterval = TimeSpan.FromSeconds(2);

    private readonly ComponentRegister _componentRegister;

    private ConcurrentDictionary<Guid, IComponent> _componentInstances = new();

    private bool _saveRequested = false;

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentService" /> class.
    /// </summary>
    /// <param name="provider">The type provider.</param>
    public ComponentService(ITypesProvider provider)
    {
        _componentRegister = new ComponentRegister(provider, this);
        _componentRegister.NewComponentAvailable += (s, e) => OnNewTypesAvailable();
        LoadConfiguration();

        StartBackgroundSaveTask(_cancellationTokenSource.Token);
    }

    #endregion

    #region delegate and events

    /// <inheritdoc />
    public event EventHandler<EventArgs>? NewComponentAvailable;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? NewInstanceRegistered;

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        SaveConfiguration().Wait();

        foreach (IComponent component in _componentInstances.Values)
        {
            if (component is ObservableElement observable)
            {
                observable.PropertyChanged -= (s, e) => RequestSaveConfiguration();
            }
        }

        _componentRegister.NewComponentAvailable -= (s, e) => OnNewTypesAvailable();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public IComponent? GetInstanceById(Guid id) => _componentInstances.GetValueOrDefault(id);

    /// <inheritdoc />
    public IComponent? GetInstanceByName(string name)
    {
        IComponent? component = _componentInstances.Values.FirstOrDefault(x => x.InstanceName == name);
        return component;
    }

    /// <inheritdoc />
    public IEnumerable<IComponent> GetInstances() => _componentInstances.Values;

    /// <inheritdoc />
    public IEnumerable<T> GetInstancesOfType<T>() => _componentInstances.Values.OfType<T>();

    /// <inheritdoc />
    public IReadOnlyDictionary<ComponentTypeDescription, Factory> GetRegisteredTypeDictionary() =>
        _componentRegister.TypesRegister.AsReadOnly();

    /// <inheritdoc />
    public void RegisterComponentInstance(IComponent component)
    {
        if (component.GetType().GetCustomAttribute<NamedElement>() is NamedElement attribute)
        {
            component.InstanceName = attribute.Name;
        }

        string baseName = component.InstanceName == string.Empty ? DefaultComponentName : component.InstanceName;
        string uniqueName = GenerateUniqueName(baseName);
        component.InstanceName = uniqueName;

        if (_componentInstances.TryAdd(component.Id, component))
        {
            RequestSaveConfiguration();
        }

        if (component is ObservableElement observable)
        {
            observable.PropertyChanged += (s, e) => RequestSaveConfiguration();
        }

        NewInstanceRegistered?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public void UnregisterComponentInstance(IComponent component)
    {
        if (component is ObservableElement observable)
        {
            observable.PropertyChanged -= (s, e) => RequestSaveConfiguration();
        }

        if (_componentInstances.TryRemove(component.Id, out _))
        {
            RequestSaveConfiguration();
        }
    }

    #endregion

    #region others methods

    /// <summary>
    ///     Generates a unique name for the component by adding suffixes if necessary.
    /// </summary>
    /// <param name="baseName">The base name of the component.</param>
    /// <returns>A unique name for the component.</returns>
    private string GenerateUniqueName(string baseName)
    {
        string uniqueName = baseName;
        int suffix = 0;

        while (_componentInstances.Values.Any(x => x.InstanceName == uniqueName))
        {
            suffix++;
            uniqueName = $"{baseName}_{suffix}";
        }

        return uniqueName;
    }

    /// <summary>
    ///     Loads the component instances from a JSON file using Newtonsoft.Json.
    /// </summary>
    private void LoadConfiguration()
    {
        if (ConfigurationUtils.FileExists(ConfigurationFileName, FolderType.Config))
        {
            string json = ConfigurationUtils.ReadFile(ConfigurationFileName, FolderType.Config);

            try
            {
                ConcurrentDictionary<Guid, IComponent>? savedInstances =
                    JsonConvert.DeserializeObject<ConcurrentDictionary<Guid, IComponent>>(
                        json,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                if (savedInstances != null)
                {
                    _componentInstances = savedInstances;
                }
                else
                {
                    LogService.LogApp(LogSeverity.Error, "Failed to load component instances. The configuration file might be corrupted.");
                }
            }
            catch (JsonException ex)
            {
                LogService.LogApp(LogSeverity.Error, $"Failed to deserialize component configuration. Error: {ex.Message}");
            }
        }
        else
        {
            LogService.LogApp(LogSeverity.Information, "Component configuration file not found.");
        }
    }

    private void OnNewTypesAvailable() => NewComponentAvailable?.Invoke(this, EventArgs.Empty);

    private async Task SaveConfiguration()
    {
        try
        {
            string json = JsonConvert.SerializeObject(
                _componentInstances,
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            await ConfigurationUtils.SaveFileAsync(ConfigurationFileName, json, FolderType.Config);
            _saveRequested = false;
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to save component configuration. Error: {ex.Message}");
        }
    }

    private void RequestSaveConfiguration() => _saveRequested = true;

    private async void StartBackgroundSaveTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_saveRequested)
            {
                await SaveConfiguration();
            }

            await Task.Delay(_saveInterval, token);
        }
    }

    #endregion
}
