<p align="center">
  <img src="https://github.com/AlexandreIorio/Lionk/assets/91125307/7102af6f-d82c-486c-a186-dc218fc22513" width="20%" alt="LIONK-logo">
</p>
<p align="center">
    <h1 align="center">LIONK</h1>
</p>  
<p align="center">
    <em><code>❯ Connecting components made easy!</code></em>
</p>
<p align="center">
	<img src="https://img.shields.io/github/license/Lionk-Framework/Lionk?style=default&logo=opensourceinitiative&logoColor=white&color=1C88BE" alt="license">
	<img src="https://img.shields.io/github/last-commit/Lionk-Framework/Lionk?style=default&logo=git&logoColor=white&color=1C88BE" alt="last-commit">
	<img src="https://img.shields.io/github/languages/top/Lionk-Framework/Lionk?style=default&color=1C88BE" alt="repo-top-language">
	<img src="https://img.shields.io/github/commit-activity/m/AlexandreIorio/Lionk?style=default&color=1C88BE" alt="repo-language-count">
</p>
<p align="center">
	<!-- default option, no dependency badges. -->
</p>

<br>

##### 🔗 Table of Contents

- [📍 Overview](#-overview)
- [👾 Features](#-features)
- [🧩 Example](#-example)
- [📂 Repository Structure](#-repository-structure)
- [🧩 Modules](#-modules)
- [🚀 Getting Started](#-getting-started)
    - [🔖 Prerequisites](#-prerequisites)
    - [📦 Installation](#-installation)
    - [🤖 Usage](#-usage)
    - [🧪 Tests](#-tests)
- [📌 Project Roadmap](#-project-roadmap)
- [🤝 Contributing](#-contributing)
- [🎗 License](#-license)
- [🙌 Acknowledgments](#-acknowledgments))

---

## 📍 Overview

Li/onk Core is a modular platform developed in .NET designed to enable the management and integration of various components through plugins.
The project is crafted to cater to end users (operators and administrators) as well as plugin developers,
offering maximum flexibility and extensibility. The primary goal is to enhance the quality of life in home environments
by allowing seamless integration and management of various components.

The application provides a web interface for configuring components and visualizing data. Users can easily add new plugins to define additional components.
The application is published as a Docker image for easy deployment, with each release also publishing new NuGet packages to facilitate plugin development.

## 👾 Features
- **Dashboard Data Consultation:** Users can add components views to the dashboard to continuously monitor component-specific data through an up-to-date dashboard.
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
---

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
---
## 🧩 Modules
this repo hosts all the software bricks making up our framework. Here's a brief description of each of them. The source code can be found in src/Lib.

<details closed><summary>.</summary>

| File | Summary |
| --- | --- |
| [LICENSE.txt](https://github.com/Lionk-Framework/Lionk/blob/main/LICENSE.txt) | <code>❯ REPLACE-ME</code> |

</details>

---

## 🚀 Getting Started

### 🔖 Prerequisites

**CSharp**: `version x.y.z`

### 📦 Installation

Build the project from source:

1. Clone the Lionk repository:
```sh
❯ git clone https://github.com/Lionk-Framework/Lionk
```

2. Navigate to the project directory:
```sh
❯ cd Lionk
```

3. Install the required dependencies:
```sh
❯ dotnet build
```

### 🤖 Usage

To run the project, execute the following command:

```sh
❯ dotnet run
```

### 🧪 Tests

Execute the test suite using the following command:

```sh
❯ dotnet test
```

---

## 📌 Project Roadmap

- [X] **`Task 1`**: <strike>Implement feature one.</strike>
- [ ] **`Task 2`**: Implement feature two.
- [ ] **`Task 3`**: Implement feature three.

---

## 🤝 Contributing

Contributions are welcome! Here are several ways you can contribute:

- **[Report Issues](https://github.com/Lionk-Framework/Lionk/issues)**: Submit bugs found or log feature requests for the `Lionk` project.
- **[Submit Pull Requests](https://github.com/Lionk-Framework/Lionk/blob/main/CONTRIBUTING.md)**: Review open PRs, and submit your own PRs.
- **[Join the Discussions](https://github.com/Lionk-Framework/Lionk/discussions)**: Share your insights, provide feedback, or ask questions.

<details closed>
<summary>Contributing Guidelines</summary>

1. **Fork the Repository**: Start by forking the project repository to your github account.
2. **Clone Locally**: Clone the forked repository to your local machine using a git client.
   ```sh
   git clone https://github.com/Lionk-Framework/Lionk
   ```
3. **Create a New Branch**: Always work on a new branch, giving it a descriptive name.
   ```sh
   git checkout -b new-feature-x
   ```
4. **Make Your Changes**: Develop and test your changes locally.
5. **Commit Your Changes**: Commit with a clear message describing your updates.
   ```sh
   git commit -m 'Implemented new feature x.'
   ```
6. **Push to github**: Push the changes to your forked repository.
   ```sh
   git push origin new-feature-x
   ```
7. **Submit a Pull Request**: Create a PR against the original project repository. Clearly describe the changes and their motivations.
8. **Review**: Once your PR is reviewed and approved, it will be merged into the main branch. Congratulations on your contribution!
</details>

<details closed>
<summary>Contributor Graph</summary>
<br>
<p align="left">
   <a href="https://github.com{/Lionk-Framework/Lionk/}graphs/contributors">
      <img src="https://contrib.rocks/image?repo=Lionk-Framework/Lionk">
   </a>
</p>
</details>

---

## 🎗 License

This project is protected under the [SELECT-A-LICENSE](https://choosealicense.com/licenses) License. For more details, refer to the [LICENSE](https://choosealicense.com/licenses/) file.

---

## 🙌 Acknowledgments

- List any resources, contributors, inspiration, etc. here.

---


