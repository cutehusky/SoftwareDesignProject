using BackendPluginTemplate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using SoftwareDesignProject.Components;
using SoftwareDesignProject.Services;
using System.Text;
using SoftwareDesignProject.Repositories;
using SoftwareDesignProject.Services.ServerPluginManagement;
using CommonDTO;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPluginRepository, PluginRepository>();
builder.Services.AddScoped<IPluginService, PluginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSingleton<DynamicPluginManager>();
builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DynamicPluginManager>());
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

builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5037/") });

builder.Services.AddSingleton<DynamicRouteTransformer>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// Register authorization policies
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(UserRoles.Admin.ToString()));

    options.AddPolicy("PremiumPlus", policy =>
        policy.RequireRole(UserRoles.Premium.ToString(), UserRoles.Admin.ToString()));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAntiforgery();

// Add authentication and authorization middleware here 
app.UseAuthentication();
app.UseAuthorization();

// Maps assets and endpoints
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SoftwareDesignProject.Client._Imports).Assembly);

app.MapDynamicControllerRoute<DynamicRouteTransformer>("api/plugin/{id}/{controller}/{action}");

// In Program.cs or a seed data class
using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await authService.RegisterAsync("admin", "admin", UserRoles.Admin);
}

app.MapFallbackToFile("index.html");

app.Run();
