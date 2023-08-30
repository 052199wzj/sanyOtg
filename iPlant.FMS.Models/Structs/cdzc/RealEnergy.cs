using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class RealEnergy
    {
        // <summary>
        /// 实时能耗数值
        /// </summary>
        public double CenterTypeNum { get; set; } = 0.0;

        /// <summary>
        /// 实时能耗 水 电 天然气 氧气 混合气 压缩气
        /// </summary>
        public string CenterTypeName { get; set; } = "";

        /// <summary>
        /// 实时能耗单位
        /// </summary>
        public string CenterTypePower { get; set; } = "";
    }

}
