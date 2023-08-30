using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FPCFlowPart
    {
        public String id { get; set; } = "";
        public String name { get; set; } = "";
        public String left { get; set; } = "";
        public String top { get; set; } = "";
        public String showclass { get; set; } = "";

        public int row { get; set; } = 0;
        public int col { get; set; } = 0;
    }
}
