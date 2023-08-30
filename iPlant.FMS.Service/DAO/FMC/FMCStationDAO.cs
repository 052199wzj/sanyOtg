using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCStationDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCStationDAO));

        #region 单实例
        private FMCStationDAO() { }
        private static FMCStationDAO _Instance;

        public static FMCStationDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCStationDAO();
                return FMCStationDAO._Instance;
            }
        }
        #endregion

        public int FMC_SaveFMCStation(FMCStation wFMCStation, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFMCStation.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fmc_station(Name,Code,WorkShopID,LineID,AreaID,CreatorID,CreateTime,Active,EditorID,EditTime,IPTModuleID,CERT,ENVIR,TestMethod,IsCalcPD,Remark,WorkName) VALUES(@wName,@wCode,@wWorkShopID,@wLineID,@wAreaID,@wCreatorID,@wCreateTime,@wActive,@wEditorID,@wEditTime,@wIPTModuleID,@wCERT,@wENVIR,@wTestMethod,@wIsCalcPD,@wRemark,@wWorkName);", wInstance);
                else if (wFMCStation.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fmc_station SET Name=@wName,Code=@wCode,WorkShopID=@wWorkShopID,LineID=@wLineID,AreaID=@wAreaID,CreatorID=@wCreatorID,CreateTime=@wCreateTime,Active=@wActive,EditorID=@wEditorID,EditTime=@wEditTime,IPTModuleID=@wIPTModuleID,CERT=@wCERT,ENVIR=@wENVIR,TestMethod=@wTestMethod,IsCalcPD=@wIsCalcPD,Remark=@wRemark,WorkName=@wWorkName WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFMCStation.ID);
                wParms.Add("wName", wFMCStation.Name);
                wParms.Add("wCode", wFMCStation.Code);
                wParms.Add("wWorkShopID", wFMCStation.WorkShopID);
                wParms.Add("wLineID", wFMCStation.LineID);
                wParms.Add("wAreaID", wFMCStation.AreaID);
                wParms.Add("wCreatorID", wFMCStation.CreatorID);
                wParms.Add("wCreateTime", wFMCStation.CreateTime);
                wParms.Add("wActive", wFMCStation.Active);
                wParms.Add("wEditorID", wFMCStation.EditorID);
                wParms.Add("wEditTime", wFMCStation.EditTime);
                wParms.Add("wIPTModuleID", wFMCStation.IPTModuleID);
                wParms.Add("wCERT", wFMCStation.CERT);
                wParms.Add("wENVIR", wFMCStation.ENVIR);
                wParms.Add("wTestMethod", wFMCStation.TestMethod);
                wParms.Add("wIsCalcPD", wFMCStation.IsCalcPD);
                wParms.Add("wRemark", wFMCStation.Remark);
                wParms.Add("wWorkName", wFMCStation.WorkName);


                wSQLText = this.DMLChange(wSQLText);

                if (wFMCStation.ID <= 0)
                    wFMCStation.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_SaveFMCStation)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_SaveFMCStation", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCStationList(List<FMCStation> wFMCStationList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCStationList != null && wFMCStationList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCStationList.Count; i++)
                    {
                        if (i == wFMCStationList.Count - 1)
                            wStringBuilder.Append(wFMCStationList[i].ID);
                        else
                            wStringBuilder.Append(wFMCStationList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fmc_station WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_DeleteFMCStationList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_DeleteFMCStationList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FMCStation> FMC_QueryFMCStationList(int wID, string wName, string wCode, int wLineID, int wActive, Pagination wPagination, out int wErrorCode)
        {
            List<FMCStation> wResultList = new List<FMCStation>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM {0}.fmc_station t "
                    + " left join {0}.mbs_user t1 on t.CreatorID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditorID=t2.ID "
                    + "WHERE 1=1"
                    + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wName is null or @wName = '' or t.Name= @wName)"
                    + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                    + " and(@wLineID <=0 or t.LineID= @wLineID)"
                    + " and(@wActive <0 or t.Active= @wActive)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wName", wName);
                wParms.Add("wCode", wCode);
                wParms.Add("wLineID", wLineID);
                wParms.Add("wActive", wActive);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCStation wFMCStation = new FMCStation();
                    wFMCStation.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFMCStation.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wFMCStation.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wFMCStation.WorkShopID = StringUtils.parseInt(wSqlDataReader["WorkShopID"]);
                    wFMCStation.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                    wFMCStation.AreaID = StringUtils.parseInt(wSqlDataReader["AreaID"]);
                    wFMCStation.CreatorID = StringUtils.parseInt(wSqlDataReader["CreatorID"]);
                    wFMCStation.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFMCStation.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFMCStation.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wFMCStation.EditorID = StringUtils.parseInt(wSqlDataReader["EditorID"]);
                    wFMCStation.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFMCStation.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wFMCStation.IPTModuleID = StringUtils.parseInt(wSqlDataReader["IPTModuleID"]);
                    wFMCStation.CERT = StringUtils.parseString(wSqlDataReader["CERT"]);
                    wFMCStation.ENVIR = StringUtils.parseString(wSqlDataReader["ENVIR"]);
                    wFMCStation.TestMethod = StringUtils.parseString(wSqlDataReader["TestMethod"]);
                    wFMCStation.IsCalcPD = StringUtils.parseInt(wSqlDataReader["IsCalcPD"]);
                    wFMCStation.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wFMCStation.WorkName = StringUtils.parseString(wSqlDataReader["WorkName"]);

                    wResultList.Add(wFMCStation);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_QueryFMCStationList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_QueryFMCStationList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

