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
    public class INFLesStationStateDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFLesStationStateDAO));

        #region 单实例
        private INFLesStationStateDAO() { }
        private static INFLesStationStateDAO _Instance;

        public static INFLesStationStateDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFLesStationStateDAO();
                return INFLesStationStateDAO._Instance;
            }
        }
        #endregion

        /// <summary>
        /// 条件查询站点状态
        /// </summary>
        /// <param name="wID"></param>
        /// <param name="wPalletCode"></param>
        /// <param name="wStationCode"></param>
        /// <param name="wStationStatus"></param>
        /// <param name="wStatus"></param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wPagination"></param>
        /// <param name="wErrorCode"></param>
        /// <returns></returns>
        public List<INFLesStationState> INF_QueryINFLesStationStateList(
            int wID, String wPalletCode, String wStationCode, SByte wStationStatus, SByte wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFLesStationState> wResultList = new List<INFLesStationState>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_les_stationstate t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wPalletCode is null OR @wPalletCode = '' OR t.PalletCode LIKE @wPalletCode) " +
                    " AND (@wStationCode is null OR @wStationCode = '' OR t.StationCode LIKE @wStationCode) " +
                    " AND (@wStationStatus <= 0 OR t.StationStatus = @wStationStatus) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wPalletCode", String.IsNullOrWhiteSpace(wPalletCode) ? "" : $"%{wPalletCode}%");
                wParms.Add("wStationCode", String.IsNullOrWhiteSpace(wStationCode) ? "" : $"%{wStationCode}%");
                wParms.Add("wStationStatus", wStationStatus);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFLesStationState wItem = new INFLesStationState();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.PalletCode = StringUtils.parseString(wSqlDataReader["PalletCode"]);
                    wItem.StationCode = StringUtils.parseString(wSqlDataReader["StationCode"]);
                    wItem.StationStatus = StringUtils.parseSByte(wSqlDataReader["StationStatus"]);
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
                    StringUtils.Format("{0} ERROR(INF_QueryINFLesStationStateList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("INF_QueryINFLesStationStateList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
