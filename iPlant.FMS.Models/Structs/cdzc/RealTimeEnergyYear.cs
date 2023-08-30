using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class RealTimeEnergyYear
    {
        /// 能源类型
        /// </summary>
        public string TypeName { get; set; } = "";
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitDuration { get; set; } = "";

        /// <summary>
        /// 实时报表 第1月
        /// </summary>
        public double Year1 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第2月
        /// </summary>
        public double Year2 { get; set; } = 0.0;



        /// <summary>
        /// 实时报表 第3月
        /// </summary>
        public double Year3 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第4月
        /// </summary>
        public double Year4 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第5月
        /// </summary>
        public double Year5 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第6月
        /// </summary>
        public double Year6 { get; set; } = 0.0;

        /// <summary>
        /// 实时报表 第7月
        /// </summary>
        public double Year7 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第8月
        /// </summary>
        public double Year8 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第9月
        /// </summary>
        public double Year9 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第10月
        /// </summary>
        public double Year10 { get; set; } = 0.0;


        /// <summary>
        /// 实时报表 第11月
        /// </summary>
        public double Year11 { get; set; } = 0.0;

        /// <summary>
        /// 实时报表 第12月
        /// </summary>
        public double Year12 { get; set; } = 0.0;
    }
}
