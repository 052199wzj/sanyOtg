using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class MCSOperationLog : BasePo
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; } = 0;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; } = "";
        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 操作类型文本
        /// </summary>
        public string TypeText { get; set; } = "";
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; } = "";
    }
}
