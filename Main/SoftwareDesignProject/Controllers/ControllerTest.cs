using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;


[ApiController]
[Route("api/load")]
public class ControllerTest: ControllerBase
{
    private DynamicPluginManager _manager;
    public ControllerTest(DynamicPluginManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet]
    public IActionResult test()
    {
        _manager.LoadAllAssemblies();
        return Ok(new { message = "Hello, World!" });
    }
}