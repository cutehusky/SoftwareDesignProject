using System.Net.Http.Json;
using CommonDTO;
using Microsoft.AspNetCore.Components.Forms;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class PublishPlugin: IPublishPlugin
{
    private HttpClient _httpClient;
    private string _endpoint;
    
    public PublishPlugin(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<bool> Submit(PluginUploadData data)
    {
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(GetStreamContent(data.ClientDLL), "ClientDLL", data.ClientDLL.Name);
        if (data.ServerDLL != null)
            multipartContent.Add(GetStreamContent(data.ServerDLL), "ServerDLL", data.ServerDLL.Name);
        multipartContent.Add(new StringContent(data.Name), "Name");
        multipartContent.Add(new StringContent(data.Description), "Description");
        multipartContent.Add(new StringContent(data.IsPremium.ToString()), "IsPremium");
        multipartContent.Add(new StringContent(data.Category), "Category");
        
        var response = await _httpClient.PostAsync(_endpoint, multipartContent);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<ActionResponse<bool>>();
            if (responseData != null)
            {
                if (responseData.Result) 
                    Console.WriteLine("Add Plugin OK");   
                else
                    Console.WriteLine("Add Plugin Fail");
                return true;
            }
            Console.WriteLine("Error: Unknown Error");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} {response.ReasonPhrase}");   
        }

        return false;
    }

    private static StreamContent GetStreamContent(IBrowserFile? file)
    {
        var fileStream = file.OpenReadStream(file.Size + 1);
        var content = new StreamContent(fileStream);
        string contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        return content;
    }
}