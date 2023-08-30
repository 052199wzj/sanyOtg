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
    public class MCSOperationLogController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MCSOperationLogController));

        [HttpPost]
        public ActionResult UpdateOperationLog()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<String, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                MCSOperationLog wMCSOperationLog = CloneTool.Clone<MCSOperationLog>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wMCSOperationLog.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.MCS_SaveOperationLog(wBMSEmployee, wMCSOperationLog);
                else
                    wServerRst = ServiceInstance.mFMCService.MCS_AddOperationLog(wBMSEmployee, wMCSOperationLog);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMCSOperationLog);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMCSOperationLog);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteOperationLogList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<String, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                List<MCSOperationLog> wMCSOperationLogList = CloneTool.CloneArray<MCSOperationLog>(wParam["data"]);
                if (wMCSOperationLogList == null || wMCSOperationLogList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.MCS_DeleteOperationLogList(wBMSEmployee, wMCSOperationLogList);

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
        public ActionResult AllOperationLog()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wModuleID = StringUtils.parseInt(Request.QueryParamString("ModuleID"));
                int wType = StringUtils.parseInt(Request.QueryParamString("Type"));
                String wContent = StringUtils.parseString(Request.QueryParamString("Content"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));


                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MCSOperationLog>> wServerRst = ServiceInstance.mFMCService.MCS_QueryOperationLogList(wBMSEmployee, wID, wModuleID, wType, wContent, wStartTime, wEndTime, wPagination);

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
        public ActionResult ActiveOperationLog()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<String, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                List<MCSOperationLog> wMCSOperationLogList = CloneTool.CloneArray<MCSOperationLog>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MCS_ActiveOperationLogList(wBMSEmployee, wActive, wMCSOperationLogList);

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
