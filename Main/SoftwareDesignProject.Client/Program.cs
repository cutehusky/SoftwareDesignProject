using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SoftwareDesignProject.Client;
using SoftwareDesignProject.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });
builder.Services.AddScoped<IDynamicPageLoader>(sp =>
    new CSRDynamicPageLoader(sp.GetService<HttpClient>()!));
builder.Services.AddScoped<INavMenuLoader>(sp =>
    new CSRNavMenuLoader(sp.GetService<HttpClient>()!, "api/plugins/GetList"));
builder.Services.AddScoped<IPluginListLoader>(sp =>
    new PluginListLoader(sp.GetService<HttpClient>()!, "api/plugins/GetListAdmin"));
builder.Services.AddScoped<IPublishPlugin>(sp =>
    new PublishPlugin(sp.GetService<HttpClient>()!, "api/plugins/Add"));

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();


await builder.Build().RunAsync();
