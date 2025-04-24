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
    private readonly IPasswordHasher _passwordHasher;
    private readonly IFormatChecker _formatChecker;

    public AuthService(IConfiguration config, IUserRepository userRepository, ILogger<AuthService> logger, IPasswordHasher passwordHasher, IFormatChecker formatChecker)
    {
        _config = config;
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _formatChecker = formatChecker;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user != null && _passwordHasher.VerifyPassword(user.Password!, password))
        {
            return GenerateJwtToken(user);
        }
        return null;
    }



    public async Task RegisterAsync(string username, string password, UserRoles role)
    {
        if (!_formatChecker.IsValidUsername(username))
        {
            throw new InvalidDataException("Invalid username format");
        }
        
        if (!_formatChecker.IsValidPassword(password))
        {
            throw new InvalidDataException("Invalid password format");
        }
        
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null) 
        {
            throw new InvalidDataException($"User {username} already exists");
        }

        var newUser = new UserDTO
        {
            Username = username,
            Password = _passwordHasher.HashPassword(password),
            UserRole = role
        };

        if (!await _userRepository.Add(newUser))
        {
            throw new InvalidOperationException("Failed to create user");
        }
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