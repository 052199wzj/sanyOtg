using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSLESSpareParts
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// LES订单表ID
        /// </summary>
        public int LesOrderID { get; set; } = 0;
        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo { get; set; } = "";
        /// <summary>
        /// 零件编号
        /// </summary>
        public string PartID { get; set; } = "";
        /// <summary>
        /// 零件名称(件号)
        /// </summary>
        public string PartName { get; set; } = "";
        /// <summary>
        /// 零件描述
        /// </summary>
        public string PartDesc { get; set; } = "";

        /// <summary>
        /// 零件宽度(mm)
        /// </summary>
        public double PartWidth { get; set; } = 0.0;
        /// <summary>
        /// 零件长度(mm)
        /// </summary>
        public double PartLength { get; set; } = 0.0;
        /// <summary>
        /// 零件SN
        /// </summary>
        public string PartSN { get; set; } = "";

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string Technics { get; set; } = "";
        /// <summary>
        /// 订单计划号
        /// </summary>
        public string NestPlanID { get; set; } = "";

        /// <summary>
        /// 需求完成日期
        /// </summary>
        public DateTime RequireDoneDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 净重
        /// </summary>
        public double NetWeight { get; set; } = 0.0;

        /// <summary>
        /// 实际消耗钢板重量
        /// </summary>
        public double CutPlateWeight { get; set; } = 0.0;


        /// <summary>
        /// 需求工厂编号
        /// </summary>
        public string RequireFactoryID { get; set; } = "";


        /// <summary>
        /// 配件工装订单
        /// </summary>
        public string ORD_XLBG { get; set; } = "";

        /// <summary>
        /// 配件工装套料标识
        /// </summary>
        public string IND_ORD { get; set; } = "";


        /// <summary>
        /// 计划订单数量
        /// </summary>
        public double PlanAmount { get; set; } = 0.0;



        /// <summary>
        /// 本钢板套零件数
        /// </summary>
        public double Amount { get; set; } = 0.0;


        /// <summary>
        /// 工作中心
        /// </summary>
        public string ARBPL { get; set; } = "";



        /// <summary>
        /// 工序短文本
        /// </summary>
        public string ZNUM { get; set; } = "";



        /// <summary>
        /// 下料计划号
        /// </summary>
        public string ABLAD { get; set; } = "";

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
