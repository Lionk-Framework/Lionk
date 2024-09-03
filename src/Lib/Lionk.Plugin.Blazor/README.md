# Lionk Plugin Blazor Library

## Overview

The Lionk Plugin Blazor Library provides an interactive and dynamic interface for managing plugins directly within Blazor applications. It includes features for uploading plugins, displaying plugin information, and managing dependencies in a user-friendly manner. This library extends the capabilities of the Lionk Plugin Library by integrating it with Blazor components, allowing seamless plugin management through a web interface.

## Features

- **File Upload Service**: Handles file uploads for plugins, saving them to a specified target directory.
- **Dynamic Plugin Management**: View, add, and remove plugins within the Blazor UI.
- **Dependency Visualization**: Display and interact with plugin dependencies, showing their load status and allowing for detailed inspection.
- **Responsive UI Components**: Utilizes the MudBlazor UI library to provide a responsive and modern interface for managing plugins.

## Installation

To install the Lionk Plugin Blazor Library, use the NuGet package manager console:

```bash
Install-Package Lionk.Plugin.Blazor
```

## Getting Started

### 1. Setting Up the FileUploadService

The `FileUploadService` is responsible for handling file uploads in the Blazor application. To set up the service, create an instance by specifying the target directory where the files should be saved:

```csharp
using Lionk.Plugin.Blazor;

// Initialize FileUploadService with the target directory
var fileUploadService = new FileUploadService("path/to/target/directory");
```

### 2. Uploading Files

The `UploadFileAsync` method of the `FileUploadService` uploads files to the specified directory and returns the new file paths if successful. If a file already exists at the target location, the upload will be canceled:

```csharp
var files = await fileInput.GetMultipleFilesAsync(); // Assume fileInput is a MudBlazor file input component
List<string>? uploadedPaths = await fileUploadService.UploadFileAsync(files);

if (uploadedPaths != null)
{
    Console.WriteLine("Files uploaded successfully:");
    foreach (var path in uploadedPaths)
    {
        Console.WriteLine(path);
    }
}
else
{
    Console.WriteLine("File upload failed. File already exists.");
}
```

### 3. Managing Plugins in Blazor

The library provides a Blazor component interface for managing plugins, allowing you to view, add, delete, and inspect plugins directly in the application. The plugin management page and components utilize MudBlazor for a clean and intuitive UI.

#### Displaying Plugins

The `PluginCard` component is used to display details about each plugin, including its name, version, author, description, and dependencies:

```razor
<PluginCard Plugin="@plugin" OnDependencySelect="HandleDependencySelect" OnDelete="DeletePlugin" />
```

#### Handling Plugin Dependencies

Dependencies are shown within each plugin card. If a dependency is not loaded, it is displayed with an error icon. Clicking on dependencies triggers further actions like inspecting or resolving issues:

```razor
<MudCollapse Expanded="@ShowDependencies">
    <MudText Typo="Typo.caption">Dependencies:</MudText>
    <MudList T="string" Class="small-font non-selectable">
        @foreach (Dependency dependency in Plugin.Dependencies)
        {
            <MudListItem OnClick="() => OnDependencySelectAsync(dependency)">
                @if (dependency.IsLoaded)
                {
                    <MudText>@dependency.AssemblyName.FullName</MudText>
                }
                else
                {
                    <MudText Color="Color.Error">@dependency.AssemblyName.FullName</MudText>
                }
            </MudListItem>
        }
    </MudList>
</MudCollapse>
```

#### Adding Plugins via File Upload

The library supports file uploads directly from the UI using the MudBlazor file upload component:

```razor
<MudFileUpload T="IReadOnlyList<IBrowserFile>" Accept=".dll" OnFilesChanged="OnInputFileChanged">
    <ActivatorContent>
        <MudPaper Height="50px" Outlined="true" Class="drag-area">
            <MudText Typo="Typo.subtitle2" Align="Align.Center">Drag files here or click to browse</MudText>
            <MudButton Color="Color.Primary" Variant="Variant.Filled" OnClick="OpenFilePickerAsync">Browse</MudButton>
        </MudPaper>
    </ActivatorContent>
</MudFileUpload>
```

Handle the file input event to upload the files and add them as plugins:

```csharp
private async Task OnInputFileChanged(InputFileChangeEventArgs e)
{
    IReadOnlyList<IBrowserFile> files = e.GetMultipleFiles();
    List<string>? uploadedPaths = await FileUploadService.UploadFileAsync(files);

    if (uploadedPaths == null)
    {
        Snackbar.Add("File upload failed: Plugin with the same name already exists!", Severity.Error);
        return;
    }

    foreach (string file in uploadedPaths)
    {
        PluginManager.AddPlugin(file);
        Snackbar.Add($"Plugin {Path.GetFileName(file)} uploaded successfully!", Severity.Success);
    }
}
```

#### Deleting Plugins

Plugins can be removed from the system using the `DeletePlugin` method. The UI provides feedback on whether the operation requires an application restart:

```csharp
private void DeletePlugin(Plugin plugin)
{
    PluginManager.RemovePlugin(plugin);
    Snackbar.Add($"Plugin {plugin.Name} deleted successfully!", Severity.Success);
    StateHasChanged();
}
```

## UI Components and Styling

The Lionk Plugin Blazor Library uses the MudBlazor component library for UI elements. This integration provides a polished and responsive interface for managing plugins. The components include cards for displaying plugin information, buttons for actions like adding or removing plugins, and dialogs for showing warnings or errors.

### Customizing the UI

The appearance of the plugin management UI can be customized through CSS or by modifying the component parameters. The `PluginCard` component, for instance, uses MudBlazor’s `MudCard`, `MudText`, and `MudButton` components, which can be styled via CSS classes or inline styles.

### Example Plugin Management Page

Here’s a complete example of a plugin management page using Lionk Plugin Blazor:

```razor
@page "/plugin-manager"
@inject IPluginManager PluginManager
@inject FileUploadService FileUploadService
@inject ISnackbar Snackbar

<MudStack Style="width: 100%">
    <MudAlert Severity="Severity.Warning" Elevation="2" Class="mb-4" Visible="@PluginManager.DoNeedARestart()">
        <MudText Typo="Typo.subtitle2" Align="Align.Center">
            A restart is required for changes to take effect. Please restart the application.
        </MudText>
    </MudAlert>

    <MudGrid>
        @foreach (var plugin in PluginManager.GetAllPlugins())
        {
            <MudItem xs="12" sm="6" md="4" lg="3">
                <PluginCard Plugin="@plugin" OnDependencySelect="HandleDependencySelect" OnDelete="DeletePlugin" />
            </MudItem>
        }
    </MudGrid>

    <MudToolBar>
        <MudFileUpload T="IReadOnlyList<IBrowserFile>" Accept=".dll" OnFilesChanged="OnInputFileChanged">
            <ActivatorContent>
                <MudPaper Class="upload-area">
                    <MudText Typo="Typo.subtitle2" Align="Align.Center">Drag files here or click to browse</MudText>
                    <MudButton Color="Color.Primary" Variant="Variant.Filled">Browse</MudButton>
                </MudPaper>
            </ActivatorContent>
        </MudFileUpload>
    </MudToolBar>
</MudStack>

@code {
    private async Task OnInputFileChanged(InputFileChangeEventArgs e)
    {
        var files = e.GetMultipleFiles();
        var result = await FileUploadService.UploadFileAsync(files);
        
        if (result != null)
        {
            foreach (var path in result)
                PluginManager.AddPlugin(path);
            
            Snackbar.Add("Plugins added successfully!", Severity.Success);
        }
        else
        {
            Snackbar.Add("Failed to upload plugins!", Severity.Error);
        }
    }
    
    private void DeletePlugin(Plugin plugin)
    {
        PluginManager.RemovePlugin(plugin);
        Snackbar.Add($"Plugin {plugin.Name} removed. Restart required.", Severity.Warning);
    }
    
    private void HandleDependencySelect(Dependency dependency)
    {
        // Handle dependency interaction
    }
}
```

## Security Considerations

- **File Validation**: Ensure that only trusted plugin files are uploaded and loaded by the application to prevent security risks.
- **Dependency Management**: Handle dependencies carefully to avoid version conflicts and ensure that all required assemblies are properly loaded.

## Contribution

Contributions to the Lionk Plugin Blazor Library are welcome! Please fork the repository, create a branch, and submit a pull request with your enhancements. For detailed contribution guidelines, please refer to the project’s repository.
