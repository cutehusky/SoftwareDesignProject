using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommonDTO;
using Microsoft.IdentityModel.Tokens;
using SoftwareDesignProject.Repositories;

namespace SoftwareDesignProject.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IConfiguration config, IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _config = config;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return GenerateJwtToken(user);
        }
        return null;
    }



    public async Task<bool> RegisterAsync(string username, string password, UserRoles role = UserRoles.Normal)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null) return false;

        var newUser = new UserDTO
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            UserRole = role
        };

        return await _userRepository.Add(newUser);
    }

    public async Task<string?> RefreshToken(string oldToken)
    {
        var user = await GetUserByToken(oldToken);
        
        _logger.LogDebug("User  {Username} from token: {token}", user?.Username, oldToken);
        if (user == null) return null;
        var newToken = GenerateJwtToken(user);
        _logger.LogDebug("New token {newToken} generated for user: {Username} ", newToken, user.Username);
        return newToken;
    }

    private async Task<UserDTO?> GetUserByToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn);
        if (userIdClaim == null) return null;
        var userId = Guid.Parse(userIdClaim.Value);
        return await _userRepository.GetById(userId);
    }


    private string GenerateJwtToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Upn, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
         };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}