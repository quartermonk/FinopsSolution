using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Dto
{
        internal class ResourceGroupDto
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string location { get; set; }
            public string tags { get; set; }
            public RProperties properties { get; set; }
        }
        public class RProperties
        {
            public string provisioningState { get; set; }
        }

        public class Tags
        {
            public string Sandbox { get; set; }
        }

    
}
