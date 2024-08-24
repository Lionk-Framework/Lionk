// Copyright © 2024 Lionk Project

namespace Lionk.Core.DataModel;

/// <summary>
///     Represents a measure.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="MeasureName">ayd.</param>
/// <param name="Time">dfg.</param>
/// <param name="Unit">dfdag.</param>
/// <param name="Value">dfdg.</param>
public record Measure<T>(string MeasureName, DateTime Time, string Unit, T Value);
