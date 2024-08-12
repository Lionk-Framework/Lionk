// Copyright © 2024 Lionk Project

using Microsoft.AspNetCore.Components.Forms;

namespace Lionk.Plugin.Blazor;

/// <summary>
/// Service to upload files.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FileUploadService"/> class.
/// </remarks>
/// <param name="targetFilePath">The target path where file will be saved.</param>
public class FileUploadService(string targetFilePath)
{
    private readonly string _targetFilePath = targetFilePath;

    /// <summary>
    /// Uploads files.
    /// </summary>
    /// <param name="files">The files to upload.</param>
    /// <returns>A new task containing a list of string which represent the new path of the files uploaded.</returns>
    public async Task<List<string>?> UploadFileAsync(IReadOnlyList<IBrowserFile> files)
    {
        var filePaths = new List<string>();

        foreach (IBrowserFile file in files)
        {
            string filePath = Path.Combine(_targetFilePath, file.Name);

            if (File.Exists(filePath)) return null;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream().CopyToAsync(stream);
            }

            filePaths.Add(filePath);
        }

        return filePaths;
    }
}
