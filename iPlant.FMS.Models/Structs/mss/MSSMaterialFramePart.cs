using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 料框零件
    /// </summary>
    public class MSSMaterialFramePart : BasePo
    {
        /// <summary>
        /// 料框名称
        /// </summary>
        public string MaterialFrameName { get; set; } = "";

        /// <summary>
        /// 料框表ID
        /// </summary>
        public int MaterialFrameID { get; set; } = 0;

        /// <summary>
        /// 料框号（来自表mss_materialframe）
        /// </summary>
        public String FrameCode { get; set; } = "";

        /// <summary>
        /// 上下料反馈接口表ID
        /// </summary>
        public int LesUpDownMaterialID { get; set; } = 0;

        /// <summary>
        /// 切割编号
        /// </summary>
        public String NestId { get; set; } = "";

        /// <summary>
        /// 订单号
        /// </summary>
        public String OrderId { get; set; } = "";

        /// <summary>
        /// 零件编号
        /// </summary>
        public String PartNo { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public int Quality { get; set; } = 0;

        /// <summary>
        /// 码盘开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 码盘结束时间
        /// </summary>
        public DateTime EndTime { get; set; } = new DateTime(2000, 1, 1);


        // 来自表：inf_les_updownmaterial

        /// <summary>
        /// 点位编号
        /// </summary>
        public String StationCode { get; set; } = "";

        /// <summary>
        /// 默认：0   发送成功：1   发送失败：2
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 发送失败原因
        /// </summary>
        public String ErroMsg { get; set; } = "";

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = new DateTime(2000, 1, 1);

    }
}
