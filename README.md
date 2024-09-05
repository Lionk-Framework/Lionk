<p align="center">
  <img src="https://github.com/AlexandreIorio/Lionk/assets/91125307/7102af6f-d82c-486c-a186-dc218fc22513" width="20%" alt="LIONK-logo">
</p>

<h1 align="center">LIONK</h1>

<p align="center">
    <em><code>❯ Connecting components made easy!</code></em>
</p>
<p align="center">
	<img src="https://img.shields.io/github/license/Lionk-Framework/Lionk?style=default&logo=opensourceinitiative&logoColor=white&color=1C88BE" alt="license">
	<img src="https://img.shields.io/github/last-commit/Lionk-Framework/Lionk?style=default&logo=git&logoColor=white&color=1C88BE" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/Lionk-Framework/Lionk?style=default&color=1C88BE" alt="repo-top-language">
	<img src="https://img.shields.io/github/commit-activity/m/AlexandreIorio/Lionk?style=default&color=1C88BE" alt="repo-language-count">
</p>

<br>

##### 🔗 Table of Contents

- [📍 Overview](#-overview)
- [👾 Features](#-features)
- [🧩 Example](#-example)
- [📂 Repository Structure](#-repository-structure)
- [🧩 Modules](#-modules)
- [🚀 Getting Started](#-getting-started)
    - [📦 Deployment](#-deployment)
    - [🤖 Usage](#-usage)
- [📌 Project Roadmap](#-project-roadmap)
- [🤝 Contributing](#-contributing)
- [🛠️ Contributing Guidelines](#️-contributing-guidelines)
- [🎗 License](#-license)

---

## 📍 Overview

### 👀 Problem
Managing a diverse set of home and industrial components in an integrated and efficient manner is challenging. Existing solutions often lack flexibility, requiring significant customization and redevelopment when integrating new components. This inflexibility can lead to increased costs and reduced system efficiency.

Additionally, many existing systems are not designed to handle real-time data exchange and monitoring efficiently, leading to delays and potential errors in critical operations. There is also a need for a secure, user-friendly interface that allows administrators and operators to supervise and control the system easily.

### 🚀 Solution
Lionk Core is a modular platform developed in .NET designed to enable the management and integration of various components through plugins. The project is crafted to cater to end users (operators and administrators) as well as plugin developers, offering maximum flexibility and extensibility. The primary goal is to enhance the quality of life in home environments by allowing seamless integration and management of various components.

The application provides a web interface for configuring components and visualizing data. Users can easily add new plugins to define additional components. The application is published as a Docker image for easy deployment, with each release also publishing new NuGet packages to facilitate plugin development.

[Full project description](https://github.com/Lionk-Framework/Lionk/blob/dev/docs/projectDescription.md)

## 👾 Features
- **Dashboard Data Consultation:** Users can add component views to the dashboard to continuously monitor component-specific data through an up-to-date dashboard.
- **Alarm Management:** Users receive notifications for system failures or specific component alerts.
- **Remote Access:** The application is accessible remotely via a web browser.
- **Application Configuration:** Administrators can configure the application, add or remove components, and link components together.
- **Plugin Integration:** Administrators can integrate, activate, or deactivate third-party plugins.
- **User Management:** Administrators can manage users and roles within the application.
- **Plugin Development:** Developers have access to comprehensive documentation and SDKs for creating compatible plugins.
- **Cyclical Execution:** The application supports cyclical execution of components, enabling them to run at specified intervals automatically.

## 🧩 Example
Use Case:  Home heating system management.
- **Energy Storage:** Administrators can manage energy storage from a living room chimney.
- **Temperature Optimization:** The system optimizes heat flow based on storage temperatures using a 3-way valve.
- **Auto-Regulation:** The system auto-regulates based on the chimney's temperature.
- **System Alerts:** Notifications are sent if the system fails or if temperatures exceed certain thresholds.

## 📂 Repository Structure

```sh
└── lionk/
    ├── LICENSE.txt
    ├── README.md
    ├── docs/
    │   ├── analysis/             # Contains all documentation related to analysis.
    │   ├── best-practices/       # Contains best practices used in the development of the app.
    │   ├── deployment/           # Contains guides for deploying the app.
    │   └── project-description.md  # General description of the project.
    └── src/
        ├── lionk.sln             # Visual Studio solution file for the Lionk project.
        ├── app/                  # Contains the Visual Studio project files for the Lionk app.
        ├── lib/                  # Contains subfolders for each module.
        ├── test/                 # Contains all unit tests.
        ├── resources/            # Contains project resources such as logs, etc.
        ├── Directory.Build.Props # MSBuild properties for all projects in the solution.
        ├── .editorconfig         # Configuration for coding style and conventions.
        ├── stylecop.json         # StyleCop settings for enforcing code style rules.
        └── DockerFile            # Dockerfile for building the Lionk app.

```

## 🧩 Modules
This repo hosts all the software bricks making up our framework. Here's a brief description of each of them. The source code can be found in `src/Lib`.

<details closed><summary>Modules Overview</summary>

| File | Summary |
| --- | --- |
| Lionk.Auth | <code>❯ This module handles authentication services and user management within the framework, providing secure access control mechanisms.</code> |
| Lionk.Auth.Razor | <code>❯ Contains Razor components for user interface integration related to authentication and user management, working in conjunction with Lionk.Auth.</code> |
| Lionk.Core | <code>❯ The core module of the framework containing the essential services, utilities, and foundational classes shared across other modules.</code> |
| Lionk.Core.Razor | <code>❯ Provides Razor UI components that are built on top of Lionk.Core functionalities, enhancing the user interface capabilities.</code> |
| Lionk.Logger | <code>❯ This module is responsible for logging and monitoring activities within the framework, supporting various logging levels and outputs.</code> |
| Lionk.Notification | <code>❯ Manages notification services, allowing different types of notifications (email, SMS, etc.) to be sent within the system.</code> |
| Lionk.Plugin | <code>❯ Provides the base infrastructure for plugin management, enabling modular extensions and custom functionality within the framework.</code> |
| Lionk.Plugin.Blazor | <code>❯ Contains Blazor components and support specifically for plugins, enhancing the modular and pluggable capabilities of the framework in Blazor-based applications.</code> |
| Lionk.Utils | <code>❯ A collection of utility classes and helper functions that provide common functionality to streamline development within the framework.</code> |

</details>

## 🚀 Getting Started

### 📦 Deployment
To see how to deploy the application, please refer to the deployment guide: [Deployment guide](https://github.com/Lionk-Framework/Lionk/blob/main/docs/deployement/DockerDeploy.md).

For a full documentation on how to deploy the application on a raspberry with GPIO enable see [Rpi guide](https://github.com/Lionk-Framework/Lionk/blob/main/docs/deployement/RaspberryDeploy.md)

Some basic plugins for raspberry can be found [here](https://github.com/Lionk-Framework/Lionk-rpi-components/releases/tag/RPI_Plugins_1.0.0)

### 🤖 Usage
To see how to use the application, please refer to the user guide: [User guide](https://github.com/Lionk-Framework/Lionk/blob/main/docs/user-guide/userGuide.md).

## 📌 Project Roadmap

- [X] **`Task 1`**: <strike>Deploy version 1.0.0.</strike>
- [ ] **`Task 2`**: Manage issue and fix bugs.
- [ ] **`Task 3`**: Implement Time series data logging and exportation.
- [ ] **`Task 4`**: Implement new notification channels.
- [ ] **`Task 5`**: Implement plugins repository to simplify the plugin download process.
- [ ] **`Task 6`**: TBD

## 🤝 Contributing

We welcome contributions! Here are several ways you can contribute to the project:

- **[Report Issues](https://github.com/Lionk-Framework/Lionk/issues)**: Submit bug reports or feature requests for the `Lionk` project.
- **[Submit Pull Requests](https://github.com/Lionk-Framework/Lionk/pulls)**: Review open PRs and submit your own to improve the codebase.
- **[Develop New Plugins](https://github.com/Lionk-Framework/Lionk/blob/main/docs/developer-guide/PluginDevGuide.md)**: Develop your own plugins and implement the components of your choice! 

Please refer to the [Contributing Guidelines](https://github.com/Lionk-Framework/Lionk/blob/main/docs/developer-guide/ContributionGuide.md) to learn how to contribute effectively.

You can find the complete documentation of the code [here](https://lionk-framework.github.io/).

**Contributor Graph**
<br>
<p align="left">
   <a href="https://github.com/Lionk-Framework/Lionk/graphs/contributors">
      <img src="https://contrib.rocks/image?repo=Lionk-Framework/Lionk">
   </a>
</p>

## 🎗 License

This project is protected under the [MIT](https://github.com/Lionk-Framework/Lionk/blob/main/LICENSE.txt) License. For more details, refer to the [LICENSE](https://choosealicense.com/licenses/) file.
