using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class AirSonVolume
    {
        // <summary>
        /// 介质温度
        /// </summary>
        public double MediumTemperature { get; set; } = 0.0;

        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double InstantaneousFlow { get; set; } = 0.0;

        /// <summary>
        /// 瞬时流速
        /// </summary>
        public double InstantaneousVelocity { get; set; } = 0.0;

        /// <summary>
        /// 传感器电压值
        /// </summary>
        public double SensorVoltageValue { get; set; } = 0.0;

        /// <summary>
        /// 累计流量百位以上
        /// </summary>
        public double CumulativeFlowAbove { get; set; } = 0.0;

        /// <summary>
        /// 累计流量百位以下
        /// </summary>
        public double CumulativeFlowBelow { get; set; } = 0.0;

        /// <summary>
        /// 累计流量
        /// </summary>
        public double CumulativeFlow { get; set; } = 0.0;

    }

}
