// Copyright © 2024 Lionk Project

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lionk.Notification.Converter;

/// <summary>
///     This class is used to convert a notifier.
/// </summary>
public class NotificationPropertiesConverter : JsonConverter
{
    #region public and override methods

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) =>
        objectType switch
        {
            Type t when typeof(IChannel).IsAssignableFrom(t) => true,
            Type t when typeof(IRecipient).IsAssignableFrom(t) => true,
            Type t when typeof(INotifier).IsAssignableFrom(t) => true,
            _ => false,
        };

    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        if (jsonObject is null)
        {
            throw new ArgumentNullException(nameof(jsonObject));
        }

        string? typeName = jsonObject["Type"]?.ToString();
        if (typeName is null)
        {
            throw new JsonSerializationException("The type of the object is not defined inside the JSON object.");
        }

        Type? type = GetFullType(typeName);
        if (type is null)
        {
            throw new JsonSerializationException($"The type {typeName} is not found.");
        }

        // Get the constructor with the JsonConstructorAttribute
        ConstructorInfo? jsonConstructor = type.GetConstructors()
            .FirstOrDefault(c => c.GetCustomAttribute<JsonConstructorAttribute>() != null);

        object? obj;
        if (jsonConstructor != null)
        {
            object?[] args = jsonConstructor.GetParameters().Select(
                p =>
                {
                    string? jsonKey = jsonObject.Properties()
                        .FirstOrDefault(prop => string.Equals(prop.Name, p.Name, StringComparison.OrdinalIgnoreCase))?.Name;

                    return jsonKey != null ? jsonObject[jsonKey]?.ToObject(p.ParameterType, serializer) : null;
                }).ToArray();
            obj = Activator.CreateInstance(type, args);
        }
        else
        {
            obj = Activator.CreateInstance(type);
        }

        if (obj is null)
        {
            throw new JsonSerializationException($"Impossible to create an instance of {type.FullName}.");
        }

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanWrite && jsonObject.TryGetValue(property.Name, out JToken? value))
            {
                object? propertyValue = value.ToObject(property.PropertyType, serializer);
                property.SetValue(obj, propertyValue);
            }
        }

        return obj;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JObject jsonObject = [];
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Type type = value.GetType();

        jsonObject.Add("Type", type.FullName);

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanRead)
            {
                object? propertyValue = property.GetValue(value);
                if (propertyValue is null)
                {
                    continue;
                }

                jsonObject.Add(property.Name, JToken.FromObject(propertyValue, serializer));
            }
        }

        jsonObject.WriteTo(writer);
    }

    #endregion

    #region others methods

    /// <summary>
    ///     Gets the type of the object.
    /// </summary>
    /// <param name="typeName"> The name of the type.</param>
    /// <returns> The type of the object.</returns>
    protected Type? GetFullType(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type? type = assemblies.Select(assembly => assembly.GetType(typeName, false, true)).FirstOrDefault((type) => type != null);
        return type;
    }

    #endregion
}
