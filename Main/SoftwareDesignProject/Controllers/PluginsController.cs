using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;

namespace SoftwareDesignProject.Controller;

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
        return Ok(new List<NavItem>()
        {
            new() { Text = "Home", Href = "home", Icon = Icons.Material.Filled.Home },
            new() { Text = "Counter", Href = "counter", Icon = Icons.Material.Filled.Add },
            new() { Text = "Weather", Href = "weather", Icon = Icons.Material.Filled.List },
            new() { Text = "Dynamic DLL", Href = "dynamicDLL/RazorClassLibrary_test", Icon = Icons.Material.Filled.List }
        });
    }
    
    private class NavItem
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
    }
    
    [HttpGet("{fileName}")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> Get(string fileName)
    {
        var filePath = Path.Combine(_env.ContentRootPath, "Root/plugins", fileName);
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