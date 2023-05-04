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
    internal static class SubscriptionManagementDAL
    {
        internal static async Task InsertSubscriptionCostToTable(SubscriptionDto subscription)
        {
            try
            {
                var account = CloudStorageAccount.Parse(Utils.StorageAccountConnStr);
                var client = account.CreateCloudTableClient();
                var table = client.GetTableReference("SubscriptionDetails");
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);
                TableOperation operation = TableOperation.InsertOrMerge(subscription);
                await table.ExecuteAsync(operation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //ex.Message;
            }
        }
    }
}
