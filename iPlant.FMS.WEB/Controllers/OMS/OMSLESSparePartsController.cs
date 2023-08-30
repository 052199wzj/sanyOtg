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
    public class OMSLESSparePartsController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSLESSparePartsController));

        [HttpPost]
        public ActionResult UpdateOMSLESSpareParts()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                OMSLESSpareParts wOMSLESSpareParts = CloneTool.Clone<OMSLESSpareParts>(wParam["data"]);

                if (wOMSLESSpareParts.LesOrderID <= 0)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "LES订单ID有问题");
                    return Json(wResult);
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wOMSLESSpareParts.ID > 0)
                    wServerRst = ServiceInstance.mOMSService.OMS_SaveOMSLESSpareParts(wBMSEmployee, wOMSLESSpareParts);
                else
                    wServerRst = ServiceInstance.mOMSService.OMS_AddOMSLESSpareParts(wBMSEmployee, wOMSLESSpareParts);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wOMSLESSpareParts);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wOMSLESSpareParts);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteOMSLESSparePartsList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                List<OMSLESSpareParts> wOMSLESSparePartsList = CloneTool.CloneArray<OMSLESSpareParts>(wParam["data"]);
                if (wOMSLESSparePartsList == null || wOMSLESSparePartsList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mOMSService.OMS_DeleteOMSLESSparePartsList(wBMSEmployee, wOMSLESSparePartsList);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServerRst.Result);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wServerRst.Result);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }

        [HttpGet]
        public ActionResult AllOMSLESSpareParts()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);
                int wLesOrderID = StringUtils.parseInt(Request.QueryParamString("LesOrderID"));
                String wPartID = StringUtils.parseString(Request.QueryParamString("PartID"));
                String wPartName = StringUtils.parseString(Request.QueryParamString("PartName"));
                String wTechnics = StringUtils.parseString(Request.QueryParamString("Technics"));
                String wORD_XLBG = StringUtils.parseString(Request.QueryParamString("ORD_XLBG"));
                String wABLAD = StringUtils.parseString(Request.QueryParamString("ABLAD"));
              
                ServiceResult<List<OMSLESSpareParts>> wServerRst = ServiceInstance.mOMSService.OMS_QueryOMSLESSparePartsList(wBMSEmployee, wID,wStartTime, wEndTime, wPagination, wPartID, wPartName, wTechnics, wORD_XLBG, wABLAD, wLesOrderID);

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

        [HttpPost]
        public ActionResult ActiveOMSLESSpareParts()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                List<OMSLESSpareParts> wOMSLESSparePartsList = CloneTool.CloneArray<OMSLESSpareParts>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mOMSService.OMS_ActiveOMSLESSparePartsList(wBMSEmployee, wActive, wOMSLESSparePartsList);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServerRst.Result);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, null);
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
