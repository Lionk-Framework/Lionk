// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.TestComponent;

/// <summary>
///     Counter test component.
/// </summary>
[NamedElement("Counter test", "test cyclic element")]
public class Counter : BaseCyclicComponent
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

    #region public and override methods

    /// <inheritdoc />
    /// abort behavior, terminates execution via cancellationToken
    /// and sets the component to error
    public override void Abort() => base.Abort();

    #endregion

    #region others methods

    /// <inheritdoc />
    protected override void OnExecute(CancellationToken ct)
    {
        // behavior that occurs at each execution, the cancellation token is cancelled on abort
        // it's up to you to manage it the way you want if you need it
        base.OnExecute(ct);
        CounterValue++;
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        // behavior that occurs only once at the first start
        Period = TimeSpan.FromSeconds(1);
        base.OnInitialize();
    }

    /// <inheritdoc />
    /// behavior at the end of execution
    /// this means executing every cycle after the end of OnExecute
    /// it's up to you to decide whether you want to use behavior, otherwise you won't even need an override
    protected override void OnTerminate() => base.OnTerminate();

    #endregion
}
