Here's a detailed and comprehensive README for the `Lionk.Core` library, focusing on the component part, which is the foundation of the application:

---

# Lionk.Core

## Overview

The Lionk.Core library serves as the foundational framework for building modular, reusable, and observable components in the Lionk application. It provides a set of base classes and interfaces for creating components that can be executed, measured, and managed with well-defined life cycles and behaviors. The core functionality includes cyclic execution, error handling, and observable properties, making it suitable for developing complex applications with dynamic and interactive components.

## Features

- **Modular Component Architecture**: Build modular components with well-defined interfaces and base classes.
- **Lifecycle Management**: Support for initialization, execution, and termination phases of component life cycles.
- **Cyclic Components**: Create components that execute periodically with configurable intervals.
- **Error Handling**: Integrated error handling to manage component states and execution flows.
- **Observable Properties**: Easily track and respond to changes in component properties.

## Installation

To use the Lionk.Core library in your application, add the following NuGet package:

```bash
dotnet add package Lionk.Core
```

## Getting Started

### 1. Defining Components

Components in Lionk.Core are defined using interfaces and base classes that provide a structured approach to building modular and reusable elements. Below are the key interfaces and classes used in the library:

#### IComponent Interface

The `IComponent` interface defines the basic structure of a component, including a unique identifier and a name:

```csharp
public interface IComponent : IDisposable
{
    Guid Id { get; }
    string InstanceName { get; set; }
}
```

#### BaseComponent Class

`BaseComponent` is an abstract class implementing `IComponent` and adding observable property support:

```csharp
public abstract class BaseComponent : ObservableElement, IComponent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string InstanceName { get; set; }
    public virtual void Dispose() => GC.SuppressFinalize(this);
}
```

### 2. Creating Executable Components

For components that require execution logic, Lionk.Core provides the `IExecutableComponent` interface and the `BaseExecutableComponent` class, which handle execution life cycles, error states, and initialization:

#### IExecutableComponent Interface

Defines the execution capabilities for a component:

```csharp
public interface IExecutableComponent : IComponent
{
    bool CanExecute { get; }
    bool IsInError { get; }
    bool IsRunning { get; }
    void Execute();
    void Abort();
    void Reset();
}
```

#### BaseExecutableComponent Class

Provides a base implementation of `IExecutableComponent` with methods to manage execution flow and error states:

```csharp
public abstract class BaseExecutableComponent : BaseComponent, IExecutableComponent
{
    public abstract bool CanExecute { get; }
    public bool IsInError { get; private set; }
    public bool IsRunning { get; private set; }

    public void Execute()
    {
        // Handles initialization, execution, and termination
    }

    public void Abort()
    {
        // Handles abort logic and sets the component in error state
    }

    public void Reset()
    {
        // Resets the component to allow re-execution
    }

    protected virtual void OnExecute(CancellationToken cancellationToken) { }
    protected virtual void OnInitialize() { }
    protected virtual void OnTerminate() { }
}
```

### 3. Working with Cyclic Components

Cyclic components are those that execute periodically based on a defined time interval. The `ICyclicComponent` interface and `BaseCyclicComponent` class provide the necessary functionality:

#### ICyclicComponent Interface

Defines the structure for cyclic components with properties for cycle management:

```csharp
public interface ICyclicComponent : IExecutableComponent
{
    CyclicComputationMethod CyclicComputationMethod { get; set; }
    DateTime LastExecution { get; }
    int NbCycle { get; }
    TimeSpan Period { get; set; }
    DateTime StartedDate { get; }
}
```

#### BaseCyclicComponent Class

Provides the base implementation for cyclic components, managing execution cycles and timing:

```csharp
public abstract class BaseCyclicComponent : BaseExecutableComponent, ICyclicComponent
{
    public CyclicComputationMethod CyclicComputationMethod { get; set; }
    public DateTime LastExecution { get; private set; }
    public int NbCycle { get; private set; }
    public TimeSpan Period { get; set; }
    public DateTime StartedDate { get; private set; }

    protected override void OnExecute(CancellationToken cancellationToken)
    {
        // Increment cycle count and execute component logic
    }
}
```

### 4. Observable Components

Lionk.Core includes observable elements that notify subscribers of property changes, allowing for dynamic updates and state management across components.

#### ObservableElement Class

`ObservableElement` is a base class that provides property change notification capabilities:

```csharp
public abstract class ObservableElement : INotifyPropertyChanged
{
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        // Updates the field and notifies subscribers
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
```

### 5. Managing Components with ComponentContainer

`ComponentContainer` is a utility class that manages component instances by tracking their unique IDs and handling component availability events.

```csharp
public class ComponentContainer : ObservableElement
{
    private readonly IComponentService _componentService;
    public IComponent? Component { get; private set; }
    public Guid ComponentId { get; set; }

    public event EventHandler? NewComponentAvailable;

    private void OnNewComponentAvailable()
    {
        // Handles logic when a new component becomes available
    }
}
```

## Extending Lionk.Core

The library is designed to be extensible, allowing developers to create new component types by implementing or extending the provided interfaces and base classes. For example, to create a custom executable component, derive from `BaseExecutableComponent` and override the necessary methods:

```csharp
public class CustomExecutableComponent : BaseExecutableComponent
{
    public override bool CanExecute => true;

    protected override void OnExecute(CancellationToken cancellationToken)
    {
        // Custom execution logic
    }
}
```

## Contribution

Contributions to Lionk.Core are welcome! Please fork the repository, make your changes in a new branch, and submit a pull request.