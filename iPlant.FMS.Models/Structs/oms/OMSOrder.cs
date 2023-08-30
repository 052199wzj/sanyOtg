using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSOrder
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// NC文件地址
        /// </summary>
        public string NCFileUri { get; set; } = "";
        /// <summary>
        /// DXF文件地址
        /// </summary>
        public string DXFFileUri { get; set; } = "";
        /// <summary>
        /// 切割编号
        /// </summary>
        public string CuttingNumber { get; set; } = "";
        /// <summary>
        /// 套料编号
        /// </summary>
        public string NestingNumber { get; set; } = "";
        /// <summary>
        /// 切割类型  1火焰  2平面 3坡口
        /// </summary>
        public int CutType { get; set; } = 0;
        /// <summary>
        /// 需求完成日期
        /// </summary>
        public DateTime DemandFinishDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 切割次数
        /// </summary>
        public int CutTimes { get; set; } = 0;
        /// <summary>
        /// 套料日期
        /// </summary>
        public DateTime NestDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 钢板物料编码(料点ID)
        /// </summary>
        public string PlateMaterialNo { get; set; } = "";
        /// <summary>
        /// 料点名称(库位名称)
        /// </summary>
        public string MaterialPointName { get; set; } = "";
        /// <summary>
        /// 结构件ID
        /// </summary>
        public int StructuralPartID { get; set; } = 0;
        /// <summary>
        /// 结构件名称
        /// </summary>
        public string StructuralPartName { get; set; } = "";
        /// <summary>
        /// 长
        /// </summary>
        public double Length { get; set; } = 0.0;
        /// <summary>
        /// 宽
        /// </summary>
        public double Width { get; set; } = 0.0;
        /// <summary>
        /// 高
        /// </summary>
        public double Height { get; set; } = 0.0;
        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; } = 0.0;
        /// <summary>
        /// 材质
        /// </summary>
        public string Texture { get; set; } = "";
        /// <summary>
        /// 厚度(板)
        /// </summary>
        public double Thickness { get; set; } = 0.0;
        /// <summary>
        /// 机动时间(保留)
        /// </summary>
        public DateTime ManeuverTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 机动时间
        /// </summary>
        public double ManeuverTimes { get; set; } = 0;

        /// <summary>
        /// 钢板
        /// </summary>
        public String Plate { get; set; } = "";
        /// <summary>
        /// 总切割长度
        /// </summary>
        public double CutLength { get; set; } = 0.0;

        //工艺参数配方
        /// <summary>
        /// 材料
        /// </summary>
        public string Material { get; set; } = "";
        /// <summary>
        /// 工艺参数厚度
        /// </summary>
        public string  TechThickness { get; set; } = "";
        /// <summary>
        /// 气体
        /// </summary>
        public string Gas { get; set; } = "";

        /// <summary>
        /// 切割速度
        /// </summary>
        public double CutSpeed { get; set; } = 0.0;

        /// <summary>
        /// 割嘴
        /// </summary>
        public string CuttingMouth { get; set; } = "";

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

        /// <summary>
        /// 是否生成工单：0未生成，1已生成
        /// </summary>
        public int Flag { get; set; } = 0;

        /// <summary>
        /// 工单完成数
        /// </summary>
        public int FinishFQTY { get; set; } = 0;
        /// <summary>
        /// 订单实际完成时间
        /// </summary>
        public DateTime FinishTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 订单类型  1中控订单  2LES订单
        /// </summary>
        public int OrderType { get; set; } = 0;

        /// <summary>
        /// 利用率
        /// </summary>
        public double Utilization { get; set; } = 0.0;

        /// <summary>
        /// 原材料类型
        /// </summary>
        public string StructuralPartMaterialTypeNo { get; set; } = "";

        /// <summary>
        /// 原材料物料编码
        /// </summary>
        public string StructuralPartMaterialNo { get; set; } = "";

        /// <summary>
        /// LES计划订单ID
        /// </summary>
        public int LesOrderID { get; set; } = 0;

        /// <summary>
        /// 材质编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 订单单显示控制参数
        /// </summary>
        public int Displayed { get; set; } = 1;

    }
}
