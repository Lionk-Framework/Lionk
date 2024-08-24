// Copyright © 2024 Lionk Project

namespace Lionk.Core.DataModel;

/// <summary>
///     Represents a measure.
/// </summary>
/// <typeparam name="T">The type of the value measured.</typeparam>
/// <param name="MeasureName">The measure name.</param>
/// <param name="Time">The time.</param>
/// <param name="Unit">The unit of the measure.</param>
/// <param name="Value">The value of the measure.</param>
public record Measure<T>(string MeasureName, DateTime Time, string Unit, T Value);
