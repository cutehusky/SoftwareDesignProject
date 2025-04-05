using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SoftwareDesignProject.Client;
using SoftwareDesignProject.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });
builder.Services.AddScoped<IDynamicPageLoader>(sp =>
    new DynamicPageLoader(sp.GetService<HttpClient>()!));
builder.Services.AddScoped<INavMenuLoader>(sp =>
    new NavMenuLoader(sp.GetService<HttpClient>()!, "api/navMenu/getList"));
builder.Services.AddScoped<IPluginService>(sp =>
    new PluginService(sp.GetService<HttpClient>()!, 
        "api/plugins/getList",
        "api/plugins/add", 
        "api/plugins/edit", 
        "api/plugins/remove"));

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();
