using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSDXFAnalysisParts
    {

        /// <summary>
        /// 主单据ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// DXF解析主表ID
        /// </summary>
        public int DxfAnalysisID { get; set; }
        /// <summary>
        /// 工件在图纸上的编号，例如：1
        /// </summary>
        public int PartNo { get; set; }

        /// <summary>
        /// 计划编号，即订单号
        /// </summary>
        public string PlanNo { get; set; }

        /// <summary>
        /// 工件名称（件号：如果只有一个@，工件名称为空；如果有两个@，取第一个作为工件名称）
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 工件型号（件号：如果只有一个@，就是工件型号；如果有两个@，取第二个作为工件型号）
        /// </summary>
        public string PartModel { get; set; }

        /// <summary>
        /// 工件数量
        /// </summary>
        public int PartNum { get; set; }

        /// <summary>
        /// 工件长度（mm）
        /// </summary>
        public double PartLenth { get; set; }

        /// <summary>
        /// 工件宽度(mm)
        /// </summary>
        public double PartWidth { get; set; }

        /// <summary>
        /// 工件尺寸类型。-1：极小件，1：小件，2：中件，3：大件，4：超大件。
        /// </summary>
        public int SizeType { get; set; }

        /// <summary>
        /// 工件重量（单位kg）。图纸上没有，可能通过excel导入。
        /// </summary>
        public double PartWeight { get; set; }

        /// <summary>
        /// 工艺路线。图纸上没有，可能通过excel导入。
        /// </summary>
        public string ProcessRoute { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
