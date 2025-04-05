using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NavMenuController: Controller
{
    private readonly PluginService _pluginService;
    private readonly IAuthService _authService;
    
    public NavMenuController( 
        PluginService pluginService,
        IAuthService authService)
    {
        _authService = authService;
        _pluginService = pluginService;
    }

    [HttpGet("getList")]
    public async Task<IActionResult> GetList()
    {
        var plugins = await _pluginService.GetList();

        var list = new List<NavItem>()
        {
            new() { 
                Text = "Home", 
                Href = "home", 
                Icon = Icons.Material.Filled.Home,
                Category = ""
            },
            new()
            {
                Text = "Dashboard", 
                Href = "dashboard", 
                Icon = Icons.Material.Filled.Dashboard,
                Category = ""
            },
        };
        list.AddRange(plugins.Select(plugin => new NavItem()
        {
            Text = plugin.Name!,
            Category = plugin.Category!,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = Icons.Material.Filled.List
        }));
        return Ok(list);
    }
}