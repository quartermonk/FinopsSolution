using FinopsSolution.API.Models;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API.Clients;

public class RecommendationManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly SubscriptionListSettings _options;

    public RecommendationManagementClient(HttpClient httpClient, IOptions<SubscriptionListSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IEnumerable<Recommendation>> AzureAdvisorRecommendationFetchAsync()
    {
        List<Recommendation> allRecommendations = new List<Recommendation>();
        foreach (Guid subscriptionId in _options.Ids)
        {
            List<Recommendation> recommendations = await GetRecommendationsForSubscriptionAsync(subscriptionId);

            allRecommendations.AddRange(recommendations);
        }

        return allRecommendations;
    }

    private async Task<List<Recommendation>> GetRecommendationsForSubscriptionAsync(Guid subscriptionId)
    {
        List<Recommendation> recommendations = new();
        Uri? advisorRecommendationURL = new($"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.Advisor/recommendations?api-version=2022-10-01");

        do
        {
            ManagementResponse<Recommendation>? advisorRecommendationResponse = await _httpClient
                                .GetFromJsonAsync<ManagementResponse<Recommendation>>(advisorRecommendationURL, WebDefaults)
                                .ConfigureAwait(false);

            recommendations.AddRange(advisorRecommendationResponse!.Value);

            advisorRecommendationURL = advisorRecommendationResponse.NextLink;
        }
        while (advisorRecommendationURL is not null);

        return recommendations;
    }
}
