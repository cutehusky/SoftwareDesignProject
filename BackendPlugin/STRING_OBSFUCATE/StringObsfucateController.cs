using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace STRING_OBSFUCATE;

[ApiController]
[Route("[controller]")]
public class StringObsfucateController : ControllerBase
{
    private IDynamicServiceProvider _sp;

    public StringObsfucateController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public IActionResult Obsfucate([FromBody] Request request)
    {
        Console.WriteLine(request.value);
        string newValue = _sp.GetService<Service>().Obsfucate(request.value, Convert.ToInt16(request.start), Convert.ToInt16(request.end));
        Console.WriteLine(newValue);
        return Ok(new Reponse() { value = newValue });
    }
}