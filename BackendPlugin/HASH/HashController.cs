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
        var hashService = _sp.GetService<HashService>()!;

        Func<byte[], string> converter;
        switch (request.type)
        {
            case "Bin":
                converter = HashService.BytesToBinString;
                break;
            case "Hex":
                converter = HashService.BytesToHexString;
                break;
            case "Base64":
                converter = HashService.BytesToBase64;
                break;
            case "Base64url":
                converter = HashService.BytesToBase64URL;
                    break;
            default:
                return BadRequest(new { Message = "Unknown result type" });
        }

        return Ok(new HashResponse()
        {
            hashMD5 = hashService.ConvertToMD5(request.value, converter),
            hashSHA1 = hashService.ConvertToSha1(request.value, converter),
            hashSHA256 = hashService.ConvertToSha2_256(request.value, converter),
            hashSHA384 = hashService.ConvertToSha2_384(request.value, converter),
            hashSHA512 = hashService.ConvertToSha2_512(request.value, converter),
            hashSHA3_256 = hashService.ConvertToSha3_256(request.value, converter),
            hashSHA3_384 = hashService.ConvertToSha3_384(request.value, converter),
            hashSHA3_512 = hashService.ConvertToSha3_512(request.value, converter),
        }); 
    }
}