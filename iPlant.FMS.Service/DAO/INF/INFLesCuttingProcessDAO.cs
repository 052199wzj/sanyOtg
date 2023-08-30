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
    public class INFLesCuttingProcessDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFLesCuttingProcessDAO));

        #region 单实例
        private INFLesCuttingProcessDAO() { }
        private static INFLesCuttingProcessDAO _Instance;

        public static INFLesCuttingProcessDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFLesCuttingProcessDAO();
                return INFLesCuttingProcessDAO._Instance;
            }
        }
        #endregion

        /// <summary>
        /// 条件查询切割过程
        /// </summary>
        /// <param name="wID"></param>
        /// <param name="wNestId"></param>
        /// <param name="wStatus"></param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public List<INFLesCuttingProcess> INF_QueryINFLesCuttingProcessList(int wID, String wNestId, int wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFLesCuttingProcess> wResultList = new List<INFLesCuttingProcess>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_les_cuttingprocess t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wNestId is null OR @wNestId = '' OR t.NestId LIKE @wNestId) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wNestId", String.IsNullOrWhiteSpace(wNestId) ? "" : $"%{wNestId}%");
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFLesCuttingProcess wItem = new INFLesCuttingProcess();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.NestId = StringUtils.parseString(wSqlDataReader["NestId"]);
                    wItem.Start = StringUtils.parseBoolean(wSqlDataReader["Start"]);
                    wItem.End = StringUtils.parseBoolean(wSqlDataReader["End"]);
                    wItem.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wItem.ErroMsg = StringUtils.parseString(wSqlDataReader["ErroMsg"]);
                    wItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wItem.SendTime = StringUtils.parseDate(wSqlDataReader["SendTime"]);

                    wResultList.Add(wItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(INF_QueryINFLesCuttingProcessList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("INF_QueryINFLesCuttingProcessList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
