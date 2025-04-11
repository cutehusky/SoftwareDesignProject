using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using ClientPluginTemplate;
using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class DynamicPageLoader: IDynamicPageLoader
{

    private HttpClient _httpClient;
    private string _apiEndpoint;
    private string _downloadEndpoint;
    private string _getDetailEndpoint;
    
    public DynamicPageLoader(HttpClient httpClient,
        string apiEndpoint,
        string downloadEndpoint,
        string getDetailEndpoint)
    {
        _httpClient = httpClient;
        _apiEndpoint = apiEndpoint;
        _downloadEndpoint = downloadEndpoint;
        _getDetailEndpoint = getDetailEndpoint;
    }

    private string GetApiEndPoint(string id)
    {
        return string.Format(_apiEndpoint, id);
    }
    
    private string GetDownloadEndPoint(string id)
    {
        return string.Format(_downloadEndpoint, id);
    }
    
    private string GetDetailEndPoint(string id)
    {
        return string.Format(_getDetailEndpoint, id);
    }
    
    private async Task<PluginDTO> GetPlugin(string id)
    {
        Console.WriteLine($"Getting Plugin in Server: " + GetDetailEndPoint(id));
        var response = await _httpClient.GetAsync(GetDetailEndPoint(id));
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException("Plugin is not valid", null, response.StatusCode);
        var plugin = await response.Content.ReadFromJsonAsync<PluginDTO>();
        if (plugin == null)
            throw new HttpRequestException("Plugin is not valid", null, HttpStatusCode.BadRequest);
        return plugin;
    }

    public async Task<DynamicPage?> LoadDynamicAssembly(string pluginId)
    {
        var plugin = await GetPlugin(pluginId);
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
                Plugin = plugin,
                PluginAssembly = Assembly.Load(dllBytes),
                EntryPoint = entryPoint
            };
        return null;
    }
}