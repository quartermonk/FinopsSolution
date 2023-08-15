using System.ComponentModel.DataAnnotations;

namespace FinopsSolution.API.Settings;

public class SubscriptionListSettings
{
    public static readonly string SettingName = "SubscriptionList";

    [Required, MinLength(1)]
    public Guid[] Ids { get; set; } = Array.Empty<Guid>();
}
