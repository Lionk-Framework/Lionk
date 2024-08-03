// Copyright © 2024 Lionk Project

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Model.Abstraction;

namespace Notifications.Model.Converters;

/// <summary>
/// This class is used to convert a notifyer.
/// </summary>
public class NotificationPropertiesConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType) => objectType switch
    {
        Type t when typeof(INotifyer).IsAssignableFrom(t) => true,
        Type t when typeof(IChannel).IsAssignableFrom(t) => true,
        _ => false,
    };

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        JObject jsonObject = new();
        if (value is null) throw new ArgumentNullException(nameof(value));
        Type type = value.GetType();

        jsonObject.Add("Type", type.FullName);

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.CanRead)
            {
                object? propertyValue = property.GetValue(value);
                if (propertyValue is null) continue;
                jsonObject.Add(property.Name, JToken.FromObject(propertyValue, serializer));
            }
        }

        jsonObject.WriteTo(writer);
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        if (jsonObject is null) throw new ArgumentNullException(nameof(jsonObject));

        string? typeName = jsonObject["Type"]?.ToString();
        if (typeName is null) throw new JsonSerializationException("The type of the object is not defined inside the JSON object.");

        Type? type = GetFullType(typeName);
        if (type is null) throw new JsonSerializationException($"The type {typeName} is not found.");

        object? obj = Activator.CreateInstance(type);
        if (obj is null) throw new JsonSerializationException($"Impossible to create an instance of {type.FullName}.");
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

    private Type? GetFullType(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type? type = assemblies.Select(assembly => assembly.GetType(typeName, false, true)).FirstOrDefault(type => type != null);
        return type;
    }
}
