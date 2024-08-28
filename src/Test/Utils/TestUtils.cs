// Copyright © 2024 Lionk Project

namespace LionkTest;

/// <summary>
///     Utility class.
/// </summary>
public static class TestUtils
{
    #region public and override methods

    /// <summary>
    ///     Method to delete all config files.
    /// </summary>
    public static void DeleteAllConfigFile()
    {
        string[] files = Directory.GetFiles("config", "*.json", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            File.Delete(file);
        }
    }

    #endregion
}
