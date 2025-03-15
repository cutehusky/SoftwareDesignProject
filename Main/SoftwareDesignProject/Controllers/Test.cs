using Microsoft.AspNetCore.Mvc;

namespace SoftwareDesignProject.Controller;

[ApiController]
[Route("api/[controller]")] 
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello, World!" });
    }
}