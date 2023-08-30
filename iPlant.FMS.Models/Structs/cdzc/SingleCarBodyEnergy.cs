using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class SingleCarBodyEnergy
    {
        /// <summary>
        /// 单台车能耗-日
        /// </summary>
        public double SingleCarBodyEnergyDay { get; set; } = 0.0;

        /// <summary>
        /// 单台车能耗-月
        /// </summary>
        public double SingleCarBodyEnergyMonth { get; set; } = 0.0;

        /// <summary>
        /// 单台车能耗-年
        /// </summary>
        /// 
        public double SingleCarBodyEnergyYear { get; set; } = 0.0;

        /// <summary>
        /// 单台车能耗-年度指标
        /// </summary>
        public double SingleCarBodyEnergyTarget { get; set; } = 0.0;
      
    }
}
