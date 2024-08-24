// Copyright © 2024 Lionk Project
using Lionk.Notification.Converter;
using Lionk.Utils;
using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
///     This class implements the way notifications are managed with a file.
/// </summary>
public static class NotificationFileHandler
{
    #region fields

    private static readonly string _folderPath = "notifications";

    private static readonly string _channelsPath = Path.Combine(_folderPath, "channels.json");

    private static readonly FolderType _folderType = FolderType.Data;

    private static readonly JsonSerializerSettings _jsonSerializerSettings;

    private static readonly string _notificationPath = Path.Combine(_folderPath, "notifications.json");

    private static readonly string _notifierChannelsPath = Path.Combine(_folderPath, "notifyerChannels.json");

    private static readonly string _notifierPath = Path.Combine(_folderPath, "notifyers.json");

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes static members of the <see cref="NotificationFileHandler" /> class.
    /// </summary>
    static NotificationFileHandler()
    {
        string dataFolder = Path.Combine(ConfigurationUtils.GetFolderPath(_folderType), _folderPath);
        FileHelper.CreateDirectoryIfNotExists(dataFolder);
        _jsonSerializerSettings = new JsonSerializerSettings();
        _jsonSerializerSettings.Converters.Add(new NotificationPropertiesConverter());
        _jsonSerializerSettings.Converters.Add(new NotifierChannelDictionaryConverter());
    }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Method to edit a notification in history.
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
    ///     Method to get channels from a file.
    /// </summary>
    /// <returns> The list of channels.</returns>
    public static List<IChannel> GetChannelsFromJson()
    {
        string json = ConfigurationUtils.ReadFile(_channelsPath, _folderType);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        IChannel[]? channels = JsonConvert.DeserializeObject<IChannel[]>(json, _jsonSerializerSettings);
        if (channels is null)
        {
            return [];
        }

        return channels.ToList();
    }

    /// <summary>
    ///     Method to get a notification by its unique identifier.
    /// </summary>
    /// <param name="guid"> The unique identifier of the notification.</param>
    /// <returns> The notification with the specified unique identifier.</returns>
    public static NotificationHistory? GetNotificationByGuid(Guid guid)
    {
        List<NotificationHistory> notifications = GetNotifications();
        return notifications.FirstOrDefault(n => n.Id == guid);
    }

    /// <summary>
    ///     Method to get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    /// <exception cref="ArgumentNullException"> If file exists but the result of the deserialization is null.</exception>
    public static List<NotificationHistory> GetNotifications()
    {
        string json = ConfigurationUtils.ReadFile(_notificationPath, _folderType);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        List<NotificationHistory> notifications = JsonConvert.DeserializeObject<List<NotificationHistory>>(json, _jsonSerializerSettings)
                                                  ?? throw new ArgumentNullException(nameof(notifications));
        return notifications;
    }

    /// <summary>
    ///     Method to get notifier channels dictionary from a file.
    /// </summary>
    /// <returns> The dictionary of notifiers and channels.</returns>
    public static Dictionary<Guid, List<IChannel>> GetNotifierChannelsFromJSon()
    {
        string json = ConfigurationUtils.ReadFile(_notifierChannelsPath, _folderType);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        Dictionary<Guid, List<IChannel>>? notifierChannels =
            JsonConvert.DeserializeObject<Dictionary<Guid, List<IChannel>>>(json, _jsonSerializerSettings);
        return notifierChannels ?? [];
    }

    /// <summary>
    ///     Method to get notifiers from a file.
    /// </summary>
    /// <returns> The list of notifiers.</returns>
    public static List<INotifier> GetNotifiersFromJson()
    {
        string json = ConfigurationUtils.ReadFile(_notifierPath, _folderType);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        INotifier[]? notifiers = JsonConvert.DeserializeObject<INotifier[]>(json, _jsonSerializerSettings);
        return notifiers is null ? [] : notifiers.ToList();
    }

    /// <summary>
    ///     Method to save channels to a file.
    /// </summary>
    /// <param name="channels"> The list of channels to save.</param>
    public static void SaveChannelToJson(List<IChannel> channels)
    {
        string json = JsonConvert.SerializeObject(channels, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_channelsPath, json, _folderType);
    }

    /// <summary>
    ///     Method to save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(NotificationHistory notification)
    {
        List<NotificationHistory> notifications = GetNotifications();
        notifications.Add(notification);
        WriteNotifications(notifications);
    }

    /// <summary>
    ///     Method to save notifier channels dictionary to a file.
    /// </summary>
    /// <param name="notifierChannels"> The dictionary of notifiers and channels to save.</param>
    public static void SaveNotifierChannelsToJson(Dictionary<Guid, List<IChannel>> notifierChannels)
    {
        string json = JsonConvert.SerializeObject(notifierChannels, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notifierChannelsPath, json, _folderType);
    }

    /// <summary>
    ///     Method to save notifiers to a file.
    /// </summary>
    /// <param name="notifiers"> The list of notifiers to save.</param>
    public static void SaveNotifierToJson(List<INotifier> notifiers)
    {
        string json = JsonConvert.SerializeObject(notifiers, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notifierPath, json, _folderType);
    }

    #endregion

    #region others methods

    /// <summary>
    ///     Method to write the notifications in the file.
    /// </summary>
    /// <param name="notifications"> The list of notifications to write.</param>
    private static void WriteNotifications(List<NotificationHistory> notifications)
    {
        string json = JsonConvert.SerializeObject(notifications, Formatting.Indented, _jsonSerializerSettings);
        ConfigurationUtils.SaveFile(_notificationPath, json, _folderType);
    }

    #endregion
}
