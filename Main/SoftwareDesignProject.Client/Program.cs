using CommonDTO;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SoftwareDesignProject.Client;
using SoftwareDesignProject.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });
builder.Services.AddScoped<IDynamicPageLoader>(sp =>
    new DynamicPageLoader(sp.GetService<HttpClient>()!,
        "/api/plugin/{0}",
        "api/plugins/{0}.dll",
        "api/plugins/check?id={0}"));
builder.Services.AddScoped<INavMenuLoader>(sp =>
    new NavMenuLoader(sp.GetService<HttpClient>()!,
        "api/navMenu/getList",
        "api/navMenu/getHomeList"));
builder.Services.AddScoped<IPluginService>(sp =>
    new PluginService(sp.GetService<HttpClient>()!,
        "api/plugins/getList?page={0}&pageSize={1}&sortBy={2}&order={3}&search={4}",
        "api/plugins/add",
        "api/plugins/edit",
        "api/plugins/upgrade",
        "api/plugins/remove",
        "api/plugins/starPlugin",
        "api/plugins/unstarPlugin"));

builder.Services.AddScoped<IUserService>(sp => new UserService(
    sp.GetService<HttpClient>()!,
    getListEndpoint: "/api/users",
    updateRoleEndpoint: "/api/users/role",
    deleteEndpoint: "/api/users",
    addEndpoint: "/api/users",
    upgradeEndpoint: "/api/users/upgrade",
    refreshTokenEndpoint: "/api/auth/refresh-token"
));
builder.Services.AddSingleton<IFormatChecker, FormatChecker>();

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

// For automatically adding the access token to the requests
builder.Services.AddHttpClient("AuthHttpClient", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthHttpClient"));
builder.Services.AddSingleton<IComponentRefreshService, ComponentRefreshService>();

// Register authentication policies
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(UserRoles.Admin.ToString()));

    options.AddPolicy("PremiumPlus", policy =>
        policy.RequireRole(UserRoles.Premium.ToString(), UserRoles.Admin.ToString()));
});

builder.Services.AddMudServices();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();
