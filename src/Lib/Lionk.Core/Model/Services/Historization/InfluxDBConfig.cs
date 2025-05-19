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
    /// Gets or sets the token for authentication with InfluxDB.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default database.
    /// </summary>
    public string DefaultDatabase { get; set; } = "lionk";

    /// <summary>
    /// Gets or sets the retention period for data in the InfluxDB bucket.
    /// </summary>
    public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromDays(30);
}
