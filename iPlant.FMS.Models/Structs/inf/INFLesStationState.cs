using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 站点状态反馈
    /// </summary>
    public class INFLesStationState
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 托盘编号
        /// </summary>
        public String PalletCode { get; set; } = "";

        /// <summary>
        /// 站点编号
        /// </summary>
        public String StationCode { get; set; } = "";

        /// <summary>
        /// 点位状态 1：无框  3：占用  4：空框   6：满框
        /// </summary>
        public SByte StationStatus { get; set; } = 0;

        /// <summary>
        /// 默认：0   发送成功：1   发送失败：2
        /// </summary>

        public SByte Status { get; set; } = 0;

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
