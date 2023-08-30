using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FPCRoutePart
    {
        public int ID { get; set; } = 0;

        public int RouteID { get; set; } = 0; // 工艺版本ID

        public int PartID { get; set; } = 0;
        public String PartName { get; set; } = "";

        public String Code { get; set; } = "";

        /**
		 * 主线层级
		 */
        public int OrderID { get; set; } = 0;

        public String Name { get; set; } = "";

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public int CreatorID { get; set; } = 0;

        public String Creator { get; set; } = "";

        public String RouteName { get; set; } = ""; // 工艺名称

        public int PrevPartID { get; set; } = 0;// 上工段
        public string PrevPartName { get; set; } = "";

        /**
		 * 标准工时
		 */
        public Double StandardPeriod { get; set; } = 0.0;

        /**
		 * 调整工时 新增字段
		 */
        public Double ActualPeriod { get; set; } = 0.0;

        /**
		 * key PartID Value : Condition value: 0 主线也是下个模块
		 */
        public Dictionary<String, String> NextPartIDMap { get; set; } = new Dictionary<string, string>();// 上工段路线ID
        /// <summary>
        /// 下工位名称
        /// </summary>
        public string NextPartNames { get; set; } = "";

        /**
		 * 转序控制
		 */
        public int ChangeControl { get; set; } = 0;

        /// <summary>
        /// 辅助属性
        /// </summary>
        public int OrderNumber { get; set; } = 0;
    }
}
