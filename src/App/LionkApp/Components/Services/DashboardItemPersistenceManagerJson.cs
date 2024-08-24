// Copyright © 2024 Lionk Project

using Lionk.Core.View;
using Lionk.Log;
using Lionk.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LionkApp.Services;

/// <summary>
///     This class implements the way dashboard items are managed with a file.
/// </summary>
public class DashboardItemPersistenceManagerJson : IDashboardItemPersistenceManager
{
    #region fields

    private const string FilePath = "dashboard.json";

    private readonly FolderType _folderType = FolderType.Data;

    private List<ComponentViewModel> _items = [];

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="DashboardItemPersistenceManagerJson" /> class.
    /// </summary>
    public DashboardItemPersistenceManagerJson() => Load();

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public List<ComponentViewModel> GetDashboardItems()
    {
        Load();
        return _items;
    }

    /// <inheritdoc />
    public void RemoveDashboardItemModel(ComponentViewModel dashboardItemModel)
    {
        _items.RemoveAll(item => item.Id == dashboardItemModel.Id);
        Save();
    }

    /// <inheritdoc />
    public void SaveDashboardItem(ComponentViewModel dashboardItemModel)
    {
        _items.Add(dashboardItemModel);
        Save();
    }

    /// <inheritdoc />
    public void UpdateDashboardItem(ComponentViewModel dashboardItemModel)
    {
        _items.RemoveAll(item => item.Id == dashboardItemModel.Id);
        _items.Add(dashboardItemModel);
        Save();
    }

    #endregion

    #region others methods

    private void Load()
    {
        string json = ConfigurationUtils.ReadFile(FilePath, _folderType);

        var validItems = new List<ComponentViewModel>();
        List<JObject>? items = JsonConvert.DeserializeObject<List<JObject>>(json);

        if (items is null) return;

        foreach (JObject item in items)
        {
            try
            {
                ComponentViewModel? component = item.ToObject<ComponentViewModel>();

                if (component != null)
                {
                    validItems.Add(component);
                }
            }
            catch (JsonSerializationException ex)
            {
                if (ex.InnerException is TypeLoadException)
                {
                    continue;
                }
                else
                {
                    throw;
                }
            }
        }

        if (validItems is null)
        {
            validItems = [];
            LogService.LogApp(LogSeverity.Warning, "Can't load dashboard");
        }

        _items = validItems;
        Save();
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
        ConfigurationUtils.SaveFile(FilePath, json, _folderType);
    }

    #endregion
}
