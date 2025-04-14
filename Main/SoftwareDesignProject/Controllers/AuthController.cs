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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Sign in request received for user: {Username}", request.Username);
        
        var token = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (token == null)
        {
            _logger.LogWarning("Invalid credentials for user: {Username}", request.Username);
            return Unauthorized(new { message = "Invalid credentials" });
        }

        _logger.LogInformation("User {Username} signed in successfully", request.Username);
        return Ok(new JwtResponse { Token = token });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        try
        {
            _logger.LogInformation("User registration request received for: {Username}", request.Username);
            await _authService.RegisterAsync(request.Username, request.Password, UserRoles.Normal);
            _logger.LogInformation("User {Username} created successfully", request.Username);
            return Ok(new { message = "Account created successfully" });
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("User registration failed: {Message}", ex.Message);
            return StatusCode(500, "Internal server error");
        }
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
}