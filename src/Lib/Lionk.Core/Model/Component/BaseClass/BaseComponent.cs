// Copyright © 2024 Lionk Project

using Lionk.Core.Observable;

namespace Lionk.Core.Component;

/// <summary>
///     Base class implementation for <see cref="IComponent" />.
/// </summary>
public abstract class BaseComponent : ObservableElement, IComponent
{
    #region fields

    private string _instanceName = string.Empty;

    #endregion

    #region properties

    /// <inheritdoc />
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <inheritdoc />
    public string InstanceName
    {
        get => _instanceName;
        set => SetField(ref _instanceName, value);
    }

    #endregion

    #region public and override methods

    /// <inheritdoc />
    public virtual void Dispose() => GC.SuppressFinalize(this);

    #endregion
}
