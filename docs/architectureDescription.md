# Project Architecture Description

## Overview

Li/onk Core is designed as a modular platform enabling the management and integration of various components through plugins. The architecture leverages .NET, allowing for a flexible, extensible system with components that can be independently developed, deployed, and managed.

## Architecture Components

### 1. Core Application

The core application serves as the foundational platform for Li/onk Core. It is responsible for the overall management, coordination, and integration of the various plugins and components.

- **Published as**: Docker Image
- **Responsibilities**:
  - Loading and managing plugins
  - Providing the main user interface for configuration and monitoring
  - Ensuring communication between different plugins and components
  - Handling security and user management

### 2. Framework

The framework provides the necessary tools and interfaces for plugin development and integration. It ensures that all plugins adhere to a common standard, facilitating interoperability and seamless integration.

- **Published as**: NuGet Packages
- **Responsibilities**:
  - Defining standardized interfaces for plugin development
  - Providing base classes and utilities for common functionalities
  - Ensuring communication protocols between the core application and plugins
  - Offering tools for logging, monitoring, and security management

### 3. Plugins

Plugins are independently developed components that extend the functionality of the core application. Each plugin represents specific functionalities, such as managing sensors, actuators, or other system components.

- **Published as**: .dll (Dynamic Link Libraries)
- **Responsibilities**:
  - Implementing specific component functionalities (e.g., sensor data collection, actuator control)
  - Providing user interface elements for their configuration and control
  - Integrating with the core application through the framework’s interfaces

## Interaction and Integration

### Core Application and Framework

The core application relies on the framework to provide the foundational services and interfaces required for plugin management. When the core application starts, it initializes the framework, which in turn handles the discovery and loading of plugins.

- **Core Application**:
  - Initializes the framework on startup
  - Manage all the frontend views and user interactions
  - Uses the framework to discover available plugins
  - Manages the lifecycle of plugins (loading, executing, unloading)
- **Framework**:
  - Provides interfaces for plugin discovery and management
  - Offers utilities and base classes for plugin development
  - Ensures secure and standardized communication between core and plugins

### Plugins and Framework

Plugins are developed using the framework’s tools and interfaces, ensuring they comply with the system’s standards. When deployed, plugins are discovered and managed by the core application through the framework.

- **Plugins**:
  - Implement functionalities using the framework’s interfaces
  - Provide user interface elements for the core application to render
  - Interact with other plugins and the core application through standardized protocols
- **Framework**:
  - Facilitates the registration and discovery of plugins
  - Ensures secure and efficient communication between plugins and the core application
  - Provides common utilities and services used by plugins (e.g., logging, security)

## Deployment and Distribution

### Docker Image

The core application is distributed as a Docker image, ensuring a consistent and reproducible deployment environment. This image includes the core application and the framework, ready to manage and integrate plugins.

- **Deployment**:
  - Pull the Docker image from the registry
  - Run the Docker container on the target environment (supports both ARM and x86 architectures)
  - The container initializes the core application, which in turn uses the framework to manage plugins

### NuGet Packages

The framework is distributed as a set of NuGet packages, providing developers with the necessary tools and interfaces for plugin development.

- **Distribution**:
  - NuGet packages are published to a NuGet repository
  - Developers add the NuGet packages to their projects to create compatible plugins
  - Ensures versioning and dependency management for plugin developers

### .dll Plugins

Plugins are distributed as .dll files, allowing for easy deployment and integration with the core application.

- **Distribution**:
  - Plugins are developed using the framework’s NuGet packages
  - Compiled plugins are packaged as .dll files
  - Plugins are deployed by placing the .dll files in a designated directory where the core application can discover and load them

## Visual Representation

![image](https://github.com/user-attachments/assets/7cab9c9f-601b-47c3-99a2-b5de82d67b8e)

### Key Points

- The **Core Application** runs inside a Docker container, ensuring consistent deployment.
- The **Framework** is provided as NuGet packages, used by both the core application and plugins.
- **Plugins** are developed independently and deployed as .dll files, allowing for easy integration and extension of the core functionality.

This architecture ensures that Li/onk Core is flexible, extensible, and easy to manage, supporting a wide range of components and use cases in home and industrial environments.
