using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;
using SOURCE_QR;
using System.Runtime.InteropServices;


namespace SOURCE_QR;

[ApiController]
[Route("[controller]")]
public class SourceQrController : ControllerBase
{
    private IDynamicServiceProvider _sp;

    public SourceQrController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public IActionResult GenerateQr([FromBody] Request request)
    {
        Console.WriteLine(request.value);
        var qrService = _sp.GetService<Service>()!;
        string ipAddresses = qrService.GenerateQrCode(request.value);
        return Ok(new Response() { value = ipAddresses });
    }
}