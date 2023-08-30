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
    public class INFSortsysSendcasingDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFSortsysSendcasingDAO));

        #region 单实例
        private INFSortsysSendcasingDAO() { }
        private static INFSortsysSendcasingDAO _Instance;

        public static INFSortsysSendcasingDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFSortsysSendcasingDAO();
                return INFSortsysSendcasingDAO._Instance;
            }
        }
        #endregion


        public List<INFSortsysSendcasing> INF_QueryINFSortsysSendcasingList(
         int wID, String wProductionLline, String wSortStationNo, String wCutStationNo, String wMissionNo, SByte wStatus,DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFSortsysSendcasing> wResultList = new List<INFSortsysSendcasing>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_sortsys_sendcasing t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wProductionLline is null OR @wProductionLline = '' OR t.ProductionLline LIKE @wProductionLline) " +
                    " AND (@wSortStationNo is null OR @wSortStationNo = '' OR t.SortStationNo LIKE @wSortStationNo) " +
                    " AND (@wCutStationNo is null OR @wCutStationNo = '' OR t.CutStationNo LIKE @wCutStationNo) " +
                    " AND (@wMissionNo is null OR @wMissionNo = '' OR t.MissionNo LIKE @wMissionNo) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wProductionLline", String.IsNullOrWhiteSpace(wProductionLline) ? "" : $"%{wProductionLline}%");
                wParms.Add("wSortStationNo", String.IsNullOrWhiteSpace(wSortStationNo) ? "" : $"%{wSortStationNo}%");
                wParms.Add("wCutStationNo", String.IsNullOrWhiteSpace(wCutStationNo) ? "" : $"%{wCutStationNo}%");
                wParms.Add("wMissionNo", String.IsNullOrWhiteSpace(wMissionNo) ? "" : $"%{wMissionNo}%");
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));


                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFSortsysSendcasing wItem = new INFSortsysSendcasing();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.ProductionLline = StringUtils.parseString(wSqlDataReader["ProductionLline"]);
                    wItem.SortStationNo = StringUtils.parseString(wSqlDataReader["SortStationNo"]);
                    wItem.CutStationNo = StringUtils.parseString(wSqlDataReader["CutStationNo"]);
                    wItem.CasingLocalUrl = StringUtils.parseString(wSqlDataReader["CasingLocalUrl"]);
                    wItem.MissionNo = StringUtils.parseString(wSqlDataReader["MissionNo"]);
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
                    StringUtils.Format("{0} ERROR(INF_QueryINFLesUpDownMaterialList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("INF_QueryINFLesUpDownMaterialList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
