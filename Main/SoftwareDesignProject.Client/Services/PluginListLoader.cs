using System.Net.Http.Json;
using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class PluginListLoader: IPluginListLoader
{
    private HttpClient _httpClient;
    private string _endpoint;
    
    public PluginListLoader(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }
    
    public async Task<List<PluginDTO>?> GetItem()
    {
        var res = await _httpClient.GetFromJsonAsync<List<PluginDTO>>(_endpoint);
        return res;
    }
}