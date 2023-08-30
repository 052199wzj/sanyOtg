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
    public class INFLesOnCompletionDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFLesOnCompletionDAO));

        #region 单实例
        private INFLesOnCompletionDAO() { }
        private static INFLesOnCompletionDAO _Instance;

        public static INFLesOnCompletionDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFLesOnCompletionDAO();
                return INFLesOnCompletionDAO._Instance;
            }
        }
        #endregion

        public List<INFLesOnCompletion> INF_QueryINFLesOnCompletionList(
            int wID, String wOrderId, String wSeqNo, int wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFLesOnCompletion> wResultList = new List<INFLesOnCompletion>();
            wErrorCode = 0;

            try
            {

                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_les_oncompletion t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wOrderId is null OR @wOrderId = '' OR t.OrderId LIKE @wOrderId) " +
                    " AND (@wSeqNo is null OR @wSeqNo = '' OR t.SeqNo LIKE @wSeqNo) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wOrderId", String.IsNullOrWhiteSpace(wOrderId) ? "" : $"%{wOrderId}%");
                wParms.Add("wSeqNo", String.IsNullOrWhiteSpace(wSeqNo) ? "" : $"%{wSeqNo}%");
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFLesOnCompletion wItem = new INFLesOnCompletion();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.OrderId = StringUtils.parseString(wSqlDataReader["OrderId"]);
                    wItem.Start=StringUtils.parseBoolean(wSqlDataReader["Start"]);
                    wItem.End = StringUtils.parseBoolean(wSqlDataReader["End"]);
                    wItem.Quality = StringUtils.parseInt(wSqlDataReader["Quality"]);
                    wItem.SeqNo = StringUtils.parseString(wSqlDataReader["SeqNo"]);
                    wItem.Status = StringUtils.parseSByte(wSqlDataReader["Status"]);
                    wItem.ErroMsg = StringUtils.parseString(wSqlDataReader["ErroMsg"]);
                    wItem.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wItem.SendTime = StringUtils.parseDate(wSqlDataReader["SendTime"]);

                    wResultList.Add(wItem);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(
                    StringUtils.Format("{0} ERROR(INF_QueryINFLesOnCompletionList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("INF_QueryINFLesOnCompletionList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
