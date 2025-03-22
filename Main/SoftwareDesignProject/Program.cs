using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using MudBlazor.Services;
using SoftwareDesignProject.Client;
using SoftwareDesignProject.Client.Services;
using SoftwareDesignProject.Components;
using SoftwareDesignProject.Services;
using PluginListLoader = SoftwareDesignProject.Services.PluginListLoader;
using PublishPlugin = SoftwareDesignProject.Services.PublishPlugin;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// Add empty service to prevent exception when force refresh page as SSR  
builder.Services.AddSingleton<IDynamicPageLoader, SSRDynamicPageLoader>();
builder.Services.AddSingleton<INavMenuLoader, SSRNavMenuLoader>();
builder.Services.AddSingleton<IPluginListLoader, PluginListLoader>();
builder.Services.AddSingleton<IPublishPlugin, PublishPlugin>();

builder.Services.AddSingleton<DynamicPluginManager>();
builder.Services.AddSingleton<IDynamicServiceProvider, DynamicServiceProvider>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSession();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IActionDescriptorChangeProvider, ActionDescriptorChangeProvider>();

builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });

builder.Services.AddSingleton<DynamicRouteTransformer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers(); 
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SoftwareDesignProject.Client._Imports).Assembly);

app.MapDynamicControllerRoute<DynamicRouteTransformer>("api/plugin/{id}/{controller}/{action}");

app.Run();
