using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace SoftwareDesignProject.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        var claims = ParseJwtClaims(token).ToList();

        // TODO: This is only for debugging, remove in production
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task<string> GetToken()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token not found");
        }
        else
        {
            return token;
        }
    }


    public async Task Login(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseJwtClaims(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Enumerable.Empty<Claim>();

        // Basic JWT structure validation
        var tokenParts = token.Split('.');
        if (tokenParts.Length != 3) // JWS has 3 parts
        {
            Console.WriteLine($"Invalid JWT structure. Parts: {tokenParts.Length}");
            return Enumerable.Empty<Claim>();
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();

            // First validate if we can read the token
            if (!handler.CanReadToken(token))
            {
                Console.WriteLine("JWT cannot be read by token handler");
                return Enumerable.Empty<Claim>();
            }

            var jwtToken = handler.ReadJwtToken(token);
            return ProcessClaims(jwtToken.Claims);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token parsing failed: {ex.Message}");
            return Enumerable.Empty<Claim>();
        }
    }

    private IEnumerable<Claim> ProcessClaims(IEnumerable<Claim> claims)
    {
        foreach (var claim in claims)
        {
            // Normalize role claims
            if (claim.Type is "role" or ClaimTypes.Role)
            {
                yield return new Claim(ClaimTypes.Role, claim.Value);
            }
            else
            {
                yield return claim;
            }
        }
    }
}