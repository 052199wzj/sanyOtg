using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 料点状态反馈(中控→MOM)
    /// </summary>
    public class INFMOMStationState
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// 料框编号
        /// </summary>
        public string PalletNo { get; set; } = "";
        /// <summary>
        /// 料框型号
        /// </summary>
        public string PalletType { get; set; } = "";
        /// <summary>
        /// 料点编号
        /// </summary>
        public string PointNo { get; set; } = "";
        /// <summary>
        /// 点位状态 1：满框  2：无框  4：空框   6：
        /// </summary>
        public int StationStatus { get; set; } = 0;
        /// <summary>
        /// 默认：0   发送成功：1   发送失败：2
        /// </summary>
        public int Status { get; set; } = 0;
        /// <summary>
        /// 发送失败原因
        /// </summary>
        public string ErroMsg { get; set; } = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;



        //料框状态
        // 0:  有料框 
        // 1：满料框，请求下料
        // 2：无料框，请求空料框
        public int FrameStatus { get; set; } = 0;
        //呼叫状态
        // 0:  有料框 
        // 1：请求下料
        // 2：请求空料框
        public int CallStatus { get; set; } = 0;
        public string WorkCenter { get; set; } = "";




        public string reqType { get; set; } = "";

    }
}
