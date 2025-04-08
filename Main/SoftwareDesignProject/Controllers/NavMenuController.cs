using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Services;
using System.Security.Claims;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NavMenuController : Controller
{
    private readonly IPluginService _pluginService;
    private readonly IAuthService _authService;

    public NavMenuController(
        IPluginService pluginService,
        IAuthService authService)
    {
        _authService = authService;
        _pluginService = pluginService;
    }

    [HttpGet("getHomeList")]
    public async Task<IActionResult> GetHomeList()
    {
        var userRole = GetCurrentUserRole();
        var plugins = await _pluginService.GetActiveList(userRole);

        var list = plugins.Select(dto => new HomeItem()
        {
            Text = dto.Name!,
            Description = dto.Description!,
            Href = $"dynamicDLL/{dto.PluginId}",
            Icon = Icons.Material.Filled.List,
            IsPremium = dto.IsPremium ?? false
        });

        return Ok(list);
    }


    [HttpGet("getList")]
    public async Task<IActionResult> GetList()
    {
        var userRole = GetCurrentUserRole();
        var plugins = await _pluginService.GetActiveList(userRole);
        var isAdmin = userRole == UserRoles.Admin;

        var list = new List<NavItem>
        {
            new() {
                Text = "Home",
                Href = "home",
                Icon = Icons.Material.Filled.Home,
                Category = ""
            }
        };

        if (isAdmin)
        {
            list.AddRange(new[]
            {
                new NavItem
                {
                    Text = "Plugin Management",
                    Href = "plugin-management",
                    Icon = Icons.Material.Filled.Extension,
                    Category = "Admin"
                },
                new NavItem
                {
                    Text = "User Management",
                    Href = "user-management",
                    Icon = Icons.Material.Filled.People,
                    Category = "Admin"
                }
            });
        }

        list.AddRange(plugins.Select(plugin => new NavItem()
        {
            Text = plugin.Name!,
            Category = plugin.Category!,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = Icons.Material.Filled.List,
            IsPremium = plugin.IsPremium ?? false // Fix for CS0266 and CS8629
        }));

        return Ok(list);
    }

    private UserRoles GetCurrentUserRole()
    {
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return Enum.Parse<UserRoles>(roleClaim?.Value ?? "Normal");
    }
}