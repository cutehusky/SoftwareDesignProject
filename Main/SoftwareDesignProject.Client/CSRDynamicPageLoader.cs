using System.Reflection;

namespace SoftwareDesignProject.Client;

public class CSRDynamicPageLoader: IDynamicPageLoader
{

    private HttpClient _httpClient;
    
    public CSRDynamicPageLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DynamicPage?> LoadDynamicAssembly(string dllUrl)
    {
        Console.WriteLine($"Downloading DLL in Server: " + dllUrl);
        var dllBytes = await _httpClient.GetByteArrayAsync(dllUrl);
        return new DynamicPage()
        {
            PluginAssembly = Assembly.Load(dllBytes)
        };
        return null;
    }
}