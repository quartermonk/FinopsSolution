using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Utilities;
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
                var serviceClient = new TableServiceClient(
                                        new Uri(Utils.storageUri),
                                        new TableSharedKeyCredential(Utils.accountName, Utils.storageAccountKey));
                TableItem table = serviceClient.CreateTableIfNotExists("SubscriptionDetails");

                var tableClient = new TableClient(
                                        new Uri(Utils.storageUri),
                                        "SubscriptionDetails",
                                        new TableSharedKeyCredential(Utils.accountName, Utils.storageAccountKey));
                tableClient.UpsertEntity(subscription);
                
            }
            catch (Exception ex)
            {
                //ex.Message;
            }
        }
    }
}
