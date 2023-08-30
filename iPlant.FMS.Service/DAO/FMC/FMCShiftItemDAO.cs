using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCShiftItemDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCShiftItemDAO));

        #region 单实例
        private FMCShiftItemDAO() { }
        private static FMCShiftItemDAO _Instance;

        public static FMCShiftItemDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCShiftItemDAO();
                return FMCShiftItemDAO._Instance;
            }
        }
        #endregion

        public int FMC_SaveFMCShiftItem(FMCShiftItem wFMCShiftItem, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                string wSQLText = "";
                if (wFMCShiftItem.ID == 0)
                {
                    wSQLText = string.Format("INSERT INTO {0}.fmc_shiftitem(ShiftID,Name,StartTime,EndTime,Minutes,Type,Active,CreateID,CreateTime,EditID,EditTime) VALUES(@wShiftID,@wName,@wStartTime,@wEndTime,@wMinutes,@wType,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime);", wInstance);
                }
                else if (wFMCShiftItem.ID > 0)
                {
                    wSQLText = string.Format("UPDATE {0}.fmc_shiftitem SET ShiftID=@wShiftID,Name=@wName,StartTime=@wStartTime,EndTime=@wEndTime,Minutes=@wMinutes,Type=@wType,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime WHERE ID=@wID", wInstance);
                }

                wParms.Add("wID", wFMCShiftItem.ID);
                wParms.Add("wShiftID", wFMCShiftItem.ShiftID);
                wParms.Add("wName", wFMCShiftItem.Name);
                wParms.Add("wStartTime", wFMCShiftItem.StartTime);
                wParms.Add("wEndTime", wFMCShiftItem.EndTime);
                wParms.Add("wMinutes", wFMCShiftItem.Minutes);
                wParms.Add("wType", wFMCShiftItem.Type);
                wParms.Add("wActive", wFMCShiftItem.Active);
                wParms.Add("wCreateID", wFMCShiftItem.CreateID);
                wParms.Add("wCreateTime", wFMCShiftItem.CreateTime);
                wParms.Add("wEditID", wFMCShiftItem.EditID);
                wParms.Add("wEditTime", wFMCShiftItem.EditTime);

                wSQLText = this.DMLChange(wSQLText);

                if (wFMCShiftItem.ID <= 0)
                    wFMCShiftItem.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FMC_SaveFMCShiftItem", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCShiftItemList(List<FMCShiftItem> wFMCShiftItemList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCShiftItemList != null && wFMCShiftItemList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCShiftItemList.Count; i++)
                    {
                        if (i == wFMCShiftItemList.Count - 1)
                            wStringBuilder.Append(wFMCShiftItemList[i].ID);
                        else
                            wStringBuilder.Append(wFMCShiftItemList[i].ID + ",");
                    }
                    string wSQLText = string.Format("DELETE From {1}.fmc_shiftitem WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_DeleteFMCShiftItemList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FMCShiftItem> FMC_QueryFMCShiftItemList(int wID, int wShiftID, string wName, int wType, int wActive, out int wErrorCode)
        {
            List<FMCShiftItem> wResultList = new List<FMCShiftItem>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                string wSQLText = string.Format("SELECT t.*,t1.Name ShiftName,t2.Name Creator,t3.Name Editor FROM {0}.fmc_shiftitem t "
                    + " left join {0}.fmc_shift t1 on t.ShiftID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.CreateID=t2.ID "
                    + " left join {0}.mbs_user t3 on t.EditID=t3.ID "
                    + "WHERE 1=1"
                    + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wShiftID <=0 or t.ShiftID= @wShiftID)"
                    + " and(@wName is null or @wName = '' or t.Name= @wName)"
                    + " and(@wType <=0 or t.Type= @wType)"
                    + " and(@wActive <=0 or t.Active= @wActive)", wInstance);

                wParms.Add("wID", wID);
                wParms.Add("wShiftID", wShiftID);
                wParms.Add("wName", wName);
                wParms.Add("wType", wType);
                wParms.Add("wActive", wActive);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCShiftItem wFMCShiftItem = new FMCShiftItem();
                    wFMCShiftItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCShiftItem.ShiftID = StringUtils.parseInt(wSqlDataReader["ShiftID"]);
                    wFMCShiftItem.ShiftName = StringUtils.parseString(wSqlDataReader["ShiftName"]);
                    wFMCShiftItem.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wFMCShiftItem.StartTime = StringUtils.parseDate(wSqlDataReader["StartTime"]);
                    wFMCShiftItem.EndTime = StringUtils.parseDate(wSqlDataReader["EndTime"]);
                    wFMCShiftItem.Minutes = StringUtils.parseDouble(wSqlDataReader["Minutes"]);
                    wFMCShiftItem.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wFMCShiftItem.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wFMCShiftItem.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFMCShiftItem.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFMCShiftItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFMCShiftItem.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wFMCShiftItem.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFMCShiftItem.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);

                    wResultList.Add(wFMCShiftItem);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCShiftItemList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

