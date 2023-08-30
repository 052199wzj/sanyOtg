using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class ElectricityMeter
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double DeviceID { get; set; } = 0.0;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; } = "";
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceNo { get; set; } = "";

        /// <summary>
        /// 正向有功总电能
        /// </summary>
        public double PositiveActiveElectricEnergy { get; set; } = 0.0;

        /// <summary>
        /// 正向无功总电能

        /// </summary>
        public double PositiveNoActiveElectricEnergy { get; set; } = 0.0;

        /// <summary>
        /// 反向有功电能
        /// </summary>
        public double ReverseActiveElectricEnergy { get; set; } = 0.0;

        /// <summary>
        /// 反向无功总电能
        /// </summary>
        public double ReverseNoActiveElectricEnergy { get; set; } = 0.0;

        /// <summary>
        /// 总有功功率
        /// </summary>
        public double UsefulPower { get; set; } = 0.0;

        /// <summary>
        /// 总无功功率
        /// </summary>
        public double NoUsefulPower { get; set; } = 0.0;


        /// <summary>
        /// 总视在功率
        /// </summary>
        public double ApparentPower { get; set; } = 0.0;

        /// <summary>
        /// 功率因素
        /// </summary>
        public String PowerFactor { get; set; } = "";

    }
}
