using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class SubscriptionDto:TableEntity
    {
        public SubscriptionDto(string subscriptionID, string costCategory)
        {
            PartitionKey = subscriptionID; RowKey = costCategory;
        }
        public string Cost { get; set; }
    }
}
