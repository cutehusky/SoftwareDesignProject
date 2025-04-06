using Microsoft.AspNetCore.Components.Authorization;
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
        "api/plugins/remove"));

builder.Services.AddScoped<IUserService>(sp => new UserService(
    sp.GetRequiredService<HttpClient>(),
    getListEndpoint: "/api/users",
    updateRoleEndpoint: "/api/users/role",
    deleteEndpoint: "/api/users",
    addEndpoint: "/api/users"
));

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();
