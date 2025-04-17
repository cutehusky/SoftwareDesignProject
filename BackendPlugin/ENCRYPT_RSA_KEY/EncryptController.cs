using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace ENCRYPT_RSA_KEY;

[ApiController]
[Route("[controller]")]
public class EncryptController: ControllerBase
{
    private IDynamicServiceProvider _sp;
    public EncryptController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost("[action]")]
    public IActionResult GenerateRSAKey([FromBody] EncryptKeyRequest request)
    {
        var rsaService = _sp.GetService<RSAService>()!;
        if (request.KeySize is not 512 and not 1024 and not 2048 and not 3072 and not 4096)
        {
            return BadRequest(new { Message = "Key size must be 512, 1024, 2048, 3072 or 4096" });
        }
        
        if (request.KeyFormat is "XML")
        {
            var key = rsaService.GenerateRSAKeyXML(request.KeySize);
            return Ok(new EncryptKeyResponse()
            {
                PublicKey = key.Item1,
                PrivateKey = key.Item2
            });
        }

        if (request.KeyFormat is "PEM")
        {
            var key = rsaService.GenerateRSAKeyPEM(request.KeySize);
            return Ok(new EncryptKeyResponse()
            {
                PublicKey = key.Item1,
                PrivateKey = key.Item2
            });
        }
        
        return BadRequest(new { Message = "Unknown key format" });
    }
}