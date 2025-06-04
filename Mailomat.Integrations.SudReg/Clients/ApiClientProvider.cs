using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Mailomat.Integrations.SudReg.Models;

namespace Mailomat.Integrations.SudReg.Clients;

public class ApiClientProvider(IConfiguration configuration)
{
    private readonly IConfiguration _configuration =
        configuration ?? throw new ArgumentNullException(nameof(configuration));

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private string _cachedToken = string.Empty;
    private DateTime _expiresAtUtc = DateTime.MinValue;

    public async Task<HttpClient> GetClientAsync()
    {
        if (string.IsNullOrEmpty(_cachedToken) || DateTime.UtcNow >= _expiresAtUtc)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (string.IsNullOrEmpty(_cachedToken) || DateTime.UtcNow >= _expiresAtUtc)
                {
                    var newToken = await RequestNewTokenAsync().ConfigureAwait(false);
                    _cachedToken = newToken.AccessToken;
                    _expiresAtUtc = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn - 30);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        var client = new HttpClient
        {
            BaseAddress = new Uri(_configuration["SudRegApi:BaseUrl"]
                                  ?? throw new InvalidOperationException("Missing BaseUrl"))
        };
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _cachedToken);

        return client;
    }

    private async Task<OAuthTokenResponse> RequestNewTokenAsync()
    {
        var clientId = _configuration["SudRegApi:ClientId"];
        var clientSecret = _configuration["SudRegApi:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            throw new InvalidOperationException("ClientId/Secret not configured.");

        using var http = new HttpClient();
        http.BaseAddress = new Uri(_configuration["SudRegApi:BaseUrl"]
                                   + "/api/oauth/token");

        var credentials = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

        using var content = new StringContent(
            "grant_type=client_credentials",
            Encoding.UTF8,
            "application/x-www-form-urlencoded"
        );

        var response = await http.PostAsync(string.Empty, content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(
            json, JsonOptions);

        if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            throw new InvalidOperationException("Failed to obtain access token.");

        return tokenResponse;
    }
}