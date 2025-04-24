using System.Threading;
using System.Threading.Tasks;
using CommonDTO;

namespace SoftwareDesignProject.Client.Services
{
    public interface IAuthService
    {
        public Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        public Task<JwtResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
