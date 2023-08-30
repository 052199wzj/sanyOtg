using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class MSSMaterialPointStock
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public int FQTY { get; set; } = 0;
    }
}
