using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class WorkshopEnergyKB
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public int WorkShopID { get; set; } = 0;
        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; } = "";
        /// <summary>
        /// 总能耗 日
        /// </summary>
        public List<RealTimeEnergyDay> WorkshopEnergyListA { get; set; } = new List<RealTimeEnergyDay>();

        /// <summary>
        /// 总能耗  月
        /// </summary>
        public List<RealTimeEnergyMonth> WorkshopEnergyListB { get; set; } = new List<RealTimeEnergyMonth>();


        /// <summary>
        /// 总能耗  年
        /// </summary>
        public List<RealTimeEnergyYear> WorkshopEnergyListC { get; set; } = new List<RealTimeEnergyYear>();
    }
}
