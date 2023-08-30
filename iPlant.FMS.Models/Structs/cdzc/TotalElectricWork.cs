using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class TotalElectricWork
    {
        // <summary>
        /// 总功率
        /// </summary>
        public double TotalElectricWork_P { get; set; } = 0.0;

        /// <summary>
        /// 功率因数
        /// </summary>
        public String TotalElectricWork_COS { get; set; } = "";

        /// <summary>
        /// 电流
        /// </summary>
        public double TotalElectricWork_I { get; set; } = 0.0;

        /// <summary>
        /// 功
        /// </summary>
        public double TotalElectricWork_W { get; set; } = 0.0;

    }

}
