using FinopsSolution.API.Clients;
using FinopsSolution.API.Models;
using FinopsSolution.API.Repository;
using FinopsSolution.API.Repository.Entities;

namespace FinopsSolution.API.Services;

public class CostManagementService
{
    private readonly SubscriptionManagementClient _subscriptionManagementClient;
    private readonly ResourceGroupManagementClient _resourceGroupManagementClient;
    private readonly RecommendationManagementClient _recommendationManagementClient;
    private readonly AlertManagementClient _alertManagementClient;
    private readonly SubscriptionManagementRepository _subscriptionManagementRepository;
    private readonly ResourceGroupCostRepository _resourceGroupCostRepository;
    private readonly AdvisorRecommendationRepository _advisorRecommendationRepository;

    public CostManagementService(SubscriptionManagementClient subscriptionManagementClient,
                                 ResourceGroupManagementClient resourceGroupManagementClient,
                                 RecommendationManagementClient recommendationManagementClient,
                                 AlertManagementClient alertManagementClient,
                                 SubscriptionManagementRepository subscriptionManagementRepository,
                                 ResourceGroupCostRepository resourceGroupCostRepository,
                                 AdvisorRecommendationRepository advisorRecommendationRepository)
    {
        _subscriptionManagementClient = subscriptionManagementClient ?? throw new ArgumentNullException(nameof(subscriptionManagementClient));
        _resourceGroupManagementClient = resourceGroupManagementClient ?? throw new ArgumentNullException(nameof(resourceGroupManagementClient));
        _recommendationManagementClient = recommendationManagementClient ?? throw new ArgumentNullException(nameof(recommendationManagementClient));
        _alertManagementClient = alertManagementClient ?? throw new ArgumentNullException(nameof(alertManagementClient));
        _subscriptionManagementRepository = subscriptionManagementRepository ?? throw new ArgumentNullException(nameof(subscriptionManagementRepository));
        _resourceGroupCostRepository = resourceGroupCostRepository ?? throw new ArgumentNullException(nameof(resourceGroupCostRepository));
        _advisorRecommendationRepository = advisorRecommendationRepository ?? throw new ArgumentNullException(nameof(advisorRecommendationRepository));
    }

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _subscriptionManagementRepository.InitializeAsync(),
            _resourceGroupCostRepository.InitializeAsync(),
            _advisorRecommendationRepository.InitializeAsync());
    }

    public async Task CallSubscriptionManagement()
    {
        List<SubscriptionCost> subscriptionCosts = await _subscriptionManagementClient.AzureBillingMonthToDateApiFetchAsync();
        IEnumerable<SubscriptionEntity> subscriptionCostEntities = subscriptionCosts.Select(ConvertToEntity);
        await _subscriptionManagementRepository.UpsertAsync(subscriptionCostEntities);

        List<ResourceGroup> resourceGroupCosts = await _resourceGroupManagementClient.AzureResourceGroupDetailsFetchAsync();
        IEnumerable<ResourceGroupEntity> resourceGroupCostEntities = resourceGroupCosts.Select(ConvertToEntity);
        await _resourceGroupCostRepository.UpsertAsync(resourceGroupCostEntities);

        IEnumerable<Recommendation> recommendations = await _recommendationManagementClient.AzureAdvisorRecommendationFetchAsync();
        IEnumerable<AdvisorRecommendationEntity> recommendationEntities = recommendations.Select(ConvertToEntity);
        await _advisorRecommendationRepository.UpsertAsync(recommendationEntities);

        await _alertManagementClient.AzureCreateAlertAsync();
    }

    private static SubscriptionEntity ConvertToEntity(SubscriptionCost model)
    {
        return new SubscriptionEntity
        {
            PartitionKey = $"{model.Timestamp:yyyyMMdd}-{model.SubscriptionId}",
            RowKey = Enum.GetName(model.CostType)!,
            SubscriptionId = model.SubscriptionId,
            CostType = Enum.GetName(model.CostType)!,
            Cost = model.Cost
        };
    }

    private static ResourceGroupEntity ConvertToEntity(ResourceGroup model)
    {
        return new ResourceGroupEntity
        {
            PartitionKey = $"{DateTime.UtcNow:yyyyMMdd}-{model.SubscriptionId}",
            RowKey = model.Name,
            Id = model.Id,
            SubscriptionId = model.SubscriptionId,
            Name = model.Name,
            Location = model.Location,
            Tags = string.Join(',', model.Tags.Select(x => $"{x.Key}:{x.Value}")),
            Cost = model.Cost
        };
    }

    private static AdvisorRecommendationEntity ConvertToEntity(Recommendation model)
    {
        return new AdvisorRecommendationEntity
        {
            PartitionKey = model.Properties.ImpactedValue,
            RowKey = model.Properties.ShortDescription.Solution,
            Id = model.Id,
            ImpactedResource = model.Properties.ImpactedValue,
            Recommendation = model.Properties.ShortDescription.Solution
        };
    }
}
