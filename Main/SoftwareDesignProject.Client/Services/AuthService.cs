// File: Client/Services/AuthService.cs
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CommonDTO;
using Microsoft.AspNetCore.Components;

namespace SoftwareDesignProject.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _nav;

        public AuthService(HttpClient httpClient, NavigationManager navManager)
        {
            _http = httpClient;
            _nav = navManager;
        }

        public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsJsonAsync("api/auth/signup", request, cancellationToken);
            if (response.IsSuccessStatusCode) return;

            var msg = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(msg))
                msg = $"Registration failed: {response.StatusCode}";
            throw new ApplicationException(msg);
        }

        public async Task<JwtResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsJsonAsync("api/auth/signin", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(msg))
                    msg = $"Login failed: {response.StatusCode}";
                throw new ApplicationException(msg);
            }

            var jwt = await response.Content.ReadFromJsonAsync<JwtResponse>(cancellationToken: cancellationToken);
            return jwt!;
        }
    }
}
