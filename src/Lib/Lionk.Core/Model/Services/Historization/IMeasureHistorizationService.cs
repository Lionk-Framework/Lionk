// Copyright © 2024 Lionk Project

using Lionk.Core.Component;

namespace Lionk.Core;

/// <summary>
/// Service responsible for monitoring measurable components and storing their measures.
/// This interface defines operations for handling IMeasurableComponent components.
/// </summary>
public interface IMeasureHistorizationService : IDisposable
{
    /// <summary>
    /// Subscribes to all IMeasurableComponent components to capture their measurement data.
    /// </summary>
    void SubscribeToComponents();

    /// <summary>
    /// Unsubscribes from all IMeasurableComponent components.
    /// </summary>
    void UnsubscribeFromComponents();
}
