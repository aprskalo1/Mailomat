namespace Mailomat.Integrations.SudReg.Models;

public class OAuthTokenResponse
{
    public required string AccessToken { get; init; }
    public required string TokenType { get; init; }
    public int ExpiresIn { get; init; }
}