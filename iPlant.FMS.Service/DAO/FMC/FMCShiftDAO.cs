using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCShiftDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCShiftDAO));

        #region 单实例
        private FMCShiftDAO() { }
        private static FMCShiftDAO _Instance;

        public static FMCShiftDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCShiftDAO();
                return FMCShiftDAO._Instance;
            }
        }
        #endregion

        public int FMC_SaveFMCShift(FMCShift wFMCShift, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFMCShift.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fmc_shift(Name) VALUES(@wName);", wInstance);
                else if (wFMCShift.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fmc_shift SET Name=@wName WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFMCShift.ID);
                wParms.Add("wName", wFMCShift.Name);

                wSQLText = this.DMLChange(wSQLText);

                if (wFMCShift.ID <= 0)
                    wFMCShift.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FMC_SaveFMCShift", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCShiftList(List<FMCShift> wFMCShiftList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCShiftList != null && wFMCShiftList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCShiftList.Count; i++)
                    {
                        if (i == wFMCShiftList.Count - 1)
                            wStringBuilder.Append(wFMCShiftList[i].ID);
                        else
                            wStringBuilder.Append(wFMCShiftList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fmc_shift WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_DeleteFMCShiftList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FMCShift> FMC_QueryFMCShiftList(int wID, string wName, out int wErrorCode)
        {
            List<FMCShift> wResultList = new List<FMCShift>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT * FROM {0}.fmc_shift WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wName is null or @wName = '' or Name= @wName)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wName", wName);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCShift wFMCShift = new FMCShift();
                    wFMCShift.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCShift.Name = StringUtils.parseString(wSqlDataReader["Name"]);

                    wResultList.Add(wFMCShift);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCShiftList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

