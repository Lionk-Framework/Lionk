﻿// Copyright © 2024 Lionk Project

namespace Lionk.Core.Component;

/// <summary>
/// This interface defines a cyclic component.
/// </summary>
public interface ICyclicComponent : IComponent
{
    TimeSpan? Execute();
}
