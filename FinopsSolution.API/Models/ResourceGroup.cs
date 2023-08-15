namespace FinopsSolution.API.Models;

public class ResourceGroup
{
    public string Id { get; set; } = null!;
    public Guid SubscriptionId { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public Dictionary<string, object> Tags { get; set; } = new();
    public double Cost { get; set; }
}
