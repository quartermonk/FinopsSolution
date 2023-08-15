namespace FinopsSolution.API.Models;

public class Recommendation
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public RecommendationProperties Properties { get; set; }
}

public class RecommendationProperties
{
    public string Category { get; set; }
    public string Impact { get; set; }
    public string ImpactedField { get; set; }
    public string ImpactedValue { get; set; }
    public DateTime LastUpdated { get; set; }
    public string RecommendationTypeId { get; set; }
    public ShortDescription ShortDescription { get; set; }
    public Dictionary<string, object> ExtendedProperties { get; set; }
    public ResourceMetadata ResourceMetadata { get; set; }
}

public class ShortDescription
{
    public string Problem { get; set; }
    public string Solution { get; set; }
}

public class ResourceMetadata
{
    public string ResourceId { get; set; }
    public string Source { get; set; }
}
