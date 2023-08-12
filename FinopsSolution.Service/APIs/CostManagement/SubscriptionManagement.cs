using FinopsSolution.Service.DAL;
using FinopsSolution.Service.Dto;
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
    public class SubscriptionManagement
    {
        private readonly HttpClient _httpClient;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private const int elementBillingValue = 0;

        public SubscriptionManagement(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AzureBillingMonthToDateApiFetchAsync()
        {
            Authentication authentication = new Authentication();

            if (string.IsNullOrEmpty(Utils.subscriptionIdList))
            {
                throw new Exception(Constants.SubscriptionEmpty);
            }

            var subscriptions = Utils.subscriptionIdList.Split(',').ToList();
            _httpClient = await authentication.GenerateClient();

            var jsonContent = new StringContent(GetBillingMonthToDateJson(),
                                                Encoding.UTF8,
                                                "application/json");

            foreach (var subscription in subscriptions)
            {
                SubscriptionDto actualSubscriptionCostDto = new SubscriptionDto(DateTime.Now, "Actual Cost");
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
                if (row.ToString() != "[]")
                {
                    var rows = row.EnumerateArray();
                    actualSubscriptionCostDto.Cost = rows
                                        .First()
                                        .EnumerateArray()
                                        .ElementAt(elementBillingValue)
                                        .GetDouble().ToString();
                    actualSubscriptionCostDto.CostType = "actual cost";
                }

                await SubscriptionManagementDAL.InsertSubscriptionCostToTable(actualSubscriptionCostDto);
                var forecastJsonContent = new StringContent(GetSubscriptionForcastToJson(),
                                            Encoding.UTF8,
                                            "application/json");
                SubscriptionDto forcastSubscriptionCostDto = new SubscriptionDto(DateTime.Now, "Forcast Cost");

                var SubscriptionForcastCostUrl = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.CostManagement/forecast?api-version=2022-10-01";

                var SubscriptionForcastCostResponse = await _httpClient
                                    .PostAsync(SubscriptionForcastCostUrl, forecastJsonContent)
                                    .ConfigureAwait(false);

                SubscriptionForcastCostResponse.EnsureSuccessStatusCode();

                var SubscriptionForecastCostData = await SubscriptionForcastCostResponse
                                        .Content
                                        .ReadFromJsonAsync<JsonElement>();

                var SubscriptionForecastCost = SubscriptionForecastCostData
                            .GetProperty("properties")
                            .GetProperty("rows");
                if (SubscriptionForecastCost.ToString() != "[]")
                {
                    var rows = SubscriptionForecastCost.EnumerateArray();
                    forcastSubscriptionCostDto.Cost = rows
                                        .First()
                                        .EnumerateArray()
                                        .ElementAt(elementBillingValue)
                                        .GetDouble().ToString();
                    forcastSubscriptionCostDto.CostType = "Forecast Cost";
                }
                await SubscriptionManagementDAL.InsertSubscriptionCostToTable(forcastSubscriptionCostDto);

            }

        }
        internal static string GetBillingMonthToDateJson()
        {
            return @"{
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
        }

        internal static string GetSubscriptionForcastToJson()
        {
            return "{ \"type\": \"Usage\",\"timeframe\": \"BillingMonthToDate\", \"dataset\": { \"granularity\": \"None\",\"aggregation\": { \"totalCost\": { \"name\": \"PreTaxCost\",\"function\": \"Sum\"} } } }";
        }
    }
}
