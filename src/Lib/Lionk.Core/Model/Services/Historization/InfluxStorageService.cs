// Copyright © 2024 Lionk Project

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Lionk.Core.DataModel;
using Lionk.Log;

namespace Lionk.Core;

/// <summary>
/// Implementation of IDataStorageService that stores data in an InfluxDB v3 time-series database.
/// This implementation creates one bucket per component and uses the measure name as the table name.
/// </summary>
public class InfluxStorageService : IDataStorageService, IDisposable
{
    #region fields

    private readonly InfluxDBClient _client;
    private readonly InfluxDBConfig _config;
    private readonly WriteApiAsync _writeApi;
    private readonly QueryApi _queryApi;
    private readonly BucketsApi _bucketsApi;
    private readonly OrganizationsApi _organizationsApi;
    private readonly ConcurrentDictionary<string, bool> _initializedBuckets = new();

    private bool _initialized = false;
    private bool _disposedValue;
    private string _organizationId = string.Empty;

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxStorageService"/> class.
    /// </summary>
    public InfluxStorageService()
        : this(new InfluxDBConfig())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InfluxStorageService"/> class with specific configuration.
    /// </summary>
    /// <param name="config">The InfluxDB configuration.</param>
    public InfluxStorageService(InfluxDBConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        InfluxDBClientOptions options = new InfluxDBClientOptions.Builder()
            .Url(_config.Url)
            .AuthenticateToken(_config.Token.ToCharArray())
            .Org(_config.Organization)
            .Build();

        _client = new InfluxDBClient(options);
        _writeApi = _client.GetWriteApiAsync();
        _queryApi = _client.GetQueryApi();
        _bucketsApi = _client.GetBucketsApi();
        _organizationsApi = _client.GetOrganizationsApi();

        Task.Run(InitializeAsync).Wait();
        LogService.LogApp(LogSeverity.Information, $"InfluxStorageService initialized with URL: {_config.Url}");
    }

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public void StoreMeasure<T>(string componentName, Measure<T> measure)
    {
        if (!_initialized)
        {
            LogService.LogApp(LogSeverity.Warning, "InfluxStorageService not initialized yet, measure will be dropped");
            return;
        }

        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);
            string sanitizedMeasureName = SanitizeForInflux(measure.MeasureName);
            string componentBucketName = GetComponentBucketName(sanitizedComponentName);

            // Determine retention period - try to find the component to get its retention period
            TimeSpan? retentionPeriod = null;
            try
            {
                // TODO: This requires a dependency on IComponentService, which would be a cleaner approach
                // For now, we'll handle this by caching bucket creations and using the default retention period

                // If we had access to the component:
                // IMeasurableComponent measurable = _componentService.GetComponentByName(componentName);
                // retentionPeriod = measurable?.HistoryDuration;
            }
            catch
            {
                // Fall back to default retention if component access fails
            }

            // Ensure the bucket exists for this component
            CreateBucketIfNotExists(componentName, componentBucketName, retentionPeriod).Wait();

            // Use the measure name as the measurement (_measurement field)
            PointData point = PointData.Measurement(sanitizedMeasureName)
                .Tag("unit", measure.Unit)
                .Field("value", ConvertToDouble(measure.Value))
                .Timestamp(measure.Time, WritePrecision.Ns);

            _writeApi.WritePointAsync(point, componentBucketName, _config.Organization);

            LogService.LogApp(
                LogSeverity.Debug,
                $"Stored measure {measure.MeasureName} with value {measure.Value} for component {componentName} in bucket {componentBucketName}");
        }
        catch (Exception ex)
        {
            LogService.LogApp(
                LogSeverity.Error,
                $"Error storing measure {measure.MeasureName} for component {componentName}: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public IEnumerable<Measure<T>> GetMeasures<T>(string componentName, DateTime startTime, DateTime endTime)
    {
        if (!_initialized)
        {
            LogService.LogApp(
                LogSeverity.Warning,
                "InfluxStorageService not initialized yet, returning empty collection");
            return Enumerable.Empty<Measure<T>>();
        }

        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);
            string componentBucketName = GetComponentBucketName(sanitizedComponentName);

            // Query all measurements from the component's bucket
            string flux = $@"
                from(bucket: ""{componentBucketName}"")
                    |> range(start: {startTime:yyyy-MM-ddTHH:mm:ssZ}, stop: {endTime:yyyy-MM-ddTHH:mm:ssZ})
            ";

            List<InfluxDB.Client.Core.Flux.Domain.FluxTable> tables = _queryApi.QueryAsync(flux, _config.Organization).Result;

            var measures = new List<Measure<T>>();

            foreach (InfluxDB.Client.Core.Flux.Domain.FluxRecord? record in tables.SelectMany(table => table.Records))
            {
                // The measurement name is now the _measurement field
                string measureName = record.GetMeasurement();
                string unit = record.GetValueByKey("unit")?.ToString() ?? string.Empty;
                T? value = ConvertToType<T>(record.GetValue());
                DateTime time = record.GetTime()?.ToDateTimeUtc() ?? DateTime.UtcNow;

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
            LogService.LogApp(
                LogSeverity.Error,
                $"Error retrieving measures for component {componentName}: {ex.Message}");
            return Enumerable.Empty<Measure<T>>();
        }
    }

    /// <inheritdoc/>
    public IEnumerable<Measure<T>> GetMeasures<T>(string componentName, string measureName, DateTime startTime, DateTime endTime)
    {
        if (!_initialized)
        {
            LogService.LogApp(
                LogSeverity.Warning,
                "InfluxStorageService not initialized yet, returning empty collection");
            return Enumerable.Empty<Measure<T>>();
        }

        try
        {
            string sanitizedComponentName = SanitizeForInflux(componentName);
            string sanitizedMeasureName = SanitizeForInflux(measureName);
            string componentBucketName = GetComponentBucketName(sanitizedComponentName);

            // Query a specific measurement from the component's bucket
            string flux = $@"
                from(bucket: ""{componentBucketName}"")
                    |> range(start: {startTime:yyyy-MM-ddTHH:mm:ssZ}, stop: {endTime:yyyy-MM-ddTHH:mm:ssZ})
                    |> filter(fn: (r) => r._measurement == ""{sanitizedMeasureName}"")
            ";

            List<InfluxDB.Client.Core.Flux.Domain.FluxTable> tables = _queryApi.QueryAsync(flux, _config.Organization).Result;

            var measures = new List<Measure<T>>();

            foreach (InfluxDB.Client.Core.Flux.Domain.FluxRecord? record in tables.SelectMany(table => table.Records))
            {
                string unit = record.GetValueByKey("unit")?.ToString() ?? string.Empty;
                T? value = ConvertToType<T>(record.GetValue());
                DateTime time = record.GetTime()?.ToDateTimeUtc() ?? DateTime.UtcNow;

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

    /// <summary>
    /// Disposes resources used by the InfluxStorageService.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region other methods

    /// <summary>
    /// Converts a value of any type to double for storage in InfluxDB.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to convert.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted double value.</returns>
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
            _ => 0,
        };
    }

    /// <summary>
    /// Converts a value from InfluxDB to the specified type.
    /// </summary>
    /// <typeparam name="TValue">The target type.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    private static TValue ConvertToType<TValue>(object? value)
    {
        if (value == null)
            return default!;

        // If T is directly the same type as value, return it
        if (value is TValue typedValue)
            return typedValue;

        // Try to convert numeric types
        if (typeof(TValue) == typeof(double) && value is double doubleValue)
            return (TValue)(object)doubleValue;
        if (typeof(TValue) == typeof(int) && value is double doubleForInt)
            return (TValue)(object)(int)doubleForInt;
        if (typeof(TValue) == typeof(long) && value is double doubleForLong)
            return (TValue)(object)(long)doubleForLong;
        if (typeof(TValue) == typeof(float) && value is double doubleForFloat)
            return (TValue)(object)(float)doubleForFloat;
        if (typeof(TValue) == typeof(decimal) && value is double doubleForDecimal)
            return (TValue)(object)(decimal)doubleForDecimal;
        if (typeof(TValue) == typeof(bool) && value is double doubleForBool)
            return (TValue)(object)(doubleForBool != 0);

        // If all else fails, try to use Convert
        try
        {
            return (TValue)Convert.ChangeType(value, typeof(TValue))!;
        }
        catch
        {
            return default!;
        }
    }

    /// <summary>
    /// Sanitizes a string for use in InfluxDB by removing invalid characters.
    /// </summary>
    /// <param name="input">The input string to sanitize.</param>
    /// <returns>A sanitized string suitable for InfluxDB tags or measurements.</returns>
    private static string SanitizeForInflux(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "unnamed";

        // Remove problematic characters for InfluxDB, replace spaces with underscores
        string sanitized = Regex.Replace(input, @"[^\w\d-_]", "_");
        return sanitized;
    }

    /// <summary>
    /// Initializes the InfluxDB bucket and organization if they don't exist.
    /// </summary>
    private async Task InitializeAsync()
    {
        try
        {
            // Check if the organization exists, create it if it doesn't
            List<Organization> organizations = await _organizationsApi.FindOrganizationsAsync();
            Organization? organization = organizations.FirstOrDefault(o => o.Name == _config.Organization);

            if (organization == null)
            {
                LogService.LogApp(
                    LogSeverity.Information,
                    $"Creating organization '{_config.Organization}' in InfluxDB");

                organization = await _organizationsApi.CreateOrganizationAsync(
                    new Organization { Name = _config.Organization });
            }

            // Get the organization ID and store it for later use
            _organizationId = organization.Id;

            _initialized = true;
            LogService.LogApp(
                LogSeverity.Information,
                $"InfluxDB initialized with organization '{_config.Organization}'");
        }
        catch (Exception ex)
        {
            LogService.LogApp(LogSeverity.Error, $"Error initializing InfluxDB: {ex.Message}");
        }
    }

    /// <summary>
    /// Releases unmanaged and managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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

    /// <summary>
    /// Gets the component bucket name with prefix.
    /// </summary>
    /// <param name="componentName">The component name.</param>
    /// <returns>The bucket name for the component.</returns>
    private string GetComponentBucketName(string componentName) => $"{_config.ComponentBucketPrefix}{componentName}";

    /// <summary>
    /// Creates a bucket for a component if it doesn't exist yet.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <param name="bucketName">The name of the bucket to create.</param>
    /// <param name="retentionPeriod">The retention period for data in the bucket.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CreateBucketIfNotExists(string componentName, string bucketName, TimeSpan? retentionPeriod = null)
    {
        // If we've already checked this bucket, don't check again
        if (_initializedBuckets.TryGetValue(bucketName, out bool exists) && exists)
        {
            return;
        }

        try
        {
            // Check if the bucket exists
            List<Bucket> buckets = await _bucketsApi.FindBucketsAsync();
            Bucket? bucket = buckets.FirstOrDefault(b => b.Name == bucketName);

            if (bucket == null)
            {
                // Use the provided retention period or fall back to the config default
                TimeSpan ttl = retentionPeriod ?? _config.RetentionPeriod;

                LogService.LogApp(
                    LogSeverity.Information,
                    $"Creating bucket '{bucketName}' for component '{componentName}' with retention period {ttl.TotalDays} days");

                var retentionRules = new List<BucketRetentionRules>
                {
                    new()
                    {
                        EverySeconds = (long)ttl.TotalSeconds,
                        Type = BucketRetentionRules.TypeEnum.Expire,
                    },
                };

                await _bucketsApi.CreateBucketAsync(
                                new Bucket
                                {
                                    Name = bucketName,
                                    OrgID = _organizationId,
                                    RetentionRules = retentionRules,
                                });
            }

            // Mark this bucket as initialized
            _initializedBuckets[bucketName] = true;
        }
        catch (Exception ex)
        {
            LogService.LogApp(
                LogSeverity.Error,
                $"Error creating bucket '{bucketName}' for component '{componentName}': {ex.Message}");
        }
    }

    #endregion
}
