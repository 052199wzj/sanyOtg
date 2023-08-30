using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.MOMSysQuerySeqNoResponse
{
    /// <summary>
    /// 中控系统订单工序（中控->MOM）
    /// </summary>
    public class MOMSysQuerySeqNoResponse
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public int version { get; set; } = 1;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string taskId { get; set; } ="";

        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; } = "";
        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = "";

        /// <summary>
        /// 消息内容
        /// </summary>
        public returnData returnData { get; set; } = new returnData();
    }

    public class returnData
    {
        /// <summary>
        /// 工序号
        /// </summary>
        public string OprSequenceNo { get; set; } = "";
        /// <summary>
        /// 工序名称
        /// </summary>
        public string OprSequenceName { get; set; } = "";
    }
}
