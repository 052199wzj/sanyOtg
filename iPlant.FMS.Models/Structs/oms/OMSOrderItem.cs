using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSOrderItem : OMSOrder
    {
        public int OrderID { get; set; } = 0;
        public string OrderNo { get; set; } = "";
        //1已创建、2生产中、3已完成 4已激活
        public int Status { get; set; } = 0;
        /// <summary>
        /// 订单优先级
        /// </summary>
        public int OrderNum { get; set; } = 0;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 激活禁用
        /// </summary>
        public int Active { get; set; } = 0;
        //解析结果：0 解析中、1解析失败、2磁吸配置失败、3解析成功、4'未解析'
        /// <summary>
        /// DXF状态
        /// </summary>
        public int DXFAnalysisStatus { get; set; } = 0;

        /// <summary>
        /// DXF解析失败原因
        /// </summary>
        public string DXFAnalysisFailReason { get; set; } = "";

        /// <summary>
        /// DXF本地文件路径
        /// </summary>
        public string DXFLocalUrl { get; set; } = "";

        /// <summary>
        /// NC本地文件路径
        /// </summary>
        public string NCLocalUrl { get; set; } = "";


        //解析结果 默认：0   发送成功：1   发送失败：2
        /// <summary>
        /// DXF下发解析状态
        /// </summary>
        public int DXFIssuedStatus { get; set; } = 0;

        /// <summary>
        /// DXF下发解析失败原因
        /// </summary>
        public string DXFIssuedFailReason { get; set; } = "";

        /// <summary>
        /// 工单显示控制参数 1为显示 2为不显示
        /// </summary>
        public new int Displayed { get; set; } = 1;

        /// <summary>
        /// 产品编号
        /// </summary>
        public String ProductNo { get; set; } = "";
    }
}
