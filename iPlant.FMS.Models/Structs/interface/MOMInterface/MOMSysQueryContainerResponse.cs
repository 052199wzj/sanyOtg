using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.MOMSysQueryContainerResponse
{
    /// <summary>
    /// MOM系统返回信息
    /// </summary>
    public class MOMSysQueryContainerResponse
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public int version { get; set; } = 0;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string taskId { get; set; } = "";
        /// <summary>
        /// 返回结果
        /// </summary>
        public string code { get; set; } = "";
        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg { get; set; } = "";
        /// <summary>
        /// 消息内容
        /// </summary>
        public returnData returnData = new returnData();
    }

    public class returnData
    {
        /// <summary>
        /// 料框类型
        /// </summary>
        public string palletType { get; set; } = "";
        /// <summary>
        /// 料框编码
        /// </summary>
        public string palletNo { get; set; } = "";
        /// <summary>
        /// 投料组编号
        /// </summary>
        public string packCode { get; set; } = "";
        /// <summary>
        /// 台套数量
        /// </summary>
        public string setQuantity { get; set; } = "";
        /// <summary>
        /// 起点编码
        /// </summary>
        public string sourceNo { get; set; } = "";
        /// <summary>
        /// 终点编码
        /// </summary>
        public string destNo { get; set; } = "";
        /// <summary>
        /// 炉批号
        /// </summary>
        public string lotNo { get; set; } = "";

        /// <summary>
        /// 消息内容
        /// </summary>
        public List<materialList> materialList { get; set; } = new List<materialList>();
       
    }
    public class materialList
    {
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string wiporderId { get; set; } = "";
        /// <summary>
        /// 物料编码
        /// </summary>
        public string materialNo { get; set; } = "";
        /// <summary>
        /// 位置
        /// </summary>
        public string position { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public int quantity { get; set; } = 0;
        /// <summary>
        /// 产品序列号
        /// </summary>
        public string serialNo { get; set; } = "";
        /// <summary>
        /// 顺序号
        /// </summary>
        public string sourceSequenceNo { get; set; } = "";
        /// <summary>
        /// 工序号
        /// </summary>
        public string sourceOprSequenceNo { get; set; } = "";
    }
}
