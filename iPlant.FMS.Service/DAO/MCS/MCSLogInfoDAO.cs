using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MCSLogInfoDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MCSLogInfoDAO));

        #region 单实例
        private MCSLogInfoDAO() { }
        private static MCSLogInfoDAO _Instance;

        public static MCSLogInfoDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MCSLogInfoDAO();
                return MCSLogInfoDAO._Instance;
            }
        }
        #endregion

        public int MCS_SaveMCSLogInfo(MCSLogInfo wMCSLogInfo, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMCSLogInfo.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mcs_loginfo(CustomerName,LineName,ProductNo,PartNo,VersionNo,FileName,FilePath,FileType,CreateTime,CreateTimeStr,BOPID,BOMID,SystemType,ProcessName,StepNo,Info) VALUES(@wCustomerName,@wLineName,@wProductNo,@wPartNo,@wVersionNo,@wFileName,@wFilePath,@wFileType,@wCreateTime,@wCreateTimeStr,@wBOPID,@wBOMID,@wSystemType,@wProcessName,@wStepNo,@wInfo);", wInstance);
                else if (wMCSLogInfo.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mcs_loginfo SET CustomerName=@wCustomerName,LineName=@wLineName,ProductNo=@wProductNo,PartNo=@wPartNo,VersionNo=@wVersionNo,FileName=@wFileName,FilePath=@wFilePath,FileType=@wFileType,CreateTime=@wCreateTime,CreateTimeStr=@wCreateTimeStr,BOPID=@wBOPID,BOMID=@wBOMID,SystemType=@wSystemType,ProcessName=@wProcessName,StepNo=@wStepNo,Info=@wInfo WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMCSLogInfo.ID);
                wParms.Add("wCustomerName", wMCSLogInfo.CustomerName);
                wParms.Add("wLineName", wMCSLogInfo.LineName);
                wParms.Add("wProductNo", wMCSLogInfo.ProductNo);
                wParms.Add("wPartNo", wMCSLogInfo.PartNo);
                wParms.Add("wVersionNo", wMCSLogInfo.VersionNo);
                wParms.Add("wFileName", wMCSLogInfo.FileName);
                wParms.Add("wFilePath", wMCSLogInfo.FilePath);
                wParms.Add("wFileType", wMCSLogInfo.FileType);
                wParms.Add("wCreateTime", wMCSLogInfo.CreateTime);
                wParms.Add("wCreateTimeStr", wMCSLogInfo.CreateTimeStr);
                wParms.Add("wBOPID", wMCSLogInfo.BOPID);
                wParms.Add("wBOMID", wMCSLogInfo.BOMID);
                wParms.Add("wSystemType", wMCSLogInfo.SystemType);
                wParms.Add("wProcessName", wMCSLogInfo.ProcessName);
                wParms.Add("wStepNo", wMCSLogInfo.StepNo);
                wParms.Add("wInfo", wMCSLogInfo.Info);
                wSQLText = this.DMLChange(wSQLText);

                if (wMCSLogInfo.ID <= 0)
                    wMCSLogInfo.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("MCS_SaveMCSLogInfo", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MCS_DeleteMCSLogInfoList(List<MCSLogInfo> wMCSLogInfoList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMCSLogInfoList != null && wMCSLogInfoList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMCSLogInfoList.Count; i++)
                    {
                        if (i == wMCSLogInfoList.Count - 1)
                            wStringBuilder.Append(wMCSLogInfoList[i].ID);
                        else
                            wStringBuilder.Append(wMCSLogInfoList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mcs_loginfo WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MCS_DeleteMCSLogInfoList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MCSLogInfo> MCS_QueryMCSLogInfoList(int wID, string wVersionNo, string wFileType, string wSystemType, string wProcessName, string wInfo, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wFileName, out int wErrorCode)
        {
            List<MCSLogInfo> wResultList = new List<MCSLogInfo>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT * FROM {0}.mcs_loginfo WHERE 1=1"
                 + " and(@wID <=0 or ID= @wID)"
                 + " and (@wVersionNo is null OR @wVersionNo = '' or  VersionNo LIKE @wVersionNo) "
                 + " and (@wFileType is null OR @wFileType = '' or  FileType LIKE @wFileType) "
                 + " and (@wProcessName is null OR @wProcessName = '' or  ProcessName LIKE @wProcessName) "
                 + " and (@wInfo is null OR @wInfo = '' or  Info LIKE @wInfo) "
                 + " and (@wSystemType is null OR @wSystemType = '' or  SystemType LIKE @wSystemType) "
                 + " and(@wFileName is null or @wFileName = '' or FileName= @wFileName)"
                 + " and(@wStartTime <= '2010-1-1' or CreateTime>= @wStartTime)"
                 + " and(@wEndTime <= '2010-1-1' or CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);

                wParms.Add("wVersionNo", String.IsNullOrWhiteSpace(wVersionNo) ? "" : $"%{wVersionNo}%");
                wParms.Add("wSystemType", String.IsNullOrWhiteSpace(wSystemType) ? "" : $"%{wSystemType}%");
                wParms.Add("wFileType", String.IsNullOrWhiteSpace(wFileType) ? "" : $"%{wFileType}%");
                wParms.Add("wProcessName", String.IsNullOrWhiteSpace(wProcessName) ? "" : $"%{wProcessName}%");
                wParms.Add("wInfo", String.IsNullOrWhiteSpace(wInfo) ? "" : $"%{wInfo}%");
                wParms.Add("wFileName", wFileName);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MCSLogInfo wMCSLogInfo = new MCSLogInfo();
                    wMCSLogInfo.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMCSLogInfo.CustomerName = StringUtils.parseString(wSqlDataReader["CustomerName"]);
                    wMCSLogInfo.LineName = StringUtils.parseString(wSqlDataReader["LineName"]);
                    wMCSLogInfo.ProductNo = StringUtils.parseString(wSqlDataReader["ProductNo"]);
                    wMCSLogInfo.PartNo = StringUtils.parseString(wSqlDataReader["PartNo"]);
                    wMCSLogInfo.VersionNo = StringUtils.parseString(wSqlDataReader["VersionNo"]);
                    wMCSLogInfo.FileName = StringUtils.parseString(wSqlDataReader["FileName"]);
                    wMCSLogInfo.FilePath = StringUtils.parseString(wSqlDataReader["FilePath"]);
                    wMCSLogInfo.FileType = StringUtils.parseString(wSqlDataReader["FileType"]);
                    wMCSLogInfo.SystemType = StringUtils.parseString(wSqlDataReader["SystemType"]);
                    wMCSLogInfo.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMCSLogInfo.CreateTimeStr = StringUtils.parseString(wSqlDataReader["CreateTimeStr"]);
                    wMCSLogInfo.BOPID = StringUtils.parseInt(wSqlDataReader["BOPID"]);
                    wMCSLogInfo.BOMID = StringUtils.parseString(wSqlDataReader["BOMID"]);
                    wMCSLogInfo.ProcessName = StringUtils.parseString(wSqlDataReader["ProcessName"]);
                    wMCSLogInfo.StepNo = StringUtils.parseInt(wSqlDataReader["StepNo"]);
                    wMCSLogInfo.Info = StringUtils.parseString(wSqlDataReader["Info"]);

                    wResultList.Add(wMCSLogInfo);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MCS_QueryMCSLogInfoList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        public int MCS_WriteContentToDB(string wContent, string wFileType, string wSystemType)
        {
            int wResult = 0;
            try
            {
                int wErrorCode = 0;

                string uuid = Guid.NewGuid().ToString().Replace("-", "");

                string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                int wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex);

                wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex + 1);

                string wPath = wBaseDir + "MyLogs\\" + uuid + ".log";

                File.WriteAllText(wPath, wContent);

                MCSLogInfo wMCSLogInfo = new MCSLogInfo();
                wMCSLogInfo.ID = 0;
                wMCSLogInfo.FileName = uuid + ".log";
                wMCSLogInfo.CreateTime = DateTime.Now;
                wMCSLogInfo.CreateTimeStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                wMCSLogInfo.FilePath = wPath;
                wMCSLogInfo.FileType = wFileType;
                wMCSLogInfo.SystemType = wSystemType;
                MCSLogInfoDAO.Instance.MCS_SaveMCSLogInfo(wMCSLogInfo, out wErrorCode);
                wResult = wMCSLogInfo.ID;
            }
            catch (Exception ex)
            {
                logger.Error("MCS_WriteContentToDB", ex);
            }
            return wResult;
        }

        public MCSLogInfo MCS_AddWriteContentToDB(string wContent, string wFileType, string wSystemType, string wVersionNo, string wProcessName, string wStepNo, string wInfo)
        {
            MCSLogInfo wResult = new MCSLogInfo();
            try
            {
                int wErrorCode = 0;

                //string uuid = Guid.NewGuid().ToString().Replace("-", "");

                string uuid = string.Format("{0}_{1}_{2}{3}{4}", wSystemType, wFileType, DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"));

                string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                int wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex);

                wIndex = wBaseDir.LastIndexOf('\\');
                wBaseDir = wBaseDir.Substring(0, wIndex + 1);

                if (!Directory.Exists(wBaseDir + "MyLogs"))
                    Directory.CreateDirectory(wBaseDir + "MyLogs");

                string wPath = wBaseDir + "MyLogs\\" + uuid + ".log";
                if (File.Exists(wPath))
                {
                    using (StreamWriter fs = new StreamWriter(wPath, true))
                    {
                        fs.WriteLine(DateTime.Now + " -- " + wContent);
                    }
                }
                else
                {
                    File.WriteAllText(wPath, DateTime.Now + " -- " + wContent + "\r\n");
                }

                List<MCSLogInfo> wList = MCSLogInfoDAO.Instance.MCS_QueryMCSLogInfoList(-1, "", "", "", "","",new DateTime(2000, 1, 1), new DateTime(2000, 1, 1), Pagination.MaxSize, uuid + ".log", out wErrorCode);
                if (wList.Count > 0)
                    return wList[0];

                MCSLogInfo wMCSLogInfo = new MCSLogInfo();
                wMCSLogInfo.ID = 0;
                wMCSLogInfo.FileName = uuid + ".log";
                wMCSLogInfo.CreateTime = DateTime.Now;
                wMCSLogInfo.CreateTimeStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                wMCSLogInfo.FilePath = wPath;
                wMCSLogInfo.VersionNo = wVersionNo;
                wMCSLogInfo.FileType = wFileType;
                wMCSLogInfo.SystemType = wSystemType;
                wMCSLogInfo.ProcessName = wProcessName;
                int wStepNoT = 0;
                wMCSLogInfo.StepNo =int.TryParse( wStepNo,out wStepNoT) ? wStepNoT :0;
                wMCSLogInfo.Info = wInfo;
                MCSLogInfoDAO.Instance.MCS_SaveMCSLogInfo(wMCSLogInfo, out wErrorCode);
                wResult = wMCSLogInfo;
            }
            catch (Exception ex)
            {
                logger.Error("MCS_WriteContentToDB", ex);
            }
            return wResult;
        }
    }
}

