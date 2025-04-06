using CommonDTO;

namespace SoftwareDesignProject.Repositories;

public interface IUserRepository: IRepository<UserDTO>
{
    public Task<UserDTO?> GetUserByUsernameAsync(string username);
}