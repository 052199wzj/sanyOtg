namespace iPlant.FMS.Models
{
    public class AnyDeviceStatus
    {
        /// <summary>
        /// 车间名称
        /// </summary>
        public string DeviceName { get; set; } = "";

        /// <summary>
        /// 设备总数
        /// </summary>
        public double DeviceNum { get; set; } = 0.0;

        /// <summary>
        /// 在线数量/占比
        /// </summary>
        public string OnlineNum { get; set; } = "";

        /// <summary>
        /// 故障数量/占比
        /// </summary>
        public string FaultNum { get; set; } = "";

        /// <summary>
        /// 离线数量/占比
        /// </summary>
        public string OfflineNum { get; set; } ="";
    }
}
