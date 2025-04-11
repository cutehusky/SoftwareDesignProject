using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Models.DTO;
using SoftwareDesignProject.Services;
using SoftwareDesignProject.Services.ServerPluginManagement;
using System.Security.Claims;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PluginsController : ControllerBase
{
    private readonly DynamicPluginManager _manager;
    private readonly IPluginService _pluginService;
    private readonly IUserService _userService;

    private static readonly SemaphoreSlim PluginSemaphore = new(1, 1);

    public PluginsController(
        DynamicPluginManager manager,
        IPluginService pluginService,
        IUserService userService)
    {
        _manager = manager;
        _pluginService = pluginService;
        _userService = userService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("load")]
    public async Task<IActionResult> Load()
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        await _manager.LoadAllAssemblies();
        return Ok(new { message = "Server Plugin Reloaded" });
    }

    [HttpGet("getPlugin")]
    public async Task<IActionResult> GetPlugin([FromQuery] string id)
    {
        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var pluginId))
        {
            return BadRequest("Invalid Plugin ID.");
        }

        var plugin = await _pluginService.GetPluginById(pluginId);
        if (plugin is not { IsEnabled: true })
        {
            return NotFound();
        }

        var userRole = await GetCurrentUserRoleAsync();
        if (plugin.IsPremium == true && userRole is null or < UserRoles.Premium)
        {
            return Forbid();
        }

        return Ok(plugin);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("upgrade")]
    public async Task<IActionResult> UpgradePlugin([FromForm] UpgradePluginRequest request)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        if (request.ClientDLL == null && request.ServerDLL == null)
        {
            return BadRequest("Client DLL or Server DLL is required.");
        }

        var acquired = await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10));
        if (!acquired)
        {
            Console.WriteLine("Semaphore is not available");
            return BadRequest("Busy. Please try again later.");
        }

        try
        {
            Console.WriteLine("Upgrading plugin with name: " + request.PluginId);
            await _pluginService.UpgradePlugin(request.PluginId, request.ClientDLL, request.ServerDLL);
            return Ok();
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (IOException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("edit")]
    public async Task<IActionResult> EditPlugin([FromBody] PluginDTO dto)
    {
        var userRole = await GetCurrentUserRoleAsync();
        Console.WriteLine(userRole);
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        var acquired = await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10));
        if (!acquired)
        {
            Console.WriteLine("Semaphore is not available");
            return BadRequest("Busy. Please try again later.");
        }

        try
        {
            Console.WriteLine("Editing plugin with name: " + dto.Name);
            await _pluginService.EditPlugin(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("Error: " + ex.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("remove")]
    public async Task<IActionResult> RemovePlugin([FromBody] PluginDTO request)
    {
        var userRole = await GetCurrentUserRoleAsync();
        Console.WriteLine(userRole);
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        var acquired = await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10));
        if (!acquired)
        {
            Console.WriteLine("Semaphore is not available");
            return BadRequest("Busy. Please try again later.");
        }
        try
        {
            Console.WriteLine("Removing plugin with name: " + request.Name);
            await _pluginService.RemovePlugin(request.PluginId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }


    [HttpGet("getList")]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "",
        [FromQuery] string order = "",
        [FromQuery] string search = "")
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        page = Math.Max(0, page);
        pageSize = Math.Max(1, pageSize);

        var sortDirection = SortDirection.None;
        if (order.Equals("Ascending", StringComparison.OrdinalIgnoreCase))
        {
            sortDirection = SortDirection.Ascending;
        }
        else if (order.Equals("Descending", StringComparison.OrdinalIgnoreCase))
        {
            sortDirection = SortDirection.Descending;
        }

        Console.WriteLine("Getting plugin list with page: " + page + " and pageSize: " + pageSize +
                          " and sortBy: " + sortBy + " and order: " + sortDirection);
        var plugins = await _pluginService.GetList(page, pageSize, sortBy, sortDirection, search);
        return Ok(plugins);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("add")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
        var userRole = await GetCurrentUserRoleAsync();
        if (userRole is null or < UserRoles.Admin)
        {
            return Forbid();
        }
        var acquired = await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10));
        if (!acquired)
        {
            Console.WriteLine("Semaphore is not available");
            return BadRequest("Busy. Please try again later.");
        }

        try
        {
            Console.WriteLine("Installing plugin with name: " + request.Name);
            await _pluginService.AddPlugin(request.Name, request.Description,
                request.Category, request.IsPremium, request.ClientDLL, request.ServerDLL);
            return Ok();
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (IOException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    //[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    [HttpGet("{fileName}")]
    public async Task<IActionResult> Get(string fileName)
    {
        try
        {
            var stream = await _pluginService.GetClientPluginFile(fileName);
            return File(stream, "application/octet-stream", fileName);
        } catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
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
    
    [HttpPost("[action]")]
    public async Task<IActionResult> StarPlugin([FromBody] PluginDTO pluginDto)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            await _pluginService.StarPlugin(pluginDto.PluginId, userId.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Failed to star plugin.");
        }

        return Ok();
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> UnstarPlugin([FromBody] PluginDTO pluginDto)
    {
        var userId = GetCurrentUserId();
        Console.WriteLine(userId);
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            await _pluginService.UnstarPlugin(pluginDto.PluginId, userId.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Failed to unstar plugin.");
        }

        return Ok();
    }
    
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        return Guid.TryParse(userIdClaim?.Value ?? string.Empty, out var res) ? res : null;
    }
}