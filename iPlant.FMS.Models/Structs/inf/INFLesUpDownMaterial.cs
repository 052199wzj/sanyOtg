using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
   public class INFLesUpDownMaterial
    {
        public int ID { get; set; } = 0;

        /// <summary>
        /// 料框号
        /// </summary>
        public String FrameCode { get; set; } = "";

        /// <summary>
        /// 切割编号
        /// </summary>
        public String NestId { get; set; } = "";

        /// <summary>
        /// 订单号
        /// </summary>
        public String Order { get; set; } = "";

        /// <summary>
        /// 物料编码
        /// </summary>
        public String ProductNo { get; set; } = "";

        /// <summary>
        /// 工序号
        /// </summary>
        public String Seq { get; set; } = "";

        /// <summary>
        /// 点位编号
        /// </summary>
        public String StationCode { get; set; } = "";

        /// <summary>
        /// 分组号
        /// </summary>
        public String Sub { get; set; } = "";

        /// <summary>
        /// 0：放料   1：取料
        /// </summary>
        public int UseType { get; set; } = 0;

        /// <summary>
        /// 默认：0   发送成功：1   发送失败：2
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 发送失败原因
        /// </summary>
        public String ErroMsg { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = new DateTime(2000, 1, 1);

    }
}
