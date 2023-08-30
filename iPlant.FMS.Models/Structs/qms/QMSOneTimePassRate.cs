using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Models
{
    public class QMSOneTimePassRate
    {
        public QMSOneTimePassRate() { }




        public int ProductID { get; set; } = 0;
        /// <summary>
        /// 产品编号
        /// </summary>
        public String ProductNo { get; set; } = "";
        /// <summary>
        /// 产品名称
        /// </summary>
        public String ProductName { get; set; } = "";
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime StatDate { get; set; } = new DateTime(2000, 1, 1);
        /// <summary>
        /// 一次性合格数
        /// </summary>
        public double OneTimePassNum { get; set; } = 0;
        /// <summary>
        /// 上料总数
        /// </summary>
        public double FeedingNum { get; set; } = 0;
        /// <summary>
        /// 成品数量
        /// </summary>
        public double Num { get; set; } = 0;
        /// <summary>
        /// 不合格数量
        /// </summary>
        public double NGNum { get; set; } = 0;
        /// <summary>
        /// 合格数量
        /// </summary>
        public double OKNum
        {
            get
            {
                if (Num <= 0 || NGNum > Num)
                    return 0;
                return Num - NGNum;
            }
            private set { }
        }
        /// <summary>
        /// 一次性合格率
        /// </summary>
        public double OneTimePassRate
        {
            get
            {
                if (OneTimePassNum <= 0 || FeedingNum <= 0)
                    return 0.0;
                if (OneTimePassNum > FeedingNum)
                    return 1.0;
                return OneTimePassNum / FeedingNum;
            }
            private set { }
        }
        /// <summary>
        /// 合格率
        /// </summary>
        public double PassRate
        {
            get
            {
                if (OKNum <= 0 || FeedingNum <= 0)
                    return 0.0;
                if (OKNum > FeedingNum)
                    return 1.0;

                return OKNum / FeedingNum;
            }
            private set { }
        }
    }
}
