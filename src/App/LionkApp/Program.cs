// Copyright © 2024 Lionk Project

using Lionk.Auth.Abstraction;
using Lionk.Auth.Identity;
using Lionk.Auth.Utils;
using Lionk.Core.Component;
using Lionk.Core.Component.Cyclic;
using Lionk.Core.Razor.Service;
using Lionk.Core.TypeRegister;
using Lionk.Core.View;
using Lionk.Log;
using Lionk.Log.Serilog;
using Lionk.Plugin;
using Lionk.Plugin.Blazor;
using LionkApp.Components;
using LionkApp.Components.Layout;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using ILoggerFactory = Lionk.Log.ILoggerFactory;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#if !DEBUG
var httpsPort = builder.Configuration.GetValue<int>("Kestrel:Endpoints:Https:Url")?.Split(':').Last() ?? 6001;
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(int.Parse(httpsPort), listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
#endif

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

// Add Theme
builder.Services.AddScoped<LionkPalette>();

// Add Basic Authentication services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserServiceRazor>();
builder.Services.AddSingleton<IUserRepository, UserFileHandler>();
builder.Services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>()));

builder.Services.AddScoped(sp =>
    new UserAuthenticationStateProvider(
        sp.GetRequiredService<UserServiceRazor>(),
        sp.GetRequiredService<IUserService>()));

builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserAuthenticationStateProvider>());

// Configure custom logger
builder.Services.AddSingleton<ILoggerFactory, SerilogFactory>();

// Register PluginManager as both IPluginManager and ITypesProvider
builder.Services.AddSingleton<IPluginManager, PluginManager>();

builder.Services.AddSingleton(
    new FileUploadService(
        Lionk.Utils.ConfigurationUtils.GetFolderPath(
            Lionk.Utils.FolderType.Plugin)));

// Register ComponentService with a factory to resolve ITypesProvider
builder.Services.AddSingleton<IComponentService>(serviceProvider =>
{
    ITypesProvider typesProvider = serviceProvider.GetRequiredService<IPluginManager>();
    return new ComponentService(typesProvider);
});

builder.Services.AddSingleton<IViewLocatorService>(serviceProvider =>
{
    ITypesProvider typesProvider = serviceProvider.GetRequiredService<IPluginManager>();
    return new ViewLocatorService(typesProvider);
});

builder.Services.AddSingleton<IViewRegistry>(serviceProvider => new ViewRegistryService());

// Register CyclicExecutorService with a factory to resolve IComponentService
builder.Services.AddSingleton<ICyclicExecutorService>(serviceProvider =>
{
    IComponentService componentService = serviceProvider.GetRequiredService<IComponentService>();
    return new CyclicExecutorService(componentService);
});

builder.Services.AddHostedService<CyclicExecutorHostedService>();

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

#if DEBUG
// Configure a default user for debug purposes if compiled in debug mode
SetupDebugUser(app);
#endif

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static void SetupDebugUser(WebApplication app)
{
    IUserService userService = app.Services.GetRequiredService<IUserService>();

    // To use debug users, set "dadmin" or "duser" as username and "password" as password.
    string adminUsername = "dadmin";
    List<string> adminRoles = ["Admin"];
    string adminEmail = "debugAdmin@email.com";
    User? admin = userService.GetUserByUsername(adminUsername);
    if (admin is not null) userService.Delete(admin);

    string userUsername = "duser";
    List<string> userRoles = ["User"];
    string userEmail = "debugUser@email.com";
    User? user = userService.GetUserByUsername(userUsername);
    if (user is not null) userService.Delete(user);

    string salt = "salt";
    string password = "password";
    string passwordHash = PasswordUtils.HashPassword(password, salt);

    admin = new(adminUsername, adminEmail, passwordHash, salt, adminRoles);
    user = new(userUsername, userEmail, passwordHash, salt, userRoles);

    admin = userService.Insert(admin);
    user = userService.Insert(user);

    if (admin is null) throw new Exception("Failed to create admin user");
    if (admin is null) throw new Exception("Failed to create simple user");
}
