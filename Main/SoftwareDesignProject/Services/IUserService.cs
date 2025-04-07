using CommonDTO;
using MudBlazor;

namespace SoftwareDesignProject.Services;

public interface IUserService
{
    public Task<PaginationList<UserDTO>> GetList(int page, int pageSize,
        string sortBy, SortDirection order, string search);
    public Task UpdateUserRole(UserDTO dto);
    public Task Delete(Guid id);
    public Task Add(UserDTO user);
    public Task<UserDTO?> GetById(Guid id);
}