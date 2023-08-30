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
    public class MSSFeedGroupController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSFeedGroupController));

        [HttpPost]
        public ActionResult UpdateFeedGroup()
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

                MSSFeedGroup wMSSFeedGroup = CloneTool.Clone<MSSFeedGroup>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wMSSFeedGroup.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.MSS_SaveFeedGroup(wBMSEmployee, wMSSFeedGroup);
                else
                    wServerRst = ServiceInstance.mFMCService.MSS_AddFeedGroup(wBMSEmployee, wMSSFeedGroup);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSFeedGroup);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMSSFeedGroup);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteFeedGroupList()
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

                List<MSSFeedGroup> wMSSFeedGroupList = CloneTool.CloneArray<MSSFeedGroup>(wParam["data"]);
                if (wMSSFeedGroupList == null || wMSSFeedGroupList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.MSS_DeleteFeedGroupList(wBMSEmployee, wMSSFeedGroupList);

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
        public ActionResult AllFeedGroup()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wCode = StringUtils.parseString(Request.QueryParamString("Code"));
                String wName = StringUtils.parseString(Request.QueryParamString("Name"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));


                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MSSFeedGroup>> wServerRst = ServiceInstance.mFMCService.MSS_QueryFeedGroupList(wBMSEmployee, wID, wCode, wName, wActive, wPagination);

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
        public ActionResult ActiveFeedGroup()
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

                List<MSSFeedGroup> wMSSFeedGroupList = CloneTool.CloneArray<MSSFeedGroup>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MSS_ActiveFeedGroupList(wBMSEmployee, wActive, wMSSFeedGroupList);

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
