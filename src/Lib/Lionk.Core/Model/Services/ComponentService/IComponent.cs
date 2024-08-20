﻿// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// Interface that define a component.
/// </summary>
public interface IComponent : IDisposable
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    string InstanceName { get; set; }

    /// <summary>
    /// Gets the unique identifier of the component.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Dispose methods.
    /// </summary>
    void IDisposable.Dispose()
        => GC.SuppressFinalize(this);
}