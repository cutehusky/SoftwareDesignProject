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
    private DynamicPluginManager _manager;
    
    public PluginsController(IHostEnvironment env, DynamicPluginManager manager)
    {
        _env = env;
        _manager = manager;
    }
    
    
    [HttpGet("Load")]
    public IActionResult Load()
    {
        _manager.LoadAllAssemblies();
        return Ok(new { message = "Hello, World!" });
    }

    [HttpGet("GetList")]
    public IActionResult GetList()
    {
        // TODO: Load plugin list in db
        var folderPath = Path.Combine(_env.ContentRootPath, "Root/ClientPlugins");
        var dlls = Directory.GetFiles(folderPath)
            .Select(Path.GetFileName)
            .ToList();
        for (int i = 0; i < dlls.Count; i++)
        {
            dlls[i] = dlls[i].Replace(".dll", "");
        }

        var list = new List<NavItem>()
        {
            new() { Text = "Home", Href = "home", Icon = Icons.Material.Filled.Home },
            new() { Text = "Plugin Management", Href = "PluginManagement", Icon = Icons.Material.Filled.Home },
        };
        list.AddRange(dlls.Select(dll => new NavItem()
        {
            Text = dll, Href = $"dynamicDLL/{dll}", Icon = Icons.Material.Filled.List
        }));
        return Ok(list);
    }
    
    
    [HttpGet("GetListAdmin")]
    public IActionResult GetListAdmin()
    {
        // TODO: Load plugin list in db
        var folderPath = Path.Combine(_env.ContentRootPath, "Root/ClientPlugins");
        var dlls = Directory.GetFiles(folderPath)
            .Select(Path.GetFileName)
            .ToList();
        for (int i = 0; i < dlls.Count; i++)
        {
            dlls[i] = dlls[i].Replace(".dll", "");
        }

        var list = new List<PluginDTO>();
        list.AddRange(dlls.Select(dll => new PluginDTO()
        {
            Name = dll, Description = $"Plugin with name: {dll}",
            Enabled = true,
            Premium = true,
            Date = DateTime.Today
        }));
        return Ok(list);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadPluginRequest request)
    {
        if (request.ClientDLL == null)
        {
            return BadRequest("Fail to upload file");
        }

        var clientUploadPath = Path.Combine(_env.ContentRootPath, "Root/ClientPlugins");
        if (!Directory.Exists(clientUploadPath))
        {
            Directory.CreateDirectory(clientUploadPath);
        }
        
        var serverUploadPath = Path.Combine(_env.ContentRootPath, "Root/BackendPlugins");
        if (!Directory.Exists(serverUploadPath))
        {
            Directory.CreateDirectory(serverUploadPath);
        }
        
        Console.WriteLine(request.ClientDLL.Name);
        Console.WriteLine(request.ClientDLL.Length);
        await SaveFileAsync(request.ClientDLL, clientUploadPath);
        
        
        Console.WriteLine(request.Name);
        Console.WriteLine(request.Description);
        Console.WriteLine(request.IsPremium);

        if (request.ServerDLL != null)
        {
            Console.WriteLine(request.ServerDLL.Name);
            Console.WriteLine(request.ServerDLL.Length);
            await SaveFileAsync(request.ServerDLL, serverUploadPath);
            _manager.LoadAllAssemblies();
        }

        return Ok(new ActionResponse<bool>() {Result = true});
    }
    
    private async Task SaveFileAsync(IFormFile file, string uploadPath)
    {
        var filePath = Path.Combine(uploadPath, file.FileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
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