using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    /// <summary>
    /// 气体切割速度表
    /// </summary>
    public class FPCGasVelocity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; } = 0;
        /// <summary>
        /// 切割类型
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 厚度
        /// </summary>
        public double Thickness { get; set; } = 0.0;
        /// <summary>
        /// 气体英文名称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 气体中文名称
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// 速度下限
        /// </summary>
        public double MinSpeed { get; set; } = 0;
        /// <summary>
        /// 速度上限
        /// </summary>
        public double MaxSpeed { get; set; } = 0;

        /// <summary>
        /// 气体字典编码
        /// </summary>
        public string Code { get; set; } = "";
        
    }
}
