using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iPlant.FMS.Models
{
    public enum MCSModuleType : int
    {
        [Description("默认")]
        Default = 0,
        [Description("用户管理")]
        UserManage = 1,
        [Description("权限管理")]
        RoleManage = 2,
        [Description("套料图解析")]
        PicParse = 3,
        [Description("订单管理")]
        OrderManage = 4,
        [Description("排班管理")]
        ShiftManage = 5,
        [Description("报工管理")]
        ReportManage = 6,
    }
}
