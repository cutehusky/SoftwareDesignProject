using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace HASH_PASSWORD;

[ApiController]
[Route("[controller]")]
public class HashController: ControllerBase
{
    private IDynamicServiceProvider _sp;
    
    public HashController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }
    
    [HttpPost("[action]")]
    public IActionResult Hash([FromBody] HashRequest request)
    {
        var hashService = _sp.GetService<HashService>()!;
        // Console.WriteLine(request.salt);
        // Console.WriteLine(request.value);
        return Ok(new HashResponse()
        {
            hashed = hashService.HashPassword(request.value, request.salt),
        }); 
    }

    [HttpPost("[action]")]
    public IActionResult Verify([FromBody] VerifyRequest request)
    {
        var hashService = _sp.GetService<HashService>()!;
        return Ok(new VerifyResponse()
        {
            ok = hashService.VerifyPassword(request.value, request.hashed),
        }); 
    }
}