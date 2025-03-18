using BackendPluginTemplate;
using Microsoft.AspNetCore.Mvc;

namespace HASH_MD5;

[ApiController]
[Route("[controller]")]
public class HashMd5Controller: ControllerBase
{
    private IDynamicServiceProvider _sp;
    
    public HashMd5Controller(IDynamicServiceProvider sp)
    {
        _sp = sp;
    }
    
    [HttpPost]
    public IActionResult Hash([FromBody] HashMD5Request request)
    {
        // Console.WriteLine(request.value);
        var hashed = _sp.GetService<HashMD5Service>().ConvertToMD5(request.value);
        return Ok(new HashMD5Response() { value = hashed }); 
    }
}