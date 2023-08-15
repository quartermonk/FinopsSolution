using Azure.Core;
using Azure.Identity;
using FinopsSolution.API.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FinopsSolution.API.HttpHandlers;

public class BearerTokenHandler : DelegatingHandler
{
    private readonly ClientSecretCredential _clientCredential;
    private readonly AzureCredentialSettings _azureCredential;

    public BearerTokenHandler(ClientSecretCredential clientCredential, IOptions<AzureCredentialSettings> options)
    {
        _clientCredential = clientCredential ?? throw new ArgumentNullException(nameof(clientCredential));
        _azureCredential = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        AccessToken accessToken = await _clientCredential.GetTokenAsync(
            new TokenRequestContext(new[] { "https://management.azure.com/.default" }, _azureCredential.TenantId));

        if (!string.IsNullOrWhiteSpace(accessToken.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
