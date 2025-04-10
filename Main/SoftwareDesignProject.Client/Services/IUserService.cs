using CommonDTO;
using MudBlazor;

namespace SoftwareDesignProject.Client.Services;

public interface IUserService
{
    public Task<PaginationList<UserDTO>> GetList(int page, int pageSize,
        string sortBy, SortDirection order, string search,
        CancellationToken cancellationToken);
    Task UpdateUserRole(UserDTO dto);
    Task Delete(Guid id);
    Task Add(UserDTO user);
    Task Upgrade(Guid id);

    Task<string> RefreshToken(string oldToken);

    public Task<UserDTO?> GetById(Guid id);
}