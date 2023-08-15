using FinopsSolution.API.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FinopsSolution.API.Clients;

public class SubscriptionManagementClient
{
    private const string billingMonthToDateJson =
        @"{
            'type': 'Usage',
            'timeframe': 'BillingMonthToDate',
            'dataset': {
              'granularity': 'None',
              'aggregation': {
                'totalCost': {
                  'name': 'PreTaxCost',
                  'function': 'Sum'
                }
              }
            }
          }";

    private readonly HttpClient _httpClient;
    private readonly SubscriptionListSettings _options;

    public SubscriptionManagementClient(HttpClient httpClient, IOptions<SubscriptionListSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<List<SubscriptionCost>> AzureBillingMonthToDateApiFetchAsync()
    {
        List<SubscriptionCost> subscriptionCosts = new List<SubscriptionCost>();

        foreach (Guid subscriptionId in _options.Ids)
        {
            SubscriptionCost actualSubscriptionCost = await GetSubscriptionCostAsync(subscriptionId, CostType.Actual);
            subscriptionCosts.Add(actualSubscriptionCost);

            SubscriptionCost forecastSubscriptionCost = await GetSubscriptionCostAsync(subscriptionId, CostType.Forecast);
            subscriptionCosts.Add(forecastSubscriptionCost);
        }

        return subscriptionCosts;
    }

    private async Task<SubscriptionCost> GetSubscriptionCostAsync(Guid subscriptionId, CostType costType)
    {
        const int elementBillingValue = 0;
        string queryType = costType switch
        {
            CostType.Actual => "query",
            CostType.Forecast => "forecast",
            _ => throw new ArgumentOutOfRangeException(nameof(costType))
        };

        var url = $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.CostManagement/{queryType}?api-version=2021-10-01";
        var jsonContent = new StringContent(billingMonthToDateJson,
                                            Encoding.UTF8,
                                            "application/json");

        var subscriptionCostResponse = await _httpClient
                            .PostAsync(url, jsonContent)
                            .ConfigureAwait(false);

        subscriptionCostResponse.EnsureSuccessStatusCode();

        var subscriptionCostData = await subscriptionCostResponse
                                .Content
                                .ReadFromJsonAsync<JsonElement>();

        var row = subscriptionCostData
                    .GetProperty("properties")
                    .GetProperty("rows");

        SubscriptionCost subscriptionCost = new SubscriptionCost
        {
            SubscriptionId = subscriptionId,
            CostType = costType,
        };

        if (row.ToString() != "[]")
        {
            var rows = row.EnumerateArray();
            subscriptionCost.Cost = rows
                                .First()
                                .EnumerateArray()
                                .ElementAt(elementBillingValue)
                                .GetDouble();
        }

        return subscriptionCost;
    }
}
