using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MCSOperationLogDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MCSOperationLogDAO));

        #region 单实例
        private MCSOperationLogDAO() { }
        private static MCSOperationLogDAO _Instance;

        public static MCSOperationLogDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MCSOperationLogDAO();
                return MCSOperationLogDAO._Instance;
            }
        }
        #endregion

        public int MCS_SaveMCSOperationLog(MCSOperationLog wMCSOperationLog, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMCSOperationLog.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mcs_operationlog(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,ModuleID,Type,Content) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wModuleID,@wType,@wContent);", wInstance);
                else if (wMCSOperationLog.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mcs_operationlog SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,ModuleID=@wModuleID,Type=@wType,Content=@wContent WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMCSOperationLog.ID);
                wParms.Add("wCode", wMCSOperationLog.Code);
                wParms.Add("wName", wMCSOperationLog.Name);
                wParms.Add("wRemark", wMCSOperationLog.Remark);
                wParms.Add("wActive", wMCSOperationLog.Active);
                wParms.Add("wCreateID", wMCSOperationLog.CreateID);
                wParms.Add("wCreateTime", wMCSOperationLog.CreateTime);
                wParms.Add("wEditID", wMCSOperationLog.EditID);
                wParms.Add("wEditTime", wMCSOperationLog.EditTime);
                wParms.Add("wModuleID", wMCSOperationLog.ModuleID);
                wParms.Add("wType", wMCSOperationLog.Type);
                wParms.Add("wContent", wMCSOperationLog.Content);

                wSQLText = this.DMLChange(wSQLText);

                if (wMCSOperationLog.ID <= 0)
                    wMCSOperationLog.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MCS_SaveMCSOperationLog)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MCS_SaveMCSOperationLog", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MCS_DeleteMCSOperationLogList(List<MCSOperationLog> wMCSOperationLogList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMCSOperationLogList != null && wMCSOperationLogList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMCSOperationLogList.Count; i++)
                    {
                        if (i == wMCSOperationLogList.Count - 1)
                            wStringBuilder.Append(wMCSOperationLogList[i].ID);
                        else
                            wStringBuilder.Append(wMCSOperationLogList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mcs_operationlog WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MCS_DeleteMCSOperationLogList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MCS_DeleteMCSOperationLogList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MCSOperationLog> MCS_QueryMCSOperationLogList(int wID, int wModuleID, int wType, string wContent, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<MCSOperationLog> wResultList = new List<MCSOperationLog>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM {0}.mcs_operationlog t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                    + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wModuleID <=0 or t.ModuleID= @wModuleID)"
                    + " and(@wType <=0 or t.Type= @wType)"
                    + " and(@wContent is null or @wContent = '' or t.Content like '%{1}%')"
                    + " and(@wStartTime <= '2010-1-1' or t.CreateTime>= @wStartTime)"
                    + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance, wContent);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wModuleID", wModuleID);
                wParms.Add("wType", wType);
                wParms.Add("wContent", wContent);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                    wMCSOperationLog.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMCSOperationLog.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMCSOperationLog.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMCSOperationLog.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMCSOperationLog.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMCSOperationLog.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMCSOperationLog.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMCSOperationLog.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMCSOperationLog.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wMCSOperationLog.ModuleID = StringUtils.parseInt(wSqlDataReader["ModuleID"]);
                    wMCSOperationLog.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wMCSOperationLog.Content = StringUtils.parseString(wSqlDataReader["Content"]);
                    wMCSOperationLog.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMCSOperationLog.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);

                    wMCSOperationLog.ModuleName = EnumTool.GetEnumDesc<MCSModuleType>(wMCSOperationLog.ModuleID);
                    wMCSOperationLog.TypeText = EnumTool.GetEnumDesc<MCSOperateType>(wMCSOperationLog.Type);

                    wResultList.Add(wMCSOperationLog);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MCS_QueryMCSOperationLogList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MCS_QueryMCSOperationLogList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

