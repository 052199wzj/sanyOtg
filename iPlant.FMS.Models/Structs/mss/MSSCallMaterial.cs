using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 手动叫料(手动录入)
    /// </summary>
    public class MSSCallMaterial : BasePo
    {
        /// <summary>
        /// 钢板规格
        /// </summary>
        public int PlateID { get; set; } = 0;
        /// <summary>
        /// 钢板名称
        /// </summary>
        public string PlateName { get; set; } = "";
        /// <summary>
        /// 需求数量
        /// </summary>
        public int DemandNumber { get; set; } = 0;
        /// <summary>
        /// 料点ID
        /// </summary>
        public int MaterialPointID { get; set; } = 0;
        /// <summary>
        /// 料点名称
        /// </summary>
        public string MaterialPointName { get; set; } = "";
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime ArriveTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 到货数量
        /// </summary>
        public int ArriveNumber { get; set; } = 0;
        /// <summary>
        /// 确认人ID
        /// </summary>
        public int ConfirmID { get; set; } = 0;
        /// <summary>
        /// 确认人名称
        /// </summary>
        public string Confirmer { get; set; } = "";
        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime ConfirmTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// 叫料类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 预计开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = new DateTime(2000, 1, 1);
        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTime EndTime { get; set; } = new DateTime(2000, 1, 1);
    }
}
