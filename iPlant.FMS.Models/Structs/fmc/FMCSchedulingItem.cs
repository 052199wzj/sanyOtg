using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FMCSchedulingItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 排班ID
        /// </summary>
        public int FMCSchedulingID { get; set; } = 0;
        /// <summary>
        /// 排班流水号
        /// </summary>
        public string SerialNo { get; set; } = "";
        /// <summary>
        /// 工位
        /// </summary>
        public int StationID { get; set; } = 0;
        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; } = "";
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 排班人员ID
        /// </summary>
        public string PersonID { get; set; } = "";
        /// <summary>
        /// 排班人员名称
        /// </summary>
        public string PersonName { get; set; } = "";
        public int CreateID { get; set; } = 0;
        public string Creator { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int EditID { get; set; } = 0;
        public string Editor { get; set; } = "";
        public DateTime EditTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 班次
        /// </summary>
        public int ShiftID { get; set; } = 0;
        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { get; set; } = "";

    }
}
