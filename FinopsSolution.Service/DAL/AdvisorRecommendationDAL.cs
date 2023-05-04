using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.DAL
{
    internal static class AdvisorRecommendationDAL
    {
        internal static async Task InsertRecommendationToTable(AdvisorRecommendationDto advisorRecommendation)
        {
            try
            {
                var account = CloudStorageAccount.Parse(Utils.StorageAccountConnStr);
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference("AdvisorRecommendation");
                //table.CreateIfNotExistsAsync();
                TableOperation operation = TableOperation.InsertOrMerge(advisorRecommendation);
                await table.ExecuteAsync(operation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //ex.Message;
            }
        }

        
    }
}
