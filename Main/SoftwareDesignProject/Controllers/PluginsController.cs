using CommonDTO;
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
    private readonly ILogger<PluginsController> _logger;

    private static readonly SemaphoreSlim PluginSemaphore = new(1, 1);

    public PluginsController(
        ILogger<PluginsController> logger,
        DynamicPluginManager manager,
        IPluginService pluginService)
    {
        _manager = manager;
        _pluginService = pluginService;
        _logger = logger;
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPut("reload")]
    public async Task<IActionResult> Load()
    {
        if (!await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogError("Semaphore is not available");
            return StatusCode(503, "Service unavailable");
        } 
        
        try
        {
            _logger.LogInformation("Reloading server plugins");
            await _manager.LoadAllAssemblies();
            _logger.LogInformation("Server plugins reloaded successfully");
            return Ok(new { message = "Server Plugin Reloaded" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to reload server plugins: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }
    
    [HttpGet("favorite/{pluginId}")]
    public async Task<IActionResult> IsStarred(Guid pluginId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();
        try
        {
            var starredPlugins = await _pluginService.GetStarredPluginUserById(userId.Value);
            return Ok(starredPlugins.Contains(pluginId));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check starred plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [ServiceFilter(typeof(GetUserInfoActionFilter))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlugin(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest($"Invalid Plugin ID: {id}");
        }

        var plugin = await _pluginService.GetPluginById(id);
        if (plugin is not { IsEnabled: true })
        {
            return NotFound();
        }

        var userRole = HttpContext.Items["UserRole"] as UserRoles?;
        if (plugin.IsPremium == true && userRole is null or < UserRoles.Premium)
        {
            return Forbid();
        }

        return Ok(plugin);
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPut("file")]
    public async Task<IActionResult> UpgradePlugin([FromForm] UpgradePluginRequest request)
    {
        if (request.ClientDLL == null && request.ServerDLL == null)
        {
            return BadRequest("Client DLL or Server DLL is required.");
        }

        if (!await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogError("Semaphore is not available"); 
            return StatusCode(503, "Service unavailable");
        }

        try
        {
            _logger.LogInformation($"Upgrading plugin with id: {request.PluginId}");
            await _pluginService.UpgradePlugin(request.PluginId, request.ClientDLL, request.ServerDLL);
            _logger.LogInformation($"Plugin {request.PluginId} upgraded successfully");
            return Ok();
        }
        catch (InvalidDataException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (IOException e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to upgrade plugins: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPut("metadata")]
    public async Task<IActionResult> EditPlugin([FromBody] PluginDTO dto)
    {
        if (!await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogError("Semaphore is not available");
            return StatusCode(503, "Service unavailable");
        }

        try
        {
            _logger.LogInformation($"Editing plugin with id: {dto.PluginId}");
            await _pluginService.EditPlugin(dto);
            _logger.LogInformation($"Plugin {dto.PluginId} edited successfully");
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to edit plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemovePlugin(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Plugin ID.");
        }
        
        if (!await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogError("Semaphore is not available");
            return StatusCode(503, "Service unavailable");
        }
        
        try
        {
            _logger.LogInformation($"Removing plugin with id: {id}");
            await _pluginService.RemovePlugin(id);
            _logger.LogInformation($"Plugin {id} removed successfully");
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to remove plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }


    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpGet()]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "",
        [FromQuery] string order = "",
        [FromQuery] string search = "")
    {
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

        _logger.LogTrace("Getting plugin list with page: " + page + " and pageSize: " + pageSize +
                          " and sortBy: " + sortBy + " and order: " + sortDirection);
        var plugins = await _pluginService.GetList(page, pageSize, sortBy, sortDirection, search);
        return Ok(plugins);
    }

    [ServiceFilter(typeof(AdminRequestAuthFilter))]
    [HttpPost()]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
        if (!await PluginSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogInformation("Semaphore is not available");
            return StatusCode(503, "Service unavailable");
        }

        try
        {
            _logger.LogInformation($"Installing plugin with name: {request.Name}");
            await _pluginService.AddPlugin(request.Name, request.Description,
                request.Category, request.IsPremium, request.Icon, request.ClientDLL, request.ServerDLL);
            _logger.LogInformation($"Plugin {request.Name} installed successfully");
            return Ok();
        }
        catch (InvalidDataException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (IOException e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to install plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        finally
        {
            PluginSemaphore.Release();
        }
    }

    //[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    [HttpGet("file/{fileName}")]
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
    
    [HttpPost("favorite")]
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
        catch (InvalidOperationException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to star plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return Ok();
    }
    
    [HttpDelete("favorite/{pluginId}")]
    public async Task<IActionResult> UnstarPlugin(Guid pluginId)
    {
        if (pluginId == Guid.Empty)
        {
            return BadRequest("Invalid Plugin ID.");
        }
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        try
        {
            await _pluginService.UnstarPlugin(pluginId, userId.Value);
        } 
        catch (InvalidOperationException e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to unstar plugin: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return Ok();
    }
    
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        return Guid.TryParse(userIdClaim?.Value ?? string.Empty, out var res) ? res : null;
    }
}