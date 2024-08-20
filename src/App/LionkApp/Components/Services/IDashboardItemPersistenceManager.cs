// Copyright © 2024 Lionk Project
using LionkApp.Components.Model;

namespace LionkApp.Services;

/// <summary>
/// Define an interface which can manage the persistence of dashboard items.
/// </summary>
public interface IDashboardItemPersistenceManager
{
    /// <summary>
    /// Method to get all the dashboard items saved.
    /// </summary>
    /// <returns> The list of dashboard items saved. </returns>
    public List<DashboardItemModel> GetDashboardItems();

    /// <summary>
    /// Method to remove a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to remove. </param>
    public void RemoveDashboardItemModel(DashboardItemModel dashboardItemModel);

    /// <summary>
    /// Method to save a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to save. </param>
    public void SaveDashboardItem(DashboardItemModel dashboardItemModel);

    /// <summary>
    /// Method to edit a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to edit. </param>
    public void UpdateDashboardItem(DashboardItemModel dashboardItemModel);
}
