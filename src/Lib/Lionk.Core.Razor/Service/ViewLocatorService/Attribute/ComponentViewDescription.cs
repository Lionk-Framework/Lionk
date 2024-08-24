// Copyright © 2024 Lionk Project

namespace Lionk.Core.View;

/// <summary>
///     This class is used to define the link between a <see cref="Component.IComponent" />
///     and a view depending on a <see cref="View.ViewContext" />.
/// </summary>
public record ComponentViewDescription(string Name, Type ComponentType, Type ViewType, ViewContext ViewContext);
