using System.Net;
using System.Reflection;
using ClientPluginTemplate;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class DynamicPageLoader: IDynamicPageLoader
{

    private HttpClient _httpClient;
    private string _apiEndpoint;
    private string _downloadEndpoint;
    private string _checkEndpoint;
    
    public DynamicPageLoader(HttpClient httpClient,
        string apiEndpoint,
        string downloadEndpoint,
        string checkEndpoint)
    {
        _httpClient = httpClient;
        _apiEndpoint = apiEndpoint;
        _downloadEndpoint = downloadEndpoint;
        _checkEndpoint = checkEndpoint;
    }

    private string GetApiEndPoint(string id)
    {
        return string.Format(_apiEndpoint, id);
    }
    
    private string GetDownloadEndPoint(string id)
    {
        return string.Format(_downloadEndpoint, id);
    }
    
    private string GetCheckEndPoint(string id)
    {
        return string.Format(_checkEndpoint, id);
    }
    
    private async Task<bool> CheckPlugin(string id)
    {
        var response = await _httpClient.GetAsync(GetCheckEndPoint(id));
        if (response.StatusCode == HttpStatusCode.OK)
            return true;
        return false;
    }

    public async Task<DynamicPage?> LoadDynamicAssembly(string pluginId)
    {
        var isValid = await CheckPlugin(pluginId);
        if (!isValid)
        {
            Console.WriteLine($"Plugin {pluginId} is not valid");
            throw new HttpRequestException("Plugin is not valid");
        }
        Console.WriteLine($"Downloading DLL in Server: " + pluginId);
        var dllBytes = await _httpClient.GetByteArrayAsync(GetDownloadEndPoint(pluginId));
        var assembly = Assembly.Load(dllBytes);
        Type? entryPoint = null;
        foreach (var type in assembly.GetExportedTypes())
        {
            Console.WriteLine(type.FullName);
            if ((typeof(IConfig)).IsAssignableFrom(type) && type.IsClass)
            {
                IConfig config = (IConfig) Activator.CreateInstance(type)!;
                entryPoint = config.EntryPoint;
                config.APIEndPoint = GetApiEndPoint(config.ID);
                break;
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