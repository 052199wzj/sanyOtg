using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FMCDataDictionaryDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCDataDictionaryDAO));

        #region 单实例
        private FMCDataDictionaryDAO() { }
        private static FMCDataDictionaryDAO _Instance;

        public static FMCDataDictionaryDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FMCDataDictionaryDAO();
                return FMCDataDictionaryDAO._Instance;
            }
        }
        #endregion

        public int FMC_SaveFMCDataDictionary(FMCDataDictionary wFMCDataDictionary, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFMCDataDictionary.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fmc_datadictionary(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,OrderID,IsDefault,Type) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wOrderID,@wIsDefault,@wType);", wInstance);
                else if (wFMCDataDictionary.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fmc_datadictionary SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,OrderID=@wOrderID,IsDefault=@wIsDefault,Type=@wType WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFMCDataDictionary.ID);
                wParms.Add("wCode", wFMCDataDictionary.Code);
                wParms.Add("wName", wFMCDataDictionary.Name);
                wParms.Add("wRemark", wFMCDataDictionary.Remark);
                wParms.Add("wActive", wFMCDataDictionary.Active);
                wParms.Add("wCreateID", wFMCDataDictionary.CreateID);
                wParms.Add("wCreateTime", wFMCDataDictionary.CreateTime);
                wParms.Add("wEditID", wFMCDataDictionary.EditID);
                wParms.Add("wEditTime", wFMCDataDictionary.EditTime);
                wParms.Add("wOrderID", wFMCDataDictionary.OrderID);
                wParms.Add("wIsDefault", wFMCDataDictionary.IsDefault);
                wParms.Add("wType", wFMCDataDictionary.Type);

                wSQLText = this.DMLChange(wSQLText);

                if (wFMCDataDictionary.ID <= 0)
                    wFMCDataDictionary.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_SaveFMCDataDictionary)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_SaveFMCDataDictionary", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FMC_DeleteFMCDataDictionaryList(List<FMCDataDictionary> wFMCDataDictionaryList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFMCDataDictionaryList != null && wFMCDataDictionaryList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFMCDataDictionaryList.Count; i++)
                    {
                        if (i == wFMCDataDictionaryList.Count - 1)
                            wStringBuilder.Append(wFMCDataDictionaryList[i].ID);
                        else
                            wStringBuilder.Append(wFMCDataDictionaryList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fmc_datadictionary WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_DeleteFMCDataDictionaryList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_DeleteFMCDataDictionaryList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FMCDataDictionary> FMC_QueryFMCDataDictionaryList(int wID, string wCode, string wName, int wActive, int wType, Pagination wPagination, out int wErrorCode)
        {
            List<FMCDataDictionary> wResultList = new List<FMCDataDictionary>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = String.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM {0}.fmc_datadictionary t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                    + " and(@wID <=0 or t.ID= @wID)"
                    + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                    + " and(@wName is null or @wName = '' or t.Name= @wName)"
                    + " and(@wActive <=0 or t.Active= @wActive)"
                    + " and(@wType <=0 or t.Type= @wType)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wName", wName);
                wParms.Add("wActive", wActive);
                wParms.Add("wType", wType);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FMCDataDictionary wFMCDataDictionary = new FMCDataDictionary();

                    PropertyInfo[] wPropertyInfos = typeof(FMCDataDictionary).GetProperties();
                    foreach (PropertyInfo wPropertyInfo in wPropertyInfos)
                    {
                        if (!wSqlDataReader.ContainsKey(wPropertyInfo.Name))
                            continue;

                        string wTypeP = wPropertyInfo.PropertyType.ToString();
                        switch (wTypeP)
                        {
                            case "System.Int32":
                                wPropertyInfo.SetValue(wFMCDataDictionary, StringUtils.parseInt(wSqlDataReader[wPropertyInfo.Name]));
                                break;
                            case "System.String":
                                wPropertyInfo.SetValue(wFMCDataDictionary, StringUtils.parseString(wSqlDataReader[wPropertyInfo.Name]));
                                break;
                            case "System.DateTime":
                                wPropertyInfo.SetValue(wFMCDataDictionary, StringUtils.parseDate(wSqlDataReader[wPropertyInfo.Name]));
                                break;
                            case "System.Double":
                                wPropertyInfo.SetValue(wFMCDataDictionary, StringUtils.parseDouble(wSqlDataReader[wPropertyInfo.Name]));
                                break;
                            default:
                                break;
                        }
                    }
                    wFMCDataDictionary.IsDefaultText = wFMCDataDictionary.IsDefault == 1 ? "是" : "否";
                    wResultList.Add(wFMCDataDictionary);

                    //wFMCDataDictionary.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    //wFMCDataDictionary.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    //wFMCDataDictionary.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    //wFMCDataDictionary.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    //wFMCDataDictionary.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    //wFMCDataDictionary.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    //wFMCDataDictionary.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    //wFMCDataDictionary.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    //wFMCDataDictionary.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    //wFMCDataDictionary.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);
                    //wFMCDataDictionary.IsDefault = StringUtils.parseInt(wSqlDataReader["IsDefault"]);
                    //wFMCDataDictionary.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    //wFMCDataDictionary.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    //wFMCDataDictionary.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    //wFMCDataDictionary.IsDefaultText = wFMCDataDictionary.IsDefault == 1 ? "是" : "否";

                    //wResultList.Add(wFMCDataDictionary);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(FMC_QueryFMCDataDictionaryList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("FMC_QueryFMCDataDictionaryList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

