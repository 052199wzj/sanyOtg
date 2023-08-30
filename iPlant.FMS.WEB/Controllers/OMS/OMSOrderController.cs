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
    public class OMSOrderController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSOrderController));

        [HttpPost]
        public ActionResult UpdateOrder()
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


                OMSOrder wFPCStructuralPart = CloneTool.Clone<OMSOrder>(wParam["data"]);

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wFPCStructuralPart.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.OMS_SaveOrder(wBMSEmployee, wFPCStructuralPart);
                else
                    wServerRst = ServiceInstance.mFMCService.OMS_AddOrder(wBMSEmployee, wFPCStructuralPart);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wFPCStructuralPart);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wFPCStructuralPart);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }

        [HttpPost]
        public ActionResult DeleteOrderList()
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

                List<OMSOrder> wFPCStructuralPartList = CloneTool.CloneArray<OMSOrder>(wParam["data"]);
                if (wFPCStructuralPartList == null || wFPCStructuralPartList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.OMS_DeleteOrderList(wBMSEmployee, wFPCStructuralPartList);

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
        public ActionResult AllOrderList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wCuttingNumber = StringUtils.parseString(Request.QueryParamString("CuttingNumber"));
                String wNestingNumber = StringUtils.parseString(Request.QueryParamString("NestingNumber"));
                int wCutType = StringUtils.parseInt(Request.QueryParamString("CutType"));
                string wPlateMaterialNo = StringUtils.parseString(Request.QueryParamString("PlateMaterialNo"));
                int wStructuralpartID = StringUtils.parseInt(Request.QueryParamString("StructuralpartID"));
                string wGas = StringUtils.parseString(Request.QueryParamString("Gas"));
                string wCuttingMouth = StringUtils.parseString(Request.QueryParamString("CuttingMouth"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                int wOrderType = StringUtils.parseInt(Request.QueryParamString("OrderType"));
                int wDisplayed = StringUtils.parseInt(Request.QueryParamString("Displayed"));



                //if (wOrderType<=0) { 
                //    wResult = GetResult(RetCode.SERVER_CODE_ERR, "订单类型必填");
                //    return Json(wResult);
                //}

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<OMSOrder>> wServerRst = ServiceInstance.mFMCService.OMS_QueryOrderList(wBMSEmployee, wID, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo, wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wPagination, wOrderType, wDisplayed);

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
        public ActionResult UpdateOrderItem()
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

                OMSOrderItem wFPCStructuralPart = CloneTool.Clone<OMSOrderItem>(wParam["data"]);
                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                if (wFPCStructuralPart.ID > 0)
                    wServerRst = ServiceInstance.mFMCService.OMS_SaveOrderItem(wBMSEmployee, wFPCStructuralPart);
                else
                    wServerRst = ServiceInstance.mFMCService.OMS_AddOrderItem(wBMSEmployee, wFPCStructuralPart);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wFPCStructuralPart);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wFPCStructuralPart);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);

        }
        /// <summary>
        /// 删除工单
        /// </summary>
        [HttpPost]
        public ActionResult DeleteOrderItemList()
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

                List<OMSOrderItem> wOrderItemList = CloneTool.CloneArray<OMSOrderItem>(wParam["data"]);
                if (wOrderItemList == null || wOrderItemList.Count < 0)
                {
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT, null, null));
                }

                ServiceResult<Int32> wServerRst = new ServiceResult<Int32>();
                wServerRst = ServiceInstance.mFMCService.OMS_DeleteOrderItemList(wBMSEmployee, wOrderItemList);

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
        public ActionResult AllOrderItemList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wOrderID = StringUtils.parseInt(Request.QueryParamString("OrderID"));
                string wOrderNo = StringUtils.parseString(Request.QueryParamString("OrderNo"));
                int wStatus = StringUtils.parseInt(Request.QueryParamString("Status"));
                String wCuttingNumber = StringUtils.parseString(Request.QueryParamString("CuttingNumber"));
                String wNestingNumber = StringUtils.parseString(Request.QueryParamString("NestingNumber"));
                int wCutType = StringUtils.parseInt(Request.QueryParamString("CutType"));
                string wPlateMaterialNo = StringUtils.parseString(Request.QueryParamString("PlateMaterialNo"));
                int wStructuralpartID = StringUtils.parseInt(Request.QueryParamString("StructuralpartID"));
                string wGas = StringUtils.parseString(Request.QueryParamString("Gas"));
                string wCuttingMouth = StringUtils.parseString(Request.QueryParamString("CuttingMouth"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                int wOrderType = StringUtils.parseInt(Request.QueryParamString("OrderType"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                int wDXFAnalysisStatus = StringUtils.parseInt(Request.QueryParamString("DXFAnalysisStatus"));
                int wLesOrderID = StringUtils.parseInt(Request.QueryParamString("LesOrderID"));
                int wDisplayed = StringUtils.parseInt(Request.QueryParamString("Displayed"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<OMSOrderItem>> wServerRst = ServiceInstance.mFMCService.OMS_QueryOrderItemList(
                    wBMSEmployee, wID, wOrderID, wOrderNo, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo,
                    wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wStatus, wPagination, wOrderType, wActive, wDXFAnalysisStatus, wLesOrderID, wDisplayed);

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

        /// <summary>
        /// 生成工单
        /// </summary>
        [HttpPost]
        public ActionResult GenerateWorkOrder()
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

                int wOrderID = StringUtils.parseInt(wParam["data"]);
                if (wOrderID <= 0)
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT));

                ServiceResult<List<OMSOrderItem>> wServerRst = ServiceInstance.mFMCService.OMS_GenerateWorkOrder(wBMSEmployee, wOrderID);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.Result, null);
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
        /// <summary>
        /// 优先级排序
        /// </summary>
        [HttpPost]
        public ActionResult MoveOrder()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                int wID = StringUtils.parseInt(wParam["OrderItemID"]);
                //移动类型 1：上移 2下移 3移到首 4移到尾
                int wMoveType = StringUtils.parseInt(wParam["MoveType"]);
                List<OMSOrderItem> wDataList = CloneTool.CloneArray<OMSOrderItem>(wParam["DataList"]);

                if (!wParam.ContainsKey("DataList"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }


                ServiceResult<int> wServerRst = ServiceInstance.mFMCService.OMS_QueryOrderList(wBMSEmployee, wID, wMoveType, wDataList);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServerRst.Result);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wServerRst.getResult());
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
        /// 通过设备编号查询未报工的工单集合
        /// </summary>
        [HttpGet]
        public ActionResult OrderItemListByDeviceNo()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                String wDeviceNo = StringUtils.parseString(Request.QueryParamString("DeviceNo"));

                ServiceResult<List<OMSOrderItem>> wServerRst = ServiceInstance.mFMCService.OMS_QueryOrderItemListByDeviceNo(wBMSEmployee, wDeviceNo);

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

        /// <summary>
        /// 通过切割类型找到优先级最高的工单
        /// </summary>
        [HttpGet]
        public ActionResult OrderItemListByCutType()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wCutType = StringUtils.parseInt(Request.QueryParamString("CutType"));

                ServiceResult<OMSOrderItem> wServerRst = ServiceInstance.mFMCService.OMS_QueryOrderItemListByCutType(wBMSEmployee, wCutType);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wServerRst.getResult());
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

        /// <summary>
        /// 通过工单号修改状态
        /// </summary>
        [HttpPost]
        public ActionResult UpdateOrderItemByCode()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("OrderNo") || !wParam.ContainsKey("Status"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                string wOrderNo = StringUtils.parseString(wParam["OrderNo"]);
                int wStatus = StringUtils.parseInt(wParam["Status"]);

                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.OMS_UpdateOrderItemByCode(wBMSEmployee, wOrderNo, wStatus);

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

        /// <summary>
        /// 通过切割编号修改状态
        /// </summary>
        [HttpPost]
        public ActionResult UpdateOrderItemByCuttingNumber()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                if (!wParam.ContainsKey("CuttingNumber") || !wParam.ContainsKey("Status"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                string wCuttingNumber = StringUtils.parseString(wParam["CuttingNumber"]);
                int wStatus = StringUtils.parseInt(wParam["Status"]);

                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.OMS_UpdateOrderItemByCuttingNumber(wBMSEmployee, wCuttingNumber, wStatus);

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
        /// <summary>
        /// 批量激活
        /// </summary>
        [HttpPost]
        public ActionResult ActiveOrderItemList()
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

                List<OMSOrderItem> wOMSOrderItemList = CloneTool.CloneArray<OMSOrderItem>(wParam["data"]);
                int wStatus = StringUtils.parseInt(wParam["Status"]);

                if (wOMSOrderItemList.Count == 0)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "传入参数为空！");
                    return Json(wResult);
                }

                //1已创建、2生产中、3已完成、4激活
                int StatusFlag = 0;
                //解析结果：0 解析中、1解析失败、2磁吸配置失败、3解析成功、4未解析
                int DXFAnalysisStatusFlag = 0;
                for (int i = 0; i < wOMSOrderItemList.Count; i++)
                {
                    if (wOMSOrderItemList[i].Status == 2 || wOMSOrderItemList[i].Status == 3)
                    {
                        StatusFlag = 1;
                    }
                    if (wOMSOrderItemList[i].DXFAnalysisStatus != 3)
                    {
                        DXFAnalysisStatusFlag = 1;
                    }
                }
                if (StatusFlag == 1)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "存在已下发工单！");
                    return Json(wResult);
                }

                if (DXFAnalysisStatusFlag == 1)
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, "存在未解析成功的工单！");
                    return Json(wResult);
                }

                ServiceResult<Int32> wServerRst = ServiceInstance.mFMCService.OMS_ActiveOrderItemLis(
                    wBMSEmployee, wStatus, wOMSOrderItemList);

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


        /// <summary>
        /// by Demin 20221117
        ///  工单数据统计
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wCutType"> 1火焰  2平面 3坡口</param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wStatus">   1已创建、2生产中、3已完成 4已激活</param>
        /// <param name="wPagination"></param>
        /// <param name="wOrderType">  1中控订单  2LES订单</param>
        /// <returns>
        /// wStatus=-1
        /// 获得每日计划工单数量（计划加工钢板数）、计划加工零件总数（当日工单包含零件数据)
        /// 
        /// wStatus=-3
        /// 获得每日实际完成工单数量（计划加工钢板数）、每日已向LES报工数量
        /// </returns>
        [HttpGet]
        public ActionResult OrderItemStatistics()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wStatus = StringUtils.parseInt(Request.QueryParamString("Status"));
                int wCutType = StringUtils.parseInt(Request.QueryParamString("CutType"));
                int wOrderType = StringUtils.parseInt(Request.QueryParamString("OrderType"));
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                string wSortFieldName = Request.QueryParamString("Sort");
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);
                if (wSortFieldName != "")
                    wPagination.Sort = wSortFieldName;
                ServiceResult<OMSOrderItemStatistics> wServerRst = ServiceInstance.mFMCService.OMS_OrderStatistics(
                    wBMSEmployee, wCutType, wStartTime, wEndTime, wStatus, wOrderType, wPagination);

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
    }
}
