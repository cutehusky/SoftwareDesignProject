using Microsoft.AspNetCore.Mvc;

namespace TestController;

[ApiController]
[Route("[controller]")] // !!! MUST [controller] here, other template NOT WORK
public class TestController: ControllerBase
{
    [HttpGet("[action]")] // !!! MUST [action] here, other template NOT WORK 
    public IActionResult Get()
    {
        return Ok(new { message = "Hello, World 123!" });
    }

    [HttpGet("[action]")]
    public IActionResult GetAll()
    {
        return Ok(new { message = "Hello, World 456!" });
    }
    
    [HttpGet("[action]")]
    public IActionResult GetByID()
    {
        return Ok(new { message = $"Hello, World 789!" });
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