using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class TotalAirVolume
    {
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double InstantaneousFlow { get; set; } =0.0;

        /// <summary>
        /// 累计流量
        /// </summary>
        public double CumulativeFlow { get; set; } = 0.0;


    }
}
