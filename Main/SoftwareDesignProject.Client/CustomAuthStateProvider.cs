using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

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
        var payload = token.Split('.')[1];
        // Add padding if needed
        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
        var jsonBytes = Convert.FromBase64String(payload);
        var claimsDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        // Handle role claim specifically
        if (claimsDict.TryGetValue("role", out var roleValue))
        {
            yield return new Claim(ClaimTypes.Role, roleValue.ToString());
        }

        // Handle other claims
        foreach (var kvp in claimsDict.Where(c => c.Key != "role"))
        {
            yield return new Claim(kvp.Key, kvp.Value.ToString());
        }
    }
}
