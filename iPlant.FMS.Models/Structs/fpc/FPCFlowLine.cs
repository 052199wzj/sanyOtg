using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FPCFlowLine
    {
        public FPCFlowPoint anode { get; set; } = new FPCFlowPoint();
        public FPCFlowPoint bnode { get; set; } = new FPCFlowPoint();
    }
}
