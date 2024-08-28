// Copyright © 2024 Lionk Project

using Lionk.Core.Component.Cyclic;
using Microsoft.Extensions.Hosting;

namespace Lionk.Core.Razor.Service;

/// <summary>
///     This class is used to nest a CyclicExecutorService into a hosted service.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="CyclicExecutorHostedService" /> class.
/// </remarks>
/// <param name="cyclicExecutorService">The cyclicExecutorService.</param>
public class CyclicExecutorHostedService(ICyclicExecutorService cyclicExecutorService) : IHostedService
{
    #region public and override methods

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        cyclicExecutorService.Start();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        cyclicExecutorService.Stop();
        return Task.CompletedTask;
    }

    #endregion
}
