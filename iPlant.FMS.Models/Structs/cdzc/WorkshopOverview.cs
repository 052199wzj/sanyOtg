using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class WorkshopOverview
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public int WorkshopID { get; set; } = 0;
        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; } = "";
        /// <summary>
        /// 车间电表集合
        /// </summary>
        public List<ElectricityMeter> WorkshopElectricityList { get; set; } = new List<ElectricityMeter>();
    }
}
