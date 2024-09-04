## Plugin Development Guide for Lionk

[Full API documentation](https://lionk-framework.github.io/)

### Overview

The Lionk framework allows developers to create custom plugins in the form of **Components** and **Views**. Components represent the core functionality, while Views provide a UI representation for these components. This guide explains how to develop these plugins, the lifecycle of components, and how to create corresponding views.

### Table of Contents

1. [Component Development](#component-development)
    - [Component Types](#component-types)
    - [Base Component Classes](#base-component-classes)
    - [Creating a Cyclic Component](#creating-a-cyclic-component)
    - [Component Lifecycle Methods](#component-lifecycle-methods)
    - [Error Handling in Components](#error-handling-in-components)
    - [NamedElement Attribute](#namedelement-attribute)
    - [Development Requirements](#development-requirements)
2. [View Development](#view-development)
    - [View Types](#view-types)
    - [Creating a Component View](#creating-a-component-view)
    - [Creating a Configuration View](#creating-a-configuration-view)
3. [Best Practices](#best-practices)
4. [Conclusion](#conclusion)

### Component Development

Components are the fundamental building blocks of a plugin in the Lionk Project. They encapsulate specific functionalities and behaviors and can be cyclic or non-cyclic. Components can inherit from base classes provided by the framework, such as `BaseComponent`, `BaseExecutableComponent`, and `BaseCyclicComponent`.

### Downloads Nuget Packages
To develop a component, you need to install the following NuGet packages:

- [Lionk.Core](https://www.nuget.org/packages/Lionk.Core): The core library that provides the base classes and interfaces for developing components and views.
- [Lionk.Core.Razor](https://www.nuget.org/packages/Lionk.Core.Razor): A library that contains the base classes for developing views for component.

Optionally, you may need additional packages based on the requirements of your component :
- [Lionk.Utils](https://www.nuget.org/packages/Lionk.Utils): A library that provides utility classes and extension methods for common tasks such as serialization and more.
- [Lionk.Auth](https://www.nuget.org/packages/Lionk.Auth): A library that provides authentication and authorization features for components.
- [Lionk.Auth.Razor](https://www.nuget.org/packages/Lionk.Auth.Razor): A library that provides base classes for developing authentication and authorization views.

    
#### Component Types

1. **Cyclic Components**: These are components that execute a specific task at a defined interval, repeating until stopped or an error occurs.
2. **Executable Components**: These components perform a specific task upon execution and may not necessarily be cyclic. They can be triggered manually or based on certain conditions.

#### Base Component Classes

To create a new component, you typically inherit from one of the provided base classes:

- **`BaseComponent`**: The most basic form of a component, which includes properties like `Id` and `InstanceName`.
- **`BaseExecutableComponent`**: Extends `BaseComponent` and provides additional methods for executing, aborting, and resetting components. It manages the lifecycle events such as `OnInitialize`, `OnExecute`, and `OnTerminate`.
- **`BaseCyclicComponent`**: Extends `BaseExecutableComponent` to provide support for cyclic execution. It adds properties and methods specific to cyclic execution, such as `Period`, `NbCycle`, and `LastExecution`.

#### Creating a Cyclic Component

To create a cyclic component, you need to inherit from `BaseCyclicComponent`. Below is an example component named `Counter`, which increments a counter value every second:

```csharp
using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.TestComponent;

/// <summary>
/// Counter test component.
/// </summary>
[NamedElement("Counter test", "test cyclic element")]
public class Counter : BaseCyclicComponent
{
    private int _counter;

    /// <summary>
    /// Gets or sets counter value.
    /// </summary>
    public int CounterValue
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    /// <inheritdoc />
    public override bool CanExecute => true;

    /// <inheritdoc />
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        CounterValue++;
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        Period = TimeSpan.FromSeconds(1);
        base.OnInitialize();
    }
}
```

In this example:
- `CounterValue` is a property that stores the current value of the counter.
- `OnExecute()` increments the counter value each time it is called.
- `OnInitialize()` sets the execution period to 1 second.

#### Component Lifecycle Methods

Components have several lifecycle methods that can be overridden to provide custom behavior:

- **`OnInitialize()`**: Called once before the component is executed. Used to set up initial states or configurations.
- **`OnExecute()`**: Called during each execution cycle. This method contains the core logic of the component.
- **`OnTerminate()`**: Called after the execution logic has completed. Used to clean up or finalize states.

#### Error Handling in Components
If an exception occurs during the execution of a component, the component will automatically enter an error state and the cyclic execution will stop and the component will be aborted.

To handle errors in components, you can use the `Abort()` method to terminate the component's execution and mark it as being in an error state. Here’s an example component that always throws an error during execution:

```csharp
using Lionk.Core;
using Lionk.Core.Component;

namespace Lionk.TestComponent;

/// <summary>
/// Error test component.
/// </summary>
[NamedElement("Error component", "test cyclic element")]
public class ComponentWhichGoToError : BaseCyclicComponent
{
    /// <inheritdoc />
    public override bool CanExecute => true;

    /// <inheritdoc />
    protected override void OnExecute(CancellationToken ct)
    {
        base.OnExecute(ct);
        throw new Exception();
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        Period = TimeSpan.FromSeconds(10);
        base.OnInitialize();
    }
}
```

This component will immediately throw an exception during execution, causing the component to enter an error state.

#### NamedElement Attribute

`NamedElement` is an attribute used to define metadata for a component. It provides a name and a description for the component, which can be used by the framework for identification, logging, and user interface purposes.

The attribute is defined as follows:

```csharp
[NamedElement("Component Name", "Component Description")]
```

For example:

```csharp
[NamedElement("Counter test", "test cyclic element")]
```

This example sets the component's name as "Counter test" and provides a description "test cyclic element."

#### Development Requirements

All components developed for the Lionk Project must meet the following requirements:

- **.NET Version**: Components must be developed with **.NET 8** or higher.
- **Deployment**: Components should be compiled into **.dll (Dynamic Link Library)** files. These .dll files are then loaded as plugins into the Lionk Project application. Make sure to properly reference the required dependencies and libraries when compiling your component into a plugin.

### View Development

Views are used to represent components in a user-friendly way. A view can be a simple display widget, or a more complex configuration panel. There are different types of views based on the context in which they are used.

#### View Types

The Lionk Project framework defines several types of views using the `ViewContext` enum:

1. **Configuration View**: This view is used for configuring a component. It allows users to change settings or parameters related to the component.
   
2. **Detail View**: This view provides detailed information about the component. It is typically used to show additional properties or statuses that are not available in the main view.
   
3. **Page View**: This view is designed to provide a full-page representation of a component or multiple components. It can include detailed information, configuration options, and interactive elements.
   
4. **Widget View**: This is a compact view designed to be displayed in a widget format, often showing a summary or key metric related to the component.

```csharp
public enum ViewContext
{
    Configuration,
    Detail,
    Page,
    Widget
}
```

These view types allow developers to create multiple representations of the same component for different purposes.

#### Creating a Component View

To create a view for a component, you need to inherit from the `ViewBase` class and use the `[ViewOf]` attribute to link it to the corresponding component. Here is an example of a view for the `Counter` component:

```razor
@using System.ComponentModel
@using Lionk.Core.View
@using MudBlazor
@namespace Lionk.TestComponent.CyclicCounter

@attribute [ViewOf("Counter view", typeof(Counter), typeof(CounterView), ViewContext.Widget)]
<MudText Typo="Typo.h4">@Component?.CounterValue</MudText>

@code {
    [Parameter]
    public Counter? Component { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Component is not null)
            Component.PropertyChanged += ModelChange;
    }

    private void ModelChange(object? sender, PropertyChangedEventArgs e)
        => InvokeAsync(StateHasChanged);
}
```

This view uses MudBlazor components to display the `CounterValue` property of the `Counter` component. It also subscribes to the `PropertyChanged` event to update the UI when the counter value changes.

#### Creating a Configuration View

A configuration view allows users to interact with and modify the state of a component. Below is an example of a configuration view for the `Counter` component:

```razor
@using Lionk.Core.View
@using MudBlazor
@namespace Lionk.TestComponent.CyclicCounter
@attribute [ViewOf("Counter Configuration", typeof(Counter), typeof(CounterConfig), ViewContext.Configuration)]

<MudButton OnClick="OnResetClick">Reset</MudButton>

@code {
    [Parameter]
    public Counter? Component { get; set; }

    private void OnResetClick()
    {
        if (Component is null) return;
        Component.CounterValue = 0;
    }
}
```

This view provides a button to reset the `CounterValue` of the `Counter` component.

### Best Practices

- **Use Proper Naming Conventions**: Ensure your component and view names are descriptive and follow the naming conventions of the framework.
- **Handle Errors Gracefully**: Always use proper error handling in your components to prevent crashes and ensure stability

.
- **Keep Views Simple**: Ensure that views are easy to understand and use. Avoid clutter and focus on the key functionalities.
- **Utilize Base Class Features**: The base classes provide a lot of built-in functionality. Make sure to utilize these features to avoid redundant code.

### Conclusion

This guide provides a comprehensive overview of creating components and views for the Lionk Project. By following the steps outlined in this guide, adhering to the best practices, and understanding the requirements for development, you can develop robust and user-friendly plugins that extend the functionality of the Lionk Project framework. Happy coding!