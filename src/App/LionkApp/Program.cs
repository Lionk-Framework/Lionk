// Copyright © 2024 Lionk Project

using Lionk.Auth.Identity;
using Lionk.Auth.Razor.Identity;
using Lionk.Core.Component;
using Lionk.Core.TypeRegistery;
using Lionk.Log;
using Lionk.Log.Serilog;
using Lionk.Plugin;
using LionkApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using ILoggerFactory = Lionk.Log.ILoggerFactory;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

// Add Basic Authentication services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserAuthenticationStateProvider>());

// Configure custom logger
builder.Services.AddSingleton<ILoggerFactory, SerilogFactory>();

// Register PluginManager as both IPluginManager and ITypesProvider
builder.Services.AddSingleton<IPluginManager, PluginManager>();

// Register ComponentService with a factory to resolve ITypesProvider
builder.Services.AddSingleton<IComponentService>(serviceProvider =>
{
    ITypesProvider typesProvider = serviceProvider.GetRequiredService<IPluginManager>();
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
