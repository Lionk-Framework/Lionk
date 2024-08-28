// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.TestComponent;

/// <summary>
///     Counter test component.
/// </summary>
[NamedElement("Error component", "test cyclic element")]
public class ComponentWhichGoToError : BaseCyclicComponent
{
    #region fields

    private int _counter;

    #endregion

    #region properties

    /// <inheritdoc />
    public override bool CanExecute => true;

    /// <summary>
    ///     Gets or sets counter value.
    /// </summary>
    public int CounterValue
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    #endregion

    #region others methods

    /// <inheritdoc />
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        throw new Exception();
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        // behavior that occurs only once at the first start
        Period = TimeSpan.FromSeconds(10);
        base.OnInitialize();
    }

    #endregion
}
