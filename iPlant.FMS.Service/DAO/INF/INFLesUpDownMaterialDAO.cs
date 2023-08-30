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
    public class INFLesUpDownMaterialDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFLesUpDownMaterialDAO));

        #region 单实例
        private INFLesUpDownMaterialDAO() { }
        private static INFLesUpDownMaterialDAO _Instance;

        public static INFLesUpDownMaterialDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new INFLesUpDownMaterialDAO();
                return INFLesUpDownMaterialDAO._Instance;
            }
        }
        #endregion


        public List<INFLesUpDownMaterial> INF_QueryINFLesUpDownMaterialList(
            int wID, String wFrameCode, String wNestId, String wOrder, String wProductNo,
            String wSeq, String wStationCode, String wSub, int wUseType, SByte wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<INFLesUpDownMaterial> wResultList = new List<INFLesUpDownMaterial>();
            wErrorCode = 0;

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                String wSQL = String.Format(
                    "SELECT t.* FROM {0}.inf_les_updownmaterial t WHERE 1=1 " +
                    " AND (@wID <=0 OR t.ID= @wID) " +
                    " AND (@wFrameCode is null OR @wFrameCode = '' OR t.FrameCode LIKE @wFrameCode) " +
                    " AND (@wNestId is null OR @wNestId = '' OR t.NestId LIKE @wNestId) " +
                    " AND (@wOrder is null OR @wOrder = '' OR t.Order LIKE @wOrder) " +
                    " AND (@wProductNo is null OR @wProductNo = '' OR t.ProductNo LIKE @wProductNo) " +
                    " AND (@wSeq is null OR @wSeq = '' OR t.Seq LIKE @wSeq) " +
                    " AND (@wStationCode is null OR @wStationCode = '' OR t.StationCode LIKE @wStationCode) " +
                    " AND (@wSub is null OR @wSub = '' OR t.Sub LIKE @wSub) " +
                    " AND (@wUseType < 0 OR t.UseType = @wUseType) " +
                    " AND (@wStatus < 0 OR t.Status = @wStatus) " +
                    " AND (@wStartTime <= '2010-1-1' OR t.CreateTime >= @wStartTime) " +
                    " AND (@wEndTime <= '2010-1-1' OR t.CreateTime <= @wEndTime) ", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wFrameCode", String.IsNullOrWhiteSpace(wFrameCode) ? "" : $"%{wFrameCode}%");
                wParms.Add("wNestId", String.IsNullOrWhiteSpace(wNestId) ? "" : $"%{wNestId}%");
                wParms.Add("wOrder", String.IsNullOrWhiteSpace(wOrder) ? "" : $"%{wOrder}%");
                wParms.Add("wProductNo", String.IsNullOrWhiteSpace(wProductNo) ? "" : $"%{wProductNo}%");
                wParms.Add("wSeq", String.IsNullOrWhiteSpace(wSeq) ? "" : $"%{wSeq}%");
                wParms.Add("wStationCode", String.IsNullOrWhiteSpace(wStationCode) ? "" : $"%{wStationCode}%");
                wParms.Add("wSub", String.IsNullOrWhiteSpace(wSub) ? "" : $"%{wSub}%");
                wParms.Add("wUseType", wUseType);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wStartTime", wStartTime.Date);
                wParms.Add("wEndTime", wEndTime.Date.AddDays(1).AddSeconds(-1));


                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms, wPagination);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    INFLesUpDownMaterial wItem = new INFLesUpDownMaterial();

                    wItem.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wItem.FrameCode = StringUtils.parseString(wSqlDataReader["FrameCode"]);
                    wItem.NestId = StringUtils.parseString(wSqlDataReader["NestId"]);
                    wItem.Order = StringUtils.parseString(wSqlDataReader["Order"]);
                    wItem.ProductNo = StringUtils.parseString(wSqlDataReader["ProductNo"]);
                    wItem.Seq = StringUtils.parseString(wSqlDataReader["Seq"]);
                    wItem.StationCode = StringUtils.parseString(wSqlDataReader["StationCode"]);
                    wItem.Sub = StringUtils.parseString(wSqlDataReader["Sub"]);
                    wItem.UseType = StringUtils.parseInt(wSqlDataReader["UseType"]);
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
