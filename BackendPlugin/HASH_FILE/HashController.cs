using BackendPluginTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HASH_FILE;


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
    public async Task<IActionResult> Hash(IFormFile? file, [FromForm] string type)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var fileStream = file.OpenReadStream();
        Console.WriteLine(fileStream.Length);
        
        var hashService = _sp.GetService<HashService>()!;

        Func<byte[], string> converter;
        switch (type)
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
            hashMD5 = await hashService.ConvertToMD5(fileStream, converter),
            hashSHA1 = await hashService.ConvertToSha1(fileStream, converter),
            hashSHA256 = await hashService.ConvertToSha2_256(fileStream, converter),
            hashSHA384 = await hashService.ConvertToSha2_384(fileStream, converter),
            hashSHA512 = await hashService.ConvertToSha2_512(fileStream, converter),
            hashSHA3_256 = await hashService.ConvertToSha3_256(fileStream, converter),
            hashSHA3_384 = await hashService.ConvertToSha3_384(fileStream, converter),
            hashSHA3_512 = await hashService.ConvertToSha3_512(fileStream, converter),
        });
    }
}