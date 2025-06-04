using System.Text.Json;
using Mailomat.Integrations.SudReg.Clients;
using Mailomat.Integrations.SudReg.Interfaces;
using Mailomat.Integrations.SudReg.Models;
using Microsoft.Extensions.Configuration;

namespace Mailomat.Integrations.SudReg.Services;

public class SubjektiService(ApiClientProvider apiClientProvider, IConfiguration configuration) : ISubjektiService
{
    private readonly ApiClientProvider _clientProvider =
        apiClientProvider ?? throw new ArgumentNullException(nameof(apiClientProvider));

    private readonly string _baseUrl =
        configuration["SudRegApi:BaseUrl"]
        ?? throw new InvalidOperationException("Missing SudRegApi:BaseUrl");

    private readonly string _getSubjektiPath =
        configuration["SudRegApi:Endpoints:GetSubjekti"]
        ?? throw new InvalidOperationException("Missing SudRegApi:Endpoints:GetSubjekti");

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IEnumerable<SubjektResponse>> GetSubjektiAsync(int snapshotId, int offset, int limit, bool onlyActive = true)
    {
        var uriBuilder = new UriBuilder(_baseUrl)
        {
            Path = _getSubjektiPath,
            Query = $"snapshotId={snapshotId}"
                    + $"&offset={offset}"
                    + $"&limit={limit}"
                    + $"&onlyActive={onlyActive.ToString().ToLowerInvariant()}"
        };

        using var client = await _clientProvider.GetClientAsync();
        var response = await client.GetAsync(uriBuilder.Uri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<IEnumerable<SubjektResponse>>(json, JsonOptions);

        return result ?? [];
    }
}