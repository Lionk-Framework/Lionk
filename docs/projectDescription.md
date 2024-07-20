# .NET Plugin Architecture Project Description

## 1. Problem Statement
### 👀 Problem
Managing a diverse set of home and industrial components in an integrated and efficient manner is challenging. Existing solutions often lack flexibility, requiring significant customization and redevelopment when integrating new components. This inflexibility can lead to increased costs and reduced system efficiency.

Additionally, many existing systems are not designed to handle real-time data exchange and monitoring efficiently, leading to delays and potential errors in critical operations. There is also a need for a secure, user-friendly interface that allows administrators and operators to supervise and control the system easily.

### 💭 Proposition
We propose developing Li/onk Core, a modular, plugin-based platform in .NET, to address these challenges. Our solution will provide:

- **Real-Time Data Handling:** Utilizing SignalR for efficient, real-time communication between components and the user interface.
- **Extensibility:** A robust plugin architecture allowing easy addition of new components without needing to recompile the core application.
- **User-Friendly Interface:** A web-based interface using Blazor Server, providing a seamless user experience for both administrators and operators.
- **Cross-Architecture Support:** The application will be compiled to run on both ARM and x86 architectures with a docker image, ensuring wide compatibility and deployment flexibility.
This approach will reduce integration time and costs, improve system efficiency, and provide a scalable and secure solution for managing diverse components in home and industrial environments.

## 2. Objective
The project aims to create a modular architecture in the form of plugins in .NET, allowing the connection of various installation components to form an integrated system.

### 2.1. Project Components

#### 2.1.1. System Base
- **Core Framework**: Provides the basic functionalities and necessary interfaces for loading and managing plugins.
- **Plugin Management**: Mechanism for discovering, loading, and executing plugins.

#### 2.1.2. Plugins
- **Installation Components**: Each plugin represents a specific component or several components of an installation (e.g., sensors, clocks, actuators, alarms, etc.).
- **Standardized Interfaces**: Define common interfaces to ensure interoperability between plugins.

#### 2.1.3. Connectivity and Communication
- **Component Utilization**: Use the communication protocol of the component directly within the plugin (e.g., HomeMatic IP via Python lib, DS18B20 sensors via GPIO, etc.).

#### 2.1.4. Supervision and Control
- **User Interface (UI)**: Provide an interface for system supervision and control. May include graphical visualizations, dashboards, and diagnostic tools. Each plugin will implement its own user interface that will be represented in the main interface.
- **Logs and Monitoring**: Logging and monitoring system to track system performance and potential errors, with a monitoring interface available for plugin implementation.

#### 2.1.5. Security
- **Authentication and Authorization**: Mechanisms to control access to different components and system functionalities.
- **Roles**: Definition of roles for users and plugins.

#### Key Features

- **Modularity**: Add and remove components without interrupting the system operation.
- **Extensibility**: Ease of adding new plugins to integrate new types of components.
- **Interoperability**: Ensure communication and cooperation between different components.
- **Security**: Implement security mechanisms to protect exchanged data and system access.

#### Technologies Used

- **.NET Core/8+**: To ensure performance, reliability, and portability.
- **C#**: Main language for plugin and core framework development.
- **Entity Framework Core**: For data management if necessary.
- **ASP.NET Core**: For creating the web user interface.
- **SignalR**: For real-time communication between components and the user interface.

refers to : [Technical Specification](technicalSpecification.md)

#### Use Cases

1. **Industry 4.0**: Integration of sensors, machines, and management systems for a connected factory.
2. **Home Automation**: Connecting smart home devices to create an automated house.
3. **Energy**: Management and supervision of energy production and distribution installations.

This project will enable users to design flexible and robust systems by combining various components through a .NET plugin architecture, promoting innovation and efficiency in various industrial and private sectors.

---

## 3. Functional and Non-Functional Requirements
this chapter is based on the user stories available here : [User Stories](userStories.md)

### 3.1 Functional Requirements

#### User: Operator

**Data Consultation via Dashboard**
- Users should be able to continuously view component-specific data via a real-time updated dashboard.

**Alarm Management**
- Users should be notified in case of system or component failure.
- Users should be notified when notifications are raised by components, for example, if a room's temperature exceeds a certain threshold.

**Remote Access**
- The application should be accessible remotely via a web browser.

#### User: Administrator
Inherits all operator needs.

**Application Configuration**
- The system should allow the addition and removal of components.

**Plugin Integration**
- Administrators should be able to integrate third-party plugins into the application.
- Administrators should be able to enable or disable plugins.

**Component management**
- Administrators should be able to configure the behavior of components.
- Administrators should be able to view the status of components.
- Administrators should be able to configure alarms and notifications for components.
- Administrators should be able to configure the data collection frequency for components.
- Administrators should be able to add or delete components.

**User Management**
- Administrators should be able to add or remove users.
- Administrators should be able to assign roles to users.

**Data Management**
- The system should allow exporting collected data in JSON format with a user-defined filename.
- The system should allow deleting data collected by a component.

#### User: Developer

**Plugin Development**
- Developers should have access to complete documentation and code examples to create new plugins compatible with the system.
- Developers should be able to access the SDK to develop plugins via NuGet packages.

**Plugin Deployment**
- Developers should be able to submit their plugins to the administrator for integration into the system as `dll`.

### 3.2 Non-Functional Requirements

**Modularity, Extensibility, and Maintenance**
- The system should be designed to facilitate the addition of new components and plugins without requiring major modifications to the existing architecture.
- The system should be designed modularly to facilitate maintenance and updates.

**Performance**
- The dashboard should update continuously with a maximum latency of 2 seconds.
- Alarm notifications should be sent within a maximum of 5 seconds after an event is detected.

**Security**
- The application should ensure secure management of users and roles.
- Data should be protected and secured, especially during the transmission of notifications and remote access.
- Sensitive data should be protected.

**Easy Deployment**
- The application should be published as a Docker image to facilitate deployment and administration.
- New versions of the application and plugins should be easily deployable via Docker and NuGet.

**Compatibility and Portability**
- The application should be compatible with major operating systems and cloud environments via Docker.
- The system should be compatible with common web browsers (Chrome, Firefox, Edge, Safari).
- Plugins should be portable and compatible with different versions of the application.

**Documentation and Developer Support**
- Documentation for plugin development should be clear, concise, and accessible.
- The SDK should be well-documented and regularly updated to facilitate the development of new plugins.

**Reliability**
- The system should be reliable, with minimal downtime, and capable of handling failures without data loss.

**Scalability**
- The system should be able to handle data and notifications from up to 100 components simultaneously without performance degradation.

**Usability**
- The user interface should be intuitive and easy to use, with a minimal learning curve.
