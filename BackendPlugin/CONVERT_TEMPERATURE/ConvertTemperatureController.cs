using BackendPluginTemplate;
using CONVERT_TEMPERATURE;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CONVERT_TEMPERATURE;

[ApiController]
[Route("[controller]")]
public class ConvertTemperatureController : ControllerBase
{
    private IDynamicServiceProvider _sp;

    public ConvertTemperatureController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public IActionResult ConvertTemp([FromBody] Request request)
    {
        Console.WriteLine(request.value);
        Response newValue = _sp.GetService<Service>().ConvertTemperature(Convert.ToDouble(request.value), request.type);
        Console.WriteLine(newValue);
        return Ok(new Response()
        {
            valueK = newValue.valueK,
            valueC = newValue.valueC,
            valueF = newValue.valueF,
            valueR = newValue.valueR,
            valueD = newValue.valueD,
            valueN = newValue.valueN
        });
    }
}