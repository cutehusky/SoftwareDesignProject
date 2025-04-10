using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Models.Entities;

namespace SoftwareDesignProject.Repositories;

public interface IUserRepository : IRepository<UserDTO>
{
    public Task<UserDTO?> GetUserByUsernameAsync(string username);
}