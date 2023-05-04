using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Resources;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.APIs.CostManagement
{
    public static class AlertManagement
    {

        private static HttpClient? _httpClient = null;
        private readonly static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private const int elementBillingValue = 0;
        public static async Task AzureCreateAlertAsync()
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

            var alertJsonContent = new StringContent(createalertPayload(),
                                                Encoding.UTF8,
                                                "application/json");
            var AlertUrl = $"https://management.azure.com/subscriptions/{Utils.subscriptionIdList}/providers/Microsoft.Consumption/budgets/{"test-akshay"}/?api-version=2019-10-01";

            var AlertResponse = await _httpClient
                                .PutAsync(AlertUrl, alertJsonContent)
                                .ConfigureAwait(false);

        }

        internal static string createalertPayload()
        {
            Root root = new Root();
            root.properties = new Properties();
            root.properties.timePeriod = new TimePeriod();
            root.properties.notifications = new Notifications();
            root.properties.notifications.Actual_GreaterThan_80_Percent = new ActualGreaterThan80Percent();
            root.eTag = "";
            root.properties.amount = 100;
            root.properties.category = "Cost";
            root.properties.timeGrain = "Monthly";
            root.properties.timePeriod.startDate = DateTime.Now;
            root.properties.timePeriod.endDate = DateTime.Now.AddYears(1);
            root.properties.notifications.Actual_GreaterThan_80_Percent.enabled = true;
            root.properties.notifications.Actual_GreaterThan_80_Percent.@operator = "GreaterThan";
            root.properties.notifications.Actual_GreaterThan_80_Percent.threshold = 80;
            root.properties.notifications.Actual_GreaterThan_80_Percent.locale = "en-US";
            root.properties.notifications.Actual_GreaterThan_80_Percent.thresholdType = "Actual";
            root.properties.notifications.Actual_GreaterThan_80_Percent.contactEmails = new List<string>() { "akshaykumar22@kpmg.com", "monaliroutray@kpmg.com" };

            string body = JsonConvert.SerializeObject(root);
            return body;



        }
    }
}
