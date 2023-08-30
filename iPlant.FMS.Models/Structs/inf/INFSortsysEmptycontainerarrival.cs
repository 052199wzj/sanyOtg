using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
   public class INFSortsysEmptycontainerarrival
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 点位编号/托盘位置编号
        /// </summary>
        public String PalletPosition { get; set; } = "";

        /// <summary>
        /// 托盘编号
        /// </summary>
        public String PalletId { get; set; } = "";

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
