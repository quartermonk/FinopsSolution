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
            await SubscriptionManagement.AzureBillingMonthToDateApiFetchAsync();
            await ResourceGroupManagement.AzureResourceGroupDetailsFetchAsync();
            await RecommendationManagement.AzureAdvisorRecommendationFetchAsync();
            await AlertManagement.AzureCreateAlertAsync();
        }

    }
}
