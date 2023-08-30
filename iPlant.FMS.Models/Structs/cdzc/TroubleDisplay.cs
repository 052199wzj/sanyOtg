namespace iPlant.FMS.Models
{
    public class TroubleDisplay
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int TroubleID { get; set; } = 0;

        /// <summary>
        /// 故障时间
        /// </summary>
        public string  TroubleTime { get; set; } = "";

        /// <summary>
        /// 故障地点
        /// </summary>
        public string FaultLocation { get; set; } = "";
    }
}
