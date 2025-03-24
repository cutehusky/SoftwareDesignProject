using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SoftwareDesignProject.Client.Models;
using System.Threading.Tasks;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}


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
        var token = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (token == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new JwtResponse { Token = token });
    }
}
