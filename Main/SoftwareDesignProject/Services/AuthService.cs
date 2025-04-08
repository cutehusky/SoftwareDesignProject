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

    public AuthService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
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


    private string GenerateJwtToken(UserDTO user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
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