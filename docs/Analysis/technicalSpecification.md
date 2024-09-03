# Technical Specification

## Li/onk Core

Li/onk Core is a modular platform developed in .NET designed to enable the management and integration of various components through plugins. The project is crafted to cater to end users (operators and administrators) as well as plugin developers, offering maximum flexibility and extensibility. The primary goal is to enhance the quality of life in home environments by allowing seamless integration and management of various components.

# FrontEnd

## Framework and Language

The frontend will be built using [Blazor Server](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) with .NET and C#. Blazor Server enables the development of interactive web applications using C#, which offers several advantages:

- **Unified Development**: Allows the use of C# for both client-side and server-side logic, leveraging the team's expertise.
- **Real-Time Updates**: Blazor Server facilitates real-time web functionality, making it ideal for interactive user interfaces.
- **Strong Typing and Compilation**: Ensures better error checking during development, leading to more reliable applications.

Challenges include ensuring optimal performance for high-frequency updates and the necessity for developers to adapt to Blazor-specific paradigms if they are more familiar with other web technologies.

# BackEnd

## Programming Language

The backend will utilize C#, a powerful and versatile language that integrates seamlessly with the .NET ecosystem, providing a consistent development experience across the entire application.

## Framework

The backend will be constructed using [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet), employing the following modules:

- **EF Core**: For database management, providing a robust ORM (Object-Relational Mapping) solution.
- **Newtonsot.Json**: For JSON serialization and deserialization, enabling efficient data storage for persistent data without using any db.
- **Influx DB** For measure logging, providing a robust time serie db optimized for time-stamped or time series data.
- **SignalR**: For asynchronous, real-time communication, enabling live updates and notifications.

# Plugin Architecture

Li/onk Core features a plugin architecture to extend functionality without requiring recompilation of the core application. Key components include:

- **NuGet Packages**: The core framework and tools for plugin development will be distributed via NuGet packages. This approach simplifies version control and dependency management.
- **Modular Design**: Encourages the development, testing, and maintenance of independent components, allowing for flexible and scalable application growth.

### Advantages of Plugin Architecture

- **Flexibility**: Easily add or remove features without modifying the core application.
- **Scalability**: Supports the independent development of components, which can be integrated as needed.
- **Community and Extensibility**: Allows external developers to contribute plugins, fostering a rich ecosystem of shared solutions.

# Deployment

The application will be distributed as a Docker image, providing a consistent and reproducible environment for deployment. This choice is driven by several factors:

- **Simplified Deployment**: Docker images ensure that the application runs consistently across various environments.
- **Isolation and Security**: Containers provide a secure environment, isolating the application from potential conflicts.
- **Scalability**: Docker facilitates the easy scaling of the application by running multiple container instances as required.

## On-Premise Deployment

Li/onk Core is designed for on-premise deployment, meaning it will run directly on the client's infrastructure. This approach offers:

- **Data Privacy**: Ensures that sensitive data remains within the client's control.
- **Customization**: Clients can tailor the application to meet their specific needs and operational environments.
- **Performance**: Local deployment reduces latency, improving the overall performance and responsiveness of the application.

# Resources and Learning

Below are some resources to help with learning Blazor, ASP.NET Core, and Docker, as well as resources for plugin development:

- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
- [NuGet Package Creation](https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package)
- [Docker Documentation](https://docs.docker.com/)

With these choices, Li/onk Core aims to provide a flexible, extensible, and user-friendly platform for managing home environment components, ensuring ease of use for both end users and developers.