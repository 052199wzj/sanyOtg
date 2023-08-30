using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class RFIDConfigure
    {
        #region 基础配置
        public int Id { get; set; } = 0;

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkshopCode { get; set; } = "";

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; } = "";

        /// <summary>
        /// 工位编码
        /// </summary>
        public string StationCode { get; set; } = "";

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; } = "";

        /// <summary>
        /// RFID读写头的IP地址
        /// </summary>
        public string IPAddress { get; set; } = "";

        /// <summary>
        /// 是否启用此RFID
        /// </summary>
        public int IsCheck { get; set; } = 0;


        /// <summary>
        /// 是否将读取出的RFID数据发送给MES系统
        /// </summary>
        public int IsToMES { get; set; } = 0;


        /// <summary>
        /// 是否将读取出的RFID数据发送给SCADA系统
        /// </summary>
        public int IsToScada { get; set; } = 0;

        /// <summary>
        /// 读写头型号
        /// </summary>
        public string RFIDModel { get; set; } = "";

        /// <summary>
        /// 关联电机所对应PLC的IP
        /// 需要采集电机运行信号
        /// </summary>
        public string MotorPLCIP { get; set; } = "";

        /// <summary>
        ////连续读取时间
        /// </summary>
        public int CycleTime { get; set; } = 0;
        #endregion

        #region 运行状态
        /// <summary>
        /// 工作状态
        /// </summary>
        public string RunningStatus { get; set; } = "";

        /// <summary>
        /// 连接状态
        /// </summary>
        public string ConnectionStatus { get; set; } = "";

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; } = "";

        /// <summary>
        /// VIN码
        /// </summary>
        public string VINCode { get; set; } = "";

        /// <summary>
        /// 车型代码
        /// </summary>
        public string CarTypeCode { get; set; } = "";

        /// <summary>
        /// 最后更新时刻
        /// </summary>
        public DateTime UpdateTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 备注信息
        /// </summary>
        public string RemarkInfo { get; set; } = "";
        #endregion
    }

}
