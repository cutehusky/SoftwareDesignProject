using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Client.Models;
using System.Threading.Tasks;

using SoftwareDesignProject.Models;
using System.Data;
using CommonDTO;


[Route("api/auth")]
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
        var token = await _authService.AuthenticateAsync(request.username, request.password);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new JwtResponse { Token = token });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        var success = await _authService.RegisterAsync(request.username, request.password);

        if (!success)
        {
            return BadRequest(new { message = "Username already exists" });
        }

        return Ok(new { message = "Account created successfully" });
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
