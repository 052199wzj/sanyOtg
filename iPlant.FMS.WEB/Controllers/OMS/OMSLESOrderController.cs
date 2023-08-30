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
    public class OMSLESOrderController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSLESOrderController));

        [HttpPost]
        public ActionResult UpdateOMSLESOrder()
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
                OMSLESOrder wOMSLESOrder = CloneTool.Clone<OMSLESOrder>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();

                if (wOMSLESOrder.ID > 0)
                    wServerRst = ServiceInstance.mOMSService.OMS_SaveOMSLESOrder(wBMSEmployee, wOMSLESOrder);
                else
                    wServerRst = ServiceInstance.mOMSService.OMS_AddOMSLESOrder(wBMSEmployee, wOMSLESOrder);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wOMSLESOrder);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wOMSLESOrder);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteOMSLESOrderList()
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

                List<OMSLESOrder> wOMSLESOrderList = CloneTool.CloneArray<OMSLESOrder>(wParam["data"]);
                if (wOMSLESOrderList == null || wOMSLESOrderList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mOMSService.OMS_DeleteOMSLESOrderList(wBMSEmployee, wOMSLESOrderList);

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
        public ActionResult AllOMSLESOrder()
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

                String wNestID = StringUtils.parseString(Request.QueryParamString("NestID"));
                int wOptionID = StringUtils.parseInt(Request.QueryParamString("OptionID"),-1);
                double wThickness = StringUtils.parseDouble(Request.QueryParamString("Thickness"),-1);
                int wDXFGetState = StringUtils.parseInt(Request.QueryParamString("DXFGetState"));
                int wNCGetState = StringUtils.parseInt(Request.QueryParamString("NCGetState"));
                int wIssueState = StringUtils.parseInt(Request.QueryParamString("IssueState"));
                int wDisplayed = StringUtils.parseInt(Request.QueryParamString("Displayed"));
                ServiceResult<List<OMSLESOrder>> wServerRst = ServiceInstance.mOMSService.OMS_QueryOMSLESOrderList(wBMSEmployee, wID,wStartTime, wEndTime, wPagination, wNestID, wOptionID, wThickness, wDXFGetState, wNCGetState, wIssueState, wDisplayed);

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
        public ActionResult ActiveOMSLESOrder()
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

                List<OMSLESOrder> wOMSLESOrderList = CloneTool.CloneArray<OMSLESOrder>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mOMSService.OMS_ActiveOMSLESOrderList(wBMSEmployee, wActive, wOMSLESOrderList);

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
