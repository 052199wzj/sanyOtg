using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 工位
    /// </summary>
    public class FMCStation
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// 车间
        /// </summary>
        public int WorkShopID { get; set; } = 0;
        /// <summary>
        /// 产线
        /// </summary>
        public int LineID { get; set; } = 0;
        /// <summary>
        /// 工区
        /// </summary>
        public int AreaID { get; set; } = 0;
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatorID { get; set; } = 0;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string Creator { get; set; } = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 激活状态
        /// </summary>
        public int Active { get; set; } = 0;
        /// <summary>
        /// 编辑人
        /// </summary>
        public int EditorID { get; set; } = 0;
        /// <summary>
        /// 编辑人名称
        /// </summary>
        public string Editor { get; set; } = "";
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 遏制工位
        /// </summary>
        public int IPTModuleID { get; set; } = 0;
        /// <summary>
        /// 资质证书
        /// </summary>
        public string CERT { get; set; } = "";
        /// <summary>
        /// 所需环境
        /// </summary>
        public string ENVIR { get; set; } = "";
        /// <summary>
        /// 检测方法
        /// </summary>
        public string TestMethod { get; set; } = "";
        /// <summary>
        /// 是否计算
        /// </summary>
        public int IsCalcPD { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
        /// <summary>
        /// 作业名称
        /// </summary>
        public string WorkName { get; set; } = "";

    }
}
