using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{

        internal class ResourceGroupDto: TableEntity
        {
        public ResourceGroupDto(string name,string location)
        {
                PartitionKey = name;RowKey = location;
        }
            public string id { get; set; }
            public string name { get; set; }
            public string tags { get; set; }
            public string location { get; set; }
            public string cost { get; set; }
        }
        

    
}
