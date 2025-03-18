using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace HASH_SHA;

[ApiController]
[Route("[controller]")]
public class HashShaController: ControllerBase
{
    private IDynamicServiceProvider _sp;
    
    public HashShaController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }
    
    [HttpPost]
    public IActionResult Hash([FromBody] HashShaRequest request)
    {
        // Console.WriteLine(request.value);
        string? hashed;
        switch (request.type)
        {
            case "sha1":
                hashed = _sp.GetService<HashShaService>().ConvertToSha1(request.value);
                return Ok(new HashShaResponse() { value = hashed }); 
            case "sha256":
                hashed = _sp.GetService<HashShaService>().ConvertToSha256(request.value);
                return Ok(new HashShaResponse() { value = hashed }); 
            case "sha512":
                hashed = _sp.GetService<HashShaService>().ConvertToSha512(request.value);
                return Ok(new HashShaResponse() { value = hashed }); 
            default:
                return BadRequest(new { Message = "Unknown SHA type" });
        }
    }
}