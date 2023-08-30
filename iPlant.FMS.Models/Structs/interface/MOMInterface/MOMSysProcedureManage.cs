using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.MOMSysProcedureManage
{
    /// <summary>
    /// 中控系统查询点位是否有料框（中控->MOM）
    /// </summary>
    public class MOMSysProcedureManage
    {
        /// <summary>
        /// 协议版本
        /// </summary>
        public int version { get; set; } = 1;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string taskId { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 接口类型
        /// </summary>
        public string taskType { get; set; } = "46";
        /// <summary>
        /// 消息内容
        /// </summary>
        public reported reported = new reported();
    }

    public class reported
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public string reqId { get; set; } = "";
        /// <summary>
        /// 系统编号
        /// </summary>
        public string reqSys { get; set; } = "ZK";
        /// <summary>
        /// 工厂编号
        /// </summary>
        public string Facility { get; set; } = "5802";
        /// <summary>
        /// 操作编号
        /// 0：下料点请求空料框 ，
        /// 1：下料点移出满料框，
        /// 2：焊接请求分拣集配来料，
        /// 3：空料框回缓存区；
        /// 4：按优先级请求空料框（产线、缓存区、仓库）；
        /// 5：按优先级移出空料框（缓存区、仓库）；
        /// 6：按优先级移出满料框；
        /// 7：下料自动分拣预请求AGV满料框配送和空料框配送；
        /// 8：下料自动分拣满料框物料信息上传
        /// </summary>
        public string reqType { get; set; } = "";
        /// <summary>
        /// 料点类型
        /// </summary>
        public string palletType { get; set; } = "";
        /// <summary>
        /// 料框编号
        /// </summary>
        public string palletNo { get; set; } = "";
        /// <summary>
        /// 投料点编码
        /// </summary>
        public string sourceNo { get; set; } = "";
        /// <summary>
        /// 物料组编码
        /// </summary>
        public string materialGroup { get; set; } = "";
        /// <summary>
        /// 目的投料点类型
        /// </summary>
        public string destPointType { get; set; } = "";
        /// <summary>
        /// 目的投料点编码
        /// </summary>
        public string destNo { get; set; } = "";
        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime requireTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime sendTime { get; set; } = DateTime.Now;

        public List<materialList> materialList{ get; set; }= new List<materialList>();

    }
    public class materialList
    {

        /// <summary>
        /// 订单号
        /// </summary>
        public string wipOrderNo { get; set; } = "";
        /// <summary>
        /// 物料编号
        /// </summary>
        public string materialNo { get; set; } = "";
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
