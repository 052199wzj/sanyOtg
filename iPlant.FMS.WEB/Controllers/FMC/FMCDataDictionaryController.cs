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
    public class FMCDataDictionaryController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCDataDictionaryController));

        [HttpPost]
        public ActionResult UpdateDataDictionary()
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

                FMCDataDictionary wFMCDataDictionary = CloneTool.Clone<FMCDataDictionary>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wFMCDataDictionary.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.FMC_SaveDataDictionary(wBMSEmployee, wFMCDataDictionary);
                else
                    wServerRst = ServiceInstance.mFMCService.FMC_AddDataDictionary(wBMSEmployee, wFMCDataDictionary);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wFMCDataDictionary);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wFMCDataDictionary);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteDataDictionaryList()
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

                List<FMCDataDictionary> wFMCDataDictionaryList = CloneTool.CloneArray<FMCDataDictionary>(wParam["data"]);
                if (wFMCDataDictionaryList == null || wFMCDataDictionaryList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.FMC_DeleteDataDictionaryList(wBMSEmployee, wFMCDataDictionaryList);

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
        public ActionResult AllDataDictionary()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wCode = StringUtils.parseString(Request.QueryParamString("Code"));
                String wName = StringUtils.parseString(Request.QueryParamString("Name"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                int wType = StringUtils.parseInt(Request.QueryParamString("Type"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<FMCDataDictionary>> wServerRst = ServiceInstance.mFMCService.FMC_QueryDataDictionaryList(wBMSEmployee, wID, wCode, wName, wActive, wType, wPagination);

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
        public ActionResult ActiveDataDictionary()
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

                List<FMCDataDictionary> wFMCDataDictionaryList = CloneTool.CloneArray<FMCDataDictionary>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.FMC_ActiveDataDictionaryList(wBMSEmployee, wActive, wFMCDataDictionaryList);

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
