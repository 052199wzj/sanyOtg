using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 数据清理
    /// </summary>
    public class INFDataManage
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 数据类型
        /// </summary>
        public String DataType { get; set; } = "";

        /// <summary>
        /// 保存时间 月数
        /// </summary>
        public int SaveTime { get; set; } = 0;


        /// <summary>
        /// 默认：0 不需清理    1  需清理
        /// </summary>

        public int Status { get; set; } = 0;


        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 上次清理时间
        /// </summary>
        public DateTime CleanTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 数据大小
        /// </summary>
        public String DataSize { get; set; } = "";

    }
}
