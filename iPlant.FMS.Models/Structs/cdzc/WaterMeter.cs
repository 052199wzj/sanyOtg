using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class WaterMeter
    {
        /// <summary>
        /// 瞬时用量
        /// </summary>
        public double InstantaneousWaterAmount { get; set; } =0.0;

        /// <summary>
        /// 总用水量
        /// </summary>
        public double TotalWaterAmount { get; set; } = 0.0;


    }
}
