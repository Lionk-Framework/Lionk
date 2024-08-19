// Copyright © 2024 Lionk Project

using Lionk.Log;

namespace Lionk.Core.TypeRegister;

/// <summary>
/// Class that create instances of a type.
/// </summary>
public abstract class Factory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Factory"/> class.
    /// </summary>
    /// <param name="type">The type created by the factory.</param>
    public Factory(Type type)
        => Type = type;

    /// <summary>
    /// Gets the type used by the factory.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Called when a new instance is created.
    /// </summary>
    /// <param name="instance">the create instance.</param>
    protected abstract void OnCreateInstance(object instance);

    /// <summary>
    /// Create a new instance of the type.
    /// </summary>
    /// <returns>A new instance of the type used by the factory.
    ///          If the creation is unsucessfull, return null. </returns>
    public object? CreateInstance()
    {
        object? result = default;

        try
        {
            result = Activator.CreateInstance(Type);

            if (result is not null)
                OnCreateInstance(result);
        }
        catch (MissingMethodException ex)
        {
            LogService.LogDebug($"Error creating instance of type" +
                $" {Type.Name}. {ex.Message}");
        }

        return result;
    }
}
