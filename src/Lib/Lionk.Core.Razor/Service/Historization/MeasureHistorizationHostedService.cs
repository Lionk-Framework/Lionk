// Copyright © 2024 Lionk Project

using Microsoft.Extensions.Hosting;

namespace Lionk.Core.Razor;

/// <summary>
/// This class is used to nest a MeasureHistorizationService into a hosted service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MeasureHistorizationHostedService" /> class.
/// </remarks>
/// <param name="measureHistorizationService">The measureHistorizationService.</param>
public class MeasureHistorizationHostedService(IMeasureHistorizationService measureHistorizationService) : IHostedService
{
    #region public and override methods

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        measureHistorizationService.SubscribeToComponents();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        measureHistorizationService.UnsubscribeFromComponents();
        return Task.CompletedTask;
    }

    #endregion
}
