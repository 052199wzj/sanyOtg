using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// by Demin 20221117
    /// 订单信息统计
    /// </summary>
    public class OMSOrderItemStatistics
    { 

        public Dictionary<string, DateTime> DicCutID_DatePlan { get; set; } = new Dictionary<string, DateTime>();

        /// <summary>
        /// key:切割编号
        /// value: 创建日期
        /// </summary>
        public Dictionary<string, int> DicCutID_NumPartInPlate { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// key:切割编号
        /// value:钢板中已向LES报工的零件数
        /// </summary>
        public Dictionary<string, int> DicCutID_NumPartToLES { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 每日钢板数
        /// </summary>
        public Dictionary<DateTime, int> DicDate_NumPlate { get; set; } = new Dictionary<DateTime, int>();
        /// <summary>
        /// 每日零件数
        /// </summary>
        public Dictionary<DateTime, int> DicDate_NumPartInPlate { get; set; } = new Dictionary<DateTime, int>();
        /// <summary>
        /// 每日报工零件数
        /// </summary>
        public Dictionary<DateTime, int> DicDate_NumPartToLES { get; set; } = new Dictionary<DateTime, int>();
        /// <summary>
        /// 钢板中零件总数
        /// </summary>
        public int NumPartTotal { get; set; } = 0;
        
        /// <summary>
        /// 钢板中已向LES报工的零件总数
        /// </summary>
        public int NumPartTotalToLES { get; set; } = 0;

        /// <summary>
        /// 钢板总数
        /// </summary>
        public int NumPlateTotal { get; set; } = 0;


    }
}
