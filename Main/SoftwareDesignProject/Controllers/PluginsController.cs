using CommonDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Models.DTO;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PluginsController : ControllerBase
{
    private readonly IHostEnvironment _env;
    private readonly DynamicPluginManager _manager;
    private readonly PluginService _pluginService;
    
    public PluginsController(
        IHostEnvironment env, 
        DynamicPluginManager manager, 
        PluginService pluginService)
    {
        _env = env;
        _manager = manager;
        _pluginService = pluginService;
    }
    
    
    [HttpGet("load")]
    public IActionResult Load()
    {
        _manager.LoadAllAssemblies();
        return Ok(new { message = "Server Plugin Reloaded" });
    }

    [HttpPost("edit")]
    public async Task<IActionResult> EditPlugin([FromBody] PluginDTO dto)
    {
        try
        {
            await _pluginService.EditPlugin(dto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemovePlugin([FromBody] PluginDTO request)
    {
        try
        {
            await _pluginService.RemovePlugin(request.PluginId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        return Ok();
    }
    
    
    [HttpGet("getList")]
    public async Task<IActionResult> GetList()
    {
        var plugins = await _pluginService.GetList();
        return Ok(plugins);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
        Console.WriteLine("Installing plugin with name: " + request.Name);
        try
        {
            await _pluginService.AddPlugin(request.Name, request.Description,
                request.Category, request.IsPremium, request.ClientDLL, request.ServerDLL);
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
        return Ok();
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
}