using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

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