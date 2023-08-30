using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FMCScheduling
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNo { get; set; } = "";
        /// <summary>
        /// 排班天数
        /// </summary>
        public int Days { get; set; } = 0;
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 激活状态
        /// </summary>
        public int Active { get; set; } = 0;
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateID { get; set; } = 0;
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; } = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
