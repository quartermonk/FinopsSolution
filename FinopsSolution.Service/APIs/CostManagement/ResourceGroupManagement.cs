using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Resources;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinopsSolution.Service.APIs.CostManagement
{
    public static class ResourceGroupManagement
    {
        private static HttpClient? _httpClient = null;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private const int elementBillingValue = 0;
        public static async Task AzureResourceGroupDetailsFetchAsync()
        {
            Authentication authentication = new Authentication();
            if (string.IsNullOrEmpty(Utils.subscriptionIdList))
                throw new Exception(Constants.SubscriptionEmpty);

            var subscriptions = Utils.subscriptionIdList.Split(',').ToList();
            _httpClient = authentication.GenerateClient().Result;

            var jsonContent = new StringContent(GetBillingMonthToDateJson(),
                                                Encoding.UTF8,
                                                "application/json");
            try
            {
                foreach (var subscription in subscriptions)
                {
                    var ResourceGroupListURL = $"https://management.azure.com/subscriptions/{subscription}/resourcegroups?api-version=2021-04-01";

                    var ResourceGroupListResponse = await _httpClient
                                        .GetAsync(ResourceGroupListURL)
                                        .ConfigureAwait(false);

                    ResourceGroupListResponse.EnsureSuccessStatusCode();

                    var ResourceGroupListData = await ResourceGroupListResponse
                                            .Content
                                            .ReadFromJsonAsync<JsonElement>();
                    var ResourceGroupListContent = (JObject)JsonConvert.DeserializeObject(ResourceGroupListData.ToString());
                    var ResourceGroupListJArray = ResourceGroupListContent["value"].Value<JArray>();
                    var ResourceGroupList = ResourceGroupListJArray.ToObject<List<dynamic>>();

                    List<ResourceGroupDto> _resourceGroupList = new List<ResourceGroupDto>();
                    foreach (var item in ResourceGroupList)
                    {
                        _httpClient = authentication.GenerateClient().Result;
                        ResourceGroupDto resourceGroupDto = new ResourceGroupDto();
                        resourceGroupDto.id = item["id"].ToString();
                        resourceGroupDto.name = item["name"].ToString();
                        resourceGroupDto.location = item["location"].ToString();
                        if (item.ContainsKey("tags"))
                        {
                            resourceGroupDto.tags = item["tags"].ToString();
                        }
                        _resourceGroupList.Add(resourceGroupDto);
                        var ResourceGroupCostUrl = $"https://management.azure.com/subscriptions/{subscription}/resourcegroups/{resourceGroupDto.name}/providers/Microsoft.CostManagement/query?api-version=2021-10-01";

                        var ResourceGroupCostResponse = await _httpClient
                                            .PostAsync(ResourceGroupCostUrl, jsonContent)
                                            .ConfigureAwait(false);
                        if(ResourceGroupCostResponse.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            //donothing
                        }
                        else if (ResourceGroupCostResponse.StatusCode == HttpStatusCode.TooManyRequests)
                        {
                            //donothing
                        }
                        //ResourceGroupCostResponse.EnsureSuccessStatusCode();
                        else
                        {
                            var ResourceGroupCostData = await ResourceGroupCostResponse
                                                    .Content
                                                    .ReadFromJsonAsync<JsonElement>();

                            var ResourceGroupCost = ResourceGroupCostData
                                        .GetProperty("properties")
                                        .GetProperty("rows");
                            Thread.Sleep(2000);
                            Console.WriteLine(ResourceGroupCost);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string GetBillingMonthToDateJson()
        {
            return "{ \"type\": \"Usage\", \"timeframe\": \"BillingMonthToDate\", \"dataset\": { \"granularity\": \"None\", \"aggregation\": { \"totalCost\": { \"name\": \"PreTaxCost\", \"function\": \"Sum\"}  } } }";
        }

    }
}
