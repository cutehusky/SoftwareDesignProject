using System.Reflection;
using PluginTemplate;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

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
        var assembly = Assembly.Load(dllBytes);
        Type? entryPoint = null;
        foreach (var type in assembly.GetExportedTypes())
        {
            Console.WriteLine(type.FullName);
            if ((typeof(Config)).IsAssignableFrom(type))
            {
                Config config = (Config) Activator.CreateInstance(type)!;
                entryPoint = config.EntryPoint;
            }
        }
        if (entryPoint != null)
            return new DynamicPage()
            {
                PluginAssembly = Assembly.Load(dllBytes),
                EntryPoint = entryPoint
            };
        return null;
    }
}