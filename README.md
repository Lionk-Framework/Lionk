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
Use Case: Boiler Room Control Integration
- **Energy Storage:** Administrators can manage energy storage from a living room chimney.
- **Temperature Optimization:** The system optimizes heat flow based on storage temperatures using a 3-way valve.
- **Auto-Regulation:** The system auto-regulates based on the chimney's temperature.
- **System Alerts:** Notifications are sent if the system fails or if temperatures exceed certain thresholds.

## 📂 Repository Structure

```sh
└── Lionk/
    ├── .github
    │   └── workflows
    ├── LICENSE.txt
    ├── README.md
    ├── docs
    │   ├── Best-practices
    │   ├── Mockup
    │   ├── RaspberryDeploy.md
    │   ├── Workflow
    │   ├── architectureDescription.md
    │   ├── processDevelopment.md
    │   ├── projectDescription.md
    │   ├── technicalSpecification.md
    │   ├── unitTests.md
    │   └── userstories.md
    └── src
        ├── .editorconfig
        ├── App
        ├── Directory.Build.Props
        ├── Dockerfile
        ├── ExportToRpi.bat
        ├── Lib
        ├── Lionk.sln
        ├── PublishLinuxArm64.bat
        ├── Test
        ├── docker-compose.yml
        ├── resources
        └── stylecop.json
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
To see how to deploy the application, please refer to the deployment guide: [Deployment guide]().

### 🤖 Usage
To see how to use the application, please refer to the user guide: [User guide]().

## 📌 Project Roadmap

- [X] **`Task 1`**: <strike>Deploy version 1.0.0.</strike>
- [ ] **`Task 2`**: Implement Time series data logging and exportation.
- [ ] **`Task 3`**: Implement new notification channels.
- [ ] **`Task 4`**: To be defined.

## 🤝 Contributing

Contributions are welcome! Here are several ways you can contribute:

- **[Report Issues](https://github.com/Lionk-Framework/Lionk/issues)**: Submit bugs found or log feature requests for the `Lionk` project.
- **[Submit Pull Requests](https://github.com/Lionk-Framework/Lionk/pulls)**: Review open PRs, and submit your own PRs.

## 🛠️ Contributing Guidelines

0. **Open an Issue**: Before making any changes, open a new issue in the repository. Clearly describe the problem or feature you want to address, including any relevant context, use cases, or screenshots. This step helps us track work and discuss possible solutions before development begins.

1. **Fork the Repository**: Start by forking the project repository to your GitHub account. This will create a copy of the repository under your control.

2. **Clone Locally**: Clone the forked repository to your local machine using a Git client.
   ```sh
   git clone https://github.com/your-username/Lionk
   ```

3. **Create a New Branch**: Always work on a new branch. Make sure to name your branch in a way that clearly reflects the issue number and the

 nature of your changes. For example:
   ```sh
   git checkout -b issue-#123-add-new-feature-x
   ```
   Ensure that your branch name includes the issue number (e.g., `issue-#123`) to maintain traceability.

4. **Develop Your Changes**: Make your changes in the new branch. Ensure that your code is clean, follows the project's coding standards, and is well-documented. Make small, incremental changes and commit often.

5. **Write Unit Tests**: Ensure that any new functionality is thoroughly covered by unit tests. Also, make sure to run the existing test suite to confirm that no existing functionality is broken.
   - To run tests, you can use the following command:
     ```sh
     dotnet test
     ```

6. **Commit Your Changes**: Once you are satisfied with your changes and tests, commit your updates with a clear and descriptive commit message. Reference the issue number in your commit message for better traceability.
   ```sh
   git commit -m 'Fixes #123: Implemented new feature x.'
   ```

7. **Push to GitHub**: Push your branch to your forked repository.
   ```sh
   git push origin issue-#123-add-new-feature-x
   ```

8. **Submit a Pull Request (PR)**: Go to the original repository and create a Pull Request (PR) from your forked repository. Make sure to:
   - Reference the issue number in the PR description (e.g., "Fixes #123") to automatically link the PR to the issue.
   - Provide a detailed description of the changes, why they are necessary, and any potential impacts.
   - Ensure your PR adheres to the project's contribution guidelines and passes all continuous integration (CI) checks.

9. **Request a Review**: Tag project maintainers or relevant team members for a review. Be open to feedback and be prepared to make changes if requested.

10. **Ensure Tests Pass**: Make sure all CI checks and tests pass before the PR is merged. This includes running unit tests, linting, and any other checks that are part of the CI process.

11. **Merge and Celebrate**: Once your PR is reviewed, approved, and all tests are passing, it will be merged into the main branch. Congratulations on your contribution!

12. **Close the Issue**: After the PR is merged, ensure that the associated issue is closed automatically by including "Fixes #123" in the PR description, or manually close it if needed.

**Contributor Graph**
<br>
<p align="left">
   <a href="https://github.com/Lionk-Framework/Lionk/graphs/contributors">
      <img src="https://contrib.rocks/image?repo=Lionk-Framework/Lionk">
   </a>
</p>

## 🎗 License

This project is protected under the [MIT](https://github.com/Lionk-Framework/Lionk/blob/main/LICENSE.txt) License. For more details, refer to the [LICENSE](https://choosealicense.com/licenses/) file.
