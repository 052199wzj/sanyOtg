using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCSchedulingItemDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCSchedulingItemDAO));

        #region 单实例
        private FMCSchedulingItemDAO() { }
        private static FMCSchedulingItemDAO _Instance;

        public static FMCSchedulingItemDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCSchedulingItemDAO();
                return FMCSchedulingItemDAO._Instance;
            }
        }
        #endregion

        public int FMC_SaveFMCSchedulingItem(FMCSchedulingItem wFMCSchedulingItem, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFMCSchedulingItem.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fmc_schedulingitem(FMCSchedulingID,StationID,ScheduleDate,PersonID,CreateID,CreateTime,EditID,EditTime,ShiftID) VALUES(@wFMCSchedulingID,@wStationID,@wScheduleDate,@wPersonID,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wShiftID);", wInstance);
                else if (wFMCSchedulingItem.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fmc_schedulingitem SET FMCSchedulingID=@wFMCSchedulingID,StationID=@wStationID,ScheduleDate=@wScheduleDate,PersonID=@wPersonID,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,ShiftID=@wShiftID WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFMCSchedulingItem.ID);
                wParms.Add("wFMCSchedulingID", wFMCSchedulingItem.FMCSchedulingID);
                wParms.Add("wStationID", wFMCSchedulingItem.StationID);
                wParms.Add("wScheduleDate", wFMCSchedulingItem.ScheduleDate);
                wParms.Add("wPersonID", wFMCSchedulingItem.PersonID);
                wParms.Add("wCreateID", wFMCSchedulingItem.CreateID);
                wParms.Add("wCreateTime", wFMCSchedulingItem.CreateTime);
                wParms.Add("wEditID", wFMCSchedulingItem.EditID);
                wParms.Add("wEditTime", wFMCSchedulingItem.EditTime);
                wParms.Add("wShiftID", wFMCSchedulingItem.ShiftID);

                wSQLText = this.DMLChange(wSQLText);

                if (wFMCSchedulingItem.ID <= 0)
                    wFMCSchedulingItem.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FMC_SaveFMCSchedulingItem", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCSchedulingItemList(List<FMCSchedulingItem> wFMCSchedulingItemList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCSchedulingItemList != null && wFMCSchedulingItemList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCSchedulingItemList.Count; i++)
                    {
                        if (i == wFMCSchedulingItemList.Count - 1)
                            wStringBuilder.Append(wFMCSchedulingItemList[i].ID);
                        else
                            wStringBuilder.Append(wFMCSchedulingItemList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fmc_schedulingitem WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_DeleteFMCSchedulingItemList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FMCSchedulingItem> FMC_QueryFMCSchedulingItemList(int wID, int wFMCSchedulingID, int wStationID, int wPersonID, DateTime wStartTime, DateTime wEndTime, int wShiftID, out int wErrorCode)
        {
            List<FMCSchedulingItem> wResultList = new List<FMCSchedulingItem>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.SerialNo,t2.Name StationName,t4.Name Creator,t5.Name Editor,t6.Name ShiftName  FROM {0}.fmc_schedulingitem t "
                    + " left join {0}.fmc_scheduling t1 on t.FMCSchedulingID=t1.ID "
                    + " left join {0}.fmc_station t2 on t.StationID=t2.ID "
                    + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                    + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                    + " left join {0}.fmc_shift t6 on t.ShiftID=t6.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wFMCSchedulingID <=0 or t.FMCSchedulingID= @wFMCSchedulingID)"
                + " and(@wStationID <=0 or t.StationID= @wStationID)"
                + " and(@wShiftID <=0 or t.ShiftID= @wShiftID)"
                + " and(@wPersonID <=0 or t.PersonID= @wPersonID)"
                + " and(@wStartTime <= '2010-1-1' or t.ScheduleDate>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.ScheduleDate<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wFMCSchedulingID", wFMCSchedulingID);
                wParms.Add("wStationID", wStationID);
                wParms.Add("wPersonID", wPersonID);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                wParms.Add("wShiftID", wShiftID);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                Dictionary<int, string> wPersonDic = GetPersonDic(out wErrorCode);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCSchedulingItem wFMCSchedulingItem = new FMCSchedulingItem();
                    wFMCSchedulingItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCSchedulingItem.FMCSchedulingID = StringUtils.parseInt(wSqlDataReader["FMCSchedulingID"]);
                    wFMCSchedulingItem.SerialNo = StringUtils.parseString(wSqlDataReader["SerialNo"]);
                    wFMCSchedulingItem.StationID = StringUtils.parseInt(wSqlDataReader["StationID"]);
                    wFMCSchedulingItem.StationName = StringUtils.parseString(wSqlDataReader["StationName"]);
                    wFMCSchedulingItem.ScheduleDate = StringUtils.parseDate(wSqlDataReader["ScheduleDate"]);
                    wFMCSchedulingItem.PersonID = StringUtils.parseString(wSqlDataReader["PersonID"]);
                    wFMCSchedulingItem.PersonName = this.GetPersonName(wFMCSchedulingItem.PersonID, wPersonDic);
                    wFMCSchedulingItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFMCSchedulingItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFMCSchedulingItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFMCSchedulingItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wFMCSchedulingItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFMCSchedulingItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wFMCSchedulingItem.ShiftID = StringUtils.parseInt(wSqlDataReader["ShiftID"]);
                    wFMCSchedulingItem.ShiftName = StringUtils.parseString(wSqlDataReader["ShiftName"]);
                    wResultList.Add(wFMCSchedulingItem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCSchedulingItemList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        private string GetPersonName(string wPersonID, Dictionary<int, string> wPersonDic)
        {
            string wResult = "";
            try
            {
                if (string.IsNullOrWhiteSpace(wPersonID))
                    return wResult;

                string[] wUserIDs = wPersonID.Split(",");
                List<string> wNames = new List<string>();
                foreach (string wUserID in wUserIDs)
                {
                    int wID = StringUtils.parseInt(wUserID);
                    if (wPersonDic.ContainsKey(wID))
                        wNames.Add(wPersonDic[wID]);
                }
                wResult = StringUtils.Join(",", wNames);
            }
            catch (Exception ex)
            {
                logger.Error("GetPersonName", ex);
            }
            return wResult;

        }

        internal List<FMCSchedulingItem> FMC_QueryMaxFMCSchedulingItemList(out int wErrorCode)
        {
            List<FMCSchedulingItem> wResultList = new List<FMCSchedulingItem>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.SerialNo,t2.Name StationName,t4.Name Creator,t5.Name Editor  FROM {0}.fmc_schedulingitem t "
                    + " left join {0}.fmc_scheduling t1 on t.FMCSchedulingID=t1.ID "
                    + " left join {0}.fmc_station t2 on t.StationID=t2.ID "
                    + " left join {0}.mbs_user t4 on t.CreateID=t4.ID "
                    + " left join {0}.mbs_user t5 on t.EditID=t5.ID "
                    + "WHERE t.FMCSchedulingID in (SELECT max(ID) FROM {0}.fmc_scheduling)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                Dictionary<int, string> wPersonDic = GetPersonDic(out wErrorCode);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCSchedulingItem wFMCSchedulingItem = new FMCSchedulingItem();
                    wFMCSchedulingItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCSchedulingItem.FMCSchedulingID = StringUtils.parseInt(wSqlDataReader["FMCSchedulingID"]);
                    wFMCSchedulingItem.SerialNo = StringUtils.parseString(wSqlDataReader["SerialNo"]);
                    wFMCSchedulingItem.StationID = StringUtils.parseInt(wSqlDataReader["StationID"]);
                    wFMCSchedulingItem.StationName = StringUtils.parseString(wSqlDataReader["StationName"]);
                    wFMCSchedulingItem.ScheduleDate = StringUtils.parseDate(wSqlDataReader["ScheduleDate"]);
                    wFMCSchedulingItem.PersonID = StringUtils.parseString(wSqlDataReader["personID"]);
                    wFMCSchedulingItem.PersonName = this.GetPersonName(wFMCSchedulingItem.PersonID, wPersonDic);
                    wFMCSchedulingItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFMCSchedulingItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFMCSchedulingItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFMCSchedulingItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wFMCSchedulingItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFMCSchedulingItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wResultList.Add(wFMCSchedulingItem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCSchedulingItemList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        private Dictionary<int, string> GetPersonDic(out int wErrorCode)
        {
            Dictionary<int, string> wResultList = new Dictionary<int, string>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT ID,Name FROM {0}.mbs_user;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    int wID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    string wName = StringUtils.parseString(wSqlDataReader["Name"]);
                    wResultList.Add(wID, wName);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCSchedulingItemList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

