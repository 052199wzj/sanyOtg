using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class InterfaceLogInfo
    {
        /// <summary>
        /// 日志内容
        /// </summary>
        public string TextContent { get; set; } = "";
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; } = "";
        /// <summary>
        /// 系统类型
        /// </summary>
        public string SystemType { get; set; } = "";
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; } = "";
    }
}
