using Microsoft.AspNetCore.Mvc;
using MudBlazor;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/plugins")]
public class PluginsController : ControllerBase
{
    private readonly IHostEnvironment _env;
    
    public PluginsController(IHostEnvironment env)
    {
        _env = env;
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
        };
        list.AddRange(dlls.Select(dll => new NavItem()
        {
            Text = dll, Href = $"dynamicDLL/{dll}", Icon = Icons.Material.Filled.List
        }));
        return Ok(list);
    }
    
    private class NavItem
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
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