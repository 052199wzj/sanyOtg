using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class OMSUploadRecordDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSUploadRecordDAO));

        #region 单实例
        private OMSUploadRecordDAO() { }
        private static OMSUploadRecordDAO _Instance;

        public static OMSUploadRecordDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSUploadRecordDAO();
                return OMSUploadRecordDAO._Instance;
            }
        }
        #endregion

        public int OMS_SaveOMSUploadRecord(OMSUploadRecord wOMSUploadRecord, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wOMSUploadRecord.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.oms_uploadrecord(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,NCFileUri,NCFileName,NCFileTime,DXFFileUri,DXFFileName,DXFFileTime,Status,Type,ParseFlag,FailReason) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wNCFileUri,@wNCFileName,@wNCFileTime,@wDXFFileUri,@wDXFFileName,@wDXFFileTime,@wStatus,@wType,@wParseFlag,@wFailReason);", wInstance);
                else if (wOMSUploadRecord.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.oms_uploadrecord SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,NCFileUri=@wNCFileUri,NCFileName=@wNCFileName,NCFileTime=@wNCFileTime,DXFFileUri=@wDXFFileUri,DXFFileName=@wDXFFileName,DXFFileTime=@wDXFFileTime,Status=@wStatus,Type=@wType,ParseFlag=@wParseFlag,FailReason=@wFailReason WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wOMSUploadRecord.ID);
                wParms.Add("wCode", wOMSUploadRecord.Code);
                wParms.Add("wName", wOMSUploadRecord.Name);
                wParms.Add("wRemark", wOMSUploadRecord.Remark);
                wParms.Add("wActive", wOMSUploadRecord.Active);
                wParms.Add("wCreateID", wOMSUploadRecord.CreateID);
                wParms.Add("wCreateTime", wOMSUploadRecord.CreateTime);
                wParms.Add("wEditID", wOMSUploadRecord.EditID);
                wParms.Add("wEditTime", wOMSUploadRecord.EditTime);
                wParms.Add("wNCFileUri", wOMSUploadRecord.NCFileUri);
                wParms.Add("wNCFileName", wOMSUploadRecord.NCFileName);
                wParms.Add("wNCFileTime", wOMSUploadRecord.NCFileTime);
                wParms.Add("wDXFFileUri", wOMSUploadRecord.DXFFileUri);
                wParms.Add("wDXFFileName", wOMSUploadRecord.DXFFileName);
                wParms.Add("wDXFFileTime", wOMSUploadRecord.DXFFileTime);
                wParms.Add("wStatus", wOMSUploadRecord.Status);
                wParms.Add("wType", wOMSUploadRecord.Type);
                wParms.Add("wParseFlag", wOMSUploadRecord.ParseFlag);
                wParms.Add("wFailReason", wOMSUploadRecord.FailReason);

                wSQLText = this.DMLChange(wSQLText);

                if (wOMSUploadRecord.ID <= 0)
                    wOMSUploadRecord.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("OMS_SaveOMSUploadRecord", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int OMS_DeleteOMSUploadRecordList(List<OMSUploadRecord> wOMSUploadRecordList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wOMSUploadRecordList != null && wOMSUploadRecordList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wOMSUploadRecordList.Count; i++)
                    {
                        if (i == wOMSUploadRecordList.Count - 1)
                            wStringBuilder.Append(wOMSUploadRecordList[i].ID);
                        else
                            wStringBuilder.Append(wOMSUploadRecordList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.oms_uploadrecord WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_DeleteOMSUploadRecordList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public string GetNewCode(out int wErrorCode)
        {
            string wResult = "";
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQL = StringUtils.Format(
                        "SELECT Code FROM {0}.oms_uploadrecord WHERE id IN( SELECT MAX(ID) FROM {0}.oms_uploadrecord);",
                        wInstance);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms);

                int wNumber = 1;
                int wMonth = DateTime.Now.Month;
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    String wDemandNo = StringUtils.parseString(wSqlDataReader["Code"]);
                    int wCodeMonth = StringUtils.parseInt(wDemandNo.Substring(4, 2));
                    if (wMonth > wCodeMonth)
                        wNumber = 1;
                    else
                        wNumber = StringUtils.parseInt(wDemandNo.Substring(9)) + 1;
                }

                //20220713F001
                wResult = StringUtils.Format("{0}{1}{2}S{3}", DateTime.Now.Year,
                        (DateTime.Now.Month).ToString("00"), DateTime.Now.Day.ToString("00"),
                        wNumber.ToString("000"));
            }
            catch (Exception ex)
            {
                logger.Error("GetNewCode", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public List<OMSUploadRecord> OMS_QueryOMSUploadRecordList(int wID, string wCode, string wNCFileName, string wDXFFileName, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<OMSUploadRecord> wResultList = new List<OMSUploadRecord>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM oms_uploadrecord t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wNCFileName is null or @wNCFileName = '' or t.NCFileName= @wNCFileName)"
                + " and(@wDXFFileName is null or @wDXFFileName = '' or t.DXFFileName= @wDXFFileName)"
                + " and(@wStatus <=0 or t.Status= @wStatus)"
                + " and(@wType <=0 or t.Type= @wType)"
                + " and(@wStartTime <= '2010-1-1' or t.EditTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wNCFileName", wNCFileName);
                wParms.Add("wDXFFileName", wDXFFileName);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wType", wType);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSUploadRecord wOMSUploadRecord = new OMSUploadRecord();
                    wOMSUploadRecord.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSUploadRecord.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wOMSUploadRecord.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wOMSUploadRecord.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wOMSUploadRecord.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wOMSUploadRecord.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wOMSUploadRecord.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wOMSUploadRecord.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wOMSUploadRecord.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wOMSUploadRecord.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wOMSUploadRecord.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wOMSUploadRecord.NCFileUri = StringUtils.parseString(wSqlDataReader["NCFileUri"]);
                    wOMSUploadRecord.NCFileName = StringUtils.parseString(wSqlDataReader["NCFileName"]);
                    wOMSUploadRecord.NCFileTime = StringUtils.parseDate(wSqlDataReader["NCFileTime"]);
                    wOMSUploadRecord.DXFFileUri = StringUtils.parseString(wSqlDataReader["DXFFileUri"]);
                    wOMSUploadRecord.DXFFileName = StringUtils.parseString(wSqlDataReader["DXFFileName"]);
                    wOMSUploadRecord.DXFFileTime = StringUtils.parseDate(wSqlDataReader["DXFFileTime"]);
                    wOMSUploadRecord.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wOMSUploadRecord.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wOMSUploadRecord.ParseFlag = StringUtils.parseInt(wSqlDataReader["ParseFlag"]);
                    wOMSUploadRecord.FailReason = StringUtils.parseString(wSqlDataReader["FailReason"]);

                    wResultList.Add(wOMSUploadRecord);
                }
            }
            catch (Exception ex)
            {
                logger.Error("OMS_QueryOMSUploadRecordList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

