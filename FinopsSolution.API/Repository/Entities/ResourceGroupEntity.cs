using Azure;
using Azure.Data.Tables;

namespace FinopsSolution.API.Repository.Entities;

public class ResourceGroupEntity : ITableEntity
{
    public string Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public string Name { get; set; }
    public string Tags { get; set; }
    public string Location { get; set; }
    public double Cost { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
