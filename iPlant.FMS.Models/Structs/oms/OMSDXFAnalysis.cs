using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class OMSDXFAnalysis
    {
        /// <summary>
        /// 主单据ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public int OrderItemID { get; set; }

        /// <summary>
        /// 产线代码（A）
        /// </summary>
        public string ProductionLine { get; set; }

        /// <summary>
        /// 钢板分拣工位号
        /// </summary>
        public string SortStationNo { get; set; }

        /// <summary>
        /// 钢板切割工位号
        /// </summary>
        public string CutStationNo { get; set; }

        /// <summary>
        /// 套料图文件下载路径
        /// </summary>
        public string CasingLocalUrl { get; set; }

        /// <summary>
        /// 切割编号，任务编号，每个生产任务唯一，与图纸中钢板编号不同
        /// </summary>
        public string MissionNo { get; set; }

        /// <summary>
        /// 钢板编号
        /// </summary>
        public string SteelNo { get; set; }

        /// <summary>
        /// 套料编号/套料图型号
        /// </summary>
        public string CasingModel { get; set; }

        /// <summary>
        /// 钢板宽（mm）
        /// </summary>
        public double SteelWidth { get; set; }

        /// <summary>
        /// 钢板长（mm）
        /// </summary>
        public double SteelHeight { get; set; }

        /// <summary>
        /// 钢板厚度（mm）
        /// </summary>
        public double SteelThickness { get; set; }

        /// <summary>
        /// 钢板材质
        /// </summary>
        public string SteelMaterial { get; set; }

        /// <summary>
        /// 工件净重，一般有值
        /// </summary>
        public double PartsWeight { get; set; }

        /// <summary>
        /// 钢板重量，一般有值
        /// </summary>
        public double SteelWeight { get; set; }

        /// <summary>
        /// 余料重量，一般有值
        /// </summary>
        public double RemainingWeight { get; set; }

        /// <summary>
        /// 空走长度，左侧格子中内容，尝试获取
        /// </summary>
        public double IdlingLength { get; set; }

        /// <summary>
        /// 切割长度
        /// </summary>
        public double CutLength { get; set; }

        /// <summary>
        /// 机动时间(切割时长)，秒
        /// </summary>
        public double CutTime { get; set; }

        /// <summary>
        /// 打孔数
        /// </summary>
        public int HoleNumber { get; set; }

        /// <summary>
        /// 利用率（%），一般有值
        /// </summary>
        public double UseRate { get; set; }

        /// <summary>
        /// 利用率1（%），一般有值
        /// </summary>
        public double UseRate1 { get; set; }

        /// <summary>
        /// 切割块数
        /// </summary>
        public int CutBlockNumber { get; set; }

        /// <summary>
        /// 割嘴数量，一般有值
        /// </summary>
        public int CutNozzleNumber { get; set; }

        /// <summary>
        /// 割嘴距离，一般有值
        /// </summary>
        public double CutNozzleDistance { get; set; }

        /// <summary>
        /// 切割次数，一般有值
        /// </summary>
        public int CutNumber { get; set; }

        /// <summary>
        /// 补偿，一般有值
        /// </summary>
        public double Compensate { get; set; }

        /// <summary>
        /// 套料日期，格式YYYY-MM-DD。从自由项1中获取。
        /// </summary>
        public DateTime NestingDate { get; set; }

        /// <summary>
        /// 套料人
        /// </summary>
        public string NestingPerson { get; set; }

        /// <summary>
        /// 解析结果：0解析中、1解析失败、2磁吸配置失败、3解析成功
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        ///  用于图纸解析失败时，描述解析失败原因
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
