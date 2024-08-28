// Copyright © 2024 Lionk Project
using Lionk.Core.View;

namespace LionkApp.Services;

/// <summary>
///     Define an interface which can manage the persistence of dashboard items.
/// </summary>
public interface IDashboardItemPersistenceManager
{
    #region public and override methods

    /// <summary>
    ///     Method to get all the dashboard items saved.
    /// </summary>
    /// <returns> The list of dashboard items saved. </returns>
    public List<ComponentViewModel> GetDashboardItems();

    /// <summary>
    ///     Method to remove a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to remove. </param>
    public void RemoveDashboardItemModel(ComponentViewModel dashboardItemModel);

    /// <summary>
    ///     Method to save a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to save. </param>
    public void SaveDashboardItem(ComponentViewModel dashboardItemModel);

    /// <summary>
    ///     Method to edit a dashboard item.
    /// </summary>
    /// <param name="dashboardItemModel"> The dashboard item to edit. </param>
    public void UpdateDashboardItem(ComponentViewModel dashboardItemModel);

    #endregion
}
