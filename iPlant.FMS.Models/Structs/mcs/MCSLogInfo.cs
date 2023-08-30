using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class MCSLogInfo
    {
        /**
		 * 主键
		 */
        public int ID { get; set; } = 0;
        /**
		 * 局段
		 */
        public String CustomerName { get; set; } = "";
        /**
		 * 修程
		 */
        public String LineName { get; set; } = "";
        /**
		 * 车型
		 */
        public String ProductNo { get; set; } = "";
        /**
		 * 车号
		 */
        public String PartNo { get; set; } = "";
        /**
		 * 版本信息
		 */
        public String VersionNo { get; set; } = "";
        /**
		 * 文件名称
		 */
        public String FileName { get; set; } = "";
        /**
		 * 文件路径
		 */
        public String FilePath { get; set; } = "";
        /**
		 * 文件类型
		 */
        public String FileType { get; set; } = "";
        /**
		 * 创建时间
		 */
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /**
		 * 创建时间文本
		 */
        public String CreateTimeStr { get; set; } = "";
        public int BOPID { get; set; } = 0;
        public string BOMID { get; set; } = "";
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemType { get; set; } = "";


        /// <summary>
        /// 过程名称
        /// </summary>
        public string ProcessName { get; set; } = "";


        /// <summary>
        /// 步骤
        /// </summary>
        public int StepNo { get; set; } =0;


        /// <summary>
        /// 主信息
        /// </summary>
        public string Info { get; set; } = "";
    }
}
