using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Resources;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinopsSolution.Service.APIs.CostManagement
{
    public static class CostManagementService
    {
        public static  async Task callSubscriptionManagement()
        {
            //await SubscriptionManagement.AzureBillingMonthToDateApiFetchAsync();
            //await ResourceGroupManagement.AzureResourceGroupDetailsFetchAsync();
            //await RecommendationManagement.AzureAdvisorRecommendationFetchAsync();
            await AlertManagement.AzureCreateAlertAsync();
        }
        //public static async Task callResourceGroupManagement()
        //{
        //    await ResourceGroupManagement.AzureResourceGroupDetailsFetchAsync();
        //}

        //private static HttpClient? _httpClient = null;
        //private readonly static SemaphoreSlim _semaphoreSlim = new(1,1);
        //private const int elementBillingValue = 0;
        //public static async Task AzureBillingMonthToDateApiFetchAsync()
        //{
        //    if (string.IsNullOrEmpty(Utils.subscriptionIdList))
        //        throw new Exception(Constants.SubscriptionEmpty);

        //    var subscriptions = Utils.subscriptionIdList.Split(',').ToList();

        //    if (_httpClient == null)
        //    {
        //        await _semaphoreSlim
        //                .WaitAsync()
        //                .ConfigureAwait(false);

        //        _httpClient = new HttpClient();

        //        _semaphoreSlim.Release();
        //    }

        //    var token = await AzureIdentityService.GetToken(new(Utils.TenantId, Utils.ClientId, Utils.ClientSecret));
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        //    var jsonContent = new StringContent(GetBillingMonthToDateJson(),
        //                                        Encoding.UTF8,
        //                                        "application/json");

        //    try
        //    {
        //        foreach (var subscription in subscriptions)
        //        {
        //            var subscriptionCostUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.CostManagement/query?api-version=2021-10-01";

        //            var subscriptionCostResponse = await _httpClient
        //                                .PostAsync(subscriptionCostUrl, jsonContent)
        //                                .ConfigureAwait(false);

        //            subscriptionCostResponse.EnsureSuccessStatusCode();

        //            var subscriptionCostData = await subscriptionCostResponse
        //                                    .Content
        //                                    .ReadFromJsonAsync<JsonElement>();

        //            var row = subscriptionCostData
        //                        .GetProperty("properties")
        //                        .GetProperty("rows");
        //            /////////////////////////////////////////////////////////////////////////////////////////////////
        //            var ResourceGroupListURL = $"https://management.azure.com/subscriptions/{subscription}/resourcegroups?api-version=2021-04-01";

        //            var ResourceGroupListResponse = await _httpClient
        //                                .GetAsync(ResourceGroupListURL)
        //                                .ConfigureAwait(false);

        //            ResourceGroupListResponse.EnsureSuccessStatusCode();

        //            var ResourceGroupListData = await ResourceGroupListResponse
        //                                    .Content
        //                                    .ReadFromJsonAsync<JsonElement>();
        //            var ResourceGroupListContent = (JObject)JsonConvert.DeserializeObject(ResourceGroupListData.ToString());
        //            var ResourceGroupListJArray = ResourceGroupListContent["value"].Value<JArray>();
        //            var ResourceGroupList = ResourceGroupListJArray.ToObject<List<dynamic>>();

        //            List<ResourceGroupDto> _resourceGroupList = new List<ResourceGroupDto>();
        //            //foreach (var item in ResourceGroupList)
        //            //{
        //            //    ResourceGroupDto resourceGroupDto = new ResourceGroupDto();
        //            //    resourceGroupDto.id = item["id"].ToString();
        //            //    resourceGroupDto.name = item["name"].ToString();
        //            //    resourceGroupDto.location = item["location"].ToString();
        //            //    if (item.ContainsKey("tags"))
        //            //    {
        //            //        resourceGroupDto.tags = item["tags"].ToString();
        //            //    }
        //            //    _resourceGroupList.Add(resourceGroupDto);
        //            //    var ResourceGroupCostUrl = $"https://management.azure.com/subscriptions/{subscription}/resourcegroups/{resourceGroupDto.name}/providers/Microsoft.CostManagement/query?api-version=2021-10-01";

        //            //    var ResourceGroupCostResponse = await _httpClient
        //            //                        .PostAsync(ResourceGroupCostUrl, jsonContent)
        //            //                        .ConfigureAwait(false);

        //            //    ResourceGroupCostResponse.EnsureSuccessStatusCode();

        //            //    var ResourceGroupCostData = await ResourceGroupCostResponse
        //            //                            .Content
        //            //                            .ReadFromJsonAsync<JsonElement>();

        //            //    var ResourceGroupCost = ResourceGroupCostData
        //            //                .GetProperty("properties")
        //            //                .GetProperty("rows");
        //            //}
        //            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //            //var forcastJsonContent = new StringContent(GetSubscriptionForcastToJson(),
        //            //                            Encoding.UTF8,
        //            //                            "application/json");
        //            //var SubscriptionForcastCostUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.CostManagement/forecast?api-version=2022-10-01";

        //            //var SubscriptionForcastCostResponse = await _httpClient
        //            //                    .PostAsync(SubscriptionForcastCostUrl, forcastJsonContent)
        //            //                    .ConfigureAwait(false);

        //            //SubscriptionForcastCostResponse.EnsureSuccessStatusCode();

        //            //var SubscriptionForcastCostData = await SubscriptionForcastCostResponse
        //            //                        .Content
        //            //                        .ReadFromJsonAsync<JsonElement>();

        //            //var SubscriptionForcastCost = SubscriptionForcastCostData
        //            //            .GetProperty("properties")
        //            //            .GetProperty("rows");


        //            var RecommendationListUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.Advisor/recommendations?api-version=2022-10-01";
        //            var advResommendationresult = await _httpClient.GetAsync(RecommendationListUrl).ConfigureAwait(false);
        //            advResommendationresult.EnsureSuccessStatusCode();
        //            var jsonData = await advResommendationresult.Content.ReadFromJsonAsync<JsonElement>();
        //            var advResommendationContent = (JObject)JsonConvert.DeserializeObject(jsonData.ToString());
        //            var advRecommendationJArray = advResommendationContent["value"].Value<JArray>();
        //            var advRecommendation = advRecommendationJArray.ToObject<List<dynamic>>();

        //            var detailUsageUrl = $"https://management.azure.com/subscriptions/{subscription}/providers//Microsoft.Consumption/usageDetails?$expand=meterDetails&api-version=2021-10-01";
        //            var deatilUsageResult = await _httpClient.GetAsync(detailUsageUrl).ConfigureAwait(false);
        //            deatilUsageResult.EnsureSuccessStatusCode();
        //            var usgaeData = await deatilUsageResult.Content.ReadFromJsonAsync<JsonElement>();
        //            var usageContent = (JObject)JsonConvert.DeserializeObject(usgaeData.ToString());
        //            var usageContentJArray = usageContent["value"].Value<JArray>();
        //            var Usage = usageContentJArray.ToObject<List<dynamic>>();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //internal static string GetBillingMonthToDateJson()
        //{
        //    return "{ \"type\": \"Usage\", \"timeframe\": \"BillingMonthToDate\", \"dataset\": { \"granularity\": \"None\", \"aggregation\": { \"totalCost\": { \"name\": \"PreTaxCost\", \"function\": \"Sum\"}  } } }";
        //}

        //internal static string GetSubscriptionForcastToJson()
        //{
        //    return "{ \"type\": \"Usage\",\"timeframe\": \"Monthly\", \"dataset\": { \"granularity\": \"None\",\"aggregation\": { \"totalCost\": { \"name\": \"PreTaxCost\",\"function\": \"Sum\"} } } }";
        //}

    }
}
