using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class AdvisorRecommendationDto :ITableEntity
    {
        public AdvisorRecommendationDto(String PartitionKey, string RowKey)
        {
            this.PartitionKey = PartitionKey; this.RowKey = RowKey;
        }
        public string Recommendation { get; set; }
        public string Id { get; set; }
        public string ImpactedResource { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
