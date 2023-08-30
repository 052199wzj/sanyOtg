using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 上传流水记录
    /// </summary>
    public class OMSUploadRecord : BasePo
    {
        /// <summary>
        /// NC文件地址
        /// </summary>
        public string NCFileUri { get; set; } = "";
        /// <summary>
        /// NC文件名称
        /// </summary>
        public string NCFileName { get; set; } = "";
        /// <summary>
        /// NC文件最后编辑时间
        /// </summary>
        public DateTime NCFileTime { get; set; } = DateTime.Now;
        /// <summary>
        /// DXF文件地址
        /// </summary>
        public string DXFFileUri { get; set; } = "";
        /// <summary>
        /// DXF文件名称
        /// </summary>
        public string DXFFileName { get; set; } = "";
        /// <summary>
        /// DXF文件最后编辑时间
        /// </summary>
        public DateTime DXFFileTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 解析状态
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// 创建方式
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 解析标记: 0:未解析 1：成功 2：失败
        /// </summary>
        public int ParseFlag { get; set; } = 0;
        /// <summary>
        /// 解析失败原因
        /// </summary>
        public string FailReason { get; set; } = "";
    }
}
