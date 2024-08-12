// Copyright © 2024 Lionk Project

using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lionk.Notification.Converter;

/// <summary>
/// This class is used to convert a dictionary of Notifyer and Channels to a json object.
/// </summary>
public class NotifyerChannelDictionaryConverter : JsonConverter
{
    /// <inheritdoc/>
    public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<INotifyer, List<IChannel>>);

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        var dictionary = (Dictionary<INotifyer, List<IChannel>>)value;
        var jsonObject = new JObject();

        foreach (KeyValuePair<INotifyer, List<IChannel>> kvp in dictionary)
        {
            var keyObject = JObject.FromObject(kvp.Key, serializer);
            var valueArray = JArray.FromObject(kvp.Value, serializer);

            string? type = keyObject["Type"]?.ToString();
            string? name = keyObject["Name"]?.ToString();

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(name)) throw new JsonSerializationException("The key object is not valid.");

            // Utiliser le type et le nom pour générer une clé unique
            string uniqueKey = $"{type}";

            // Écraser le contenu si la clé existe déjà
            jsonObject[uniqueKey] = new JObject
            {
                ["Key"] = keyObject,
                ["Value"] = valueArray,
            };
        }

        jsonObject.WriteTo(writer);
    }

    /// <inheritdoc/>
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var dictionary = new Dictionary<INotifyer, List<IChannel>>();

        foreach (KeyValuePair<string, JToken?> kvp in jsonObject)
        {
            var entry = kvp.Value as JObject;
            if (entry is null) throw new JsonSerializationException("Invalid entry in JSON object.");

            JObject? notifyerObject = entry["Key"]?.ToObject<JObject>(serializer);
            if (notifyerObject is null) throw new JsonSerializationException("The notifyer object is null.");

            var notifyer = notifyerObject.ToObject(GetFullType(notifyerObject["Type"]?.ToString() ?? string.Empty), serializer) as INotifyer;
            if (notifyer is null) throw new JsonSerializationException("The notifyer object is null.");

            List<IChannel> channels = entry["Value"]?.ToObject<List<IChannel>>(serializer) ?? [];

            dictionary[notifyer] = channels; // Écraser l'entrée si la clé existe déjà
        }

        return dictionary;
    }

    private Type GetFullType(string typeName)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type? type = assemblies.Select(assembly => assembly.GetType(typeName, false, true)).FirstOrDefault(t => t != null);
        if (type is null) throw new JsonSerializationException($"The type {typeName} is not found.");
        return type;
    }
}
