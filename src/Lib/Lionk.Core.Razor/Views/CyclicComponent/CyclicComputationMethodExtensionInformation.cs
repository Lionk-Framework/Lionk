// Copyright © 2024 Lionk Project


// Copyright © 2024 Lionk Project

using Lionk.Core.Component.Cyclic;

namespace Lionk.Core.Razor.Views.CyclicComponent;

/// <summary>
/// This class defines the information for the <see cref="CyclicComputationMethod"/> enumeration.
/// </summary>
public static class CyclicComputationMethodExtensionInformation
{
    /// <summary>
    /// This method returns the friendly string representation of the <see cref="CyclicComputationMethod"/> enumeration.
    /// </summary>
    /// <param name="method"> The cyclic computation method. </param>
    /// <returns> The friendly string representation. </returns>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when the method is not recognized. </exception>
    public static string ToFriendlyString(this CyclicComputationMethod method)
    {
        switch (method)
        {
            case CyclicComputationMethod.RelativeToLastExecution:
                return "Relative to last execution";
            case CyclicComputationMethod.RelativeToStartTime:
                return "Relative to start time";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// This method returns the description of the <see cref="CyclicComputationMethod"/> enumeration.
    /// </summary>
    /// <param name="method"> The cyclic computation method. </param>
    /// <returns> The description. </returns>
    public static string GetDescription(this CyclicComputationMethod method)
    {
        switch (method)
        {
            case CyclicComputationMethod.RelativeToLastExecution:
                return "The computation is relative to the last execution, if the component is executed late, the next execution will be impacted.";
            case CyclicComputationMethod.RelativeToStartTime:
                return "The computation is relative to the start time adding the number of cycles multiplied by the periode, " +
                    "if the component is executed late, the next execution will not be impacted. " +
                    "Be careful, if the execution is too late, the component will be executed multiple times with high frequency.";
            default:
                return "Unknown";
        }
    }
}
