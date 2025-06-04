using System.Text.Json.Serialization;

namespace Mailomat.Integrations.SudReg.Models;

public class OAuthTokenResponse
{
    [JsonPropertyName("access_token")] public required string AccessToken { get; init; }

    [JsonPropertyName("token_type")] public required string TokenType { get; init; }

    [JsonPropertyName("expires_in")] public int ExpiresIn { get; init; }
}