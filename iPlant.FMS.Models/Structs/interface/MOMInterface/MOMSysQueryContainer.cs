using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 中控系统查询点位是否有料框（中控->MOM）
    /// </summary>
    public class MOMSysQueryContainer
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
        public string taskType { get; set; } = "47";
        /// <summary>
        /// 消息内容
        /// </summary>
        public reported reported = new reported();
    }

    public class reported
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string reqSys { get; set; } = "ZK";
        /// <summary>
        /// 工厂编号
        /// </summary>
        public string Facility { get; set; } = "";
        /// <summary>
        /// 料框编码
        /// </summary>
        public string palletNo { get; set; } = "";
        /// <summary>
        /// 料点编码
        /// </summary>
        public string pointNo { get; set; } = "";
    }
}
