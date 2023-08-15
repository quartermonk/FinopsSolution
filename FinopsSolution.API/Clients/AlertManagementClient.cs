using FinopsSolution.API.Models;
using Microsoft.Extensions.Options;

namespace FinopsSolution.API.Clients;

public class AlertManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly SubscriptionListSettings _options;

    public AlertManagementClient(HttpClient httpClient, IOptions<SubscriptionListSettings> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task AzureCreateAlertAsync()
    {
        foreach (Guid subscriptionId in _options.Ids)
        {
            // TODO: have to check the url
            string alertUrl = $"https://management.azure.com/subscriptions/{subscriptionId}" +
                $"/providers/Microsoft.Consumption/budgets/test-akshay/" +
                $"?api-version=2019-10-01";

            await _httpClient
                .PutAsJsonAsync(alertUrl, CreateAlertPayload(), WebDefaults)
                .ConfigureAwait(false);
        }
    }

    private static Alert CreateAlertPayload()
    {
        DateTime date = DateTime.UtcNow;
        Alert alert = new()
        {
            Properties = new AlertProperties
            {
                Amount = 100,
                Category = "Cost",
                TimeGrain = "Monthly",
                TimePeriod = new TimePeriod
                {
                    StartDate = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    EndDate = new DateTime(date.Year, date.Month, 1).AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
                },
                Notifications = new Notifications
                {
                    Actual_GreaterThan_80_Percent = new ActualGreaterThan80Percent
                    {
                        Enabled = true,
                        @operator = "GreaterThan",
                        Threshold = 80,
                        Locale = "en-US",
                        ThresholdType = "Actual",
                        // TODO: make email configurable
                        ContactEmails = new List<string>() { "akshaykumar22@kpmg.com" }
                    }
                }
            },
            // TODO: check if this is required
            ETag = "1d34d016a593709"
        };

        return alert;
    }
}
