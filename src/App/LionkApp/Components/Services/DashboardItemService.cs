// Copyright © 2024 Lionk Project

using LionkApp.Components.Model;

namespace LionkApp.Services;

/// <summary>
/// This class implements the way dashboard items are managed.
/// </summary>
public static class DashboardItemService
{
    /// <summary>
    /// Method to get all the dashboard items saved.
    /// </summary>
    /// <returns> The list of dashboard items saved. </returns>
    public static List<DashboardItemModel> GetDashboardItems() => DashboardItemFileHandler.GetDashBoardItemModels();

    /// <summary>
    /// Method to save a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to save. </param>
    public static void SaveDashboardItem(DashboardItemModel dashboardItemModel)
        => DashboardItemFileHandler.SaveDashboardItemModel(dashboardItemModel);

    /// <summary>
    /// Method to edit a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to edit. </param>
    public static void UpdateDashboardItem(DashboardItemModel dashboardItemModel)
        => DashboardItemFileHandler.EditDashboardItemModels(dashboardItemModel);

    /// <summary>
    /// Method to remove a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to remove. </param>
    public static void RemoveDashboardItemModel(DashboardItemModel dashboardItemModel)
        => DashboardItemFileHandler.RemoveDashboardItemModel(dashboardItemModel);
}
