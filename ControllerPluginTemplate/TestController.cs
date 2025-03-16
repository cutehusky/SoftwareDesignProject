using Microsoft.AspNetCore.Mvc;

namespace ControllerPluginTemplate;

[ApiController]
public class TestController: ControllerBase
{
    [HttpGet("/getTest")]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello, World!" });
    }
}