using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class DAQResponse
    {
        /// <summary>
        /// 应答时间戳
        /// </summary>
        public string response_ID { get; set; } ="";
        /// <summary>
        /// 应答时间戳
        /// </summary>
        public DateTime response_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 1000：正常，其它：异常码
        /// </summary>
        public string response_result { get; set; } = "1000";
        /// <summary>
        /// 应答数据,例如异常描述信息
        /// </summary>
        public string response_data { get; set; } = "";
    }
}
