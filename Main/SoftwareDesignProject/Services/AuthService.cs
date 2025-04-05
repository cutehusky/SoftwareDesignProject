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
    private readonly UserRepository _userRepository;

    public AuthService(IConfiguration config, UserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        //Console.WriteLine("User: " + user.HashedPassword);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
        {
            return GenerateJwtToken(user.Username);
        }

        return null;
    }


    public async Task<bool> RegisterAsync(string username, string password)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null)
        {
            return false;
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new UserDTO { Username = username, HashedPassword = hashedPassword };
        return await _userRepository.InsertUserAsync(newUser);
    }


    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("role", "User")
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