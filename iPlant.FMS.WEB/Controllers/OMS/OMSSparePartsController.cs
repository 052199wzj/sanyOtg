using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace iPlant.FMS.WEB
{
    public class OMSSparePartsController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSSparePartsController));

        [HttpPost]
        public ActionResult UpdateSpareParts()
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

                OMSSpareParts wOMSSpareParts = CloneTool.Clone<OMSSpareParts>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();

                if (wOMSSpareParts.PartType <= 0)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "PartTypeID必填！");
                    return Json(wResult);
                }

                if (wOMSSpareParts.LesOrderID <= 0 && wOMSSpareParts.PartType == 2)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "LES订单ID有问题");
                    return Json(wResult);
                }

                if (wOMSSpareParts.OrderID <= 0 && wOMSSpareParts.PartType == 1)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "订单ID有问题");
                    return Json(wResult);
                }

                if (wOMSSpareParts.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.OMS_SaveSpareParts(wBMSEmployee, wOMSSpareParts);
                else
                    wServerRst = ServiceInstance.mFMCService.OMS_AddSpareParts(wBMSEmployee, wOMSSpareParts);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wOMSSpareParts);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wOMSSpareParts);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteSparePartsList()
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

                List<OMSSpareParts> wOMSSparePartsList = CloneTool.CloneArray<OMSSpareParts>(wParam["data"]);
                if (wOMSSparePartsList == null || wOMSSparePartsList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.OMS_DeleteSparePartsList(wBMSEmployee, wOMSSparePartsList);

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
        public ActionResult AllSpareParts()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                int wType = StringUtils.parseInt(Request.QueryParamString("Type"));
                int wOrderID = StringUtils.parseInt(Request.QueryParamString("OrderID"));
                int wLesOrderID = StringUtils.parseInt(Request.QueryParamString("LesOrderID"));
                int wPartType = StringUtils.parseInt(Request.QueryParamString("PartType"));
                string wPlanNumber = StringUtils.parseString(Request.QueryParamString("PlanNumber"));
                string wPieceNo = StringUtils.parseString(Request.QueryParamString("PieceNo"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<OMSSpareParts>> wServerRst = ServiceInstance.mFMCService.OMS_QuerySparePartsList(wBMSEmployee, wID, wActive, wType, wOrderID, wLesOrderID, wPartType, wPlanNumber, wPieceNo, wPagination);

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
        public ActionResult ActiveSpareParts()
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

                List<OMSSpareParts> wOMSSparePartsList = CloneTool.CloneArray<OMSSpareParts>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.OMS_ActiveSparePartsList(wBMSEmployee, wActive, wOMSSparePartsList);

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

        [HttpGet]
        public ActionResult TestQRCode()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                ServiceResult<string> wServerRst = ServiceInstance.mFMCService.OMS_TestQRCode(wBMSEmployee);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
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
