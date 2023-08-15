using System.ComponentModel.DataAnnotations;

namespace FinopsSolution.API.Settings;

public class TableStorageSettings
{
    public static readonly string SettingName = "TableStorage";

    [Required]
    public string ConnectionString { get; set; } = null!;
    public TablesNames TableNames { get; set; } = new();
}

public class TablesNames
{
    public string AdvisorRecommendation { get; set; } = "AdvisorRecommendation";
    public string ResourceGroupDetails { get; set; } = "ResourceGroupDetails";
    public string SubscriptionDetails { get; set; } = "SubscriptionDetails";
}
