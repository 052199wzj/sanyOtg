using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.MOMSysQuerySeqNo
{
    /// <summary>
    /// 中控系统订单工序（中控->MOM）
    /// </summary>
    public class MOMSysQuerySeqNo
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public int version { get; set; } = 1;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string taskId { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 接口类型
        /// </summary>
        public string taskType { get; set; } = "175";
        /// <summary>
        /// 消息内容
        /// </summary>
        public reported reported = new reported();
    }

    public class reported
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Wiporder { get; set; } = "";
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialNumber { get; set; } = "";
        /// <summary>
        /// 工作中心编号
        /// </summary>
        public string WorkCenter { get; set; } = "";
    }
}
