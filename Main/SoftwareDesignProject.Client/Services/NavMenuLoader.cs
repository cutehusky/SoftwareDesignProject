using System.Net.Http.Json;
using CommonDTO;

namespace SoftwareDesignProject.Client.Services;

public class NavMenuLoader: INavMenuLoader
{
    private readonly HttpClient _httpClient;
    private readonly string _navItemEndpoint;
    private readonly string _homeItemEndpoint;
    private readonly string _searchEndpoint;
    
    public NavMenuLoader(HttpClient httpClient,
        string navItemEndpoint,
        string homeItemEndpoint,
        string searchEndpoint)
    {
        _httpClient = httpClient;
        _navItemEndpoint = navItemEndpoint;
        _homeItemEndpoint = homeItemEndpoint;
        _searchEndpoint = searchEndpoint;
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

    public async Task<List<NavItem>?> Search(string queryValue)
    {
        var res = await _httpClient.GetFromJsonAsync<List<NavItem>>($"{_searchEndpoint}?queryValue={queryValue}");
        return res;
    }
}