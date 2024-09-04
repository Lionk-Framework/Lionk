// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using System.Reflection;
using Lionk.Core.Observable;
using Lionk.Core.TypeRegister;
using Lionk.Log;
using Lionk.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    private Task? _saveTask;

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

        Task.Run(() => StartBackgroundSaveTask(_cancellationTokenSource.Token));
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

        foreach (IComponent component in _componentInstances.Values)
        {
            if (component is ObservableElement observable)
            {
                observable.PropertyChanged -= (s, e) => RequestSaveConfiguration();
            }
        }

        _componentRegister.NewComponentAvailable -= (s, e) => OnNewTypesAvailable();

        if (_saveTask?.IsCompleted ?? true)
            _saveTask = SaveConfigurationAsync();

        _saveTask.Wait();

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

    private void LoadConfiguration()
    {
        if (!ConfigurationUtils.FileExists(ConfigurationFileName, FolderType.Config))
        {
            LogService.LogApp(LogSeverity.Information, "Component configuration file not found.");
            return;
        }

        string json = ConfigurationUtils.ReadFile(ConfigurationFileName, FolderType.Config);
        JObject? jsonObject = ParseJson(json);

        if (jsonObject == null)
        {
            LogService.LogApp(LogSeverity.Error, "Failed to load component instances. The configuration file might be corrupted.");
            return;
        }

        _componentInstances = DeserializeComponents(jsonObject);
    }

    private JObject? ParseJson(string json)
    {
        try
        {
            return JObject.Parse(json);
        }
        catch (JsonException ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Failed to parse JSON configuration. Error: {ex.Message}");
            return null;
        }
    }

    private ConcurrentDictionary<Guid, IComponent> DeserializeComponents(JObject jsonObject)
    {
        var loadedInstances = new ConcurrentDictionary<Guid, IComponent>();

        foreach (JProperty property in jsonObject.Properties())
        {
            Guid? componentId = ParseComponentId(property.Name);
            if (componentId == null) continue;

            IComponent? component = DeserializeComponent(property.Value, componentId.Value);
            if (component != null)
            {
                loadedInstances[componentId.Value] = component;
            }
        }

        return loadedInstances;
    }

    private Guid? ParseComponentId(string componentIdString)
    {
        try
        {
            return Guid.Parse(componentIdString);
        }
        catch (FormatException)
        {
            LogService.LogApp(LogSeverity.Error, $"Invalid component ID format: {componentIdString}");
            return null;
        }
    }

    private IComponent? DeserializeComponent(JToken token, Guid componentId)
    {
        try
        {
            IComponent? component = token.ToObject<IComponent>(new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.All,
            });

            if (component == null)
            {
                LogService.LogApp(LogSeverity.Warning, $"Failed to deserialize component with ID {componentId}. The component is null.");
            }

            return component;
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Error deserializing component with ID {componentId}: {ex.Message}");
            return null;
        }
    }

    private void OnNewTypesAvailable() => NewComponentAvailable?.Invoke(this, EventArgs.Empty);

    private async Task SaveConfigurationAsync()
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
            try
            {
                if (_saveRequested)
                {
                    _saveTask = SaveConfigurationAsync();
                    await _saveTask;
                }

                await Task.Delay(_saveInterval, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    #endregion
}
