// Copyright © 2024 Lionk Project

using Lionk.Auth.Abstraction;
using Lionk.Auth.Identity;
using Lionk.Core.Component;
using Lionk.Core.Component.Cyclic;
using Lionk.Core.Razor.Service;
using Lionk.Core.View;
using Lionk.Log;
using Lionk.Log.Serilog;
using Lionk.Plugin;
using Lionk.Plugin.Blazor;
using Lionk.Utils;
using LionkApp.Components;
using LionkApp.Components.Layout;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using ILoggerFactory = Lionk.Log.ILoggerFactory;

#if DEBUG
using Lionk.Auth.Utils;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for non-DEBUG builds
#if !DEBUG
ConfigureKestrel(builder);
#endif

// Add services to the container
ConfigureServices(builder.Services);

WebApplication app = builder.Build();

// Configure the logger
ConfigureLogging(app);

// Configure the HTTP request pipeline
ConfigureRequestPipeline(app);

#if DEBUG
SetupDebugUser(app);
#endif

app.Run();

#if !DEBUG
// Methods for configuring services, Kestrel, logging, and request pipeline
static void ConfigureKestrel(WebApplicationBuilder builder)
{
    string? configPort = builder.Configuration.GetValue<string>("Kestrel:Endpoints:Https:Url")?.Split(':').Last();
    int httpsPort = int.TryParse(configPort, out int port) ? port : 6001;

    builder.WebHost.UseKestrel(options =>
        options.ListenAnyIP(httpsPort, listenOptions => listenOptions.UseHttps()));
}
#endif

static void ConfigureServices(IServiceCollection services)
{
    services.AddRazorComponents().AddInteractiveServerComponents();
    services.AddMudServices();

    // Add Theme
    services.AddScoped<LionkPalette>();

    // Add Basic Authentication services
    services.AddScoped<UserService>();
    services.AddScoped<UserServiceRazor>();
    services.AddSingleton<IUserRepository, UserFileHandler>();
    services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>()));
    services.AddScoped<UserAuthenticationStateProvider>();
    services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserAuthenticationStateProvider>());

    // Configure custom logger
    services.AddSingleton<ILoggerFactory, SerilogFactory>();

    // Register PluginManager as both IPluginManager and ITypesProvider
    services.AddSingleton<IPluginManager, PluginManager>();

    services.AddSingleton(new FileUploadService(ConfigurationUtils.GetFolderPath(FolderType.Plugin)));

    // Register ComponentService with a factory to resolve ITypesProvider
    services.AddSingleton<IComponentService>(sp =>
        new ComponentService(sp.GetRequiredService<IPluginManager>()));
    services.AddSingleton<IViewLocatorService>(sp =>
        new ViewLocatorService(sp.GetRequiredService<IPluginManager>()));
    services.AddSingleton<IViewRegistryService, ViewRegistryService>();

    // Register CyclicExecutorService with a factory to resolve IComponentService
    services.AddSingleton<ICyclicExecutorService>(sp =>
        new CyclicExecutorService(sp.GetRequiredService<IComponentService>()));
    services.AddHostedService<CyclicExecutorHostedService>();

    // Registers NotificationStateService as a singleton to share notification state across the application
    services.AddSingleton<NotificationStateService>();
}

static void ConfigureLogging(WebApplication app)
{
    ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    LogService.Configure(loggerFactory);
}

static void ConfigureRequestPipeline(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", true);
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAntiforgery();
    app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
}

#if DEBUG
static void SetupDebugUser(WebApplication app)
{
    IUserService userService = app.Services.GetRequiredService<IUserService>();

    string adminUsername = "dadmin";
    string userUsername = "duser";
    string password = "password";
    string salt = "salt";
    string passwordHash = PasswordUtils.HashPassword(password, salt);

    User? admin = userService.GetUserByUsername(adminUsername);
    if (admin != null) userService.Delete(admin);

    User? user = userService.GetUserByUsername(userUsername);
    if (user != null) userService.Delete(user);

    admin = new User(
        adminUsername,
        "debugAdmin@email.com",
        passwordHash,
        salt,
        ["Admin"]);

    user = new User(
        userUsername,
        "debugUser@email.com",
        passwordHash,
        salt,
        ["User"]);

    if (userService.Insert(admin) == null)
        throw new Exception("Failed to create admin user");

    if (userService.Insert(user) == null)
        throw new Exception("Failed to create simple user");
}
#endif
