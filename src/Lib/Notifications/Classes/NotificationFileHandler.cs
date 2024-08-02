// Copyright © 2024 Lionk Project
using Newtonsoft.Json;

namespace Notifications.Classes;

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
    }

    /// <summary>
    /// Method to save a notification in history.
    /// </summary>
    /// <param name="notification"> The notification to save.</param>
    public static void SaveNotification(Notification notification)
    {
        List<Notification> notifications = GetNotifications();
        notifications.Add(notification);
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(notifications, Formatting.Indented));
    }

    /// <summary>
    /// Method to get all the notifications saved.
    /// </summary>
    /// <returns> The list of notifications saved.</returns>
    /// <exception cref="ArgumentNullException"> If file exists but the result of the deserialization is null.</exception>
    public static List<Notification> GetNotifications()
    {
        List<Notification> notifications = new();
        if (File.Exists(FilePath))
        {
            notifications = JsonConvert.DeserializeObject<List<Notification>>(File.ReadAllText(FilePath)) ?? throw new ArgumentNullException(nameof(notifications));
        }

        return notifications;
    }
}
