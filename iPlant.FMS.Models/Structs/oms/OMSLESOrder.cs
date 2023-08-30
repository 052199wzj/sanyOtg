using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSLESOrder
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// 切割编号
        /// </summary>
        public string NestID { get; set; } = "";

        /// <summary>
        /// 套料编号
        /// </summary>
        public string GroupID { get; set; } = "";

        /// <summary>
        /// 利用率
        /// </summary>
        public double Rate { get; set; } = 0;

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string FactoryID { get; set; } = "";

        /// <summary>
        /// 钢板物料编码
        /// </summary>
        public string ExMaterielID { get; set; } = "";


        /// <summary>
        /// 钢板物料名称
        /// </summary>
        public string MaterielName { get; set; } = "";


        /// <summary>
        /// 钢板物料类型编号
        /// </summary>
        public string MaterielTypeID { get; set; } = "";


        /// <summary>
        /// 材料
        /// </summary>
        public string Texture { get; set; } = "";

        /// <summary>
        /// 板厚(mm)材质厚度
        /// </summary>
        public string TechThickness { get; set; } = "";

        /// <summary>
        /// 板厚(mm)
        /// </summary>
        public double Thickness { get; set; } = 0;

        /// <summary>
        /// 宽度(mm)
        /// </summary>
        public double Width { get; set; } = 0;


        /// <summary>
        /// 长度(mm)
        /// </summary>
        public double Length { get; set; } = 0;


        /// <summary>
        ///原材料重量
        /// </summary>
        public double MTWeight { get; set; } = 0;

        /// <summary>
        /// 套料日期
        /// </summary>
        public DateTime NestDate { get; set; } = DateTime.Now;


        /// <summary>
        ///归档号
        /// </summary>
        public string BookSheet { get; set; } = "";


        /// <summary>
        ///切割类型
        /// </summary>
        public int OptionID { get; set; } = 0;

        /// <summary>
        /// 需求完成日期
        /// </summary>
        public DateTime RequireDoneDate { get; set; } = DateTime.Now;


        /// <summary>
        ///套料任务编号
        /// </summary>
        public string NestTaskID { get; set; } = "";


        /// <summary>
        ///总切割长度(mm)
        /// </summary>
        public double CutLength { get; set; } = 0;


        /// <summary>
        ///机动时间(min)
        /// </summary>
        public double WorkTime { get; set; } = 0;



        /// <summary>
        ///加密字符串需要加密锁解密
        /// </summary>
        public string JsonMap { get; set; } = "";


        /// <summary>
        ///加密字符串需要加密锁解密 错误码
        /// </summary>
        public string JsonMapError { get; set; } = "";



        /// <summary>
        ///状态 I 新增、 D 删除
        /// </summary>
        public string State { get; set; } = "";


        /// <summary>
        ///NC文件名称
        /// </summary>
        public string NCUrlName { get; set; } = "";

        /// <summary>
        ///NC文件地址
        /// </summary>
        public string NCUrl { get; set; } = "";

        /// <summary>
        ///NC文件地址 本地
        /// </summary>
        public string NCLocalUrl { get; set; } = "";

        /// <summary>
        ///DXF文件地址
        /// </summary>
        public string DXFUrl { get; set; } = "";

        /// <summary>
        ///DXF文件地址  本地
        /// </summary>
        public string DXFLocalUrl { get; set; } = "";

        /// <summary>
        ///DXF文件名称
        /// </summary>
        public string DXFUrlName { get; set; } = "";




        /// <summary>
        /// DXF文件获取状态(0:未获取；1：获取成功；2：获取失败)
        /// </summary>
        public int DXFGetState { get; set; } = 0;


        /// <summary>
        ///DXF文件获取失败原因
        /// </summary>
        public string DXFGetFailReason { get; set; } = "";



        /// <summary>
        /// NC文件获取状态(0:未获取；1：获取成功；2：获取失败)
        /// </summary>
        public int NCGetState { get; set; } = 0;


        /// <summary>
        ///NC文件获取失败原因
        /// </summary>
        public string NCGetFailReason { get; set; } = "";


        /// <summary>
        /// 下发状态(0：未下发；1：已下发)
        /// </summary>
        public int IssueState { get; set; } = 0;

        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime IssueDate { get; set; } = DateTime.Now;

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
        /// 材质编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 订单单显示控制参数
        /// </summary>
        public int Displayed { get; set; } = 1;
    }
}
