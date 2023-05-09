using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using FinopsSolution.Service.Dto;
using FinopsSolution.Service.Utilities;
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
                var serviceClient = new TableServiceClient(
                                        new Uri(Utils.storageUri),
                                        new TableSharedKeyCredential(Utils.accountName, Utils.storageAccountKey));
                TableItem table = serviceClient.CreateTableIfNotExists("AdvisorRecommendation");

                var tableClient = new TableClient(
                                        new Uri(Utils.storageUri),
                                        "AdvisorRecommendation",
                                        new TableSharedKeyCredential(Utils.accountName, Utils.storageAccountKey));
                tableClient.UpsertEntity(advisorRecommendation);
            }
            catch (Exception ex)
            {
                //ex.Message;
            }
        }

        
    }
}
