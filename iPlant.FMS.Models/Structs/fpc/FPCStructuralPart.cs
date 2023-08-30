using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class FPCStructuralPart
    {
        public int ID { get; set; } = 0;
        public String Name { get; set; } = "";
        public String Code { get; set; } = "";
        public double Length { get; set; } = 0.0;
        public double Width { get; set; } = 0.0;
        public double Height { get; set; } = 0.0;
        public double Weight { get; set; } = 0.0;
        public string Remark { get; set; } = "";
        public int Active { get; set; } = 0;
        public int CreateID { get; set; } = 0;
        public String Creator { get; set; } = "";
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int EditID { get; set; } = 0;
        public String Editor { get; set; } = "";
        public DateTime EditTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialNo { get; set; } = "";

        /// <summary>
        /// 物料物料类型
        /// </summary>
        public string MaterialTypeNo { get; set; } = "";
    }
}
