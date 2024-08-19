// Copyright © 2024 Lionk Project

using Lionk.Log;
using Lionk.Utils;
using LionkApp.Components.Model;
using Newtonsoft.Json;

namespace LionkApp.Services;

/// <summary>
/// This class implements the way dashboard items are managed with a file.
/// </summary>
public class DashboardItemPersistenceManagerJson : IDashboardItemPersistenceManager
{
    private const string FilePath = "dashboard.json";
    private readonly FolderType _folderType = FolderType.Data;
    private List<DashboardItemModel> _items = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="DashboardItemPersistenceManagerJson"/> class.
    /// </summary>
    public DashboardItemPersistenceManagerJson()
        => Load();

    /// <inheritdoc/>
    public List<DashboardItemModel> GetDashboardItems()
    {
        Load();
        return _items;
    }

    /// <inheritdoc/>
    public void RemoveDashboardItemModel(DashboardItemModel dashboardItemModel)
    {
        _items.Remove(dashboardItemModel);
        Save();
    }

    /// <inheritdoc/>
    public void SaveDashboardItem(DashboardItemModel dashboardItemModel)
    {
        _items.Add(dashboardItemModel);
        Save();
    }

    /// <inheritdoc/>
    public void UpdateDashboardItem(DashboardItemModel dashboardItemModel)
    {
        DashboardItemModel? itemToRemove = _items.Find(n => n.Id == dashboardItemModel.Id);

        if (itemToRemove is not null)
            _items.Remove(itemToRemove);

        _items.Add(dashboardItemModel);

        Save();
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
        ConfigurationUtils.SaveFile(FilePath, json, _folderType);
    }

    private void Load()
    {
        string json = ConfigurationUtils.ReadFile(FilePath, _folderType);
        List<DashboardItemModel>? items = JsonConvert.DeserializeObject<List<DashboardItemModel>>(json);

        if (items is null)
        {
            items = [];
            LogService.LogApp(LogSeverity.Warning, "Can't load dashboard");
        }

        _items = items;
    }
}
