using System.Net;
using System.Net.Http.Json;
using CommonDTO;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class PluginService: IPluginService
{
    private HttpClient _httpClient;
    private string _getListEndpoint;
    private string _addEndpoint;
    private string _removeEndpoint;
    private string _editEndpoint;
    private string _upgradeEndpoint;
    private string _starEndpoint;
    private string _unstarEndpoint;
    
    public PluginService(HttpClient httpClient, 
        string getListEndpoint,
        string addEndpoint,
        string editEndpoint,
        string upgradeEndpoint,
        string removeEndpoint,
        string starEndpoint,
        string unstarEndpoint)
    {
        _httpClient = httpClient;
        _getListEndpoint = getListEndpoint;
        _addEndpoint = addEndpoint;
        _upgradeEndpoint = upgradeEndpoint;
        _editEndpoint = editEndpoint;
        _removeEndpoint = removeEndpoint;
        _starEndpoint = starEndpoint;
        _unstarEndpoint = unstarEndpoint;
    }

    public async Task Edit(PluginDTO dto)
    {
        var response = await _httpClient.PostAsJsonAsync(_editEndpoint, dto);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    }

    public async Task Upgrade(PluginUpgradeData data)
    {
        var multipartContent = new MultipartFormDataContent();
        if (data.ClientDLL != null)
            multipartContent.Add(GetStreamContent(data.ClientDLL), "ClientDLL", data.ClientDLL.Name);
        if (data.ServerDLL != null)
            multipartContent.Add(GetStreamContent(data.ServerDLL), "ServerDLL", data.ServerDLL.Name);
        multipartContent.Add(new StringContent(data.PluginId.ToString()), "PluginId");
        var response = await _httpClient.PostAsync(_upgradeEndpoint, multipartContent);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    }
    
    private string GetListEndPoint(int page, int pageSize, string sortBy, 
        SortDirection order, string search = "")
    {
        return string.Format(_getListEndpoint, page, pageSize, sortBy,
            order.ToString().ToLower(), search);
    }

    public async Task<PaginationList<PluginDTO>> GetList(int page, int pageSize, 
        string sortBy, SortDirection order, string search, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Getting list with page: {page}, pageSize: {pageSize}, sortBy: {sortBy}, order: {order}, search: {search}");
        var res = await _httpClient.GetFromJsonAsync<PaginationList<PluginDTO>>(
            GetListEndPoint(page, pageSize, sortBy, order, search), cancellationToken);
        if (res == null)
        {
            Console.WriteLine("Error: No data received from server");
            throw new HttpRequestException("Error: No data received from server");
        }
        return res;
    }

    public async Task Remove(Guid id)
    {
        var response = await _httpClient.PostAsJsonAsync(_removeEndpoint, new PluginDTO()
        {
            PluginId = id
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    }

    public async Task StarPlugin(Guid pluginId)
    {
        var response = await _httpClient.PostAsJsonAsync(_starEndpoint, new PluginDTO()
        {
            PluginId = pluginId
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}", null, response.StatusCode);
        }
    }

    public async Task UnstarPlugin(Guid pluginId)
    {
        var response = await _httpClient.PostAsJsonAsync(_unstarEndpoint, new PluginDTO()
        {
            PluginId = pluginId
        });
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}", null, response.StatusCode);
        }
    }

    public async Task Add(PluginUploadData data)
    {
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(GetStreamContent(data.ClientDLL), "ClientDLL", data.ClientDLL.Name);
        if (data.ServerDLL != null)
            multipartContent.Add(GetStreamContent(data.ServerDLL), "ServerDLL", data.ServerDLL.Name);
        multipartContent.Add(new StringContent(data.Name), "Name");
        multipartContent.Add(new StringContent(data.Description), "Description");
        multipartContent.Add(new StringContent(data.IsPremium.ToString()), "IsPremium");
        multipartContent.Add(new StringContent(data.Category), "Category");
        
        var response = await _httpClient.PostAsync(_addEndpoint, multipartContent);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
            throw new HttpRequestException($"Error: {response.StatusCode}, Message: {errorMessage}");
        }
    }

    private static StreamContent GetStreamContent(IBrowserFile file)
    {
        var fileStream = file.OpenReadStream(file.Size + 1);
        var content = new StreamContent(fileStream);
        string contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        return content;
    }
}