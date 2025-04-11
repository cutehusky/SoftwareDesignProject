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
    private readonly IUserService _userService;

    public NavMenuController(
        IPluginService pluginService,
        IUserService userService)
    {
        _pluginService = pluginService;
        _userService = userService;
    }

    [HttpGet("getHomeList")]
    public async Task<IActionResult> GetHomeList()
    {
        var userRole = await GetCurrentUserRoleAsync();
        var plugins = await _pluginService.GetActiveList(userRole);

        var pluginItems = plugins.Select(dto => new HomeItem()
        {
            PluginId = dto.PluginId,
            Text = dto.Name!,
            Description = dto.Description!,
            Href = $"dynamicDLL/{dto.PluginId}",
            Icon = Icons.Material.Filled.List,
            IsPremium = dto.IsPremium ?? false
        });
        
        var userId = GetCurrentUserId();
        IEnumerable<Guid> favoriteItems;
        if (userId != null)
            favoriteItems = await _pluginService.GetStarredPluginUserById(userId.Value);
        else 
            favoriteItems = [];

        return Ok(new HomeData()
        {
            PluginItems = pluginItems,
            FavoriteItems = [..favoriteItems]
        });
    }


    [HttpGet("getList")]
    public async Task<IActionResult> GetList()
    {
        var userRole = await GetCurrentUserRoleAsync();
        var plugins = await _pluginService.GetActiveList(userRole);
        var isAdmin = userRole == UserRoles.Admin;

        List<NavItem> navItems = new()
        {
            new () {
                Text = "Home",
                Href = "home",
                Icon = Icons.Material.Filled.Home,
                Category = ""
            }
        };
        
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
            Icon = Icons.Material.Filled.List,
            IsPremium = plugin.IsPremium ?? false
        }));
        
        var userId = GetCurrentUserId();
        IEnumerable<Guid> favoriteItems;
        if (userId != null)
            favoriteItems = await _pluginService.GetStarredPluginUserById(userId.Value);
        else 
            favoriteItems = [];

        return Ok(new NavData()
        {
            NavItems = navItems,
            FavoriteItems = [..favoriteItems]
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string queryValue)
    {
        var userRole = await GetCurrentUserRoleAsync();
        var plugins = await _pluginService.SearchPlugin(queryValue, userRole);

        List<NavItem> navItems = [];
        navItems.AddRange(plugins.Select(plugin => new NavItem()
        {
            PluginId = plugin.PluginId,
            Text = plugin.Name!,
            Category = plugin.Category!,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = Icons.Material.Filled.List,
            IsPremium = plugin.IsPremium ?? false
        }));

        return Ok(navItems);
    }
    
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        return Guid.TryParse(userIdClaim?.Value ?? string.Empty, out var res) ? res : null;
    }
    
    private async Task<UserRoles?> GetCurrentUserRoleAsync()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return null;
        }

        var user = await _userService.GetById(userId.Value);
        return user?.UserRole;
    }
}