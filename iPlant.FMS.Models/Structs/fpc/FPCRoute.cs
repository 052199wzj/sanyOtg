using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FPCRoute
    {
        public int ID { get; set; } = 0;
        public string RouteName { get; set; } = "";
        public string Code { get; set; } = "";
        public int Active { get; set; } = 0;
        public int IsStandard { get; set; } = 0;
        public int CreateID { get; set; } = 0;
        public String Creator { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int EditID { get; set; } = 0;
        public string Editor { get; set; } = "";
        public DateTime EditTime { get; set; } = DateTime.Now;
        public int SonNumber { get; set; } = 0;
    }
}
