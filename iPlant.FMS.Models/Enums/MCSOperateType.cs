using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iPlant.FMS.Models
{
    public enum MCSOperateType : int
    {
        [Description("默认")]
        Default = 0,
        [Description("新增")]
        Add = 1,
        [Description("删除")]
        Delete = 2,
        [Description("修改")]
        Update = 3,
        [Description("接收")]
        Receive = 4,
        [Description("上传")]
        Upload = 5,
        [Description("手动创建")]
        Create = 6,
    }
}
