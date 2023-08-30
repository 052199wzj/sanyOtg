namespace iPlant.FMS.Models
{
    /// <summary>
    /// 车间能耗 结构
    /// </summary>
    public class WorkshopEnergy
    {
        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; } = "";

        /// <summary>
        /// 实时能耗
        /// </summary>
        public double RealTimeEnergy { get; set; } = 0.0;

        /// <summary>
        /// 日单台能耗
        /// </summary>
        /// 
        public double DayEnergyTotal { get; set; } = 0.0;

        /// <summary>
        /// 月单台能耗
        /// </summary>
        public double MonthEnergyTotal { get; set; } = 0.0;

        /// <summary>
        /// 年单台能耗
        /// </summary>
        public double YearEnergyTotal { get; set; } = 0.0;

        /// <summary>
        /// 能耗指标
        /// </summary>
        public double EnergyTarget { get; set; } = 0.0;

    }
}
