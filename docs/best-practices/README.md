# Best Practices

## Code Structure

### General
All code and code documentation must be in English.

### Code Conventions
As the application primarily uses C# and .NET technologies, follow the standard .NET best practices: [.NET Convention](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

In brief, follow these rules:
- **Tools and Analyzers**: Enforce conventions with tools like `.editorconfig` and code analysis in Visual Studio.
- **Modern Language Features**: Utilize the latest C# features; avoid obsolete constructs.
- **Exception Handling**: Catch specific exceptions and use `using` statements for resource management.
- **Type Usage**: Prefer language keywords (e.g., `string` over `System.String`).
- **String Handling**: Use string interpolation for concatenation and `StringBuilder` for large text manipulations.
- **LINQ Queries**: Use meaningful names, implicit typing, and align query clauses for readability.
- **Delegates**: Use `Func<>` and `Action<>` for delegates and lambda expressions for event handlers.
- **Variable Declarations**: Use `var` only when the type is obvious.
- **Style Guidelines**: Use four spaces for indentation, align code consistently, and follow the "Allman" style for braces.
- **Comment Style**: Use single-line comments for brief explanations and XML comments for methods and classes.

### Identifier Names
Identifiers must be clear, concise, and meaningful. Follow these rules: [Naming Rules](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names).

In brief, follow these rules:
- **Starting Characters**: Methods start with a capital letter and a verb (e.g., `GetName`, `Execute`). Properties start with a capital letter. Private fields start with an underscore (e.g., `_name`). Local variables start with a lowercase letter.
- **Valid Characters**: Can include Unicode letters, digits, connecting characters, combining characters, or formatting characters.
- **Interfaces**: Prefix with `I`.
- **Attributes**: Suffix with `Attribute`.
- **Enums**: Singular for non-flags, plural for flags.
- **Clarity**: Prefer meaningful, descriptive names over abbreviations.

Tools to apply these rules:
- **StyleCop**: Analyzes C# source code to enforce style and consistency rules.
- **EditorConfig**: Defines coding styles and formatting rules for Visual Studio.

Refer to: [Apply StyleCop](apply-stylecop.md).

### Code Documentation
All code must be documented using XML comments. Write clear and concise XML comments in English for all public methods and classes.

### OOP and SOLID Principles
Write code following OOP and SOLID principles. Ensure code is modular, readable, extendable, and maintainable. Use interfaces and abstract classes to enhance modularity and testability.

### Unit Tests
All code must be tested using unit tests written with the xUnit framework. Write clear and concise unit tests in English for all public methods and classes. Ensure tests are understandable and maintainable.