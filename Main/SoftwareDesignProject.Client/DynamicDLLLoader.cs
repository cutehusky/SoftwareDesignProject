using System.Reflection;

namespace SoftwareDesignProject.Client;

public class DynamicDLLLoader
{

    private HttpClient _httpClient;
    
    public DynamicDLLLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DynamicPage> LoadDynamicAssembly(string dllUrl)
    {
        var dllBytes = await _httpClient.GetByteArrayAsync(dllUrl);
        return new DynamicPage()
        {
            PluginAssembly = Assembly.Load(dllBytes)
        };
    }
}