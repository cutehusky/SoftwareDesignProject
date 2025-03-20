using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace HASH_MD5;

[ApiController]
[Route("[controller]")]
public class HashController: ControllerBase
{
    private IDynamicServiceProvider _sp;
    
    public HashController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }
    
    [HttpPost]
    public IActionResult Hash([FromBody] HashRequest request)
    {
        // Console.WriteLine(request.value);
        var hashService = _sp.GetService<HashService>()!;

        return Ok(new HashResponse()
        {
            hashMD5 = hashService.ConvertToMD5(request.value),
            hashSHA1 = hashService.ConvertToSha1(request.value),
            hashSHA256 = hashService.ConvertToSha2_256(request.value),
            hashSHA384 = hashService.ConvertToSha2_384(request.value),
            hashSHA512 = hashService.ConvertToSha2_512(request.value),
            hashSHA3_256 = hashService.ConvertToSha3_256(request.value),
            hashSHA3_384 = hashService.ConvertToSha3_384(request.value),
            hashSHA3_512 = hashService.ConvertToSha3_512(request.value),
        }); 
    }
}