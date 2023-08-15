using System.ComponentModel.DataAnnotations;

namespace FinopsSolution.API.Settings;

public class AzureCredentialSettings
{
    public static readonly string SettingName = "AzureCredential";

    /// <summary>
    /// Tenant ID Azure AD
    /// </summary>
    [Required]
    public string TenantId { get; set; } = null!;

    /// <summary>
    /// Application ID in Azure AD
    /// </summary>
    [Required]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// App's secret Value (not Secret ID)
    /// </summary>
    [Required]
    public string ClientSecret { get; set; } = null!;
}
