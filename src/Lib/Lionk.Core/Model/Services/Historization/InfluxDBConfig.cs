// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Configuration for InfluxDB connection.
/// </summary>
public class InfluxDBConfig
{
    /// <summary>
    /// Gets or sets the URL of the InfluxDB instance.
    /// </summary>
    public string Url { get; set; } = "http://localhost:8086";

    /// <summary>
    /// Gets or sets the organization name in InfluxDB.
    /// </summary>
    public string Organization { get; set; } = "lionk";

    /// <summary>
    /// Gets or sets the bucket name in InfluxDB.
    /// </summary>
    [Obsolete("This property is deprecated as buckets are now created per component. It is only kept for backward compatibility.")]
    public string Bucket { get; set; } = "measurements";

    /// <summary>
    /// Gets or sets the prefix for component bucket names. Will be prepended to component names to create bucket names.
    /// </summary>
    public string ComponentBucketPrefix { get; set; } = "comp_";

    /// <summary>
    /// Gets or sets the token for authentication with InfluxDB.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the retention period for data in the InfluxDB bucket.
    /// </summary>
    public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromDays(30);
}
