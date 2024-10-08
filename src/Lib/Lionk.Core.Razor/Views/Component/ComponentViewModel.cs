﻿// Copyright © 2024 Lionk Project

using Newtonsoft.Json;

namespace Lionk.Core.View;

/// <summary>
///     Dashboard item model.
/// </summary>
public class ComponentViewModel
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentViewModel" /> class.
    /// </summary>
    /// <param name="componentId"> The component unique id.</param>
    /// <param name="viewType"> The view type.</param>
    public ComponentViewModel(Guid componentId, Type viewType)
    {
        ComponentUniqueId = componentId;
        ViewType = viewType;
        Indexes = new int[Enum.GetValues<ViewContext>().Length];
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComponentViewModel" /> class.
    /// </summary>
    /// <param name="id"> The unique identifier of the dashboard item model.</param>
    /// <param name="componentId"> The component instance name.</param>
    /// <param name="viewType"> The view type.</param>
    /// <param name="indexes"> The indexes of selected views.</param>
    [JsonConstructor]
    public ComponentViewModel(Guid id, Guid componentId, Type viewType, int[] indexes)
    {
        Id = id;
        ComponentUniqueId = componentId;
        ViewType = viewType;
        Indexes = indexes;
    }

    #endregion

    #region properties

    /// <summary>
    ///     Gets or sets the component instance name.
    /// </summary>
    public Guid ComponentUniqueId { get; set; }

    /// <summary>
    ///     Gets the unique identifier of the dashboard history.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    ///     Gets the indexes of selected views.
    /// </summary>
    public int[] Indexes { get; }

    /// <summary>
    ///     Gets or sets the view type.
    /// </summary>
    public Type ViewType { get; set; }

    #endregion

    #region public and override methods

    /// <summary>
    ///     This method saves the current index of the view.
    /// </summary>
    /// <param name="viewContext"> The view context.</param>
    /// <param name="index"> The index of the view.</param>
    public void SaveCurrentIndex(ViewContext viewContext, int index) => Indexes[(int)viewContext] = index;

    #endregion
}
