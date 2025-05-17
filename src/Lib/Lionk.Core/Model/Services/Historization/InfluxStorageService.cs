// Copyright © 2024 Lionk Project

using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.Core;

/// <summary>
/// Implementation of IDataStorageService that stores data in an InfluxDB time-series database.
/// </summary>
public class InfluxStorageService : IDataStorageService
{
    #region fields
    
    // TODO: Add required fields for InfluxDB connection and configuration
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxStorageService"/> class.
    /// </summary>
    public InfluxStorageService()
    {
        // TODO: Initialize InfluxDB client and connection
        LogService.LogApp(LogSeverity.Information, "InfluxStorageService initialized");
    }
    
    #endregion
    
    #region public and override methods
    
    /// <inheritdoc />
    public void StoreMeasure<T>(Guid componentId, Measure<T> measure)
    {
        // TODO: Implement actual storage to InfluxDB
        LogService.LogApp(LogSeverity.Debug, 
            $"Storing measure {measure.MeasureName} with value {measure.Value} for component {componentId}");
    }
    
    /// <inheritdoc />
    public IEnumerable<Measure<T>> GetMeasures<T>(Guid componentId, DateTime startTime, DateTime endTime)
    {
        // TODO: Implement query to InfluxDB
        LogService.LogApp(LogSeverity.Debug, 
            $"Getting measures for component {componentId} from {startTime} to {endTime}");
        
        // Return empty collection for now
        return Enumerable.Empty<Measure<T>>();
    }
    
    #endregion
}
