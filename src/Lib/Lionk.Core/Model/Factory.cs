// Copyright © 2024 Lionk Project

using Lionk.Log;

namespace Lionk.Core.TypeRegistery;

/// <summary>
/// Class that create instances of a type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Factory"/> class.
/// </remarks>
/// <param name="type">asd.</param>
public class Factory(Type type)
{
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
            result = Activator.CreateInstance(type);
        }
        catch (MissingMethodException ex)
        {
            LogService.LogDebug($"Error creating instance of type" +
                $" {type.Name}. {ex.Message}");
        }

        return result;
    }
}
