using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NavMenuController: Controller
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
        // TODO: check if the user and plugin premium
        var plugins = await _pluginService.GetActiveList();
        var list = plugins.Select((dto => new HomeItem()
        {
            Text = dto.Name!,
            Description = dto.Description!,
            Href = $"dynamicDLL/{dto.PluginId}",
            Icon = Icons.Material.Filled.List
        }));
        return Ok(list);
    }

    [HttpGet("getList")]
    public async Task<IActionResult> GetList()
    {
        // TODO: check if the user and plugin premium
        var plugins = await _pluginService.GetActiveList();

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