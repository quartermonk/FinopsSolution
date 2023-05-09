using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class SubscriptionDto:ITableEntity
    {
        public SubscriptionDto(DateTime PartitionKey, String RowKey)
        {
            this.PartitionKey = PartitionKey.ToString("yyyy-MM-dd"); this.RowKey = RowKey;
        }
        public string CostType { get; set; }
        public string Cost { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
