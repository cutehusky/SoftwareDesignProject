using System.Net.Http.Json;
using CommonDTO;

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
    
    public async Task<NavData?> GetNavItem()
    {
        var res = await _httpClient.GetFromJsonAsync<NavData>(_navItemEndpoint);
        return res;
    }

    public Task<HomeData?> GetHomeItem()
    {
        var res = _httpClient.GetFromJsonAsync<HomeData>(_homeItemEndpoint);
        return res;
    }
}