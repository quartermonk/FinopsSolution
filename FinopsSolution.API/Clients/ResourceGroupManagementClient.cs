using FinopsSolution.API.Models;
using FinopsSolution.API.Repository;
using FinopsSolution.API.Repository.Entities;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace FinopsSolution.API.Clients;

public class ResourceGroupManagementClient
{
    private const string BillingMonthToDateJson =
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
    private readonly ILogger<ResourceGroupManagementClient> _logger;
    private readonly SubscriptionListSettings _options;

    public ResourceGroupManagementClient(HttpClient httpClient,
                                         IOptions<SubscriptionListSettings> options,
                                         ILogger<ResourceGroupManagementClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<ResourceGroup>> AzureResourceGroupDetailsFetchAsync()
    {
        List<ResourceGroup> resourceGroupDetails = new List<ResourceGroup>();
        foreach (Guid subscriptionId in _options.Ids)
        {
            List<ResourceGroup> resourceGroups = await GetResourceGroupsAsync(subscriptionId);

            _logger.LogInformation("Retrieved '{RGCount}' resource groups in '{SubscriptionId}' subscriptions", resourceGroups.Count, subscriptionId);

            foreach (ResourceGroup resourceGroup in resourceGroups)
            {
                _logger.LogDebug("Retrieving cost for '{RG}'", resourceGroup.Name);
                resourceGroup.SubscriptionId = subscriptionId;
                resourceGroup.Cost = await GetResourceGroupCostAsync(subscriptionId, resourceGroup);
                _logger.LogInformation("Retrieved cost for '{RG}'", resourceGroup.Name);
            }

            resourceGroupDetails.AddRange(resourceGroups);
        }

        return resourceGroupDetails;
    }

    private async Task<List<ResourceGroup>> GetResourceGroupsAsync(Guid subscriptionId)
    {
        List<ResourceGroup> resourceGroups = new();
        Uri? resourceGroupListURL = new($"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups?api-version=2021-04-01");

        do
        {
            ManagementResponse<ResourceGroup>? resourceGroupResponse = await _httpClient
                                .GetFromJsonAsync<ManagementResponse<ResourceGroup>>(resourceGroupListURL, WebDefaults)
                                .ConfigureAwait(false);

            resourceGroups.AddRange(resourceGroupResponse!.Value);
            resourceGroupListURL = resourceGroupResponse.NextLink;
        }
        while (resourceGroupListURL is not null);

        return resourceGroups;
    }

    private async Task<double> GetResourceGroupCostAsync(Guid subscriptionId, ResourceGroup resourceGroup)
    {
        const int elementBillingValue = 0;
        StringContent jsonContent = new(BillingMonthToDateJson,
                                            Encoding.UTF8,
                                            "application/json");
        string ResourceGroupCostUrl = $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroup.Name}/providers/Microsoft.CostManagement/query?api-version=2021-10-01";

        HttpResponseMessage resourceGroupCostResponse = await _httpClient
                            .PostAsync(ResourceGroupCostUrl, jsonContent)
                            .ConfigureAwait(false);

        if (resourceGroupCostResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Failed to retrieve cost for '{RG}'", resourceGroup.Name);
            return 0d;
        }

        resourceGroupCostResponse.EnsureSuccessStatusCode();

        JsonElement ResourceGroupCostData = await resourceGroupCostResponse
                                .Content
                                .ReadFromJsonAsync<JsonElement>();

        JsonElement ResourceGroupCost = ResourceGroupCostData
                    .GetProperty("properties")
                    .GetProperty("rows");
        if (ResourceGroupCost.ToString() != "[]")
        {
            JsonElement.ArrayEnumerator rows = ResourceGroupCost.EnumerateArray();
            return rows
                .First()
                .EnumerateArray()
                .ElementAt(elementBillingValue)
                .GetDouble();
        }

        return 0d;
    }
}
