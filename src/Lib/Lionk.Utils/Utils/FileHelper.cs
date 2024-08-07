// Copyright © 2024 Lionk Project

namespace Lionk.Utils;

/// <summary>
/// Static class that contains utility methods for files.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Method to create a directory if it does not exist.
    /// </summary>
    /// <param name="path">The path of the directory.</param>
    public static void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
