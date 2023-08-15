namespace FinopsSolution.API.Models;

public class SubscriptionCost
{
    public Guid SubscriptionId { get; set; }
    public CostType CostType { get; set; }
    public double Cost { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public enum CostType
{
    Actual,
    Forecast
}
