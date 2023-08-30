using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MSSFeedGroupDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSFeedGroupDAO));

        #region 单实例
        private MSSFeedGroupDAO() { }
        private static MSSFeedGroupDAO _Instance;

        public static MSSFeedGroupDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSFeedGroupDAO();
                return MSSFeedGroupDAO._Instance;
            }
        }
        #endregion

        public int MSS_SaveMSSFeedGroup(MSSFeedGroup wMSSFeedGroup, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMSSFeedGroup.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mss_materialfeedgroup(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime);", wInstance);
                else if (wMSSFeedGroup.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mss_materialfeedgroup SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMSSFeedGroup.ID);
                wParms.Add("wCode", wMSSFeedGroup.Code);
                wParms.Add("wName", wMSSFeedGroup.Name);
                wParms.Add("wRemark", wMSSFeedGroup.Remark);
                wParms.Add("wActive", wMSSFeedGroup.Active);
                wParms.Add("wCreateID", wMSSFeedGroup.CreateID);
                wParms.Add("wCreateTime", wMSSFeedGroup.CreateTime);
                wParms.Add("wEditID", wMSSFeedGroup.EditID);
                wParms.Add("wEditTime", wMSSFeedGroup.EditTime);


                wSQLText = this.DMLChange(wSQLText);

                if (wMSSFeedGroup.ID <= 0)
                    wMSSFeedGroup.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MSS_SaveMSSFeedGroup)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_SaveMSSFeedGroup", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MSS_DeleteMSSFeedGroupList(List<MSSFeedGroup> wMSSFeedGroupList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMSSFeedGroupList != null && wMSSFeedGroupList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMSSFeedGroupList.Count; i++)
                    {
                        if (i == wMSSFeedGroupList.Count - 1)
                            wStringBuilder.Append(wMSSFeedGroupList[i].ID);
                        else
                            wStringBuilder.Append(wMSSFeedGroupList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mss_materialfeedgroup WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MSS_DeleteMSSFeedGroupList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_DeleteMSSFeedGroupList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MSSFeedGroup> MSS_QueryMSSFeedGroupList(int wID, string wCode, string wName, int wActive, Pagination wPagination, out int wErrorCode)
        {
            List<MSSFeedGroup> wResultList = new List<MSSFeedGroup>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = String.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM {0}.mss_materialfeedgroup t "
                   + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                   + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                   + "WHERE 1=1" + " and(@wID <=0 or t.ID= @wID)"
                   + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                   + " and(@wName is null or @wName = '' or t.Name= @wName)"
                   + " and(@wActive < 0 or t.Active= @wActive)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wName", wName);
                wParms.Add("wActive", wActive);


                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSFeedGroup wMSSFeedGroup = new MSSFeedGroup();
                    wMSSFeedGroup.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSFeedGroup.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMSSFeedGroup.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSFeedGroup.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMSSFeedGroup.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMSSFeedGroup.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMSSFeedGroup.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMSSFeedGroup.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMSSFeedGroup.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMSSFeedGroup.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMSSFeedGroup.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);

                    wResultList.Add(wMSSFeedGroup);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(MSS_QueryMSSFeedGroupList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("MSS_QueryMSSFeedGroupList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

