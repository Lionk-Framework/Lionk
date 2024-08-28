# Lionk.Core.Razor

Lionk.Core.Razor is a Blazor-focused extension of the Lionk.Core library, providing essential components and services for dynamically managing and rendering views in a Blazor application. This library integrates MudBlazor components to create interactive, configurable dialogs and dynamic view handling within Blazor.

## Overview

The Lionk.Core.Razor library focuses on:
- **Dynamic Component Rendering**: Allows rendering of components dynamically based on user interactions and context.
- **View Management**: Provides services to locate, register, and manage views of components.
- **Dialog Management**: Uses MudBlazor dialogs for displaying views with flexible and reusable UI patterns.

## Installation

To use Lionk.Core.Razor in your Blazor project, add the package via NuGet:

```bash
dotnet add package Lionk.Core.Razor
```

## Key Components and Services

### 1. Dynamic Dialogs

The library provides Blazor components for displaying views within MudBlazor dialogs, leveraging `MudDialog` to present content in a clean and interactive manner.

#### Example: Creating a Dynamic Dialog with a Carousel

```razor
@using Lionk.Core.Dialog
@using Lionk.Core.View

<MudDialog Style="height:50%">
    <DialogContent>
        <MudCarousel ShowArrows="true"
                     ShowBullets="false"
                     EnableSwipeGesture="true"
                     AutoCycle="false"
                     TData="object"
                     SelectedIndexChanged="ViewChanged"
                     Style="height:100%"
                     SelectedIndex="CurrentIndex">

            @if (ViewDescriptions is not null && ViewDescriptions.Count > 0)
            {
                foreach (ComponentViewDescription viewDescription in ViewDescriptions)
                {
                    <MudCarouselItem Style="padding-left:50px; padding-right:50px; overflow-y: scroll;">
                        <MudGrid Style="width:inherit%">
                            <MudItem>
                                <MudText Typo="Typo.h6">@viewDescription.Name</MudText>
                            </MudItem>
                            <MudItem Style="width:inherit">
                                <DynamicComponentWrapper Type="@viewDescription.ViewType" Parameters="_parameter"/>
                            </MudItem>
                        </MudGrid>
                    </MudCarouselItem>
                }
            }
        </MudCarousel>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Submit">Ok</MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter]
    MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public object? Component { get; set; }

    [Parameter]
    public List<ComponentViewDescription>? ViewDescriptions { get; set; }

    [Parameter]
    public int CurrentIndex { get; set; }

    private readonly Dictionary<string, object> _parameter = new();

    protected override void OnInitialized()
    {
        if (Component is null) return;
        _parameter.Add("Component", Component);
        if (CurrentIndex >= ViewDescriptions?.Count) CurrentIndex = 0;
    }

    private void Submit()
    {
        MudDialog?.Close(DialogResult.Ok(CurrentIndex));
    }

    private void ViewChanged(int index)
    {
        CurrentIndex = index;
    }
}
```

#### Explanation
- **MudDialog**: A MudBlazor dialog is used as the main container for the carousel and actions.
- **MudCarousel**: Allows navigation between different component views dynamically based on the provided list of `ComponentViewDescription`.
- **DynamicComponentWrapper**: Renders components dynamically based on the `ViewType` specified in the `ComponentViewDescription`.

### 2. Simple Confirmation Dialog

Another example of a dialog is a simpler setup for confirmation actions.

```razor
@namespace Lionk.Core.Dialog

<MudDialog>
    <DialogContent>
        <MudText>@ContentText</MudText>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">Cancel</MudButton>
        <MudButton Color="@Color" Variant="Variant.Filled" OnClick="Submit">@ButtonText</MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public string? ContentText { get; set; }

    [Parameter]
    public string? ButtonText { get; set; }

    [Parameter]
    public Color Color { get; set; }

    private void Submit()
    {
        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog?.Cancel();
    }
}
```

### 3. View Locator and Registration Services

The library includes services for locating and managing views dynamically within Blazor applications:

#### **IViewLocatorService**
- Provides methods to locate views associated with specific component types and contexts.
- Supports different view contexts like configuration, detail, page, and widget views.

#### **ViewLocatorService**
- Implements `IViewLocatorService` to dynamically discover and provide views for components.
- Uses a types provider (`ITypesProvider`) to fetch and create views based on defined attributes (`ViewOfAttribute`).

#### Example: Locating Views for a Component

```csharp
using Lionk.Core.View;

public class ExampleComponent { }

public void FindViews()
{
    ITypesProvider typesProvider = new CustomTypesProvider(); // Implement ITypesProvider
    IViewLocatorService viewLocator = new ViewLocatorService(typesProvider);
    
    var views = viewLocator.GetViewOf(typeof(ExampleComponent), ViewContext.Detail);
    foreach (var view in views)
    {
        Console.WriteLine($"View found: {view.Name}, Type: {view.ViewType}");
    }
}
```

### 4. Dynamic Component Rendering

The `DynamicComponentWrapper` component is used to render views dynamically. It integrates with the `IViewRegistryService` to manage the lifecycle of dynamically rendered views.

#### Example: Rendering a Dynamic Component

```razor
@using Lionk.Core.View

<DynamicComponentWrapper Type="@viewType" Parameters="@viewParameters" />

@code {
    [Parameter]
    public Type? viewType { get; set; }

    [Parameter]
    public IDictionary<string, object>? viewParameters { get; set; }
}
```

### 5. View Registry Service

The `ViewRegistryService` helps in managing active views within the application, allowing checking for active instances and registering or unregistering views dynamically.

#### Example: Using View Registry Service

```csharp
using Lionk.Core.View;

var registry = new ViewRegistryService();

// Register a view instance
object viewInstance = new CustomView();
registry.Register(viewInstance);

// Check if a view type has active instances
bool isActive = registry.HasActiveViews(typeof(CustomView));
Console.WriteLine($"Is CustomView active: {isActive}");

// Unregister the view instance
registry.Unregister(viewInstance);
```

## Conclusion

Lionk.Core.Razor provides a robust foundation for dynamic component rendering and view management in Blazor applications. By leveraging MudBlazor for UI components and a flexible service architecture for view management, it allows developers to create interactive and configurable user interfaces easily.


## Contribution

Contributions to Lionk.Core are welcome! Please fork the repository, make your changes in a new branch, and submit a pull request.