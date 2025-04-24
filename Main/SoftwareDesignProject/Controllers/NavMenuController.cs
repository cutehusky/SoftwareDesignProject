using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NavMenuController : Controller
{
    private readonly IPluginService _pluginService;
    private readonly ILogger<NavMenuController> _logger;

    public NavMenuController(
        IPluginService pluginService, ILogger<NavMenuController> logger)
    {
        _pluginService = pluginService;
        _logger = logger;
    }

    [ServiceFilter(typeof(GetUserInfoActionFilter))]
    [HttpGet("home")]
    public async Task<IActionResult> GetHomeList()
    {
        var plugins = await _pluginService.GetActiveList();

        var pluginItems = plugins.Select(dto => new HomeItem()
        {
            PluginId = dto.PluginId,
            Text = dto.Name!,
            Description = dto.Description!,
            Href = $"dynamicDLL/{dto.PluginId}",
            Icon = string.IsNullOrEmpty(dto.Icon) ? Icons.Material.Filled.List: dto.Icon,
            IsPremium = dto.IsPremium ?? false
        });

        IEnumerable<Guid> favoriteItems;
        if (HttpContext.Items["UserId"] is Guid userId)
            favoriteItems = await _pluginService.GetStarredPluginUserById(userId);
        else 
            favoriteItems = [];

        return Ok(new HomeData()
        {
            PluginItems = pluginItems,
            FavoriteItems = [..favoriteItems]
        });
    }


    [ServiceFilter(typeof(GetUserInfoActionFilter))]
    [HttpGet("nav")]
    public async Task<IActionResult> GetNavList()
    {
        var userRole = HttpContext.Items["UserRole"] as UserRoles?;
        var plugins = await _pluginService.GetActiveList();
        var isAdmin = userRole == UserRoles.Admin;

        List<NavItem> navItems = [];
        
        if (isAdmin)
        {
            navItems.AddRange(new[]
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
        
        navItems.AddRange(plugins.Select(plugin => new NavItem()
        {
            PluginId = plugin.PluginId,
            Text = plugin.Name!,
            Category = plugin.Category!,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = string.IsNullOrEmpty(plugin.Icon) ? Icons.Material.Filled.List: plugin.Icon,
            IsPremium = plugin.IsPremium ?? false
        }));

        IEnumerable<Guid> favoriteItems;
        if (HttpContext.Items["UserId"] is Guid userId)
            favoriteItems = await _pluginService.GetStarredPluginUserById(userId);
        else 
            favoriteItems = [];

        return Ok(new NavData()
        {
            NavItems = navItems,
            FavoriteItems = [..favoriteItems]
        });
    }

    [ServiceFilter(typeof(GetUserInfoActionFilter))]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string queryValue)
    {
        var plugins = await _pluginService.SearchPlugin(queryValue);

        List<NavItem> navItems = [];
        navItems.AddRange(plugins.Select(plugin => new NavItem()
        {
            PluginId = plugin.PluginId,
            Text = plugin.Name!,
            Category = plugin.Category!,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = string.IsNullOrEmpty(plugin.Icon) ? Icons.Material.Filled.List: plugin.Icon,
            IsPremium = plugin.IsPremium ?? false
        }));

        return Ok(navItems);
    }
}