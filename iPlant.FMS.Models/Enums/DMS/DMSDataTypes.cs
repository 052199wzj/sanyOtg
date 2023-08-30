using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iPlant.FMS.Models
{
    public enum DMSDataTypes : int
    {
        [Description("默认")]
        Default = 0,
        [Description("bool")]
        Bool = 1,
        [Description("int")]
        Int = 2,
        [Description("string")]
        String = 3,
        [Description("float")]
        Float = 4,
        [Description("double")]
        Double = 5
    }
}
