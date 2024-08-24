// Copyright © 2024 Lionk Project
using Lionk.Core.Component.Cyclic;

namespace Lionk.Core.Component;

/// <summary>
///     This interface defines a cyclic component.
/// </summary>
public interface ICyclicComponent : IExecutableComponent
{
    #region properties

    /// <summary>
    ///     Gets or sets the cyclic computation method.
    /// </summary>
    public CyclicComputationMethod CyclicComputationMethod { get; set; }

    /// <summary>
    ///     Gets the last execution time of the component.
    /// </summary>
    DateTime LastExecution { get; }

    /// <summary>
    ///     Gets the number of cycles executed.
    /// </summary>
    int NbCycle { get; }

    /// <summary>
    ///     Gets the next execution time of the component depending on the <see cref="CyclicComputationMethod" />.
    /// </summary>
    DateTime NextExecution => CyclicComputationMethod.GetNextExecution(this);

    /// <summary>
    ///     Gets or sets the period of the component.
    /// </summary>
    TimeSpan Period { get; set; }

    /// <summary>
    ///     Gets the starting date of the component.
    /// </summary>
    DateTime StartedDate { get; }

    #endregion
}
