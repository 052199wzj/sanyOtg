using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 切割过程
    /// </summary>
    public class INFLesCuttingProcess
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 切割编号
        /// </summary>
        public String NestId { get; set; } = "";

        /// <summary>
        /// 切割开始：true  其它：false
        /// </summary>
        public bool Start { get; set; } = false;

        /// <summary>
        /// 切割完成：true  其它：false
        /// </summary>
        public bool End { get; set; } = false;

        /// <summary>
        /// 默认：0   发送成功：1   发送失败：2
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 发送失败原因
        /// </summary>
        public String ErroMsg { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = new DateTime(2000, 1, 1);
    }
}
