namespace FinopsSolution.API.Settings;

public class CostManagementWorkerSettings
{
    public static readonly string SettingName = "WorkerJob";

    public int JobIntervalInMinutes { get; set; } = 60;
}
