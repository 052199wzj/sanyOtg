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
    public class OMSDXFAnalysisPartsController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSDXFAnalysisPartsController));

        [HttpGet]
        public ActionResult AllDXFAnalysisParts()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wDxfAnalysisID = StringUtils.parseInt(Request.QueryParamString("DxfAnalysisID"));
                String wPlanNo = StringUtils.parseString(Request.QueryParamString("PlanNo"));
                String wPartName = StringUtils.parseString(Request.QueryParamString("PartName"));
                String wPartModel = StringUtils.parseString(Request.QueryParamString("PartModel"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                if (wID <= 0 && wDxfAnalysisID <= 0)
                {
                    List<OMSDXFAnalysisParts> NullArray = new List<OMSDXFAnalysisParts>();
                    //Object[] NullArray = new Object[0];
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", NullArray, null);
                }
                else {
                    ServiceResult<List<OMSDXFAnalysisParts>> wServerRst = ServiceInstance.mFMCService.OMS_QueryDXFAnalysisPartsList(wBMSEmployee, wID, wDxfAnalysisID, wPlanNo, wPartName, wPartModel, wPagination);

                    if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    {
                        wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), wPagination.TotalPage);
                    }
                    else
                    {
                        wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
                    }
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
