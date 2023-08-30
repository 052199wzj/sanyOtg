using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 料点管理
    /// </summary>
    public class MSSMaterialPoint : BasePo
    {
        /// <summary>
        /// 产线名称
        /// </summary>
        public String LineName { get; set; } = "";
        /// <summary>
        /// 产线ID
        /// </summary>
        public int LineID { get; set; } = 0;
        /// <summary>
        /// 工位名称
        /// </summary>
        public String AssetName { get; set; } = "";
        /// <summary>
        /// 工位编号
        /// </summary>
        public int AssetID { get; set; } = 0;
        /// <summary>
        /// 工位料点
        /// </summary>
        public String StationPoint { get; set; } = "";
        /// <summary>

        /// <summary>
        /// 送达料点
        /// </summary>
        public String DeliveryPoint { get; set; } = "";

        /// <summary>
        ///物料编码
        /// </summary>
        public string MaterialNo { get; set; } = "";
        /// <summary>
        
        /// <summary>
        /// 物流方案
        /// </summary>
        public int PlanNo { get; set; } = 0;

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 料框ID
        /// </summary>
        public int FrameID { get; set; } = 0;
        /// <summary>
        /// 料框状态
        /// </summary>
        public int FrameStatus { get; set; } = 0;
        /// <summary>
        /// 操作代码 
        /// 2：请求空料框
        /// 3：移走满料框
        /// </summary>
        public int ReqID { get; set; } = 0;
    }
}
