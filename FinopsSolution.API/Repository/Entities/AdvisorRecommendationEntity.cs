using Azure;
using Azure.Data.Tables;

namespace FinopsSolution.API.Repository.Entities;

public class AdvisorRecommendationEntity : ITableEntity
{
    public string Recommendation { get; set; }
    public string Id { get; set; }
    public string ImpactedResource { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
