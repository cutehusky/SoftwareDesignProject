using Microsoft.AspNetCore.Mvc;

namespace TestController2;

[ApiController]
[Route("/hello/[controller]")] // !!! MUST [controller] here, other template NOT WORK
public class TestController: ControllerBase
{
    [HttpGet("[action]")] // !!! MUST [action] here, other template NOT WORK 
    public IActionResult Get()
    {
        return Ok(new { message = "Hello, World abc 123!" });
    }

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        return Ok(new { message = "Hello, World abc 456!" });
    }
    
    [HttpGet("[action]")]
    public IActionResult GetByID()
    {
        return Ok(new { message = $"Hello, World abc 789!" });
    }
    
    [HttpGet("[action]")]
    public IActionResult GetData()
    {
        var queries = HttpContext.Request.Query;
    
        var result = new Dictionary<string, string>();
        foreach (var key in queries.Keys)
        {
            result[key] = queries[key];
        }
        return Ok(result);
    }
}