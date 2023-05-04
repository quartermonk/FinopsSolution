using FinopsSolution.Service.Resources;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinopsSolution.Service.APIs.CostManagement
{
    public static class SubscriptionManagement
    {
        private static HttpClient? _httpClient = null;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private const int elementBillingValue = 0;
        public static async Task AzureBillingMonthToDateApiFetchAsync()
        {
            Authentication authentication = new Authentication();
            if (string.IsNullOrEmpty(Utils.subscriptionIdList))
                throw new Exception(Constants.SubscriptionEmpty);

            var subscriptions = Utils.subscriptionIdList.Split(',').ToList();
            _httpClient = authentication.GenerateClient().Result;

            //if (_httpClient == null)
            //{
            //    await _semaphoreSlim
            //            .WaitAsync()
            //            .ConfigureAwait(false);

            //    _httpClient = new HttpClient();

            //    _semaphoreSlim.Release();
            //}

            //var token = await AzureIdentityService.GetToken(new(Utils.TenantId, Utils.ClientId, Utils.ClientSecret));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var jsonContent = new StringContent(GetBillingMonthToDateJson(),
                                                Encoding.UTF8,
                                                "application/json");

            try
            {
                foreach (var subscription in subscriptions)
                {
                    var subscriptionCostUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.CostManagement/query?api-version=2021-10-01";

                    var subscriptionCostResponse = await _httpClient
                                        .PostAsync(subscriptionCostUrl, jsonContent)
                                        .ConfigureAwait(false);

                    subscriptionCostResponse.EnsureSuccessStatusCode();

                    var subscriptionCostData = await subscriptionCostResponse
                                            .Content
                                            .ReadFromJsonAsync<JsonElement>();

                    var row = subscriptionCostData
                                .GetProperty("properties")
                                .GetProperty("rows");
                    await Console.Out.WriteLineAsync("subcost");
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var forcastJsonContent = new StringContent(GetSubscriptionForcastToJson(),
                                                Encoding.UTF8,
                                                "application/json");
                    var SubscriptionForcastCostUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.CostManagement/forecast?api-version=2022-10-01";

                    var SubscriptionForcastCostResponse = await _httpClient
                                        .PostAsync(SubscriptionForcastCostUrl, forcastJsonContent)
                                        .ConfigureAwait(false);

                    SubscriptionForcastCostResponse.EnsureSuccessStatusCode();

                    var SubscriptionForcastCostData = await SubscriptionForcastCostResponse
                                            .Content
                                            .ReadFromJsonAsync<JsonElement>();

                    var SubscriptionForcastCost = SubscriptionForcastCostData
                                .GetProperty("properties")
                                .GetProperty("rows");
                    await Console.Out.WriteLineAsync( "Forcast"  );

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

        internal static string GetSubscriptionForcastToJson()
        {
            return "{ \"type\": \"Usage\",\"timeframe\": \"BillingMonthToDate\", \"dataset\": { \"granularity\": \"None\",\"aggregation\": { \"totalCost\": { \"name\": \"PreTaxCost\",\"function\": \"Sum\"} } } }";
        }
    }
}
