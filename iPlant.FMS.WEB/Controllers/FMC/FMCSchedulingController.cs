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
    public class FMCSchedulingController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCSchedulingController));

        [HttpPost]
        public ActionResult UpdateScheduling()
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

                FMCScheduling wFMCScheduling = CloneTool.Clone<FMCScheduling>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wFMCScheduling.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.FMC_SaveScheduling(wBMSEmployee, wFMCScheduling);
                else
                    wServerRst = ServiceInstance.mFMCService.FMC_AddScheduling(wBMSEmployee, wFMCScheduling);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wFMCScheduling);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wFMCScheduling);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult ActiveScheduling()
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

                List<FMCScheduling> wFMCSchedulingList = CloneTool.CloneArray<FMCScheduling>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.FMC_ActiveSchedulingList(wBMSEmployee, wActive, wFMCSchedulingList);

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
        public ActionResult AllScheduling()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wSerialNo = StringUtils.parseString(Request.QueryParamString("SerialNo"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                DateTime wQueryDate = StringUtils.parseDate(Request.QueryParamString("QueryDate"));

                ServiceResult<List<FMCScheduling>> wServerRst = ServiceInstance.mFMCService.FMC_QuerySchedulingList(wBMSEmployee, wID, wSerialNo, wActive, wQueryDate);

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

        [HttpPost]
        public ActionResult UpdateSchedulingItemList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                string wString = JsonTool.ObjectToJson(new FMCShiftItem());

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                List<FMCSchedulingItem> wFMCShiftItemList = CloneTool.CloneArray<FMCSchedulingItem>(wParam["data"]);
                if (wFMCShiftItemList == null || wFMCShiftItemList.Count <= 0 || wFMCShiftItemList.Exists(p => p.FMCSchedulingID <= 0))
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));

                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.FMC_UpdateSchedulingItemList(wBMSEmployee, wFMCShiftItemList);

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
        public ActionResult AllSchedulingItem()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wFMCSchedulingID = StringUtils.parseInt(Request.QueryParamString("FMCSchedulingID"));
                int wShiftID = StringUtils.parseInt(Request.QueryParamString("ShiftID"));
                int wStationID = StringUtils.parseInt(Request.QueryParamString("StationID"));
                int wPersonID = StringUtils.parseInt(Request.QueryParamString("PersonID"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));

                ServiceResult<List<FMCSchedulingItem>> wServerRst = ServiceInstance.mFMCService.FMC_QuerySchedulingItemList(wBMSEmployee, wID, wFMCSchedulingID, wStationID, wPersonID, wStartTime, wEndTime, wShiftID);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
                    this.SetResult(wResult, "StationList", wServerRst.CustomResult["StationList"]);
                    this.SetResult(wResult, "DateList", wServerRst.CustomResult["DateList"]);
                }
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }

        [HttpGet]
        public ActionResult CreateTemplate()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                DateTime wStartDate = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndDate = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wShiftID = StringUtils.parseInt(Request.QueryParamString("ShiftID"));

                ServiceResult<List<FMCSchedulingItem>> wServerRst = ServiceInstance.mFMCService.FMC_CreateSchedulingItemTemplate(wBMSEmployee, wStartDate, wEndDate, wShiftID);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
                    this.SetResult(wResult, "StationList", wServerRst.CustomResult["StationList"]);
                    this.SetResult(wResult, "DateList", wServerRst.CustomResult["DateList"]);
                }
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
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
