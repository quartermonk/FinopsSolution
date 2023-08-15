using Azure;
using Azure.Data.Tables;

namespace FinopsSolution.API.Repository.Entities;

public class SubscriptionEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public Guid SubscriptionId { get; set; }
    public string CostType { get; set; }
    public double Cost { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
