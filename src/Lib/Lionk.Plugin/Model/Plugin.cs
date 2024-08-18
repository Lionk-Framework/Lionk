// Copyright © 2024 Lionk Project

using System.Reflection;

namespace Lionk.Plugin;

/// <summary>
/// Represents a plugin within the Lionk project, encapsulating its assembly
/// and metadata information such as name, version, description, author, and dependencies.
/// </summary>
public class Plugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class using the provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly that contains the plugin.</param>
    public Plugin(Assembly assembly)
    {
        Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        Name = GetAttribute<AssemblyTitleAttribute>(assembly)?.Title ?? "Unknown";
        Version = assembly.GetName().Version?.ToString() ?? "Unknown";
        Description = GetAttribute<AssemblyDescriptionAttribute>(assembly)?.Description ?? "No description";
        Author = GetAttribute<AssemblyCompanyAttribute>(assembly)?.Company ?? "Unknown";

        AssemblyName[] assemblies = assembly.GetReferencedAssemblies();

        foreach (AssemblyName assemblyName in assemblies)
        {
            Dependencies.Add(new Dependency(false, assemblyName));
        }
    }

    /// <summary>
    /// Gets the assembly associated with this plugin.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the version of the plugin.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets the description of the plugin.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the author of the plugin.
    /// </summary>
    public string Author { get; }

    /// <summary>
    /// Gets the dependencies of the plugin as an array of strings.
    /// </summary>
    public List<Dependency> Dependencies { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the plugin has been correctly loaded.
    /// </summary>
    public bool IsLoaded { get; set; }

    private T? GetAttribute<T>(Assembly assembly)
        where T : Attribute
    {
        object[] attributes = assembly.GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0 ? (T)attributes[0] : null;
    }
}
