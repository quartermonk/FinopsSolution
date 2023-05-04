using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FinopsSolution.Service.Resources;

namespace FinopsSolution.Service.APIs.CostManagement
{
    public static class RecommendationManagement
    {
        private static HttpClient? _httpClient = null;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private const int elementBillingValue = 0;
        public static async Task AzureAdvisorRecommendationFetchAsync()
        {
            Authentication authentication = new Authentication();
            if (string.IsNullOrEmpty(Utils.subscriptionIdList))
                throw new Exception(Constants.SubscriptionEmpty);

            var subscriptions = Utils.subscriptionIdList.Split(',').ToList();
            _httpClient = authentication.GenerateClient().Result;

            try
            {
                foreach (var subscription in subscriptions)
                {
                    var AdvisorRecommendationURL = $"https://management.azure.com/subscriptions/{subscription}/providers/Microsoft.Advisor/recommendations?api-version=2022-10-01";

                    var AdvisorRecommendationResponse = await _httpClient
                                        .GetAsync(AdvisorRecommendationURL)
                                        .ConfigureAwait(false);

                    AdvisorRecommendationResponse.EnsureSuccessStatusCode();

                    var AdvisorRecommendationData = await AdvisorRecommendationResponse
                                            .Content
                                            .ReadFromJsonAsync<JsonElement>();
                    var AdvisorRecommendationContent = (JObject)JsonConvert.DeserializeObject(AdvisorRecommendationData.ToString());
                    var AdvisorRecommendationJArray = AdvisorRecommendationContent["value"].Value<JArray>();
                    List<AdvisorRecommendationDto> listAdvisorRecommendations = new List<AdvisorRecommendationDto>();
                    foreach(JObject recommendation in AdvisorRecommendationJArray)
                    {
                        AdvisorRecommendationDto advisorRecommendation = new AdvisorRecommendationDto();
                        advisorRecommendation.category = recommendation["properties"]["category"].ToString();
                        advisorRecommendation.ImpactedResource = recommendation["properties"]["impactedValue"].ToString();
                        advisorRecommendation.recommendationName = recommendation["properties"]["shortDescription"]["solution"].ToString();
                        advisorRecommendation.id  = recommendation["id"].ToString();
                        listAdvisorRecommendations.Add(advisorRecommendation);
                    }
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
