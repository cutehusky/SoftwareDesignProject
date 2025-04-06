// UserService.cs
using System.Net;
using System.Net.Http.Json;
using CommonDTO;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly string _getListEndpoint;
    private readonly string _updateRoleEndpoint;
    private readonly string _deleteEndpoint;
    private readonly string _addEndpoint;

    public UserService(HttpClient httpClient,
        string getListEndpoint,
        string updateRoleEndpoint,
        string deleteEndpoint,
        string addEndpoint)
    {
        _httpClient = httpClient;
        _getListEndpoint = getListEndpoint;
        _updateRoleEndpoint = updateRoleEndpoint;
        _deleteEndpoint = deleteEndpoint;
        _addEndpoint = addEndpoint;
    }

    public async Task<List<UserDTO>?> GetList()
    {
        return await _httpClient.GetFromJsonAsync<List<UserDTO>>(_getListEndpoint);
    }

    public async Task UpdateUserRole(UserDTO dto)
    {
        var response = await _httpClient.PutAsJsonAsync(_updateRoleEndpoint, dto);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error updating role: {errorMessage}");
        }
    }

    public async Task Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{_deleteEndpoint}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error deleting user: {errorMessage}");
        }
    }

    public async Task Add(UserDTO user)
    {
        var response = await _httpClient.PostAsJsonAsync(_addEndpoint, user);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error adding user: {errorMessage}");
        }
    }
}