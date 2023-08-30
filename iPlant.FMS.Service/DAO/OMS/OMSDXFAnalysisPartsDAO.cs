using System;
using System.Collections.Generic;
using System.Text;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class OMSDXFAnalysisPartsDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSDXFAnalysisPartsDAO));

        #region 单实例
        private OMSDXFAnalysisPartsDAO() { }
        private static OMSDXFAnalysisPartsDAO _Instance;

        public static OMSDXFAnalysisPartsDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new OMSDXFAnalysisPartsDAO();
                return OMSDXFAnalysisPartsDAO._Instance;
            }
        }
        #endregion

        public List<OMSDXFAnalysisParts> OMS_QueryOMSDXFAnalysisPartsList(int wID, int wDxfAnalysisID, string wPlanNo, string wPartName, string wPartModel, Pagination wPagination, out int wErrorCode)
        {
            List<OMSDXFAnalysisParts> wResultList = new List<OMSDXFAnalysisParts>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = "SELECT * FROM oms_dxf_analysis_parts WHERE 1=1"
                    + " and(@wID <=0 or ID= @wID)"
                    + " and(@wDxfAnalysisID <=0 or DxfAnalysisID= @wDxfAnalysisID)"
                    + " and (@wPlanNo is null OR @wPlanNo = '' OR PlanNo LIKE @wPlanNo) "
                    + " and (@wPartName is null OR @wPartName = '' OR PartName LIKE @wPartName) "
                    + " and (@wPartModel is null OR @wPartModel = '' OR PartModel LIKE @wPartModel) ";

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wDxfAnalysisID", wDxfAnalysisID);
                wParms.Add("wPlanNo", String.IsNullOrWhiteSpace(wPlanNo) ? "" : $"%{wPlanNo}%");
                wParms.Add("wPartName", String.IsNullOrWhiteSpace(wPartName) ? "" : $"%{wPartName}%");
                wParms.Add("wPartModel", String.IsNullOrWhiteSpace(wPartModel) ? "" : $"%{wPartModel}%");

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    OMSDXFAnalysisParts wOMSDXFAnalysisParts = new OMSDXFAnalysisParts();
                    wOMSDXFAnalysisParts.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wOMSDXFAnalysisParts.DxfAnalysisID = StringUtils.parseInt(wSqlDataReader["DxfAnalysisID"]);
                    wOMSDXFAnalysisParts.PartNo = StringUtils.parseInt(wSqlDataReader["PartNo"]);
                    wOMSDXFAnalysisParts.PlanNo = StringUtils.parseString(wSqlDataReader["PlanNo"]);
                    wOMSDXFAnalysisParts.PartName = StringUtils.parseString(wSqlDataReader["PartName"]);
                    wOMSDXFAnalysisParts.PartModel = StringUtils.parseString(wSqlDataReader["PartModel"]);
                    wOMSDXFAnalysisParts.PartNum = StringUtils.parseInt(wSqlDataReader["PartNum"]);
                    wOMSDXFAnalysisParts.PartLenth = StringUtils.parseDouble(wSqlDataReader["PartLenth"]);
                    wOMSDXFAnalysisParts.PartWidth = StringUtils.parseDouble(wSqlDataReader["PartWidth"]);
                    wOMSDXFAnalysisParts.SizeType = StringUtils.parseInt(wSqlDataReader["SizeType"]);
                    wOMSDXFAnalysisParts.PartWeight = StringUtils.parseDouble(wSqlDataReader["PartWeight"]);
                    wOMSDXFAnalysisParts.ProcessRoute = StringUtils.parseString(wSqlDataReader["ProcessRoute"]);
                    wOMSDXFAnalysisParts.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wResultList.Add(wOMSDXFAnalysisParts);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_QueryOMSDXFAnalysisPartsList)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error("OMS_QueryOMSDXFAnalysisPartsList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

