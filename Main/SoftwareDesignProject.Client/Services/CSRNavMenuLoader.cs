using System.Net.Http.Json;
using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class CSRNavMenuLoader: INavMenuLoader
{
    private HttpClient _httpClient;
    private string _endpoint;
    
    public CSRNavMenuLoader(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }
    
    public async Task<List<NavItem>?> GetNavItem()
    {
        var res = await _httpClient.GetFromJsonAsync<List<NavItem>>(_endpoint);
        return res;
    }
}