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
    public class MSSMaterialPointController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialPointController));

        /// <summary>
        /// by wzj 2022/12/7
        ///  更新料点信息
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wId"> ID（自增）</param>
        /// <param name="wLineID"> 产线ID</param>
        /// <param name="wAssetNo"> 工位编号</param>
        /// <param name="wName"> 工作中心所名称</param>
        /// <param name="wStationPoint"> 工位料点</param>
        /// <param name="wDeliveryPoint"> 送达料点</param>
        /// <param name="wMaterialNo"> 物料编码</param>
        /// <param name="wPagination"></param>
        /// <returns>
        [HttpPost]
        public ActionResult UpdateMaterialPoint()
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

                MSSMaterialPoint wMSSMaterialPoint = CloneTool.Clone<MSSMaterialPoint>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wMSSMaterialPoint.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.MSS_SaveMaterialPoint(wBMSEmployee, wMSSMaterialPoint);
                else
                    wServerRst = ServiceInstance.mFMCService.MSS_AddMaterialPoint(wBMSEmployee, wMSSMaterialPoint);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSMaterialPoint);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wMSSMaterialPoint);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }

        /// <summary>
        /// by wzj 2022/12/7
        ///  删除料点信息
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wId"> ID（自增）</param>
        /// <param name="wLineID"> 产线ID</param>
        /// <param name="wAssetNo"> 工位编号</param>
        /// <param name="wName"> 工作中心所名称</param>
        /// <param name="wStationPoint"> 工位料点</param>
        /// <param name="wDeliveryPoint"> 送达料点</param>
        /// <param name="wMaterialNo"> 物料编码</param>
        /// <param name="wPagination"></param>
        /// <returns>
        [HttpPost]
        public ActionResult DeleteMaterialPointList()
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

                List<MSSMaterialPoint> wMSSMaterialPointList = CloneTool.CloneArray<MSSMaterialPoint>(wParam["data"]);
                if (wMSSMaterialPointList == null || wMSSMaterialPointList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.MSS_DeleteMaterialPointList(wBMSEmployee, wMSSMaterialPointList);

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
        /// <summary>
        /// by wzj 2022/12/7
        ///  料点信息查询
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wId"> ID(自增)</param>
        /// <param name="wLineID"> 产线ID</param>
        /// <param name="wAsseID"> 工位编号</param>
        /// <param name="wName"> 工作中心所名称</param>
        /// <param name="wStationPoint"> 工位料点</param>
        /// <param name="wDeliveryPoint"> 送达料点</param>
        /// <param name="wMaterialNo"> 物料编码</param>
        /// <param name="wPagination"></param>
        /// <returns>
        [HttpGet]
        public ActionResult AllMaterialPoint()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wLineID = StringUtils.parseInt(Request.QueryParamString("LineID"));
                int wAssetID = StringUtils.parseInt(Request.QueryParamString("AssetID"));
                String wName = StringUtils.parseString(Request.QueryParamString("Name"));
                String wStationPoint = StringUtils.parseString(Request.QueryParamString("StationPoint"));
                String wDeliveryPoint = StringUtils.parseString(Request.QueryParamString("DeliveryPoint"));
                String wMaterialNo = StringUtils.parseString(Request.QueryParamString("MaterialNo"));
                int wPlanNo = StringUtils.parseInt(Request.QueryParamString("PlanNo"));
                DateTime wUpdateTime = StringUtils.parseDate(Request.QueryParamString("UpdateTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MSSMaterialPoint>> wServerRst = ServiceInstance.mFMCService.MSS_QueryMaterialPointList(wBMSEmployee, wID, wLineID, wAssetID,wName, wStationPoint, wDeliveryPoint, wMaterialNo, wPlanNo, wUpdateTime, wPagination);

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
        public ActionResult ActiveMaterialPoint()
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

                List<MSSMaterialPoint> wMSSMaterialPointList = CloneTool.CloneArray<MSSMaterialPoint>(wParam["data"]);
                int wActive = StringUtils.parseInt(wParam["Active"]);
                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.MSS_ActiveMaterialPointList(wBMSEmployee, wActive, wMSSMaterialPointList);

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
        //public ActionResult AllStock()
        //{
        //    Dictionary<String, Object> wResult = new Dictionary<String, Object>();
        //    try
        //    {
        //        BMSEmployee wBMSEmployee = GetSession();

        //        String wName = StringUtils.parseString(Request.QueryParamString("Name"));

        //        int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
        //        int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

        //        Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

        //        ServiceResult<List<MSSMaterialPoint>> wServerRst = ServiceInstance.mFMCService.MSS_QueryAllStockMaterialPointList(wBMSEmployee, wName, wPagination);

        //        if (StringUtils.isEmpty(wServerRst.getFaultCode()))
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), wPagination.TotalPage);
        //        }
        //        else
        //        {
        //            wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
        //    }
        //    return Json(wResult);
        //}

        [HttpGet]
        public ActionResult PointDetail()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wMaterialPointID = StringUtils.parseInt(Request.QueryParamString("MaterialPointID"));

                ServiceResult<List<MSSMaterialPointStock>> wServerRst = ServiceInstance.mFMCService.MSS_QueryPointDetail(wBMSEmployee, wMaterialPointID);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
                    this.SetResult(wResult, "CallDetail", wServerRst.CustomResult["CallDetail"]);
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

        //上料点料框物料消耗接口 LoadBinConsume(BinID)

        public ActionResult LoadBinConsume()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wBinID = StringUtils.parseInt(Request.QueryParamString("BinID"));

                ServiceResult<List<MSSMaterialStock>> wServerRst = ServiceInstance.mFMCService.MSS_QueryAllMaterialStockActive(wBMSEmployee, wBinID);

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
