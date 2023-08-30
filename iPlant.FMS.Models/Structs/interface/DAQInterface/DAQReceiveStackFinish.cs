using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models.DAQReceiveStackFinish
{
    /// <summary>
    /// 中控系统接收工件码盘报工接口(分拣→中控)
    /// </summary>
    public class DAQReceiveStackFinish
    {
        /// <summary>
        /// 请求码32位GUID
        /// </summary>
        public string request_code { get; set; } = "";
        /// <summary>
        /// 请求名称
        /// </summary>
        public string request_name { get; set; } = "";
        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime request_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 应答数据,例如异常描述信息
        /// </summary>
        public request_data request_data = new request_data();
    }

    public class request_data
    {
        /// <summary>
        /// 产线代码
        /// </summary>
        public string production_line { get; set; } = "A";
        /// <summary>
        /// 钢板分拣工位号。按照搬迁后布局，传A302。
        /// </summary>
        public string sort_station_no { get; set; } = "A302";
        /// <summary>
        /// 钢板切割工位号。按照搬迁后布局，传QG02。
        /// </summary>
        public string cut_station_no { get; set; } = "QG02";
        /// <summary>
        /// 任务编号，每个生产任务唯一，与图纸中钢板编号不同。中控系统可通过任务编号区分A线还是B线任务。
        /// </summary>
        public string mission_no { get; set; } = "";
        /// <summary>
        /// 钢板编号
        /// </summary>
        public string steel_no { get; set; } = "";
        /// <summary>
        /// 套料编号/套料图型号
        /// </summary>
        public string casing_model { get; set; } = "";
        /// <summary>
        /// 计划编号，即订单号
        /// </summary>
        public string plan_no { get; set; } = "";
        /// <summary>
        /// 工件型号（件号：如果只有一个@，就是工件型号；如果有两个@，取第二个作为工件型号）。
        /// </summary>
        public string part_model { get; set; } = "";
        /// <summary>
        /// 工件尺寸类型。-1：极小件，1：小件，2：中件，3：大件，4：超大件。
        /// </summary>
        public int size_type { get; set; } = 0;
        /// <summary>
        /// 分拣方式。1 自动分拣、 2 人工分拣。
        /// </summary>
        public int sort_way { get; set; } = 0;
        /// <summary>
        /// 分拣结果代码。0为正常，其他为异常或失败
        /// </summary>
        public int sort_result { get; set; } = 0;
        /// <summary>
        /// 分拣异常信息
        /// </summary>
        public string error_msg { get; set; } = "";
        /// <summary>
        /// 分拣开始时间
        /// </summary>
        public DateTime start_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 分拣结束时间
        /// </summary>
        public DateTime end_time { get; set; } = DateTime.Now;
    }
}
