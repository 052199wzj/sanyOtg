using iPlant.Common.Tools;
using iPlant.Data.EF;
using iPlant.Data.EF.Repository;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public class MESServer
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MESServer));

        public static int Instance = 0; // 0:单机版,1000,网络版

        public static Boolean ERPEnable = false;

        public static DateTime ExpiredTime = DateTime.Now;
        static MESServer()
        {
            try
            {
                String wExpiredTimeString = StringUtils.parseString( GlobalConstant.GlobalConfiguration.GetValue("Role.Manager.ExpiredDate"));
                if (StringUtils.isNotEmpty(wExpiredTimeString))
                { 
                    wExpiredTimeString = DesUtil.Decrypt(wExpiredTimeString, BaseDAO.appSecret);
                    ExpiredTime = StringUtils.parseDate(wExpiredTimeString);
                }
                else { 
                    ExpiredTime = new DateTime(2099, 12, 31);
                }
                ExpiredTime = new DateTime(ExpiredTime.Year, ExpiredTime.Month, ExpiredTime.Day, 23, 59, 59);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                logger.Error("static",
                      ex);
            }


        }

        // public static List<MESEntry> EntryList = new List<MESEntry>();

        private static DBPool mDBPool { get { return RepositoryFactory.GetDBPool(); } }
         


        // 数据库实例&文件系统实例
        public static int DB_QueryMaxID(String wInstanceName, String wTableName)
        {
            int wMaxID = 1;
            // 判断客户信息是否存在(中国：统一社会信用代码，国外:提醒是否有重复）
            try
            {
                using (DbConnection wConnection = mDBPool.GetConnection())
                {
                    String wSQLText = StringUtils.Format("Select ifnull(max(ID),0) as ID from {0}.{1}", wInstanceName,
                         wTableName);
                    CommandTool wCommandTool = new CommandTool(wSQLText, wConnection);

                    using (DbDataReader wSqlDataReader = wCommandTool.ExecuteReader())
                    {
                        while (wSqlDataReader.Read())
                        {
                            int wID = StringUtils.parseInt(wSqlDataReader["ID"]);
                            wMaxID = wID + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("DB_QueryMaxID",
                        ex);
            }
            return wMaxID;
        }



        private static DateTime MES_QueryMondayByDate(DateTime wShiftTime)
        {
            DateTime wMonday = wShiftTime;
            try
            {
                int weeknow = Convert.ToInt32(wShiftTime.DayOfWeek);

                // 因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
                weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
                int daydiff = (-1) * weeknow;

                // 本周第一天
                wMonday = Convert.ToDateTime(wShiftTime.AddDays(daydiff));
            }
            catch (Exception ex)
            {
                logger.Error("MES_QueryMondayByDate", ex);
            }
            return wMonday;
        }
        public static DateTime MES_GetShiftTimeByShiftID(int wCompanyID, int wShiftID)
        {
            DateTime wShiftTime = DateTime.Now;
            try
            {
                int wYear = wShiftID / 1000000;
                int wMonth = (wShiftID / 10000) % 100;
                int wDay = (wShiftID / 100) % 100;
                wShiftTime = new DateTime(wYear, wMonth, wDay, 0, 0, 0);
            }
            catch (Exception ex)
            {
                logger.Error("MES_GetShiftTimeByShiftID", ex);
            }
            return wShiftTime;
        }

        public static int MES_QueryShiftID(DateTime wDateTime)
        { 
            return int.Parse(wDateTime.ToString("yyyyMMdd"));
        }
    }
}
