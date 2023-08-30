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
    public class INFSortsysContainertakenawayDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFSortsysContainertakenawayDAO));

        #region 单实例
        private INFSortsysContainertakenawayDAO() { }
        private static INFSortsysContainertakenawayDAO _Instance;

        public static INFSortsysContainertakenawayDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFSortsysContainertakenawayDAO();
                return INFSortsysContainertakenawayDAO._Instance;
            }
        }
        #endregion

        public List<INFSortsysContainertakenaway> INF_QueryINFSortsysContainertakenawayList(
         int wID, String wPalletPosition, String wPalletId, SByte wStatus,DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFSortsysContainertakenaway> wResultList = new List<INFSortsysContainertakenaway>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_sortsys_containertakenaway t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wPalletPosition is null OR @wPalletPosition = '' OR t.PalletPosition LIKE @wPalletPosition) " +
                    " AND (@wPalletId is null OR @wPalletId = '' OR t.PalletId LIKE @wPalletId) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wPalletPosition", String.IsNullOrWhiteSpace(wPalletPosition) ? "" : $"%{wPalletPosition}%");
                wParms.Add("wPalletId", String.IsNullOrWhiteSpace(wPalletId) ? "" : $"%{wPalletId}%");
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFSortsysContainertakenaway wItem = new INFSortsysContainertakenaway();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.PalletPosition = StringUtils.parseString(wSqlDataReader["PalletPosition"]);
                    wItem.PalletId = StringUtils.parseString(wSqlDataReader["PalletId"]);
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
                logger.Error("INF_QueryINFSortsysContainertakenawayList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }

            return wResultList;
        }
    }
}
