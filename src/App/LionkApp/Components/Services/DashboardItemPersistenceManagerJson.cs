// Copyright © 2024 Lionk Project

using Lionk.Core.View;
using Lionk.Log;
using Lionk.Utils;
using Newtonsoft.Json;

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
        _items.Remove(dashboardItemModel);
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
        ComponentViewModel? itemToRemove = _items.Find(n => n.Id == dashboardItemModel.Id);

        if (itemToRemove is not null)
        {
            _items.Remove(itemToRemove);
        }

        _items.Add(dashboardItemModel);

        Save();
    }

    #endregion

    #region others methods

    private void Load()
    {
        string json = ConfigurationUtils.ReadFile(FilePath, _folderType);
        List<ComponentViewModel>? items = JsonConvert.DeserializeObject<List<ComponentViewModel>>(json);

        if (items is null)
        {
            items = [];
            LogService.LogApp(LogSeverity.Warning, "Can't load dashboard");
        }

        _items = items;
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
        ConfigurationUtils.SaveFile(FilePath, json, _folderType);
    }

    #endregion
}
