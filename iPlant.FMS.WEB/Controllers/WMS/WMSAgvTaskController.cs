using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.WEB
{
    public class WMSAgvTaskController : BaseController
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(WMSAgvTaskController));

        /// <summary>
        /// by wzj 2022/12/7
        ///  AGV调度信息
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wLineID"> 产线ID</param>
        /// <param name="wLineName"> 产线名称</param>
        /// <param name="wDeviceID"> 设备ID</param>
        /// <param name="wSourcePositionID"> 起始位置ID</param>
        /// <param name="wTaskType"> 任务类型</param>
        /// 
        /// <param name="wStatus"> 任务状态  </param>
        /// 
        /// <param name="wPagination"></param>
        /// <returns>
        [HttpGet]
        public ActionResult All()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {

                BMSEmployee wBMSEmployee = GetSession();

                int wLineID = StringUtils.parseInt(Request.QueryParamString("LineID"));
                int wDeviceID = StringUtils.parseInt(Request.QueryParamString("DeviceID"));
                String wDeviceLike = StringUtils.parseString(Request.QueryParamString("DeviceNo"));
                int wSourcePositionID = StringUtils.parseInt(Request.QueryParamString("SourcePositionID"));
                string wWorkCenterName = StringUtils.parseString(Request.QueryParamString("WorkCenterName"));
                String wSourcePositionLike = StringUtils.parseString(Request.QueryParamString("SourcePosition"));
                int wTaskType = StringUtils.parseInt(Request.QueryParamString("TaskType"));
                //List<int> wStatus = StringUtils.parseIntList(Request.QueryParamString("Status"), ",");
                int wStatus = StringUtils.parseInt(Request.QueryParamString("Status"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));


                String wSort = StringUtils.parseString(Request.QueryParamString("Sort"));
                if (StringUtils.isEmpty(wSort))
                    wSort = "EditTime";
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize, wSort);

                ServiceResult<List<WMSAgvTask>> wServiceResult = ServiceInstance.mWMSService.WMS_SelectAgvTaskAll(wBMSEmployee, wLineID, wWorkCenterName,wDeviceID, wDeviceLike,
                        wSourcePositionID, wSourcePositionLike, wTaskType, wStatus, wStartTime, wEndTime, wPagination);

                if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, wPagination.TotalPage);
                    SetResult(wResult, "Header", wServiceResult.Get("Header"));
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }


        /// <summary>
        /// 获取设备实时任务信息
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult Device()
        //{
        //    Dictionary<String, Object> wResult = new Dictionary<String, Object>();
        //    try
        //    {

        //        BMSEmployee wBMSEmployee = GetSession();

        //        int wLineID = StringUtils.parseInt(Request.QueryParamString("LineID"));
        //        int wDeviceID = StringUtils.parseInt(Request.QueryParamString("DeviceID"));
        //        String wDeviceLike = StringUtils.parseString(Request.QueryParamString("DeviceNo"));
        //        int wSourcePositionID = StringUtils.parseInt(Request.QueryParamString("SourcePositionID"));
        //        String wSourcePositionLike = StringUtils.parseString(Request.QueryParamString("SourcePosition"));
        //        int wTaskType = StringUtils.parseInt(Request.QueryParamString("TaskType"));
        //        string wWorkCenterName = StringUtils.parseString(Request.QueryParamString("WorkCenterName"));

        //        ServiceResult<List<WMSAgvTask>> wServiceResult = ServiceInstance.mWMSService.WMS_SelectAgvCurrentTaskAll(wBMSEmployee, wLineID, wWorkCenterName, wDeviceID, wDeviceLike,
        //                wSourcePositionID, wSourcePositionLike, wTaskType);

        //        if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServiceResult.Result, null);
        //            SetResult(wResult, "Header", wServiceResult.Get("Header"));
        //        }
        //        else
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
        //    }
        //    return Json(wResult);
        //}


        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Info()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {

                BMSEmployee wBMSEmployee = GetSession();
                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wCode = StringUtils.parseString(Request.QueryParamString("Code"));
                ServiceResult<WMSAgvTask> wServiceResult = ServiceInstance.mWMSService.WMS_SelectAgvTask(wBMSEmployee, wID, wCode);

                if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServiceResult.Result);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }


        /// <summary>
        /// 修改任务信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {

                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                BMSEmployee wLoginUser = this.GetSession();

                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                WMSAgvTask wWMSAgvTask = CloneTool.Clone<WMSAgvTask>(wParam["data"]);
                if (wWMSAgvTask == null)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }


                ServiceResult<int> wServiceResult = ServiceInstance.mWMSService.WMS_UpdateAgvTask(wLoginUser, wWMSAgvTask);

                if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wWMSAgvTask);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }


        /// <summary>
        /// 修改任务状态信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateStatus()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {

                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                BMSEmployee wLoginUser = this.GetSession();

                if (!wParam.ContainsKey("Status") || (!wParam.ContainsKey("TaskID") && !wParam.ContainsKey("TaskCode")))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                int wTaskID = wParam.ContainsKey("TaskID") ? StringUtils.parseInt(wParam["TaskID"]) : 0;
                String wTaskCode = wParam.ContainsKey("TaskCode") ? StringUtils.parseString(wParam["TaskCode"]) : "";
                int wStatus = StringUtils.parseInt(wParam["Status"]);

                DateTime wStatusTime = wParam.ContainsKey("StatusTime") ? StringUtils.parseDate(wParam["StatusTime"]) : DateTime.Now;
                int wConfirmerID = wParam.ContainsKey("ConfirmerID") ? StringUtils.parseInt(wParam["ConfirmerID"]) : 0;
                String wTargetPositionCode = wParam.ContainsKey("TargetPositionCode") ? StringUtils.parseString(wParam["TargetPositionCode"]) : "";

                ServiceResult<int> wServiceResult = ServiceInstance.mWMSService.WMS_UpdateAgvTaskStatus(wLoginUser,
                    wTaskID, wTaskCode, wStatus, wStatusTime, wConfirmerID, wTargetPositionCode);


                if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "");
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode());
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }


        /// <summary>

    }
}
