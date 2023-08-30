using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Service
{
    public class INFDataManageDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFDataManageDAO));

        #region 单实例
        private INFDataManageDAO() { }
        private static INFDataManageDAO _Instance;

        public static INFDataManageDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFDataManageDAO();
                return INFDataManageDAO._Instance;
            }
        }
        #endregion


        public List<INFDataManage> INF_QueryINFDataManageList(
            int wID, int wSaveTime, int wStatus, Pagination wPagination, out int wErrorCode)
        {
            List<INFDataManage> wResultList = new List<INFDataManage>();
            wErrorCode = 0;
            try
            {

                if (wID == 1 || wID == 2||wID==3)
                {
                    wResultList.AddRange(QueryINFDataManageList(wID, wSaveTime, wStatus, wPagination, out wErrorCode));
                }
                else
                {
                    wResultList.AddRange(QueryINFDataManageList(1, wSaveTime, wStatus, wPagination ,out wErrorCode));
                    wResultList.AddRange(QueryINFDataManageList(2, wSaveTime, wStatus, wPagination, out wErrorCode));
                    wResultList.AddRange(QueryINFDataManageList(3, wSaveTime, wStatus, wPagination, out wErrorCode));
                }

            }
            catch (Exception ex)
            {
                logger.Error("INF_QueryINFDataManageList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
        /// <summary>
        /// 条件查询数据信息
        /// </summary>
        /// <param name="wID"></param>
        /// <param name="wStatus"></param>
        /// <param name="wPagination"></param>
        public List<INFDataManage> QueryINFDataManageList(
            int wID, int wSaveTime, int wStatus, Pagination wPagination, out int wErrorCode)
        {
            List<INFDataManage> wResultList = new List<INFDataManage>();
            wErrorCode = 0;

            try
            {

                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                string wSQLText = "";
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                //wParms.Add("wOrderId", String.IsNullOrWhiteSpace(wOrderId) ? "" : $"%{wOrderId}%");
                //wParms.Add("wSeqNo", String.IsNullOrWhiteSpace(wSeqNo) ? "" : $"%{wSeqNo}%");
                wParms.Add("wSaveTime", wSaveTime);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wCaclTime", DateTime.Now);
                wSQLText = string.Format("UPDATE {0}.inf_datamanage  SET Status=1 WHERE  (@wID <=0 or ID=@wID) And DATE_ADD(CleanTime,INTERVAL Savetime*30 DAY)<@wCaclTime", wInstance);
                wSQLText = this.DMLChange(wSQLText);
                mDBPool.update(wSQLText, wParms);
                switch (wID)
                {
                    case 1:

                        wSQLText = string.Format("SELECT t.* ,"
                        + " (select concat(round(sum(data_length/1024/1024),2),'MB') as data from information_schema.tables where table_schema='{0}' and table_name='dms_device_hisstatus') AS 'DataSize'"
                        + "FROM {0}.inf_datamanage t WHERE 1=1 " +
                        " AND (@wID <=0 OR t.ID= @wID) " +
                        " AND (@wSaveTime<=0 OR t.Savetime=@wSaveTime) " +
                        " AND (@wStatus <=0 OR t.Status = @wStatus) ", wInstance);
                        break;
                    case 2:
                        wSQLText = string.Format("SELECT t.* ,"
                        + " (select concat(round(sum(data_length/1024/1024),2),'MB') as data from information_schema.tables where table_schema='{0}' and table_name='dms_device_hisalarmnew') AS 'DataSize'"
                        + "FROM {0}.inf_datamanage t WHERE 1=1 " +
                        " AND (@wID <=0 OR t.ID= @wID) " +
                        " AND (@wSaveTime<=0 OR t.Savetime=@wSaveTime) " +
                        " AND (@wStatus <=0 OR t.Status = @wStatus) ", wInstance);
                        break;
                    case 3:
                        wSQLText = string.Format("SELECT t.* ,"
                        + " (select concat(round(sum(data_length/1024/1024),2),'MB') as data from information_schema.tables where table_schema='{0}' and table_name='dms_device_processrecord') AS 'DataSize'"
                        + "FROM {0}.inf_datamanage t WHERE 1=1 " +
                        " AND (@wID <=0 OR t.ID= @wID) " +
                        " AND (@wSaveTime<=0 OR t.Savetime=@wSaveTime) " +
                        " AND (@wStatus <=0 OR t.Status = @wStatus) ", wInstance);
                        break;
                    default:
                        break;
                }

            List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFDataManage wItem = new INFDataManage();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.DataType = StringUtils.parseString(wSqlDataReader["DataType"]);
                    wItem.Status = StringUtils.parseSByte(wSqlDataReader["Status"]);
                    wItem.SaveTime = StringUtils.parseInt(wSqlDataReader["SaveTime"]);
                    wItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wItem.CleanTime = StringUtils.parseDate(wSqlDataReader["CleanTime"]);
                    wItem.DataSize = StringUtils.parseString(wSqlDataReader["DataSize"]);
                    wResultList.Add(wItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(INF_QueryINFDataManageList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSanyOTG");
                logger.Error("INF_QueryINFLesOnCompletionList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }

        /// <summary>
        /// 更新数据保存时间
        /// </summary>
        public int INF_SaveINFDataManageList(INFDataManage wINFDataManage, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                wSQLText = string.Format("UPDATE {0}.inf_datamanage SET DataType=@wDataType,SaveTime=@wSaveTime,Status=@wStatus,CreateTime=@wCreateTime,CleanTime=@wCleanTime WHERE ID=@wID", wInstance);
                wParms.Clear();
                wParms.Add("wID", wINFDataManage.ID);
                wParms.Add("wDataType", wINFDataManage.DataType);
                wParms.Add("wSaveTime", wINFDataManage.SaveTime);
                wParms.Add("wStatus", wINFDataManage.Status);
                wParms.Add("wCreateTime", wINFDataManage.CreateTime);
                wParms.Add("wCleanTime", wINFDataManage.CleanTime);
                wSQLText = this.DMLChange(wSQLText);
                mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("INF_SaveINFDataManageList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        /// <summary>
        /// 数据清理
        /// </summary>
        public int INF_CleanData(INFDataManage wINFDataManage, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Clear();
                wParms.Add("wID", wINFDataManage.ID);
                wParms.Add("wDataType", wINFDataManage.DataType);
                wParms.Add("wSaveTime", wINFDataManage.SaveTime);
                wParms.Add("wStatus", wINFDataManage.Status);
                wParms.Add("wCreateTime", wINFDataManage.CreateTime);
                wParms.Add("wCleanTime", wINFDataManage.CleanTime);
                wParms.Add("wCurrentTime", DateTime.Now);
                wParms.Add("wCaclTime", DateTime.Now.AddDays(-wINFDataManage.SaveTime * 30));
                String wSQLText = "";

                switch (wINFDataManage.ID)
                {
                    case 1:
                        wSQLText = string.Format("DELETE FROM {0}.dms_device_detailstatus t  WHERE   t.UpdateDate<=@wCaclTime ", wInstance);
                        wSQLText = this.DMLChange(wSQLText);
                        mDBPool.update(wSQLText, wParms);
                        wSQLText = string.Format("UPDATE {0}.inf_datamanage t SET Status=0,CleanTime=@wCurrentTime WHERE  t.ID=@wID", wInstance);
                        break;
                    case 2:
                        wSQLText = string.Format("DELETE FROM {0}.dms_device_hisalarm t  WHERE  t.UpdateDate<=@wCaclTime", wInstance);
                        wSQLText = this.DMLChange(wSQLText);
                        mDBPool.update(wSQLText, wParms);
                        wSQLText = string.Format("UPDATE {0}.inf_datamanage t SET Status=0,CleanTime=@wCurrentTime WHERE  t.ID=@wID", wInstance);
                        break;
                    case 3:
                        wSQLText = string.Format("DELETE FROM {0}.dms_device_processrecord t  WHERE  t.StartTime<=@wCaclTime ", wInstance);
                        wSQLText = this.DMLChange(wSQLText);
                        mDBPool.update(wSQLText, wParms);
                        wSQLText = string.Format("UPDATE {0}.inf_datamanage t SET Status=0,CleanTime=@wCurrentTime WHERE  t.ID=@wID", wInstance);
                        break;
                    default:
                        break;
                }
                wSQLText = this.DMLChange(wSQLText);
                mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("INF_CleanData", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }
    }
}
