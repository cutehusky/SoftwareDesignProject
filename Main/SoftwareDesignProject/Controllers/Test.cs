using Microsoft.AspNetCore.Mvc;

namespace SoftwareDesignProject.Controllers;

[ApiController]
[Route("api/2b8bdd5e-d543-4fe1-bb84-719f31a4d068")] 
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello, World!" });
    }
}