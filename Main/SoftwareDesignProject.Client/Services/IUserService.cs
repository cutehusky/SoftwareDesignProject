using CommonDTO;

namespace SoftwareDesignProject.Client.Services;

public interface IUserService
{
    Task<List<UserDTO>?> GetList();
    Task UpdateUserRole(UserDTO dto);
    Task Delete(Guid id);
    Task Add(UserDTO user);
}