using CommonDTO;
using MudBlazor;

namespace SoftwareDesignProject.Client.Services;

public interface IUserService
{
    public Task<PaginationList<UserDTO>> GetList(int page, int pageSize,
        string sortBy, SortDirection order, string search,
        CancellationToken cancellationToken);
    public Task UpdateUserRole(UserDTO dto);
    public Task Delete(Guid id);
    public Task Add(UserDTO user);
    public Task Upgrade(Guid id);

    public Task<string> RefreshToken(string oldToken);

    public Task<UserDTO?> GetById(Guid id);
}