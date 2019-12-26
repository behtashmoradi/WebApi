using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Configurations
{
    public class CampsApiConfiguration
    {
        public bool RegisterationIsClosed { get; set; }
        public Detail DetailInfo { get; set; }
        public class Detail
        {
            public string Key { get; set; }
        }
    }
}
