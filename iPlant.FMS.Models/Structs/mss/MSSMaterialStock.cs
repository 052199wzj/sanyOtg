using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class MSSMaterialStock : BasePo
    {
        /// <summary>
        /// 料点ID
        /// </summary>
        public int MaterialPointID { get; set; } = 0;
        /// <summary>
        /// 料点名称
        /// </summary>
        public string MaterialPointName { get; set; } = "";
        /// <summary>
        /// 钢板ID
        /// </summary>
        public int PlateID { get; set; } = 0;
        /// <summary>
        /// 钢板名称
        /// </summary>
        public string PlateName { get; set; } = "";
        /// <summary>
        /// 顺序
        /// </summary>
        public int OrderID { get; set; } = 0;
    }
}
