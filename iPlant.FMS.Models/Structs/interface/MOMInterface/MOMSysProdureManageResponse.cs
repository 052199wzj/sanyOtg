using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.MOMSysProdureManageResponse
{
    /// <summary>
    /// 中控系统获取工序配送返回信息（中控->MOM）
    /// </summary>
    public class MOMSysProdureManageResponse
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public int version { get; set; } = 1;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string taskId { get; set; } = "";

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
        public string returnData ="";
    }


}
