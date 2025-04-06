using System.Net.Http.Json;
using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class NavMenuLoader: INavMenuLoader
{
    private HttpClient _httpClient;
    private string _navItemEndpoint;
    private string _homeItemEndpoint;
    
    public NavMenuLoader(HttpClient httpClient,
        string navItemEndpoint,
        string homeItemEndpoint)
    {
        _httpClient = httpClient;
        _navItemEndpoint = navItemEndpoint;
        _homeItemEndpoint = homeItemEndpoint;
    }
    
    public async Task<List<NavItem>?> GetNavItem()
    {
        var res = await _httpClient.GetFromJsonAsync<List<NavItem>>(_navItemEndpoint);
        return res;
    }

    public Task<List<HomeItem>?> GetHomeItem()
    {
        var res = _httpClient.GetFromJsonAsync<List<HomeItem>>(_homeItemEndpoint);
        return res;
    }
}