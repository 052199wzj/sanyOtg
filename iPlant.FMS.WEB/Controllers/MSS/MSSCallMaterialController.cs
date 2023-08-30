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
    public class MSSCallMaterialController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSCallMaterialController));

        [HttpGet]
        public ActionResult AllCallMaterial()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wName = StringUtils.parseString(Request.QueryParamString("Name"));
                String wCode = StringUtils.parseString(Request.QueryParamString("Code"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                int wPlateID = StringUtils.parseInt(Request.QueryParamString("PlateID"));
                int wMaterialPointID = StringUtils.parseInt(Request.QueryParamString("MaterialPointID"));
                int wStatus = StringUtils.parseInt(Request.QueryParamString("Status"));
                int wType = StringUtils.parseInt(Request.QueryParamString("Type"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MSSCallMaterial>> wServerRst = ServiceInstance.mFMCService.MSS_QueryCallMaterialList(wBMSEmployee, wID, wName, wCode, wActive, wPlateID, wMaterialPointID, wStatus, wType, wStartTime, wEndTime, wPagination);

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

        //[HttpPost]
        //public ActionResult ManualEntry()
        //{
        //    Dictionary<String, Object> wResult = new Dictionary<String, Object>();
        //    try
        //    {
        //        Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

        //        BMSEmployee wBMSEmployee = GetSession();
        //        if (!wParam.ContainsKey("data"))
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
        //            return Json(wResult);
        //        }

        //        MSSCallMaterial wMSSCallMaterial = CloneTool.Clone<MSSCallMaterial>(wParam["data"]);
        //        ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MSS_ManualEntryCallMaterial(wBMSEmployee, wMSSCallMaterial);

        //        if (StringUtils.isEmpty(wServerRst.getFaultCode()))
        //            wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSCallMaterial);
        //        else
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMSSCallMaterial);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
        //    }
        //    return Json(wResult);
        //}

        //[HttpPost]
        //public ActionResult ManualCallMaterial()
        //{
        //    Dictionary<String, Object> wResult = new Dictionary<String, Object>();
        //    try
        //    {
        //        Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

        //        BMSEmployee wBMSEmployee = GetSession();
        //        if (!wParam.ContainsKey("data"))
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
        //            return Json(wResult);
        //        }

        //        MSSCallMaterial wMSSCallMaterial = CloneTool.Clone<MSSCallMaterial>(wParam["data"]);
        //        ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MSS_ManualCallMaterial(wBMSEmployee, wMSSCallMaterial);

        //        if (StringUtils.isEmpty(wServerRst.getFaultCode()))
        //            wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSCallMaterial);
        //        else
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMSSCallMaterial);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
        //    }
        //    return Json(wResult);
        //}

        //[HttpPost]
        //public ActionResult ConfirmMaterial()
        //{
        //    Dictionary<String, Object> wResult = new Dictionary<String, Object>();
        //    try
        //    {
        //        Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

        //        BMSEmployee wBMSEmployee = GetSession();
        //        if (!wParam.ContainsKey("data"))
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
        //            return Json(wResult);
        //        }

        //        MSSCallMaterial wMSSCallMaterial = CloneTool.Clone<MSSCallMaterial>(wParam["data"]);
        //        ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MSS_ConfirmMaterial(wBMSEmployee, wMSSCallMaterial);

        //        if (StringUtils.isEmpty(wServerRst.getFaultCode()))
        //            wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSCallMaterial);
        //        else
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMSSCallMaterial);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
        //    }
        //    return Json(wResult);
        //}
    }
}
