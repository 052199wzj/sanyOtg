using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    class QMSOneTimePassRateDAO : BaseDAO
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QMSOneTimePassRateDAO));
        private static QMSOneTimePassRateDAO Instance = null;

        private QMSOneTimePassRateDAO() : base()
        {

        }

        public static QMSOneTimePassRateDAO getInstance()
        {
            if (Instance == null)
                Instance = new QMSOneTimePassRateDAO();
            return Instance;
        }


        /// <summary>
        /// 查询产品的一次性合格率    按产品分类 也可以合并查询
        /// </summary>
        /// <param name="wLoginUser"></param>
        /// <param name="wLineID"></param>
        /// <param name="wOrderID"></param>
        /// <param name="wProductID"></param>
        /// <param name="wStatType"></param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>

        public List<QMSOneTimePassRate> QMS_GetOneTimePassAll(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList,
                  int wStatType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {
                switch (wStatType)
                {
                    case ((int)DMSStatTypes.Day):
                        wResult = this.GetOneTimePassRateDayList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Week):
                        wResult = this.GetOneTimePassRateWeekList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Month):
                        wResult = this.GetOneTimePassRateMonthList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Quarter):
                        wResult = this.GetOneTimePassRateYearList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Year):
                        wResult = this.GetOneTimePassRateYearList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;

                    default:
                        wResult = this.GetOneTimePassRateDayList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime, wPagination,
                             wErrorCode);
                        break;
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateDayList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                wPagination.Sort = "StatDate";

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date(  DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date(  DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by t2.LineID,t2.ProductID,StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap, wPagination);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        /// <summary>
        /// 按订单统计/按产品统计
        /// </summary>
        /// <param name="wLoginUser"></param>
        /// <param name="wLineID"></param>
        /// <param name="wOrderID"></param>
        /// <param name="wProductID"></param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public List<QMSOneTimePassRate> GetOneTimePassRateWeekList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                wPagination.Sort = "StatDate";

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w')  >= @wStartTime "
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by t2.LineID,t2.ProductID,StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap, wPagination);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateMonthList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                wPagination.Sort = "StatDate";

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by t2.LineID,t2.ProductID,StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap, wPagination);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public List<QMSOneTimePassRate> GetOneTimePassRateQuarterList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                wPagination.Sort = "StatDate";

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by t2.LineID,t2.ProductID,StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap, wPagination);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateYearList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                wPagination.Sort = "StatDate";

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by t2.LineID,t2.ProductID,StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap, wPagination);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public List<QMSOneTimePassRate> GetAllForChart(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, int wStatType, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {
                if (wProductIDList == null)
                    wProductIDList = new List<int>();


                switch (wStatType)
                {
                    case ((int)DMSStatTypes.Day):
                        wResult = this.GetOneTimePassRateForChartDayList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Week):
                        wResult = this.GetOneTimePassRateForChartWeekList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Month):
                        wResult = this.GetOneTimePassRateForChartMonthList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Quarter):
                        wResult = this.GetOneTimePassRateForChartYearList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;
                    case ((int)DMSStatTypes.Year):
                        wResult = this.GetOneTimePassRateForChartYearList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;

                    default:
                        wResult = this.GetOneTimePassRateForChartDayList(wLoginUser, wLineID, wProductIDList,
                             wStartTime, wEndTime,
                             wErrorCode);
                        break;
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateForChartDayList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-%d'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateForChartWeekList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w')  >= @wStartTime "
                                                   + " and str_to_date( DATE_FORMAT(t.StartTime,'%Y-%u-1'),'%Y-%u-%w') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateForChartMonthList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date(  DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date(  DATE_FORMAT(t.StartTime,'%Y-%m-1'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<QMSOneTimePassRate> GetOneTimePassRateForChartQuarterList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( concat(  year(t.StartTime),'-',(quarter(t.StartTime) * 3)-2,'-1'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public List<QMSOneTimePassRate> GetOneTimePassRateForChartYearList(BMSEmployee wLoginUser, int wLineID, List<int> wProductIDList, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            List<QMSOneTimePassRate> wResult = new List<QMSOneTimePassRate>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = string.Format("select t2.ProductID,t2.LineID,Count(t.ID) as FeedingNum,"
                                                   + " Count(t.CheckResult=2 or null) as NGNum,Count(t.Status>1 or null) as Num ,"
                                                   + " Count(t.CheckResult=1&&t.RepairCount<=0 or null) as  OneTimePassNum,str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d') as StatDate,"
                                                   + " t3.ProductNo,t3.ProductName"
                                                   + " FROM {0}.qms_workpiece t "
                                                   + " inner join {0}.oms_order t2 on t2.ID = t.OrderID "
                                                   + " inner join {0}.fpc_product t3 on t3.ID = t2.ProductID "
                                                   + " where  t.Status>0 and (@wLineID<=0 or t2.LineID=@wLineID)"
                                                   + " and str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d')  >= @wStartTime "
                                                   + " and str_to_date( concat(  year(t.StartTime),'-01','-01'),'%Y-%m-%d') < @wEndTime "
                                                   + " and ( @wProductID ='' or t2.ProductID IN( {1} ) ) "
                                                   + " group by StatDate ", wInstance, wProductIDList.Count > 0 ? StringUtils.Join(",", wProductIDList) : "0");


                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wStartTime", wStartTime);
                wParamMap.Add("wEndTime", wEndTime);
                wParamMap.Add("wProductID", StringUtils.Join(",", wProductIDList));

                wSQL = this.DMLChange(wSQL);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);

                if (wQueryResult.Count <= 0)
                {
                    return wResult;
                }
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    QMSOneTimePassRate wQMSOneTimePassRate = new QMSOneTimePassRate();
                    wQMSOneTimePassRate.ProductID = StringUtils.parseInt(wReader["ProductID"]);
                    wQMSOneTimePassRate.ProductNo = StringUtils.parseString(wReader["ProductNo"]);
                    wQMSOneTimePassRate.ProductName = StringUtils.parseString(wReader["ProductName"]);

                    wQMSOneTimePassRate.StatDate = StringUtils.parseDate(wReader["StatDate"]);
                    wQMSOneTimePassRate.OneTimePassNum = StringUtils.parseDouble(wReader["OneTimePassNum"]);
                    wQMSOneTimePassRate.FeedingNum = StringUtils.parseDouble(wReader["FeedingNum"]);
                    wQMSOneTimePassRate.Num = StringUtils.parseDouble(wReader["Num"]);
                    wQMSOneTimePassRate.NGNum = StringUtils.parseDouble(wReader["NGNum"]);

                    wResult.Add(wQMSOneTimePassRate);
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

    }
}
