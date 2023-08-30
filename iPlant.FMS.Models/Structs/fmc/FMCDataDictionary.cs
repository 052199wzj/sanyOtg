using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class FMCDataDictionary : BasePo
    {
        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderID { get; set; } = 0;
        /// <summary>
        /// 是否默认
        /// </summary>
        public int IsDefault { get; set; } = 0;
        /// <summary>
        /// 是否默认文本
        /// </summary>
        public string IsDefaultText { get; set; } = "";
        /// <summary>
        /// 类型 1：材质 2：厚度 3：气体 4：喷嘴
        /// </summary>
        public int Type { get; set; } = 0;
    }
}
