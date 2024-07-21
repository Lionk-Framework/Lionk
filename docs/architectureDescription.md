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

<svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 405.60003662109375 487.2000274658203" width="405.60003662109375" height="487.2000274658203">
  <!-- svg-source:excalidraw -->
  
  <defs>
    <style class="style-fonts">
      @font-face {
        font-family: "Virgil";
        src: url("https://excalidraw.com/Virgil.woff2");
      }
      @font-face {
        font-family: "Cascadia";
        src: url("https://excalidraw.com/Cascadia.woff2");
      }
      @font-face {
        font-family: "Assistant";
        src: url("https://excalidraw.com/Assistant-Regular.woff2");
      }
    </style>
    
  </defs>
  <rect x="0" y="0" width="405.60003662109375" height="487.2000274658203" fill="#ffffff"></rect><g stroke-linecap="round" transform="translate(10 10) rotate(0 192.80001831054688 233.60001373291016)"><path d="M32 0 C155.64 0, 279.28 0, 353.6 0 C374.93 0, 385.6 10.67, 385.6 32 C385.6 127.23, 385.6 222.46, 385.6 435.2 C385.6 456.53, 374.93 467.2, 353.6 467.2 C226.85 467.2, 100.1 467.2, 32 467.2 C10.67 467.2, 0 456.53, 0 435.2 C0 354.49, 0 273.78, 0 32 C0 10.67, 10.67 0, 32 0" stroke="none" stroke-width="0" fill="#eebefa"></path><path d="M32 0 C96.58 0, 161.16 0, 353.6 0 M32 0 C154.71 0, 277.42 0, 353.6 0 M353.6 0 C374.93 0, 385.6 10.67, 385.6 32 M353.6 0 C374.93 0, 385.6 10.67, 385.6 32 M385.6 32 C385.6 131.84, 385.6 231.67, 385.6 435.2 M385.6 32 C385.6 160.21, 385.6 288.41, 385.6 435.2 M385.6 435.2 C385.6 456.53, 374.93 467.2, 353.6 467.2 M385.6 435.2 C385.6 456.53, 374.93 467.2, 353.6 467.2 M353.6 467.2 C278.54 467.2, 203.49 467.2, 32 467.2 M353.6 467.2 C257.9 467.2, 162.21 467.2, 32 467.2 M32 467.2 C10.67 467.2, 0 456.53, 0 435.2 M32 467.2 C10.67 467.2, 0 456.53, 0 435.2 M0 435.2 C0 289.68, 0 144.16, 0 32 M0 435.2 C0 342.51, 0 249.82, 0 32 M0 32 C0 10.67, 10.67 0, 32 0 M0 32 C0 10.67, 10.67 0, 32 0" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g stroke-linecap="round" transform="translate(136.39999389648438 53.20001220703125) rotate(0 66.4000244140625 56.400001525878906)"><path d="M28.2 0 C50.82 0, 73.44 0, 104.6 0 C123.4 0, 132.8 9.4, 132.8 28.2 C132.8 43.7, 132.8 59.21, 132.8 84.6 C132.8 103.4, 123.4 112.8, 104.6 112.8 C89.31 112.8, 74.02 112.8, 28.2 112.8 C9.4 112.8, 0 103.4, 0 84.6 C0 68.85, 0 53.1, 0 28.2 C0 9.4, 9.4 0, 28.2 0" stroke="none" stroke-width="0" fill="#a5d8ff"></path><path d="M28.2 0 C53.91 0, 79.62 0, 104.6 0 M28.2 0 C46.89 0, 65.58 0, 104.6 0 M104.6 0 C123.4 0, 132.8 9.4, 132.8 28.2 M104.6 0 C123.4 0, 132.8 9.4, 132.8 28.2 M132.8 28.2 C132.8 39.73, 132.8 51.27, 132.8 84.6 M132.8 28.2 C132.8 40.29, 132.8 52.38, 132.8 84.6 M132.8 84.6 C132.8 103.4, 123.4 112.8, 104.6 112.8 M132.8 84.6 C132.8 103.4, 123.4 112.8, 104.6 112.8 M104.6 112.8 C83.89 112.8, 63.19 112.8, 28.2 112.8 M104.6 112.8 C80.02 112.8, 55.44 112.8, 28.2 112.8 M28.2 112.8 C9.4 112.8, 0 103.4, 0 84.6 M28.2 112.8 C9.4 112.8, 0 103.4, 0 84.6 M0 84.6 C0 64.92, 0 45.24, 0 28.2 M0 84.6 C0 64.47, 0 44.35, 0 28.2 M0 28.2 C0 9.4, 9.4 0, 28.2 0 M0 28.2 C0 9.4, 9.4 0, 28.2 0" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(147.22872924804688 86.60001373291016) rotate(0 55.5712890625 23)"><text x="55.5712890625" y="18.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic"></text><text x="55.5712890625" y="41.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">(NuGet Pkg)</text></g><g transform="translate(155.79998779296875 79.80000305175781) rotate(0 50.0048828125 11.5)"><text x="50.0048828125" y="18.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">Framework</text></g><g stroke-linecap="round" transform="translate(43.5999755859375 219.6000213623047) rotate(0 68.80001831054688 28)"><path d="M14 0 C50.66 0, 87.32 0, 123.6 0 C132.93 0, 137.6 4.67, 137.6 14 C137.6 24.5, 137.6 35, 137.6 42 C137.6 51.33, 132.93 56, 123.6 56 C88.4 56, 53.2 56, 14 56 C4.67 56, 0 51.33, 0 42 C0 31.27, 0 20.54, 0 14 C0 4.67, 4.67 0, 14 0" stroke="none" stroke-width="0" fill="#b2f2bb"></path><path d="M14 0 C38.54 0, 63.08 0, 123.6 0 M14 0 C44.19 0, 74.37 0, 123.6 0 M123.6 0 C132.93 0, 137.6 4.67, 137.6 14 M123.6 0 C132.93 0, 137.6 4.67, 137.6 14 M137.6 14 C137.6 21.32, 137.6 28.63, 137.6 42 M137.6 14 C137.6 20.27, 137.6 26.54, 137.6 42 M137.6 42 C137.6 51.33, 132.93 56, 123.6 56 M137.6 42 C137.6 51.33, 132.93 56, 123.6 56 M123.6 56 C101.64 56, 79.68 56, 14 56 M123.6 56 C80.02 56, 36.43 56, 14 56 M14 56 C4.67 56, 0 51.33, 0 42 M14 56 C4.67 56, 0 51.33, 0 42 M0 42 C0 35.12, 0 28.24, 0 14 M0 42 C0 35.66, 0 29.32, 0 14 M0 14 C0 4.67, 4.67 0, 14 0 M0 14 C0 4.67, 4.67 0, 14 0" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(76.26229858398438 224.6000213623047) rotate(0 36.1376953125 23)"><text x="36.1376953125" y="18.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">Plugin 1</text><text x="36.1376953125" y="41.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">(.dll)</text></g><g stroke-linecap="round" transform="translate(223.59994506835938 218.00001525878906) rotate(0 68.80001831054688 28)"><path d="M14 0 C36.18 0, 58.37 0, 123.6 0 C132.93 0, 137.6 4.67, 137.6 14 C137.6 23.85, 137.6 33.7, 137.6 42 C137.6 51.33, 132.93 56, 123.6 56 C94.13 56, 64.66 56, 14 56 C4.67 56, 0 51.33, 0 42 C0 32.04, 0 22.08, 0 14 C0 4.67, 4.67 0, 14 0" stroke="none" stroke-width="0" fill="#b2f2bb"></path><path d="M14 0 C50.93 0, 87.86 0, 123.6 0 M14 0 C42.14 0, 70.29 0, 123.6 0 M123.6 0 C132.93 0, 137.6 4.67, 137.6 14 M123.6 0 C132.93 0, 137.6 4.67, 137.6 14 M137.6 14 C137.6 23.05, 137.6 32.11, 137.6 42 M137.6 14 C137.6 25.16, 137.6 36.31, 137.6 42 M137.6 42 C137.6 51.33, 132.93 56, 123.6 56 M137.6 42 C137.6 51.33, 132.93 56, 123.6 56 M123.6 56 C88.93 56, 54.25 56, 14 56 M123.6 56 C89.52 56, 55.44 56, 14 56 M14 56 C4.67 56, 0 51.33, 0 42 M14 56 C4.67 56, 0 51.33, 0 42 M0 42 C0 31.05, 0 20.1, 0 14 M0 42 C0 35.04, 0 28.08, 0 14 M0 14 C0 4.67, 4.67 0, 14 0 M0 14 C0 4.67, 4.67 0, 14 0" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(256.26226806640625 223.00001525878906) rotate(0 36.1376953125 23)"><text x="36.1376953125" y="18.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">Plugin 2</text><text x="36.1376953125" y="41.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">(.dll)</text></g><g transform="translate(120.54904174804688 342.8000030517578) rotate(0 82.2509765625 23)"><text x="82.2509765625" y="18.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">Core application</text><text x="82.2509765625" y="41.400390625" font-family="Helvetica, Segoe UI Emoji" font-size="20px" fill="#1e1e1e" text-anchor="middle" style="white-space: pre;" direction="ltr" dominant-baseline="alphabetic">(Docker container)</text></g><g stroke-linecap="round"><g transform="translate(113.20001220703125 218.8000030517578) rotate(0 21.5999755859375 -26.800003051757812)"><path d="M0 0 C7.2 -8.93, 36 -44.67, 43.2 -53.6 M0 0 C7.2 -8.93, 36 -44.67, 43.2 -53.6" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(113.20001220703125 218.8000030517578) rotate(0 21.5999755859375 -26.800003051757812)"><path d="M35.12 -29.94 C37.99 -38.36, 40.87 -46.77, 43.2 -53.6 M35.12 -29.94 C37.82 -37.86, 40.52 -45.77, 43.2 -53.6" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(113.20001220703125 218.8000030517578) rotate(0 21.5999755859375 -26.800003051757812)"><path d="M21.8 -40.67 C29.41 -45.27, 37.03 -49.87, 43.2 -53.6 M21.8 -40.67 C28.96 -45, 36.12 -49.32, 43.2 -53.6" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g></g><mask></mask><g stroke-linecap="round"><g transform="translate(297.20001220703125 217.1999969482422) rotate(0 -22 -27.199996948242188)"><path d="M0 0 C-7.33 -9.07, -36.67 -45.33, -44 -54.4 M0 0 C-7.33 -9.07, -36.67 -45.33, -44 -54.4" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(297.20001220703125 217.1999969482422) rotate(0 -22 -27.199996948242188)"><path d="M-22.58 -41.51 C-30.85 -46.49, -39.12 -51.46, -44 -54.4 M-22.58 -41.51 C-30.34 -46.18, -38.09 -50.85, -44 -54.4" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g><g transform="translate(297.20001220703125 217.1999969482422) rotate(0 -22 -27.199996948242188)"><path d="M-35.87 -30.76 C-39.01 -39.89, -42.15 -49.01, -44 -54.4 M-35.87 -30.76 C-38.82 -39.32, -41.76 -47.88, -44 -54.4" stroke="#1e1e1e" stroke-width="2" fill="none"></path></g></g><mask></mask></svg>

### Key Points

- The **Core Application** runs inside a Docker container, ensuring consistent deployment.
- The **Framework** is provided as NuGet packages, used by both the core application and plugins.
- **Plugins** are developed independently and deployed as .dll files, allowing for easy integration and extension of the core functionality.

This architecture ensures that Li/onk Core is flexible, extensible, and easy to manage, supporting a wide range of components and use cases in home and industrial environments.