using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class AlertDto
    {
    }
    public class ActualGreaterThan80Percent
    {
        public bool enabled { get; set; }
        public string @operator { get; set; }
        public int threshold { get; set; }
        public string locale { get; set; }
        public List<string> contactEmails { get; set; }
        public List<string> contactRoles { get; set; }
        public List<string> contactGroups { get; set; }
        public string thresholdType { get; set; }
    }

    public class Notifications
    {
        public ActualGreaterThan80Percent Actual_GreaterThan_80_Percent { get; set; }
    }

    public class Properties
    {
        public string category { get; set; }
        public double amount { get; set; }
        public string timeGrain { get; set; }
        public TimePeriod timePeriod { get; set; }
        public Notifications notifications { get; set; }
    }

    public class Root
    {
        public string eTag { get; set; }
        public Properties properties { get; set; }
    }

    public class TimePeriod
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }


}
