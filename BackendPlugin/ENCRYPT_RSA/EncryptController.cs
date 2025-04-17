using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace ENCRYPT_RSA;

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
        if (request.KeyFormat == "XML")
        {
            var res = encryptService.Encrypt_XML(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }

        if (request.KeyFormat == "PEM")
        {
            var res = encryptService.Encrypt_PEM(request.Data, request.Key);
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
        if (request.KeyFormat == "XML")
        {
            var res = encryptService.Decrypt_XML(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }
        if (request.KeyFormat == "PEM")
        {
            var res = encryptService.Decrypt_PEM(request.Data, request.Key);
            return Ok(new EncryptResponse()
            {
                Result = res
            });
        }
        return BadRequest(new { Message = "Unknown encryption type" });
    }
}