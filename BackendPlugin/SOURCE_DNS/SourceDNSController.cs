using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;


namespace SOURCE_DNS;

[ApiController]
[Route("[controller]")]
public class SourceDNSController : ControllerBase
{
    private IDynamicServiceProvider _sp;

    public SourceDNSController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public async Task<IActionResult> LookUp([FromBody] Request request)
    {
        Console.WriteLine(request.value);
        var dnsService = _sp.GetService<Service>()!;
        List<string> ipAddresses = await dnsService.ResolveDns(request.value);
        return Ok(new Response() { value = ipAddresses });
    }
}