using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{

        internal class ResourceGroupDto: ITableEntity
        {
        public ResourceGroupDto(DateTime PartitionKey, string RowKey)
        {
                this.PartitionKey = PartitionKey.ToString("yyyy-MM-dd"); this.RowKey = RowKey;
        }
            public string id { get; set; }
            public string name { get; set; }
            public string tags { get; set; }
            public string location { get; set; }
            public string cost { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
        

    
}
