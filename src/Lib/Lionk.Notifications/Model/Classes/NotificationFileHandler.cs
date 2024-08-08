// Copyright © 2024 Lionk Project
using Lionk.Notification.Converter;
using Lionk.Utils;
using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
/// This class implements the way notifications are managed with a file.
/// </summary>
public static class NotificationFileHandler
{
    private static readonly FolderType _folderType = FolderType.Data;
    private static readonly string _folderPath = "notifications";
    private static readonly string _notificationPath = Path.Combine(_folderPath, "notifications.json");
    private static readonly string _notifyersPath = Path.Combine(_folderPath, "notifyers.json");
    private static readonly string _channelsPath = Path.Combine(_folderPath, "channels.json");
    private static readonly string _notifyerChannelsPath = Path.Combine(_folderPath, "notifyerChannels.json");
    private static readonly JsonSerializerSettings _jsonSerializerSettings;

    /// <summary>
    /// Initializes static members of the <see cref="NotificationFileHandler"/> class.
    /// </summary>
    static NotificationFileHandler()
    {
        string datafolder = Path.Combine(ConfigurationUtils.GetFolderPath(_folderType), _folderPath);
        FileHelper.CreateDirectoryIfNotExists(datafolder);
        _jsonSerializerSettings = new JsonSerializerSettings();
        _jsonSerializerSettings.Converters.Add(new NotificationPropertiesConverter());
        _jsonSerializerSettings.Converters.Add(new NotifyerChannelDictionaryConverter());
    }

    /// <summary>
    /// Method to save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(NotificationHistory notification)
    {
        List<NotificationHistory> notifications = GetNotifications();
        notifications.Add(notification);
        WriteNotifications(notifications);
    }

    /// <summary>
    /// Method to write the notifications in the file.
    /// </summary>
    /// <param name="notifications"> The list of notifications to write.</param>
    private static void WriteNotifications(List<NotificationHistory> notifications)
    {
        string json = JsonConvert.SerializeObject(notifications, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notificationPath, json, _folderType);
    }

    /// <summary>
    /// Method to get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    /// <exception cref="ArgumentNullException"> If file exists but the result of the deserialization is null.</exception>
    public static List<NotificationHistory> GetNotifications()
    {
        string json = ConfigurationUtils.ReadFile(_notificationPath, _folderType);
        if (string.IsNullOrEmpty(json)) return new();
        List<NotificationHistory> notifications = JsonConvert.DeserializeObject<List<NotificationHistory>>(json, _jsonSerializerSettings) ?? throw new ArgumentNullException(nameof(notifications));
        return notifications;
    }

    /// <summary>
    /// Method to get a notification by its unique identifier.
    /// </summary>
    /// <param name="guid"> The unique identifier of the notification.</param>
    /// <returns> The notification with the specified unique identifier.</returns>
    public static NotificationHistory? GetNotificationByGuid(Guid guid)
    {
        List<NotificationHistory> notifications = GetNotifications();
        return notifications.FirstOrDefault(n => n.Id == guid);
    }

    /// <summary>
    /// Method to edit a notification in history.
    /// </summary>
    /// <param name="notificationHistory">The notification to edit.</param>
    public static void EditNotificationHistory(NotificationHistory notificationHistory)
    {
        List<NotificationHistory> notificationHistories = GetNotifications();
        int index = notificationHistories.FindIndex(n => n.Id == notificationHistory.Id);
        notificationHistories[index] = notificationHistory;
        WriteNotifications(notificationHistories);
    }

    /// <summary>
    /// Method to save notifyers to a file.
    /// </summary>
    /// <param name="notifyers"> The list of notifyers to save.</param>
    public static void SaveNotifyerToJson(List<INotifyer> notifyers)
    {
        string json = JsonConvert.SerializeObject(notifyers, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notifyersPath, json, _folderType);
    }

    /// <summary>
    /// Method to save channels to a file.
    /// </summary>
    /// <param name="channels"> The list of channels to save.</param>
    public static void SaveChannelToJson(List<IChannel> channels)
    {
        string json = JsonConvert.SerializeObject(channels, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_channelsPath, json, _folderType);
    }

    /// <summary>
    /// Method to save notifyer channels dictionary to a file.
    /// </summary>
    /// <param name="notifyerChannels"> The dictionary of notifyers and channels to save.</param>
    public static void SaveNotifyerChannelsToJson(Dictionary<Guid, List<IChannel>> notifyerChannels)
    {
        string json = JsonConvert.SerializeObject(notifyerChannels, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notifyerChannelsPath, json, _folderType);
    }

    /// <summary>
    /// Method to get notifyers from a file.
    /// </summary>
    /// <returns> The list of notifyers.</returns>
    public static List<INotifyer> GetNotifyersFromJson()
    {
        string json = ConfigurationUtils.ReadFile(_notifyersPath, _folderType);
        if (string.IsNullOrEmpty(json)) return new();
        INotifyer[]? notifyers = JsonConvert.DeserializeObject<INotifyer[]>(json, _jsonSerializerSettings);
        if (notifyers is null) return new();
        return notifyers.ToList();
    }

    /// <summary>
    /// Method to get channels from a file.
    /// </summary>
    /// <returns> The list of channels.</returns>
    public static List<IChannel> GetChannelsFromJson()
    {
        string json = ConfigurationUtils.ReadFile(_channelsPath, _folderType);
        if (string.IsNullOrEmpty(json)) return new();
        IChannel[]? channels = JsonConvert.DeserializeObject<IChannel[]>(json, _jsonSerializerSettings);
        if (channels is null) return new();
        return channels.ToList();
    }

    /// <summary>
    /// Method to get notifyer channels dictionary from a file.
    /// </summary>
    /// <returns> The dictionary of notifyers and channels.</returns>
    public static Dictionary<Guid, List<IChannel>> GetNotifyerChannelsFromJson()
    {
        string json = ConfigurationUtils.ReadFile(_notifyerChannelsPath, _folderType);
        if (string.IsNullOrEmpty(json)) return new();
        Dictionary<Guid, List<IChannel>>? notifyerChannels = JsonConvert.DeserializeObject<Dictionary<Guid, List<IChannel>>>(json, _jsonSerializerSettings);
        if (notifyerChannels is null) return new();
        return notifyerChannels;
    }
}
