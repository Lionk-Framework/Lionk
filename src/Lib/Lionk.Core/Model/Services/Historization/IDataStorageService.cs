// Copyright © 2024 Lionk Project

using Lionk.Core.DataModel;

namespace Lionk.Core;

/// <summary>
/// Interface defining a service for storing measurement data.
/// </summary>
public interface IDataStorageService
{
    /// <summary>
    /// Stores a measurement from a component.
    /// </summary>
    /// <typeparam name="T">The type of the measurement value.</typeparam>
    /// <param name="componentId">The ID of the component that produced the measurement.</param>
    /// <param name="measure">The measurement data to store.</param>
    void StoreMeasure<T>(Guid componentId, Measure<T> measure);
    
    /// <summary>
    /// Retrieves measurements for a component within a specified time range.
    /// </summary>
    /// <typeparam name="T">The type of the measurement value.</typeparam>
    /// <param name="componentId">The ID of the component to retrieve measurements for.</param>
    /// <param name="startTime">The start time of the range to retrieve.</param>
    /// <param name="endTime">The end time of the range to retrieve.</param>
    /// <returns>An enumerable of measurements within the specified time range.</returns>
    IEnumerable<Measure<T>> GetMeasures<T>(Guid componentId, DateTime startTime, DateTime endTime);
}
