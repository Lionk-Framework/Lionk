// Copyright © 2024 Lionk Project

using Lionk.Core.Component;
using Lionk.Core.TypeRegistery;
using Lionk.Log;
using Lionk.Log.Serilog;
using Lionk.Plugin;
using LionkApp.Components;
using MudBlazor.Services;
using ILoggerFactory = Lionk.Log.ILoggerFactory;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

// Configure custom logger
builder.Services.AddSingleton<ILoggerFactory, SerilogFactory>();

// Register PluginManager as both IPluginManager and ITypesProvider
builder.Services.AddSingleton<IPluginManager, PluginManager>();
builder.Services.AddSingleton<ITypesProvider>(provider => provider.GetRequiredService<IPluginManager>());

// Register ComponentService with a factory to resolve ITypesProvider
builder.Services.AddSingleton(serviceProvider =>
{
    ITypesProvider typesProvider = serviceProvider.GetRequiredService<ITypesProvider>();
    return new ComponentService(typesProvider);
});

WebApplication app = builder.Build();

// Configure the LogService with the singleton logger
ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogService.Configure(loggerFactory);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
