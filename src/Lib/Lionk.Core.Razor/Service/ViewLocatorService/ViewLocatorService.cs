// Copyright © 2024 Lionk Project

using System.Reflection;
using Lionk.Core.TypeRegistery;

namespace Lionk.Core.View;

/// <summary>
/// This class is used to locate the view of a component.
/// </summary>
public class ViewLocatorService : IViewLocatorService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewLocatorService"/> class.
    /// </summary>
    /// <param name="provider">The views provider.</param>
    public ViewLocatorService(ITypesProvider provider)
    {
        _provider = provider;
        provider.NewTypesAvailable += OnNewTypesAvailable;
        CreateViewFromTypes(provider.GetTypes());
    }

    /// <inheritdoc/>
    public IEnumerable<Type> GetViewOf<T>(ViewContext context)
        => _views.Where(x => x.ComponentType == typeof(T) && x.ViewContext == context).Select(x => x.ViewType);

    /// <inheritdoc/>
    public void Dispose()
    {
        _provider.NewTypesAvailable -= OnNewTypesAvailable;
        GC.SuppressFinalize(this);
    }

    private void OnNewTypesAvailable(object? sender, TypesEventArgs e)
    => CreateViewFromTypes(e.Types);

    private void CreateViewFromTypes(IEnumerable<Type> types)
    {
        foreach (Type type in types)
        {
            ViewOfAttribute? viewOf = type.GetCustomAttribute<ViewOfAttribute>();
            if (viewOf is not null)
            {
                _views.Add(viewOf.Description);
            }
        }
    }

    private readonly List<ComponentViewDescription> _views = [];

    private readonly ITypesProvider _provider;
}
