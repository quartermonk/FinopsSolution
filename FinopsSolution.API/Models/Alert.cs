namespace FinopsSolution.API.Models;

public class Alert
{
    public AlertProperties Properties { get; set; } = null!;
    public string ETag { get; set; } = null!;
}

public class ActualGreaterThan80Percent
{
    public bool Enabled { get; set; }
    public string @operator { get; set; } = null!;
    public int Threshold { get; set; }
    public string Locale { get; set; } = null!;
    public List<string> ContactEmails { get; set; } = null!;
    public string ThresholdType { get; set; } = null!;
}

public class Notifications
{
    public ActualGreaterThan80Percent Actual_GreaterThan_80_Percent { get; set; } = null!;
}

public class AlertProperties
{
    public string Category { get; set; } = null!;
    public double Amount { get; set; }
    public string TimeGrain { get; set; } = null!;
    public TimePeriod TimePeriod { get; set; } = null!;
    public Notifications Notifications { get; set; } = null!;
}

public class TimePeriod
{
    public string StartDate { get; set; } = null!;
    public string EndDate { get; set; } = null!;
}
