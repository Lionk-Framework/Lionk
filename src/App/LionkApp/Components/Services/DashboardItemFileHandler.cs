// Copyright © 2024 Lionk Project

using Lionk.Utils;
using LionkApp.Components.Model;
using Newtonsoft.Json;

namespace LionkApp.Services;

/// <summary>
/// This class implements the way dashboard items are managed with a file.
/// </summary>
public static class DashboardItemFileHandler
{
    private static readonly FolderType _folderType = FolderType.Data;
    private static readonly string _filePath = "dashboard.json";

    /// <summary>
    /// Method to save a dashboardItemModel.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboardItemModel to save. </param>
    public static void SaveDashboardItemModel(DashboardItemModel dashboardItemModel)
    {
        List<DashboardItemModel> dashboardItemModels = GetDashBoardItemModels();
        dashboardItemModels.Add(dashboardItemModel);
        WriteDashboardItemModels(dashboardItemModels);
    }

    /// <summary>
    /// Method to get all the dashboardItemModels saved.
    /// </summary>
    /// <returns> The list of dashboardItemModels saved. </returns>
    public static List<DashboardItemModel> GetDashBoardItemModels()
    {
        string json = ConfigurationUtils.ReadFile(_filePath, _folderType);
        if (string.IsNullOrEmpty(json)) return [];
        List<DashboardItemModel> dashboardItemModels = JsonConvert.DeserializeObject<List<DashboardItemModel>>(json) ?? throw new ArgumentNullException(nameof(dashboardItemModels));
        return dashboardItemModels;
    }

    /// <summary>
    /// This method writes the dashboardItemModels in the file.
    /// </summary>
    /// <param name="dashboardItemModels"> The list of dashboardItemModels to write. </param>
    public static void WriteDashboardItemModels(List<DashboardItemModel> dashboardItemModels)
    {
        string json = JsonConvert.SerializeObject(dashboardItemModels, Formatting.Indented);
        ConfigurationUtils.SaveFile(_filePath, json, _folderType);
    }

    /// <summary>
    /// Method to edit a dashboardItem model.
    /// </summary>
    /// <param name="dashboardItemModel">The dashboardItem model to edit.</param>
    public static void EditDashboardItemModels(DashboardItemModel dashboardItemModel)
    {
        List<DashboardItemModel> dashboardItemModels = GetDashBoardItemModels();
        int index = dashboardItemModels.FindIndex(n => n.Id == dashboardItemModel.Id);
        dashboardItemModels[index] = dashboardItemModel;
        WriteDashboardItemModels(dashboardItemModels);
    }

    /// <summary>
    /// Method to remove a dashboardItem model.
    /// </summary>
    /// <param name="dashboardItemModel">The dashboardItem model to remove.</param>
    public static void RemoveDashboardItemModel(DashboardItemModel dashboardItemModel)
    {
        List<DashboardItemModel> dashboardItemModels = GetDashBoardItemModels();
        int index = dashboardItemModels.FindIndex(n => n.Id == dashboardItemModel.Id);
        if (index == -1) return;
        dashboardItemModels.RemoveAt(index);
        WriteDashboardItemModels(dashboardItemModels);
    }
}
