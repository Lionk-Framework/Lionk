// Copyright © 2024 Lionk Project

using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.TestComponent;

/// <summary>
/// Counter test component.
/// </summary>
[NamedElement("Counter test", "test cyclic element")]
public class Counter : BaseCyclicComponent
{
    private int _counter;

    /// <summary>
    /// Gets or sets counter value.
    /// </summary>
    public int CounterValue
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    /// <inheritdoc/>
    public override bool CanExecute => true;

    /// <inheritdoc/>
    protected override void OnExecute(CancellationToken ct)
    {
        // comportement qui a lieu à chaque execution, le cancellation token est cancel lors d'un abort
        // a toi de le gérer comme tu veux si tu en as besoin
        base.OnExecute(ct);
        CounterValue++;
    }

    /// <inheritdoc/>
    protected override void OnInitialize()
    {
        // comportement qui a lieu qu'une fois au premier start
        Period = TimeSpan.FromSeconds(1);
        base.OnInitialize();
    }

    /// <inheritdoc/>
    /// comportement qui a lieu à la fin de l'execution
    /// c'est donc executer à chaque cycle après la fin de OnExecute
    /// a toi de voir si tu as du comportement à mettre, autrement même pas besoin d'override
    protected override void OnTerminate()
        => base.OnTerminate();

    /// <inheritdoc/>
    /// comportement qui a lieu lors d'un abort, met fin à l'execution via le cancellationToken
    /// et met le composant en erreur
    public override void Abort() =>
        base.Abort();
}
