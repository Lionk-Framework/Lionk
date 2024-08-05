// Copyright © 2024 Lionk Project

using Lionk.Log;

namespace Lionk.Core.TypeRegistery;

/// <summary>
/// Class that create instances of a type.
/// </summary>
public class Factory
{
    private readonly Type _type;

    /// <summary>
    /// Initializes a new instance of the <see cref="Factory"/> class.
    /// </summary>
    /// <param name="type">asd.</param>
    public Factory(Type type)
        => _type = type;

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
            result = Activator.CreateInstance(_type);
        }
        catch (MissingMethodException ex)
        {
            LogService.LogDebug($"Error creating instance of type" +
                $" {_type.Name}. {ex.Message}");
        }

        return result;
    }
}
