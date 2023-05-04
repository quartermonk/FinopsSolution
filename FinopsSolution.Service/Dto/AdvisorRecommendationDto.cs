using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
    internal class AdvisorRecommendationDto :TableEntity
    {
        public AdvisorRecommendationDto(string category, string recommendationName)
        {
            PartitionKey = category; RowKey = recommendationName;
        }
        public string Id { get; set; }
        public string ImpactedResource { get; set; }
    }
}
