// Copyright © 2024 Lionk Project

namespace Lionk.Core;

/// <summary>
/// Utility class for managing measurement hierarchies in the InfluxDB storage system.
/// This provides structure for organizing component measurements in a logical hierarchy.
/// </summary>
public static class MeasurementHierarchy
{
    /// <summary>
    /// Defines default top-level measurement categories.
    /// </summary>
    public static class Categories
    {
        /// <summary>
        /// Category for system-level measurements (CPU, memory, etc.).
        /// </summary>
        public const string System = "system";
        
        /// <summary>
        /// Category for component-level measurements.
        /// </summary>
        public const string Components = "components";
        
        /// <summary>
        /// Category for application-level measurements.
        /// </summary>
        public const string Application = "application";
        
        /// <summary>
        /// Category for user-defined measurements.
        /// </summary>
        public const string Custom = "custom";
    }
    
    /// <summary>
    /// Creates a measurement path combining category, subcategory, and measure name.
    /// </summary>
    /// <param name="category">The top-level category.</param>
    /// <param name="subcategory">The subcategory.</param>
    /// <param name="measureName">The specific measure name.</param>
    /// <returns>A hierarchical path string for the measurement.</returns>
    public static string CreatePath(string category, string subcategory, string measureName)
    {
        return $"{SanitizePath(category)}/{SanitizePath(subcategory)}/{SanitizePath(measureName)}";
    }
    
    /// <summary>
    /// Creates a component measurement path.
    /// </summary>
    /// <param name="componentType">The component type.</param>
    /// <param name="componentName">The component name.</param>
    /// <param name="measureName">The measure name.</param>
    /// <returns>A hierarchical path string for the component measurement.</returns>
    public static string CreateComponentPath(string componentType, string componentName, string measureName)
    {
        return CreatePath(Categories.Components, $"{componentType}/{componentName}", measureName);
    }

    /// <summary>
    /// Sanitizes a path segment by replacing invalid characters.
    /// </summary>
    /// <param name="segment">The path segment to sanitize.</param>
    /// <returns>The sanitized path segment.</returns>
    private static string SanitizePath(string segment)
    {
        if (string.IsNullOrEmpty(segment))
            return "unknown";
        
        return segment
            .Replace(" ", "_")
            .Replace("/", "_")
            .Replace("\\", "_")
            .Replace(".", "_")
            .ToLowerInvariant();
    }
}
