using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.WEB.Models
{
    public class Result
    {
        public int resultCode { get; set; } = 0;
        public returnObject returnObject = new returnObject();
    }

    public class returnObject
    {
        public string msg { get; set; } = "";
        public Array[] list { get; set; }
        public string info { get; set; } = "";
    }
}
