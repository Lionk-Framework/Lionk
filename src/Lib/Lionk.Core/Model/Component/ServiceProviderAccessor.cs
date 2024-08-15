// Copyright © 2024 Lionk Project

namespace Lionk.Core.Service;

/// <summary>
/// This class is responsible for managing the service provider.
/// </summary>
public static class ServiceProviderAccessor
{
    /// <summary>
    /// Gets or sets the service provider.
    /// </summary>
    public static IServiceProvider? ServiceProvider { get; set; }
}
