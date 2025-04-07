using CommonDTO;
using MudBlazor;
using SoftwareDesignProject.Models.Entities;

namespace SoftwareDesignProject.Repositories;

public interface IUserRepository : IRepository<UserDTO>
{
    public Task<UserDTO?> GetUserByUsernameAsync(string username);
    public Task<PaginationList<UserDTO>> GetAll(int page, int pageSize,
        string sortBy, SortDirection order, string search);

    public Task<UserDTO?> GetById(Guid id);

    public Task<bool> Add(UserDTO dto);

    public Task<bool> Remove(Guid id);

    public Task<bool> Update(UserDTO entity);

}