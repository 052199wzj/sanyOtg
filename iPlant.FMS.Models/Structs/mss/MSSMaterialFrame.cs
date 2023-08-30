using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 料框
    /// </summary>
    public class MSSMaterialFrame : BasePo
    {

        public double FrameHeight { get; set; } = 0.0;

        public double SteelPlateHeight { get; set; } = 0.0;

        public double SteelPlateNum { get; set; } = 0.0;

        public String FrameCode { get; set; } = "";

        public bool IsExistFrame { get; set; } = false;

        public DateTime ArrivalTime { get; set; } = new DateTime(2000, 1, 1);

        //工单编号
        public string OrderNo { get; set; } = "";
        //物料编码
        public string MaterialNo { get; set; } = "";
        //产品编号
        public string PartID { get; set; } = "";
        //产品类型
        public string PartType { get; set; } = "";

        //料框状态
        // 1:  有料框 
        // 2：无料框，请求空料框
        // 3：满料框，请求下料
        public int FrameStatus { get; set; } = 0;
        //呼叫状态
        // 0:  无呼叫 
        // 1：请求下料
        // 2：请求空料框
        public int CallStatus { get; set; } = 0;
        public string WorkCenter { get; set; } = "";


        /// <summary>
        /// 顺序号
        /// </summary>
        public string SourceSequenceNo { get; set; } = "";
        /// <summary>
        /// 工序号
        /// </summary>
        public string SourceOprSequenceNo { get; set; } = "";
    }
}