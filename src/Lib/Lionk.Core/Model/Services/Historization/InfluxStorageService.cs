// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using InfluxDB3.Client;
using InfluxDB3.Client.Query;
using InfluxDB3.Client.Write;
using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.Core;

/// <inheritdoc cref="IDataStorageService"/>
public class InfluxStorageService : IDataStorageService, IDisposable
{
    private readonly InfluxDBClient _client;
    private readonly ConcurrentDictionary<string, TimeSpan> _retentionCache = new();
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxStorageService"/> class.
    /// </summary>
    /// <param name="config">The config.</param>
    public InfluxStorageService(InfluxDBConfig config)
    {
        _client = new InfluxDBClient(
            host: config.Url,
            token: config.Token,
            database: config.DefaultDatabase);

        LogService.LogApp(
            LogSeverity.Information,
            $"InfluxStorageService initialized with URL: {config.Url}, Database: {config.DefaultDatabase}");
    }

    /// <inheritdoc/>
    public async Task StoreMeasureAsync<T>(string componentName, Measure<T> measure, TimeSpan retentionTime)
    {
        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);
            string sanitizedMeasureName = SanitizeForInflux(measure.MeasureName);

            PointData point = PointData.Measurement(sanitizedMeasureName)
                .SetTag("component", sanitizedComponentName)
                .SetTag("unit", measure.Unit)
                .SetField("value", ConvertToDouble(measure.Value))
                .SetTimestamp(measure.Time);

            await _client.WritePointAsync(point: point, precision: WritePrecision.Ms);

            LogService.LogApp(
                LogSeverity.Debug,
                $"Stored measure {measure.MeasureName} with value {measure.Value} for component {componentName}");
        }
        catch (Exception ex)
        {
            LogService.LogApp(
                LogSeverity.Error,
                $"Error storing measure {measure.MeasureName} for component {componentName}: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Measure<T>>> GetMeasuresAsync<T>(string componentName, DateTime startTime, DateTime endTime)
    {
        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);

            // This query returns the columns in this order: time, measurement, unit, value
            string sql = $@"
            SELECT time, _measurement AS measurement, unit, value
            FROM _measurement
            WHERE component = '{sanitizedComponentName}'
            AND time >= '{startTime:O}' AND time <= '{endTime:O}'
            ORDER BY time ASC
            ";

            var measures = new List<Measure<T>>();

            await foreach (object?[] row in _client.Query(query: sql, queryType: QueryType.SQL))
            {
                // Acces to columns by index
                var time = DateTime.Parse(row[0]?.ToString() ?? DateTime.UtcNow.ToString());
                string measureName = row[1]?.ToString() ?? string.Empty;
                string unit = row[2]?.ToString() ?? string.Empty;
                T value = ConvertToType<T>(row[3]);

                measures.Add(new Measure<T>(
                    measureName,
                    time,
                    unit,
                    value));
            }

            LogService.LogApp(
                LogSeverity.Debug,
                $"Retrieved {measures.Count} measures for component {componentName} from {startTime} to {endTime}");
            return measures;
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Error retrieving measures for component {componentName}: {ex.Message}");
            return Enumerable.Empty<Measure<T>>();
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Measure<T>>> GetMeasuresAsync<T>(
        string componentName,
        string measureName,
        DateTime startTime,
        DateTime endTime)
    {
        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);
            string sanitizedMeasureName = SanitizeForInflux(measureName);

            // This query returns the columns in this order: time, unit, value
            string sql = $@"
            SELECT time, unit, value
            FROM {sanitizedMeasureName}
            WHERE component = '{sanitizedComponentName}'
            AND time >= '{startTime:O}' AND time <= '{endTime:O}'
            ORDER BY time ASC
            ";

            var measures = new List<Measure<T>>();

            await foreach (object?[] row in _client.Query(query: sql, queryType: QueryType.SQL))
            {
                // Acces to columns by index
                var time = DateTime.Parse(row[0]?.ToString() ?? DateTime.UtcNow.ToString());
                string unit = row[1]?.ToString() ?? string.Empty;
                T value = ConvertToType<T>(row[2]);

                measures.Add(new Measure<T>(
                    measureName,
                    time,
                    unit,
                    value));
            }

            LogService.LogApp(
                LogSeverity.Debug,
                $"Retrieved {measures.Count} measures for component {componentName}, measure {measureName} from {startTime} to {endTime}");
            return measures;
        }
        catch (Exception ex)
        {
            LogService.LogApp(
                LogSeverity.Error,
                $"Error retrieving measures for component {componentName}, measure {measureName}: {ex.Message}");
            return Enumerable.Empty<Measure<T>>();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the InfluxStorageService.
    /// </summary>
    /// <param name="disposing">bool disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _client.Dispose();
            }

            _disposedValue = true;
        }
    }

    private static string SanitizeForInflux(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "unnamed";

        return Regex.Replace(input, @"[^\w\d-_]", "_");
    }

    private static double ConvertToDouble<TValue>(TValue value)
    {
        if (value == null)
            return 0;

        return value switch
        {
            double d => d,
            int i => i,
            long l => l,
            float f => f,
            decimal m => (double)m,
            bool b => b ? 1 : 0,
            _ => throw new NotImplementedException("Conversion for influx not implemented."),
        };
    }

    private static TValue ConvertToType<TValue>(object? value)
    {
        if (value == null)
            return default!;

        if (value is TValue typedValue)
            return typedValue;

        try
        {
            return (TValue)Convert.ChangeType(value, typeof(TValue))!;
        }
        catch
        {
            throw new NotImplementedException("Conversion for influx not implemented.");
        }
    }
}
