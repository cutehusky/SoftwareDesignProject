using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace ENCRYPT_AES;

[ApiController]
[Route("[controller]")]
public class EncryptController: ControllerBase
{
    private IDynamicServiceProvider _sp;
    
    public EncryptController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public IActionResult Encrypt([FromBody] EncryptRequest request)
    {
        var encryptService = _sp.GetService<EncryptService>()!;
        if (request.Type == "AES")
        {
            var res = encryptService.Encrypt_AES(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }

        if (request.Type == "TripleDES")
        {
            var res = encryptService.Encrypt_TripleDES(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }
        return BadRequest(new { Message = "Unknown encryption type" });
    }
    
    [HttpPost("[action]")]
    public IActionResult Decrypt([FromBody] EncryptRequest request)
    {
        var encryptService = _sp.GetService<EncryptService>()!;
        if (request.Type == "AES")
        {
            var res = encryptService.Decrypt_AES(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }
        if (request.Type == "TripleDES")
        {
            var res = encryptService.Decrypt_TripleDES(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }
        return BadRequest(new { Message = "Unknown encryption type" });
    }
}