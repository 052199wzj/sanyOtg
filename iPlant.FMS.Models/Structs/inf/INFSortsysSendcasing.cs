using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
   public class INFSortsysSendcasing
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 产线代码（A）
        /// </summary>
        public String ProductionLline { get; set; } = "";

        /// <summary>
        /// 钢板分拣工位号。按照搬迁后布局，传A302。
        /// </summary>
        public String SortStationNo { get; set; } = "";

        /// <summary>
        /// 钢板切割工位号。按照搬迁后布局，传QG02。
        /// </summary>
        public String CutStationNo { get; set; } = "";

        /// <summary>
        /// (NC)套料图文件下载路径，HTTP协议下载地址
        /// </summary>
        public String CasingLocalUrl { get; set; } = "";

        /// <summary>
        /// 切割编号/任务编号
        /// </summary>
        public String MissionNo { get; set; } = "";
       
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
