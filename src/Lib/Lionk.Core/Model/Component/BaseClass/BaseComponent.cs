// Copyright © 2024 Lionk Project

using Lionk.Core.Observable;

namespace Lionk.Core.Component;

/// <summary>
/// Base class implementation for <see cref="IComponent"/>.
/// </summary>
public abstract class BaseComponent : ObservableElement, IComponent
{
    private string _instanceName = string.Empty;

    /// <inheritdoc/>
    public string InstanceName
    {
        get => _instanceName;
        set => SetField(ref _instanceName, value);
    }

    /// <inheritdoc/>
    public Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public virtual void Dispose()
        => GC.SuppressFinalize(this);
}
