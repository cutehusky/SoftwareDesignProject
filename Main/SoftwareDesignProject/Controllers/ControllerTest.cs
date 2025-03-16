using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;


[ApiController]
[Route("api/load")]
public class ControllerTest: ControllerBase
{
    private DynamicControllerLoader _loader;
    public ControllerTest(DynamicControllerLoader loader)
    {
        _loader = loader;
    }
    
    [HttpGet]
    public IActionResult test()
    {
        _loader.LoadAllAssemblies();
        return Ok(new { message = "Hello, World!" });
    }
}