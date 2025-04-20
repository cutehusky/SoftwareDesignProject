using System.Threading;
using System.Threading.Tasks;
using CommonDTO;

namespace SoftwareDesignProject.Client.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<JwtResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
