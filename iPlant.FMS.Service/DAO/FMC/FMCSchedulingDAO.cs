using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCSchedulingDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCSchedulingDAO));

        #region 单实例
        private FMCSchedulingDAO() { }
        private static FMCSchedulingDAO _Instance;

        public static FMCSchedulingDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCSchedulingDAO();
                return FMCSchedulingDAO._Instance;
            }
        }
        #endregion

        public string FMC_GetNewCode(out int wErrorCode)
        {
            string wResult = "";
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQL = StringUtils.Format(
                        "SELECT SerialNo FROM {0}.fmc_scheduling WHERE id IN( SELECT MAX(ID) FROM {0}.fmc_scheduling);",
                        wInstance);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms);

                int wNumber = 1;
                int wMonth = DateTime.Now.Month + 1;
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    String wDemandNo = StringUtils.parseString(wSqlDataReader["SerialNo"]);
                    int wCodeMonth = StringUtils.parseInt(wDemandNo.Substring(8, 2));
                    if (wMonth > wCodeMonth)
                        wNumber = 1;
                    else
                        wNumber = StringUtils.parseInt(wDemandNo.Substring(10)) + 1;
                }

                wResult = StringUtils.Format("PBLS{0}{1}{2}", DateTime.Now.Year,
                        (DateTime.Now.Month + 1).ToString("00"),
                        wNumber.ToString("0000"));
            }
            catch (Exception ex)
            {
                logger.Error("FMC_GetNewCode", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_SaveFMCScheduling(FMCScheduling wFMCScheduling, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFMCScheduling.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fmc_scheduling(SerialNo,Days,StartDate,EndDate,Active,CreateID,CreateTime) VALUES(@wSerialNo,@wDays,@wStartDate,@wEndDate,@wActive,@wCreateID,@wCreateTime);", wInstance);
                else if (wFMCScheduling.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fmc_scheduling SET SerialNo=@wSerialNo,Days=@wDays,StartDate=@wStartDate,EndDate=@wEndDate,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFMCScheduling.ID);
                wParms.Add("wSerialNo", wFMCScheduling.SerialNo);
                wParms.Add("wDays", wFMCScheduling.Days);
                wParms.Add("wStartDate", wFMCScheduling.StartDate);
                wParms.Add("wEndDate", wFMCScheduling.EndDate);
                wParms.Add("wActive", wFMCScheduling.Active);
                wParms.Add("wCreateID", wFMCScheduling.CreateID);
                wParms.Add("wCreateTime", wFMCScheduling.CreateTime);

                wSQLText = this.DMLChange(wSQLText);

                if (wFMCScheduling.ID <= 0)
                    wFMCScheduling.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FMC_SaveFMCScheduling", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCSchedulingList(List<FMCScheduling> wFMCSchedulingList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCSchedulingList != null && wFMCSchedulingList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCSchedulingList.Count; i++)
                    {
                        if (i == wFMCSchedulingList.Count - 1)
                            wStringBuilder.Append(wFMCSchedulingList[i].ID);
                        else
                            wStringBuilder.Append(wFMCSchedulingList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fmc_scheduling WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_DeleteFMCSchedulingList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        internal Dictionary<int, string> FMC_QueryStationDic(out int wErrorCode)
        {
            Dictionary<int, string> wResult = new Dictionary<int, string>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT ID,Name FROM {0}.fmc_station;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    int wID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    string wName = StringUtils.parseString(wSqlDataReader["Name"]);
                    wResult.Add(wID, wName);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCSchedulingList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public List<FMCScheduling> FMC_QueryFMCSchedulingList(int wID, string wSerialNo, int wActive, DateTime wQueryDate, out int wErrorCode)
        {
            List<FMCScheduling> wResultList = new List<FMCScheduling>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator FROM {0}.fmc_scheduling t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wSerialNo is null or @wSerialNo = '' or t.SerialNo= @wSerialNo)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wQueryDate <= '2010-1-1' or (t.StartDate<= @wQueryDate and @wQueryDate<=t.EndDate))", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wSerialNo", wSerialNo);
                wParms.Add("wActive", wActive);
                wParms.Add("wQueryDate", wQueryDate);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCScheduling wFMCScheduling = new FMCScheduling();
                    wFMCScheduling.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCScheduling.SerialNo = StringUtils.parseString(wSqlDataReader["SerialNo"]);
                    wFMCScheduling.Days = StringUtils.parseInt(wSqlDataReader["Days"]);
                    wFMCScheduling.StartDate = StringUtils.parseDate(wSqlDataReader["StartDate"]);
                    wFMCScheduling.EndDate = StringUtils.parseDate(wSqlDataReader["EndDate"]);
                    wFMCScheduling.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wFMCScheduling.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFMCScheduling.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFMCScheduling.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);

                    wResultList.Add(wFMCScheduling);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCSchedulingList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

