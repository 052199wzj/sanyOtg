using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSSpareParts : BasePo
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; } = 0;
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; } = "";
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 计划编号
        /// </summary>
        public string PlanNumber { get; set; } = "";
        /// <summary>
        /// 长
        /// </summary>
        public double Length { get; set; } = 0.0;
        /// <summary>
        /// 宽
        /// </summary>
        public double Width { get; set; } = 0.0;
        /// <summary>
        /// 重量
        /// </summary>
        public double Weigth { get; set; } = 0.0;
        /// <summary>
        /// 件号
        /// </summary>
        public string PieceNo { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public int FQTY { get; set; } = 0;

        /// <summary>
        /// 零件类型
        /// </summary>
        public int PartType { get; set; } = 0;

        /// <summary>
        /// LES订单ID
        /// </summary>
        public int LesOrderID { get; set; } = 0;
    }
}
