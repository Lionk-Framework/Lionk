// Copyright © 2024 Lionk Project

namespace Lionk.Utils;

/// <summary>
///     Static class that contains utility methods for save files.
/// </summary>
public static class ConfigurationUtils
{
    #region fields

    private static readonly Dictionary<FolderType, string>
        _keyValuePairs = new()
            {
                {
                    // Combine to get the complete path from the folder where the app is running.
                    FolderType.Config,
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "config")
                },
                {
                    FolderType.Logs,
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "logs")
                },
                {
                    FolderType.Data,
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "data")
                },
                {
                    FolderType.Temp,
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "temp")
                },
                {
                    FolderType.Plugin,
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "plugins")
                },
            };

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes static members of the <see cref="ConfigurationUtils" /> class.
    ///     Static constructor to ensure all directories exist.
    /// </summary>
    static ConfigurationUtils()
    {
        foreach (string path in _keyValuePairs.Values)
        {
            FileHelper.CreateDirectoryIfNotExists(path);
        }
    }

    #endregion

    #region public and override methods

    /// <summary>
    ///     Appends content to a file. If the file does not exist, it will be created.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="content">The content to append.</param>
    /// <param name="folderType">The folder type.</param>
    public static void AppendFile(string filename, string content, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        File.AppendAllText(filePath, content);
    }

    /// <summary>
    ///     Method to copy a file to a folder.
    /// </summary>
    /// <param name="sourcePath">The source file path.</param>
    /// <param name="folderType">The folder type.</param>
    public static void CopyFileToFolder(string sourcePath, FolderType folderType)
    {
        string destinationPath = Path.Combine(_keyValuePairs[folderType], Path.GetFileName(sourcePath));

        if (!File.Exists(sourcePath) || File.Exists(destinationPath))
        {
            return;
        }

        File.Copy(sourcePath, destinationPath, false);
    }

    /// <summary>
    ///     Deletes a file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="folderType">The folder type.</param>
    public static void DeleteFile(string filename, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        File.Delete(filePath);
    }

    /// <summary>
    ///     Checks if a file exists.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="folderType">The folderType.</param>
    /// <returns>True if it exists, false otherwise.</returns>
    public static bool FileExists(string filename, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        return File.Exists(filePath);
    }

    /// <summary>
    ///     Gets the folder path for the specified folder type.
    /// </summary>
    /// <param name="folderType">The folder type.</param>
    /// <returns>The path. </returns>
    public static string GetFolderPath(FolderType folderType) => _keyValuePairs[folderType];

    /// <summary>
    ///     Reads content from a file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="folderType">The folder type.</param>
    /// <returns>The content of the file.</returns>
    public static string ReadFile(string filename, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
    }

    /// <summary>
    ///     Saves a file with the specified content.
    ///     If the file already exists, it will be overwritten.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="content">The content to save.</param>
    /// <param name="folderType">The folder type.</param>
    public static void SaveFile(string filename, string content, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        File.WriteAllText(filePath, content);
    }

    /// <summary>
    ///     Saves a file with the specified content.
    ///     If the file already exists, it will be overwritten.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="content">The content to save.</param>
    /// <param name="folderType">The folder type.</param>
    /// <returns>A task.</returns>
    public static async Task SaveFileAsync(string filename, string content, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        await File.WriteAllTextAsync(filePath, content);
    }

    /// <summary>
    ///     Tries to delete a file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="folderType">The folder type.</param>
    /// <returns>True if the file is deleted, false otherwise.</returns>
    public static bool TryDeleteFile(string filename, FolderType folderType)
    {
        string filePath = Path.Combine(_keyValuePairs[folderType], filename);
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    #endregion
}
