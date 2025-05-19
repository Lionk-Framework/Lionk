// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component.Cyclic;

/// <summary>
///     This class defines the extension methods for the <see cref="CyclicComputationMethod" /> enumeration.
/// </summary>
public static class CyclicComputationMethodExtension
{
    #region public and override methods

    /// <summary>
    ///     This method returns the next execution time of a cyclic component.
    /// </summary>
    /// <param name="method"> The cyclic computation method. </param>
    /// <param name="component"> The cyclic component. </param>
    /// <returns> The next execution time. </returns>
    public static DateTime GetNextExecution(this CyclicComputationMethod method, ICyclicComponent component)
    {
        return method switch
        {
            CyclicComputationMethod.RelativeToLastExecution => component.LastExecution + component.Period,
            CyclicComputationMethod.RelativeToStartTime => component.StartedDate + (component.Period * component.NbCycle),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        };
    }

    #endregion
}
