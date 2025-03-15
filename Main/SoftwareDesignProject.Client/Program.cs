using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SoftwareDesignProject.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });
builder.Services.AddScoped(sp =>
    new DynamicDLLLoader(
        new HttpClient() { BaseAddress = new Uri("http://localhost:5037/") }));

builder.Services.AddMudServices();

await builder.Build().RunAsync();
