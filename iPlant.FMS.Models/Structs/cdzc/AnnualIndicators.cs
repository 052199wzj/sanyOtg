using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class AnnualIndicators
    {

        public int ID { get; set; } = 0;
        // <summary>
        /// 年度指标-电
        /// </summary>
        public double DAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        /// 年度指标-天然气
        /// </summary>
        public double TRQAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        /// 年度指标-水
        /// </summary>
        public double SAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        /// 年度指标-混合气
        /// </summary>
        public double HHQAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        /// 年度指标-压缩气
        /// </summary>
        public double YSQAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        ///  年度指标-氧气
        /// </summary>
        public double YQAnnualIndicators { get; set; } = 0.0;

        /// <summary>
        ///  质量安环部-电
        /// </summary>
        public double Quality_D { get; set; } = 0.0;

        /// <summary>
        ///  质量安环部-天然气
        /// </summary>
        public double Quality_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  质量安环部-水
        /// </summary>
        public double Quality_S { get; set; } = 0.0;

        /// <summary>
        ///  质量安环部-混合气
        /// </summary>
        public double Quality_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  质量安环部-压缩气
        /// </summary>
        public double Quality_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  质量安环部-氧气
        /// </summary>
        public double Quality_YQ { get; set; } = 0.0;

        /// <summary>
        ///  综合管理部-电
        /// </summary>
        public double Synthesis_D { get; set; } = 0.0;

        /// <summary>
        ///  综合管理部-天然气
        /// </summary>
        public double Synthesis_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  综合管理部-水
        /// </summary>
        public double Synthesis_S { get; set; } = 0.0;

        /// <summary>
        ///  综合管理部-混合气
        /// </summary>
        public double Synthesis_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  综合管理部-压缩气
        /// </summary>
        public double Synthesis_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  综合管理部-氧气
        /// </summary>
        public double Synthesis_YQ { get; set; } = 0.0;



        /// <summary>
        ///  制件车间-电
        /// </summary>
        public double PartsWorkShop_D { get; set; } = 0.0;

        /// <summary>
        ///  制件车间-天然气
        /// </summary>
        public double PartsWorkShop_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  制件车间-水
        /// </summary>
        public double PartsWorkShop_S { get; set; } = 0.0;

        /// <summary>
        ///  制件车间-混合气
        /// </summary>
        public double PartsWorkShop_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  制件车间-压缩气
        /// </summary>
        public double PartsWorkShop_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  制件车间-氧气
        /// </summary>
        public double PartsWorkShop_YQ { get; set; } = 0.0;



        /// <summary>
        ///  车身车间-电
        /// </summary>
        public double BodyWorkShop_D { get; set; } = 0.0;

        /// <summary>
        ///  车身车间-天然气
        /// </summary>
        public double BodyWorkShop_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  车身车间-水
        /// </summary>
        public double BodyWorkShop_S { get; set; } = 0.0;

        /// <summary>
        ///  车身车间-混合气
        /// </summary>
        public double BodyWorkShop_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  车身车间-压缩气
        /// </summary>
        public double BodyWorkShop_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  车身车间-氧气
        /// </summary>
        public double BodyWorkShop_YQ { get; set; } = 0.0;


        /// <summary>
        ///  涂装车间-电
        /// </summary>
        public double PaintingWorkShop_D { get; set; } = 0.0;

        /// <summary>
        ///  涂装车间-天然气
        /// </summary>
        public double PaintingWorkShop_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  涂装车间-水
        /// </summary>
        public double PaintingWorkShop_S { get; set; } = 0.0;

        /// <summary>
        ///  涂装车间-混合气
        /// </summary>
        public double PaintingWorkShop_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  涂装车间-压缩气
        /// </summary>
        public double PaintingWorkShop_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  涂装车间-氧气
        /// </summary>
        public double PaintingWorkShop_YQ { get; set; } = 0.0;


        /// <summary>
        ///  总装车间-电
        /// </summary>
        public double AssemblyWorkShop_D { get; set; } = 0.0;

        /// <summary>
        ///  总装车间-天然气
        /// </summary>
        public double AssemblyWorkShop_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  总装车间-水
        /// </summary>
        public double AssemblyWorkShop_S { get; set; } = 0.0;

        /// <summary>
        ///  总装车间-混合气
        /// </summary>
        public double AssemblyWorkShop_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  总装车间-压缩气
        /// </summary>
        public double AssemblyWorkShop_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  总装车间-氧气
        /// </summary>
        public double AssemblyWorkShop_YQ { get; set; } = 0.0;


        /// <summary>
        ///  调检车间-电
        /// </summary>
        public double CheckWorkShop_D { get; set; } = 0.0;

        /// <summary>
        ///  调检车间-天然气
        /// </summary>
        public double CheckWorkShop_TRQ { get; set; } = 0.0;

        /// <summary>
        ///  调检车间-水
        /// </summary>
        public double CheckWorkShop_S { get; set; } = 0.0;

        /// <summary>
        ///  调检车间-混合气
        /// </summary>
        public double CheckWorkShop_HHQ { get; set; } = 0.0;

        /// <summary>
        ///  调检车间-压缩气
        /// </summary>
        public double CheckWorkShop_YSQ { get; set; } = 0.0;


        /// <summary>
        ///  调检车间-氧气
        /// </summary>
        public double CheckWorkShop_YQ { get; set; } = 0.0;
    }

}
