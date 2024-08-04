// Copyright © 2024 Lionk Project
using Lionk.Notification.Converter;
using Newtonsoft.Json;

namespace Lionk.Notification;

/// <summary>
/// This class implements the way notifications are managed with a file.
/// </summary>
public static class NotificationFileHandler
{
    /// <summary>
    /// Gets the folder name where the notifications are saved.
    /// </summary>
    public static string FolderName => "Notifications";

    /// <summary>
    /// Gets the file path where the notifications are saved.
    /// </summary>
    public static string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName, "Notifications.json");

    /// <summary>
    /// Gets the settings for the JSON serializer.
    /// </summary>
    public static JsonSerializerSettings JsonSerializerSettings { get; private set; }

    /// <summary>
    /// Initializes static members of the <see cref="NotificationFileHandler"/> class.
    /// </summary>
    static NotificationFileHandler()
    {
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName)))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName));
        }

        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }

        JsonSerializerSettings = new JsonSerializerSettings();
        JsonSerializerSettings.Converters.Add(new NotificationPropertiesConverter());
        JsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    /// <summary>
    /// Method to save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(NotificationHistory notification)
    {
        List<NotificationHistory> notifications = GetNotifications();
        notifications.Add(notification);
        string json = JsonConvert.SerializeObject(notifications, Formatting.Indented, JsonSerializerSettings);
        File.WriteAllText(FilePath, json);
    }

    /// <summary>
    /// Method to get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    /// <exception cref="ArgumentNullException"> If file exists but the result of the deserialization is null.</exception>
    public static List<NotificationHistory> GetNotifications()
    {
        List<NotificationHistory> notifications = new();
        if (File.Exists(FilePath))
        {
            notifications = JsonConvert.DeserializeObject<List<NotificationHistory>>(File.ReadAllText(FilePath), JsonSerializerSettings) ?? throw new ArgumentNullException(nameof(notifications));
        }

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
        return notifications.FirstOrDefault(n => n.Notification.Id == guid);
    }
}
