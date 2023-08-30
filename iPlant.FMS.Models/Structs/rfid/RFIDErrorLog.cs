using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class RFIDErrorLog
    {
        #region 基础配置
        public int ID { get; set; } = 0;

        /// <summary>
        /// 工位编码
        /// </summary>
        public string StationCode { get; set; } = "";

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; } = "";

        /// <summary>
        /// 错误日志类型
        /// </summary>
        public int LogTypeID { get; set; } = 0;

        /// <summary>
        /// 交互对象
        /// </summary>
        public int InteractiveObjectID { get; set; } = 0;
             
        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName { get; set; } = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        public string LogInformation { get; set; } = "";

        /// <summary>
        ////用户名称
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 发生时刻
        /// </summary>
        public DateTime UpdateTime { get; set; } = new DateTime(2000, 1, 1);
        #endregion
     
    }

}
