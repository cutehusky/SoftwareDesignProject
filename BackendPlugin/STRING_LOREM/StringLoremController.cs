using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace STRING_LOREM;

[ApiController]
[Route("[controller]")]
public class StringLoremController : ControllerBase
{
    private IDynamicServiceProvider _sp;

    public StringLoremController(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }

    [HttpPost]
    public IActionResult Lorem([FromBody] StringLoremRequest request)
    {
        Console.WriteLine(request.value);
        string lorem = _sp.GetService<StringLoremService>().GenerateLoremIpsum(Convert.ToInt16(request.value));
        Console.WriteLine(lorem);
        return Ok(new StringLoremResponse() { value = lorem });
    }
}