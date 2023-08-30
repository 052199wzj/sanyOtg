using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.DAQContainerStatus
{
    /// <summary>
    /// 中控系统接收料框状态信息接口(数采→中控)
    /// </summary>
    public class DAQContainerStatus
    {
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public string request_ID { get; set; } = "";
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime request_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 请求报文
        /// </summary>
        public request_data request_data = new request_data();
    }

    public class request_data
    {
        /// <summary>
        /// 点位编号
        /// </summary>
        public int UnloadPositionNo { get; set; } = 0;
        /// <summary>
        /// 料框状态
        /// </summary>
        public int FrameStatus { get; set; } = 0;

    }
}
