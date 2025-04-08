using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Models.DTO;
using SoftwareDesignProject.Models.Entities;
using SoftwareDesignProject.Services;
using SoftwareDesignProject.Services.ServerPluginManagement;
using System.Security.Claims;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PluginsController : ControllerBase
{
    private readonly IHostEnvironment _env;
    private readonly DynamicPluginManager _manager;
    private readonly IPluginService _pluginService;

    private static readonly SemaphoreSlim PluginSemaphore = new(1, 1);

    public PluginsController(
        IHostEnvironment env,
        DynamicPluginManager manager,
        IPluginService pluginService)
    {
        _env = env;
        _manager = manager;
        _pluginService = pluginService;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("load")]
    public async Task<IActionResult> Load()
    {
        await _manager.LoadAllAssemblies();
        return Ok(new { message = "Server Plugin Reloaded" });
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckPlugin([FromQuery] string id)
    {
        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var pluginId))
        {
            return BadRequest("Invalid Plugin ID.");
        }

        var plugin = await _pluginService.GetPluginById(pluginId);
        if (plugin == null || plugin.IsEnabled != true)
        {
            return NotFound();
        }

        var userRole = GetCurrentUserRole();
        if (plugin.IsPremium == true && userRole < CommonDTO.UserRoles.Premium)
        {
            return Forbid();
        }

        return Ok();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("upgrade")]
    public async Task<IActionResult> UpgradePlugin([FromForm] UpgradePluginRequest request)
    {
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
        var userRole = GetCurrentUserRole();
        var plugins = await _pluginService.GetList(page, pageSize, sortBy, sortDirection, search, userRole);
        return Ok(plugins);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("add")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
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
        var filePath = Path.Combine(_env.ContentRootPath, "Root/ClientPlugins", fileName);
        Console.WriteLine("Getting file: " + fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        // add caching here to optimize performance
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return File(stream, "application/octet-stream", fileName);
    }

    private CommonDTO.UserRoles GetCurrentUserRole()
    {
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return Enum.Parse<CommonDTO.UserRoles>(roleClaim?.Value ?? "Normal");
    }
}