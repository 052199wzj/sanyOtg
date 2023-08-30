using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class MCSInterfaceConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 接口类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Uri { get; set; } = "";
        /// <summary>
        /// 枚举标识
        /// </summary>
        public string EnumFlag { get; set; } = "";
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateID { get; set; } = 0;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public String Creator { get; set; } = "";
        /// <summary>
        /// 创建时刻
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 编辑人ID
        /// </summary>
        public int EditID { get; set; } = 0;
        /// <summary>
        /// 编辑人名称
        /// </summary>
        public string Editor { get; set; } = "";
        /// <summary>
        /// 编辑时刻
        /// </summary>
        public DateTime EditTime { get; set; } = DateTime.Now;
    }
}
