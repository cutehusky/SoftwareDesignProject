using CommonDTO;

namespace SoftwareDesignProject.Services;

public interface IAuthService
{
    public Task<string?> AuthenticateAsync(string username, string password);
    public Task RegisterAsync(string username, string password, UserRoles role);

    public Task<string?> RefreshToken(string oldToken);
}