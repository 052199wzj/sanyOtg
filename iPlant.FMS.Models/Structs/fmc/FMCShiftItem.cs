using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FMCShiftItem
    {
        public int ID { get; set; }
        public int ShiftID { get; set; }
        public String ShiftName { get; set; }
        public String Name { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public double Minutes { get; set; } = 0.0;
        public int Type { get; set; }
        public int Active { get; set; }
        public int CreateID { get; set; }
        public String Creator { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int EditID { get; set; }
        public String Editor { get; set; }
        public DateTime EditTime { get; set; } = DateTime.Now;
    }
}
