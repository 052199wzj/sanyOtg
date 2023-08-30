using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class OMSDXFAnalysisDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSDXFAnalysisDAO));

        #region 单实例
        private OMSDXFAnalysisDAO() { }
        private static OMSDXFAnalysisDAO _Instance;

        public static OMSDXFAnalysisDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSDXFAnalysisDAO();
                return OMSDXFAnalysisDAO._Instance;
            }
        }
        #endregion

        public List<OMSDXFAnalysis> OMS_QueryOMSDXFAnalysisList(int wID, int wOrderItemID, string wMissionNo, string wSteelNo, string wCasingModel, Pagination wPagination, out int wErrorCode)
        {
            List<OMSDXFAnalysis> wResultList = new List<OMSDXFAnalysis>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = "SELECT * FROM oms_dxf_analysis WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wOrderItemID <=0 or OrderItemID= @wOrderItemID)"
                    + " and (@wMissionNo is null OR @wMissionNo = '' OR  MissionNo LIKE @wMissionNo) "
                    + " and (@wSteelNo is null OR @wSteelNo = '' OR SteelNo LIKE @wSteelNo) "
                    + " and (@wCasingModel is null OR @wCasingModel = '' OR CasingModel LIKE @wCasingModel) " ;

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wOrderItemID", wOrderItemID);
                wParms.Add("wMissionNo", String.IsNullOrWhiteSpace(wMissionNo) ? "" : $"%{wMissionNo}%");
                wParms.Add("wSteelNo", String.IsNullOrWhiteSpace(wSteelNo) ? "" : $"%{wSteelNo}%");
                wParms.Add("wCasingModel", String.IsNullOrWhiteSpace(wCasingModel) ? "" : $"%{wCasingModel}%");

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSDXFAnalysis wOMSDXFAnalysis = new OMSDXFAnalysis();
                    wOMSDXFAnalysis.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSDXFAnalysis.OrderItemID = StringUtils.parseInt(wSqlDataReader["OrderItemID"]);
                    wOMSDXFAnalysis.ProductionLine = StringUtils.parseString(wSqlDataReader["ProductionLine"]);
                    wOMSDXFAnalysis.SortStationNo = StringUtils.parseString(wSqlDataReader["SortStationNo"]);
                    wOMSDXFAnalysis.CutStationNo = StringUtils.parseString(wSqlDataReader["CutStationNo"]);
                    wOMSDXFAnalysis.CasingLocalUrl = StringUtils.parseString(wSqlDataReader["CasingLocalUrl"]);
                    wOMSDXFAnalysis.MissionNo = StringUtils.parseString(wSqlDataReader["MissionNo"]);
                    wOMSDXFAnalysis.SteelNo = StringUtils.parseString(wSqlDataReader["SteelNo"]);
                    wOMSDXFAnalysis.CasingModel = StringUtils.parseString(wSqlDataReader["CasingModel"]);
                    wOMSDXFAnalysis.SteelWidth = StringUtils.parseDouble(wSqlDataReader["SteelWidth"]);
                    wOMSDXFAnalysis.SteelHeight = StringUtils.parseDouble(wSqlDataReader["SteelHeight"]);
                    wOMSDXFAnalysis.SteelThickness = StringUtils.parseDouble(wSqlDataReader["SteelThickness"]);
                    wOMSDXFAnalysis.SteelMaterial = StringUtils.parseString(wSqlDataReader["SteelMaterial"]);
                    wOMSDXFAnalysis.PartsWeight = StringUtils.parseDouble(wSqlDataReader["PartsWeight"]);
                    wOMSDXFAnalysis.SteelWeight = StringUtils.parseDouble(wSqlDataReader["SteelWeight"]);
                    wOMSDXFAnalysis.RemainingWeight = StringUtils.parseDouble(wSqlDataReader["RemainingWeight"]);
                    wOMSDXFAnalysis.IdlingLength = StringUtils.parseDouble(wSqlDataReader["IdlingLength"]);
                    wOMSDXFAnalysis.CutLength = StringUtils.parseDouble(wSqlDataReader["CutLength"]);
                    wOMSDXFAnalysis.CutTime = StringUtils.parseDouble(wSqlDataReader["CutTime"]);
                    wOMSDXFAnalysis.HoleNumber = StringUtils.parseInt(wSqlDataReader["HoleNumber"]);
                    wOMSDXFAnalysis.UseRate = StringUtils.parseDouble(wSqlDataReader["UseRate"]);
                    wOMSDXFAnalysis.UseRate1 = StringUtils.parseDouble(wSqlDataReader["UseRate1"]);
                    wOMSDXFAnalysis.CutBlockNumber = StringUtils.parseInt(wSqlDataReader["CutBlockNumber"]);
                    wOMSDXFAnalysis.CutNozzleNumber = StringUtils.parseInt(wSqlDataReader["CutNozzleNumber"]);
                    wOMSDXFAnalysis.CutNozzleDistance = StringUtils.parseDouble(wSqlDataReader["CutNozzleDistance"]);
                    wOMSDXFAnalysis.CutNumber = StringUtils.parseInt(wSqlDataReader["CutNumber"]);
                    wOMSDXFAnalysis.Compensate = StringUtils.parseDouble(wSqlDataReader["Compensate"]);
                    wOMSDXFAnalysis.NestingDate = StringUtils.parseDate(wSqlDataReader["NestingDate"]);
                    wOMSDXFAnalysis.NestingPerson = StringUtils.parseString(wSqlDataReader["NestingPerson"]);
                    wOMSDXFAnalysis.Result = StringUtils.parseInt(wSqlDataReader["Result"]);
                    wOMSDXFAnalysis.ErrMsg = StringUtils.parseString(wSqlDataReader["ErrMsg"]);
                    wOMSDXFAnalysis.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wResultList.Add(wOMSDXFAnalysis);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_QueryOMSDXFAnalysisList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("OMS_QueryOMSDXFAnalysisList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

