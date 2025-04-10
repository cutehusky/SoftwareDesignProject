using System.Data;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Services;

namespace SoftwareDesignProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
    {
        Console.WriteLine("Sign in request received");
        var token = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new JwtResponse { Token = token });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        var success = await _authService.RegisterAsync(request.Username, request.Password, UserRoles.Normal);

        if (!success)
        {
            return BadRequest(new { message = "Username already exists" });
        }

        return Ok(new { message = "Account created successfully" });
    }

    [HttpPut("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string oldToken)
    {
        if (string.IsNullOrWhiteSpace(oldToken))
        {
            return BadRequest(new { message = "Token is required" });
        }
        var newToken = await _authService.RefreshToken(oldToken);
        if (newToken == null)
        {
            return Unauthorized(new { message = "Invalid token" });
        }
        return Ok(new JwtResponse { Token = newToken });
    }

    [HttpGet("test-db")]
    public async Task<IActionResult> TestDatabaseConnection([FromServices] IDbConnection dbConnection)
    {
        try
        {
            dbConnection.Open();
            return Ok("Database connection successful!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database connection failed: {ex.Message}");
        }
    }

}