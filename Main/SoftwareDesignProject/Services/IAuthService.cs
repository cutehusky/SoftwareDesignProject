using CommonDTO;

namespace SoftwareDesignProject.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string password, UserRoles role);

    Task<string?> RefreshToken(string oldToken);
}