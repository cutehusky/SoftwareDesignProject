// UserService.cs
using System.Net;
using System.Net.Http.Json;
using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Client.Models;

namespace SoftwareDesignProject.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly string _getListEndpoint;
    private readonly string _updateRoleEndpoint;
    private readonly string _deleteEndpoint;
    private readonly string _addEndpoint;
    private readonly string _upgradeEndpoint;
    private readonly string _refreshTokenEndpoint;

    public UserService(HttpClient httpClient,
        string getListEndpoint,
        string updateRoleEndpoint,
        string deleteEndpoint,
        string addEndpoint,
        string upgradeEndpoint,
        string refreshTokenEndpoint)
    {
        _httpClient = httpClient;
        _getListEndpoint = getListEndpoint;
        _updateRoleEndpoint = updateRoleEndpoint;
        _deleteEndpoint = deleteEndpoint;
        _addEndpoint = addEndpoint;
        _upgradeEndpoint = upgradeEndpoint;
        _refreshTokenEndpoint = refreshTokenEndpoint;
    }

    public async Task<PaginationList<UserDTO>> GetList(int page, int pageSize,
        string sortBy, SortDirection order, string search, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Getting list with page: {page}, pageSize: {pageSize}, sortBy: {sortBy}, order: {order}, search: {search}");
        var res = await _httpClient.GetFromJsonAsync<PaginationList<UserDTO>>(
            GetListEndPoint(page, pageSize, sortBy, order, search), cancellationToken);
        if (res == null)
        {
            Console.WriteLine("Error: No data received from server");
            throw new HttpRequestException("Error: No data received from server");
        }
        return res;
    }

    private string GetListEndPoint(int page, int pageSize, string sortBy, SortDirection order, string search)
    {
        return $"{_getListEndpoint}?page={page}&pageSize={pageSize}&sortBy={sortBy}&order={order}&search={search}";
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

    public async Task<UserDTO?> GetById(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_getListEndpoint}/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error fetching user by id: {errorMessage}");
        }
    }

    public async Task Upgrade(Guid id)
    {
        var response = await _httpClient.PutAsJsonAsync(_upgradeEndpoint, id);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error upgrading user: {errorMessage}");
        }
    }

    public async Task<string> RefreshToken(string oldToken)
    {
        var response = await _httpClient.PutAsJsonAsync(_refreshTokenEndpoint, oldToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error refreshing token: {errorMessage}");
        }
        else
        {
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

    }
}