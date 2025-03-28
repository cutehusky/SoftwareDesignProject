using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SoftwareDesignProject.Models.DTO;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/plugins")]
public class PluginsController : ControllerBase
{
    private readonly IHostEnvironment _env;
    private readonly DynamicPluginManager _manager;
    private readonly PluginService _pluginService;
    
    public PluginsController(IHostEnvironment env, DynamicPluginManager manager, PluginService pluginService)
    {
        _env = env;
        _manager = manager;
        _pluginService = pluginService;
    }
    
    
    [HttpGet("Load")]
    public IActionResult Load()
    {
        _manager.LoadAllAssemblies();
        return Ok(new { message = "Hello, World!" });
    }

    [HttpGet("GetList")]
    public async Task<IActionResult> GetList()
    {
        // TODO: Load plugin list in db
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
                Href = "/", 
                Icon = Icons.Material.Filled.Dashboard
            },
        };
        list.AddRange(plugins.Select(plugin => new NavItem()
        {
            Text = plugin.Name,
            Category = plugin.Category,
            Href = $"dynamicDLL/{plugin.PluginId}",
            Icon = Icons.Material.Filled.List
        }));
        return Ok(list);
    }
    
    
    [HttpGet("GetListAdmin")]
    public async Task<IActionResult> GetListAdmin()
    {
        var plugins = await _pluginService.GetList();
        return Ok(plugins);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
        Console.WriteLine(request.Name);
        Console.WriteLine(request.Description);
        Console.WriteLine(request.IsPremium);

        var clientUid = await _pluginService.SaveClientDLL(request.ClientDLL);
        if (clientUid == null)
        {
            Console.WriteLine("Fail to load Client DLL");
            return BadRequest("Fail to load Client DLL");
        }

        Guid? serverUid = null;
        if (request.ServerDLL != null)
        {
            serverUid = await _pluginService.SaveServerDLL(request.ServerDLL);
            if (serverUid == null)
            {
                await _pluginService.Rollback();
                Console.WriteLine("Fail to load Server DLL");
                return BadRequest("Fail to load Server DLL");
            }

            if (serverUid != clientUid)
            {
                await _pluginService.Rollback();
                return BadRequest("Server and client dll must have same id");
            }
        }
        
        var res = await _pluginService.Add(new PluginDTO()
        {
            PluginId = (Guid)clientUid,
            Name = request.Name,
            Description = request.Description,
            IsPremium = request.IsPremium,
            Category = request.Category
        });

        if (!res)
        {
            Console.WriteLine("Fail to install Plugin");
            return BadRequest("Fail to install Plugin");
        }

        if (serverUid != null)
        {
            _manager.LoadAllAssemblies();
        }
        
        return Ok(new ActionResponse<bool>() {Result = true});
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