using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Resources;
using FinopsSolution.Service.Token;
using FinopsSolution.Service.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            var alertJsonContent = new StringContent(createalertPayload(),
                                                Encoding.UTF8,
                                                "application/json");
            var AlertUrl = $"https://management.azure.com/subscriptions/{Utils.subscriptionIdList}/providers/Microsoft.Consumption/budgets/test-akshay/?api-version=2019-10-01";

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
            root.eTag = "1d34d016a593709";
            root.properties.amount = 100;
            root.properties.category = "Cost";
            root.properties.timeGrain = "Monthly";
            DateTime date = DateTime.Now;
            root.properties.timePeriod.startDate = new DateTime( date.Year,date.Month,1).ToString("yyyy-MM-ddTHH:mm:ssZ");
            root.properties.timePeriod.endDate = new DateTime(date.Year, date.Month, 1).AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ");
            root.properties.notifications.Actual_GreaterThan_80_Percent.enabled = true;
            root.properties.notifications.Actual_GreaterThan_80_Percent.@operator = "GreaterThan";
            root.properties.notifications.Actual_GreaterThan_80_Percent.threshold = 80;
            root.properties.notifications.Actual_GreaterThan_80_Percent.locale = "en-US";
            root.properties.notifications.Actual_GreaterThan_80_Percent.thresholdType = "Actual";
            root.properties.notifications.Actual_GreaterThan_80_Percent.contactEmails = new List<string>() { "akshaykumar22@kpmg.com" };

            string body = JsonConvert.SerializeObject(root);
            return body;



        }
    }
}
