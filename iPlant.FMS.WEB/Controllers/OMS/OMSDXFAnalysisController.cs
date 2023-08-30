using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iPlant.FMS.WEB
{
    public class OMSDXFAnalysisController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSDXFAnalysisController));

        [HttpGet]
        public ActionResult AllDXFAnalysis()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wOrderItemID = StringUtils.parseInt(Request.QueryParamString("OrderItemID"));
                String wMissionNo = StringUtils.parseString(Request.QueryParamString("MissionNo"));
                String wSteelNo = StringUtils.parseString(Request.QueryParamString("SteelNo"));
                String wCasingModel = StringUtils.parseString(Request.QueryParamString("CasingModel"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<OMSDXFAnalysis>> wServerRst = ServiceInstance.mFMCService.OMS_QueryDXFAnalysisList(wBMSEmployee, wID, wOrderItemID, wMissionNo, wSteelNo, wCasingModel, wPagination);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), wPagination.TotalPage);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }
    }
}
