using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class AdvisorRecommendationDto
    {
        public string id { get; set; }
        public string category { get; set; }
        public string recommendationName { get; set; }
        public string ImpactedResource { get; set; }
    }
}
