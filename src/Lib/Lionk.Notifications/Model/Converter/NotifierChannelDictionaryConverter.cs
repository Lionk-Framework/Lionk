// Copyright © 2024 Lionk Project

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lionk.Notification.Converter;

/// <summary>
///     This class is used to convert a dictionary of Notifier and Channels to a json object.
/// </summary>
public class NotifierChannelDictionaryConverter : JsonConverter
{
    #region public and override methods

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<INotifier, List<IChannel>>);

    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var dictionary = new Dictionary<INotifier, List<IChannel>>();

        foreach (KeyValuePair<string, JToken?> kvp in jsonObject)
        {
            if (kvp.Value is not JObject entry)
            {
                throw new JsonSerializationException("Invalid entry in JSON object.");
            }

            JObject? jObject = entry["Key"]?.ToObject<JObject>(serializer);
            if (jObject is null)
            {
                throw new JsonSerializationException("The notifyer object is null.");
            }

            if (jObject.ToObject(
                    GetFullType(jObject["Type"]?.ToString() ?? string.Empty),
                    serializer) is not INotifier notifier)
            {
                throw new JsonSerializationException("The notifyer object is null.");
            }

            List<IChannel> channels = entry["Value"]?.ToObject<List<IChannel>>(serializer) ?? [];

            dictionary[notifier] = channels;
        }

        return dictionary;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var dictionary = (Dictionary<INotifier, List<IChannel>>)value;
        var jsonObject = new JObject();

        foreach (KeyValuePair<INotifier, List<IChannel>> kvp in dictionary)
        {
            var keyObject = JObject.FromObject(kvp.Key, serializer);
            var valueArray = JArray.FromObject(kvp.Value, serializer);

            string? type = keyObject["Type"]?.ToString();
            string? name = keyObject["Name"]?.ToString();

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name))
            {
                throw new JsonSerializationException("The key object is not valid.");
            }

            string uniqueKey = $"{type}";

            jsonObject[uniqueKey] = new JObject { ["Key"] = keyObject, ["Value"] = valueArray };
        }

        jsonObject.WriteTo(writer);
    }

    #endregion

    #region others methods

    private Type GetFullType(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type? type = assemblies.Select(assembly => assembly.GetType(typeName, false, true)).FirstOrDefault((t) => t != null);
        if (type is null)
        {
            throw new JsonSerializationException($"The type {typeName} is not found.");
        }

        return type;
    }

    #endregion
}
