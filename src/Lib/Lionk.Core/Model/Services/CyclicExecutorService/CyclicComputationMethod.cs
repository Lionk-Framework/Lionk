// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Cyclic;

/// <summary>
///     This enumeration defines the cyclic computation method.
/// </summary>
public enum CyclicComputationMethod
{
    /// <summary>
    ///     The computation is relative to the last execution.
    /// </summary>
    RelativeToLastExecution,

    /// <summary>
    ///     The computation is relative to the start time + the number of cycles * period.
    /// </summary>
    RelativeToStartTime,
}
