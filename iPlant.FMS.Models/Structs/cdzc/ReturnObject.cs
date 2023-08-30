using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class ReturnObject
    {
        public object info { get; set; } = new object();

        public List<object> list { get; set; } = new List<object>();


    }
}
