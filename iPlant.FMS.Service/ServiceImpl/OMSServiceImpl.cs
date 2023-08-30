using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public class OMSServiceImpl : OMSService
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OMSServiceImpl));
        private static OMSService _instance = new OMSServiceImpl();

        public static OMSService getInstance()
        {
            if (_instance == null)
                _instance = new OMSServiceImpl();

            return _instance;
        }

        public ServiceResult<int> OMS_AuditOrder(BMSEmployee wLoginUser, List<OMSOrder> wResultList)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);

                //OMSOrderDAO.getInstance().OMS_AuditOrder(wLoginUser, wResultList, false, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.ToString();
            }
            return wResult;
        }


        public ServiceResult<int> OMS_OrderPriority(BMSEmployee wLoginUser, int wID, int wNum)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //OMSOrderDAO.getInstance().OMS_OrderPriority(wLoginUser, wID, wNum, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.ToString();
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_ConditionAll(BMSEmployee wLoginUser, int wProductID,
            int wWorkShopID, int wLine, int wCustomerID, string wWBSNo, DateTime wStartTime, DateTime wEndTime,
            int wStatus, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //wResult.Result = OMSOrderDAO.getInstance().OMS_ConditionAll(wLoginUser, wProductID, wWorkShopID, wLine,
                //    wCustomerID, wWBSNo, wStartTime, wEndTime, wStatus, wPagination, wErrorCode);

                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<int> OMS_DeleteCommandList(BMSEmployee wLoginUser, List<OMSCommand> wList)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                OMSCommandDAO.getInstance().OMS_DeleteCommandList(wLoginUser, wList, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                wResult.FaultCode += e.Message;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_DeleteOrderList(BMSEmployee wLoginUser, List<OMSOrder> wList)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //OMSOrderDAO.getInstance().OMS_DeleteOrderList(wLoginUser, wList, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.ToString();
            }
            return wResult;
        }

        public ServiceResult<OMSOrder> OMS_QueryOrderByNo(BMSEmployee wLoginUser, string wOrderNo)
        {
            ServiceResult<OMSOrder> wResult = new ServiceResult<OMSOrder>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_QueryOrderByNo(wLoginUser, wOrderNo, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                wResult.FaultCode += e.Message;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_QueryOrderByStatus(BMSEmployee wLoginUser, int wWorkShopID, int wLineID, List<int> wStateIDList, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_QueryOrderByStatus(wLoginUser, wWorkShopID, wLineID, wStateIDList, wPagination, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }


        public ServiceResult<OMSOrder> OMS_QueryCurrentOrder(BMSEmployee wLoginUser, int wWorkShopID, int wLineID)
        {

            ServiceResult<OMSOrder> wResult = new ServiceResult<OMSOrder>();

            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectCurrentOrder(wLoginUser, wWorkShopID, wLineID, wErrorCode);


                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_JudgeOrderImport(BMSEmployee wLoginUser, List<OMSOrder> wOMSOrderList, out List<OMSOrder> wBadOrderList)
        {

            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            wResult.Result = new List<OMSOrder>();
            wBadOrderList = new List<OMSOrder>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //foreach (OMSOrder wOMSOrder in wOMSOrderList)
                //{

                //    if (wOMSOrder == null || StringUtils.isEmpty(wOMSOrder.PartNo) || StringUtils.isEmpty(wOMSOrder.WBSNo)
                //    || wOMSOrder.StationID <= 0 || StringUtils.isEmpty(wOMSOrder.OrderNo))
                //    {
                //        wOMSOrder.WBSNo = "订单数据数据不完整（工位、车号、订单号）";
                //        wBadOrderList.Add(wOMSOrder);
                //        continue;
                //    }

                //    OMSOrder wOMSOrderDB = OMSOrderDAO.getInstance().OMS_CheckOrder(wLoginUser, wOMSOrder, wErrorCode);
                //    if (wOMSOrderDB != null && wOMSOrderDB.ID > 0)
                //    {
                //        wOMSOrder.WBSNo = "订单数据重复";
                //        wBadOrderList.Add(wOMSOrder);
                //        continue;
                //    }

                //    wResult.Result.Add(wOMSOrder);
                //}
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }



        public ServiceResult<OMSCommand> OMS_SelectCommandByCode(BMSEmployee wLoginUser, string wWBSNo)
        {
            ServiceResult<OMSCommand> wResult = new ServiceResult<OMSCommand>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = OMSCommandDAO.getInstance().OMS_SelectCommandByCode(wLoginUser, wWBSNo, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<OMSCommand> OMS_SelectCommandByID(BMSEmployee wLoginUser, int wID)
        {
            ServiceResult<OMSCommand> wResult = new ServiceResult<OMSCommand>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = OMSCommandDAO.getInstance().OMS_SelectCommandByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<OMSCommand> OMS_SelectCommandByPartNo(BMSEmployee wLoginUser, string PartNo)
        {
            ServiceResult<OMSCommand> wResult = new ServiceResult<OMSCommand>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = OMSCommandDAO.getInstance().OMS_SelectCommandByPartNo(wLoginUser, PartNo, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSCommand>> OMS_SelectCommandList(BMSEmployee wLoginUser,
            int wFactoryID, int wBusinessUnitID, int wWorkShopID, int wCustomerID, int wProductID,
            DateTime wStartTime, DateTime wEndTime)
        {
            ServiceResult<List<OMSCommand>> wResult = new ServiceResult<List<OMSCommand>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = OMSCommandDAO.getInstance().OMS_SelectCommandList(wLoginUser, wFactoryID, wBusinessUnitID,
                    wWorkShopID, wCustomerID, wProductID,
             wStartTime, wEndTime, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_SelectFinishListByTime(BMSEmployee wLoginUser, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectFinishListByTime(wLoginUser, wStartTime, wEndTime, wPagination, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_SelectList(BMSEmployee wLoginUser, int wCommandID, int wFactoryID,
            int wWorkShopID, int wLineID, int wStationID, int wProductID, int wCustomerID, int wTeamID,
            string wPartNo, List<int> wStateIDList, String wOrderNoLike, DateTime wPreStartTime, DateTime wPreEndTime,
            DateTime wRelStartTime, DateTime wRelEndTime, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectList(wLoginUser, -1, wCommandID, "", wFactoryID, wWorkShopID, wLineID,
                //    wStationID, wProductID, wCustomerID, wTeamID, wPartNo, wStateIDList, wOrderNoLike, wPreStartTime,
                //    wPreEndTime, wRelStartTime, wRelEndTime, wPagination, wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_SelectList(BMSEmployee wLoginUser, int wCommandID, String wCommandNo, String wFollowNo, String wOperatorNo, int wFactoryID,
          int wWorkShopID, int wLineID, int wStationID, int wProductID, int wCustomerID, int wTeamID,
          string wPartNo, List<int> wStateIDList, String wOrderNoLike, DateTime wPreStartTime, DateTime wPreEndTime,
          DateTime wRelStartTime, DateTime wRelEndTime, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectList(wLoginUser, -1, wCommandID, "", wCommandNo, wFollowNo, wOperatorNo, wFactoryID, wWorkShopID, wLineID,
                //    wStationID, wProductID, wCustomerID, wTeamID, wPartNo, wStateIDList, wOrderNoLike, wPreStartTime,
                //    wPreEndTime, wRelStartTime, wRelEndTime, wPagination, wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }


        public ServiceResult<Dictionary<String, Dictionary<String, int>>> OMS_SelectOrderPositionList(BMSEmployee wLoginUser, int wLineID, List<String> wOrderNoList)
        {
            ServiceResult<Dictionary<String, Dictionary<String, int>>> wResult = new ServiceResult<Dictionary<String, Dictionary<String, int>>>();
            try
            {
                wResult.Result = new Dictionary<string, Dictionary<String, int>>();

                if (wLineID <= 0 || wOrderNoList == null || wOrderNoList.Count <= 0)
                    return wResult;

                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);



                List<DMSDeviceLedger> wDMSDeviceLedgerList = DMSDeviceLedgerDAO.getInstance().DMS_SelectDeviceLedgerList(wLoginUser, "", "",
                    -1, -1, -1, -1, -1, -1, -1, 1, Pagination.Create(1, int.MaxValue), wErrorCode);

                if (wErrorCode.Result != 0)
                {
                    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                    return wResult;
                }
                if (wDMSDeviceLedgerList == null || wDMSDeviceLedgerList.Count <= 0)
                {
                    return wResult;
                }

                Dictionary<String, DMSDeviceLedger> wDeviceDic = wDMSDeviceLedgerList.GroupBy(p => p.AssetNo).ToDictionary(p => p.Key, p => p.FirstOrDefault());


                foreach (String wOrderNo in wOrderNoList)
                {
                    if (StringUtils.isEmpty(wOrderNo))
                        continue;
                    if (!wResult.Result.ContainsKey(wOrderNo))
                        wResult.Result.Add(wOrderNo, new Dictionary<string, int>());
                    foreach (DMSDeviceLedger wDeviceLedger in wDMSDeviceLedgerList)
                    {
                        if (!wResult.Result[wOrderNo].ContainsKey(wDeviceLedger.Name))
                            wResult.Result[wOrderNo].Add(wDeviceLedger.Name, 0);
                    }

                }

                Dictionary<String, List<String>> wWorkpiecePosition = DMSServiceImpl.getInstance().DMS_GetPositionWorkpieceNo(wLoginUser, wLineID).Result; ;

                Dictionary<String, List<String>> wWorkpiecePositionDic = new Dictionary<string, List<string>>();
                foreach (String wAssetNo in wWorkpiecePosition.Keys)
                {
                    if (StringUtils.isEmpty(wAssetNo))
                        continue;
                    if (!wDeviceDic.ContainsKey(wAssetNo))
                        continue;
                    if (wDeviceDic[wAssetNo] == null || wDeviceDic[wAssetNo].ID <= 0)
                        continue;

                    foreach (String wWorkpieceNo in wWorkpiecePosition[wAssetNo])
                    {
                        if (StringUtils.isEmpty(wWorkpieceNo))
                            continue;
                        if (!wWorkpiecePositionDic.ContainsKey(wWorkpieceNo))
                            wWorkpiecePositionDic.Add(wWorkpieceNo, new List<String>());
                        if (wWorkpiecePositionDic[wWorkpieceNo].Contains(wDeviceDic[wAssetNo].Name))
                            continue;
                        wWorkpiecePositionDic[wWorkpieceNo].Add(wDeviceDic[wAssetNo].Name);
                    }
                }


                List<String> wWorkpieceNoList = new List<string>();
                if (wWorkpiecePosition == null || wWorkpiecePosition.Count <= 0)
                {
                    return wResult;
                }
                foreach (String wAssetNo in wWorkpiecePosition.Keys)
                {
                    wWorkpieceNoList.AddRange(wWorkpiecePosition[wAssetNo]);


                }
                wWorkpieceNoList = wWorkpieceNoList.Distinct().ToList();

                List<QMSWorkpiece> wQMSWorkpieceList = QMSWorkpieceDAO.getInstance().QMS_SelectWorkpieceAll(wLoginUser, wLineID, wWorkpieceNoList, wErrorCode);

                // wQMSWorkpieceList.GroupBy(p=>p.OrderNo).ToDictionary(p=>p.Key,p=>p.GroupBy(p=> wWorkpiecePosition.ContainsKey(  p.WorkpieceNo))

                foreach (QMSWorkpiece wQMSWorkpiece in wQMSWorkpieceList)
                {
                    if (!wWorkpiecePositionDic.ContainsKey(wQMSWorkpiece.WorkpieceNo))
                        continue;
                    if (!wResult.Result.ContainsKey(wQMSWorkpiece.OrderNo))
                        continue;

                    foreach (String wDeviceName in wWorkpiecePositionDic[wQMSWorkpiece.WorkpieceNo])
                    {
                        if (!wResult.Result[wQMSWorkpiece.OrderNo].ContainsKey(wDeviceName))
                            continue;
                        wResult.Result[wQMSWorkpiece.OrderNo][wDeviceName]++;
                    }
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }




        public ServiceResult<Dictionary<String, int>> OMS_SelectStatusCount(BMSEmployee wLoginUser, int wCommandID, int wFactoryID, int wWorkShopID,
              int wLineID, int wStationID, int wProductID, int wCustomerID, int wTeamID, String wPartNo,
              List<Int32> wStateIDList, String wOrderNoLike, DateTime wPreStartTime, DateTime wPreEndTime, DateTime wRelStartTime,
              DateTime wRelEndTime)
        {
            ServiceResult<Dictionary<String, int>> wResult = new ServiceResult<Dictionary<String, int>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectStatusCount(wLoginUser, wCommandID, wFactoryID, wWorkShopID, wLineID,
                //    wStationID, wProductID, wCustomerID, wTeamID, wPartNo, wStateIDList, wOrderNoLike, wPreStartTime,
                //    wPreEndTime, wRelStartTime, wRelEndTime, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_SelectListByIDList(BMSEmployee wLoginUser, List<int> wIDList)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectOrderListByIDList(wLoginUser, wIDList, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrder>> OMS_SelectList_RF(BMSEmployee wLoginUser, int wCustomerID, int wWorkShopID,
            int wLineID, int wProductID, string wPartNo, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectList_RF(wLoginUser, wCustomerID,
                //    wWorkShopID, wLineID, wProductID, wPartNo, wStartTime, wEndTime, wPagination, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<OMSOrder> OMS_SelectOrderByID(BMSEmployee wLoginUser, int wID)
        {
            ServiceResult<OMSOrder> wResult = new ServiceResult<OMSOrder>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                //wResult.Result = OMSOrderDAO.getInstance().OMS_SelectOrderByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<int> OMS_UpdateCommand(BMSEmployee wLoginUser, OMSCommand wOMSCommand)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                OMSCommandDAO.getInstance().OMS_UpdateCommand(wLoginUser, wOMSCommand, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }

        public ServiceResult<int> OMS_UpdateOrder(BMSEmployee wLoginUser, OMSOrder wOMSOrder)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //OMSOrderDAO.getInstance().OMS_UpdateOrder(wLoginUser, wOMSOrder, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.Message;
            }
            return wResult;
        }


        public ServiceResult<List<String>> OMS_SyncOrderList(BMSEmployee wLoginUser, List<OMSOrder> wOrderList)
        {
            ServiceResult<List<String>> wResult = new ServiceResult<List<String>>();
            try
            {
                //wResult.Result = new List<string>();
                //if (wOrderList == null || wOrderList.Count <= 0)
                //    return wResult;

                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //List<String> wOrderNoList = wOrderList.Select(p => p.OrderNo).ToList();


                //List<OMSOrder> wSourceList = OMSOrderDAO.getInstance().OMS_SelectOrderListByOrderNoList(wLoginUser, wOrderNoList, wErrorCode);
                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}

                //List<FMCStation> wStationSourceList = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "", -1, -1, -1, 1, Pagination.MaxSize, wErrorCode);
                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}

                //List<BMSTeamManage> wTeamManageSourceList = BMSTeamManageDAO.getInstance().BMS_GetTeamManageList(wLoginUser, "", -1, -1,
                // -1, -1, -1, Pagination.MaxSize, wErrorCode);
                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}

                //List<BMSEmployee> wEmployeeSourveList = BMSEmployeeDAO.getInstance().BMS_QueryEmployeeAll(wLoginUser, -1, wErrorCode);

                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}

                //Dictionary<string, FMCStation> wStationSourceDic = wStationSourceList.ToDictionary(p => p.Code, p => p);

                //Dictionary<string, Int32> wEmployeeIDSourveDic = wEmployeeSourveList.ToDictionary(p => p.LoginID, p => p.ID);

                //Dictionary<string, BMSTeamManage> wTeamManageSourveDic = wTeamManageSourceList.ToDictionary(p => p.Code, p => p);

                //Dictionary<string, OMSOrder> wSourveDic = wSourceList.ToDictionary(p => p.OrderNo, p => p);


                //int i = 0;
                //foreach (OMSOrder wOrder in wOrderList)
                //{
                //    i++;
                //    if (wOrder == null)
                //    {
                //        wResult.Result.Add(StringUtils.Format("第{0}条数据不完整  !", i));
                //        continue;
                //    }
                //    if (StringUtils.isEmpty(wOrder.OrderNo))
                //    {
                //        wResult.Result.Add(StringUtils.Format("第{0}条数据不完整 OrderNo:{1} !", i,
                //               wOrder.OrderNo));
                //        continue;
                //    }

                //    //检查设备是否存在 不存在提示报错
                //    if (wSourveDic.ContainsKey(wOrder.OrderNo))
                //    {
                //        if (wSourveDic[wOrder.OrderNo].Status > (int)OMSOrderStatus.WeekPlantOrder)
                //        {
                //            wResult.Result.Add(StringUtils.Format("OrderNo:{0} Update Error:Status has been changed, modification not allowed!",
                //                                         wOrder.OrderNo));
                //            continue;
                //        }
                //        wOrder.ID = wSourveDic[wOrder.OrderNo].ID;
                //        wOrder.CommandID = wSourveDic[wOrder.OrderNo].CommandID;
                //    }


                //    if (wTeamManageSourveDic.ContainsKey(wOrder.StationNo))
                //    {
                //        wOrder.StationID = wTeamManageSourveDic[wOrder.StationNo].ID;
                //    }
                //    else
                //    {
                //        wResult.Result.Add(StringUtils.Format("OrderNo:{0} StationNo：{1}  Error:Station Not Found!",
                //               wOrder.OrderNo, wOrder.StationNo));
                //        continue;
                //    }
                //    if (wTeamManageSourveDic.ContainsKey(wOrder.TeamNo))
                //    {
                //        wOrder.TeamID = wTeamManageSourveDic[wOrder.TeamNo].ID;
                //    }
                //    else
                //    {
                //        wResult.Result.Add(StringUtils.Format("OrderNo:{0} TeamNo：{1}  Error:Team Not Found!",
                //               wOrder.OrderNo, wOrder.TeamNo));
                //        continue;
                //    }

                //    wOrder.WorkerIDList = StringUtils.parseIntList(wOrder.WorkerName, wEmployeeIDSourveDic);


                //    wOrder.Status = (int)OMSOrderStatus.WeekPlantOrder;
                //    wOrder.FactoryID = 1;
                //    wOrder.CustomerID = 1;
                //    wOrder.LineID = 0;


                //    OMSOrderDAO.getInstance().OMS_UpdateOrder(wLoginUser, wOrder, wErrorCode);
                //    if (wErrorCode.Result != 0)
                //    {
                //        wResult.Result.Add(StringUtils.Format("OrderNo:{0} Update Error:{1}",
                //            wOrder.OrderNo, MESException.getEnumType(wErrorCode.get()).getLabel()));

                //    }

                //}
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wResult.FaultCode += e.ToString();
            }
            return wResult;
        }

        //public ServiceResult<List<String>> OMS_SyncOrderChangeList(BMSEmployee wLoginUser, List<OMSOrder> wOrderList)
        //{
        //    ServiceResult<List<String>> wResult = new ServiceResult<List<String>>();
        //    try
        //    {
        //        wResult.Result = new List<string>();
        //        if (wOrderList == null || wOrderList.Count <= 0)
        //            return wResult;

        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

        //        List<String> wOrderNoList = wOrderList.Select(p => p.OrderNo).ToList();


        //        List<OMSOrder> wSourceList = OMSOrderDAO.getInstance().OMS_SelectOrderListByOrderNoList(wLoginUser, wOrderNoList, wErrorCode);
        //        if (wErrorCode.Result != 0)
        //        {
        //            wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
        //            return wResult;
        //        }

        //        List<FMCStation> wStationSourceList = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "", -1, -1, -1, 1, Pagination.MaxSize, wErrorCode);
        //        if (wErrorCode.Result != 0)
        //        {
        //            wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
        //            return wResult;
        //        }

        //        List<BMSTeamManage> wTeamManageSourceList = BMSTeamManageDAO.getInstance().BMS_GetTeamManageList(wLoginUser, "", -1, -1,
        //         -1, -1, -1, Pagination.MaxSize, wErrorCode);
        //        if (wErrorCode.Result != 0)
        //        {
        //            wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
        //            return wResult;
        //        }

        //        List<BMSEmployee> wEmployeeSourveList = BMSEmployeeDAO.getInstance().BMS_QueryEmployeeAll(wLoginUser, -1, wErrorCode);

        //        if (wErrorCode.Result != 0)
        //        {
        //            wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
        //            return wResult;
        //        }

        //        Dictionary<string, FMCStation> wStationSourceDic = wStationSourceList.ToDictionary(p => p.Code, p => p);

        //        Dictionary<string, Int32> wEmployeeIDSourveDic = wEmployeeSourveList.ToDictionary(p => p.LoginID, p => p.ID);

        //        Dictionary<string, BMSTeamManage> wTeamManageSourveDic = wTeamManageSourceList.ToDictionary(p => p.Code, p => p);

        //        Dictionary<string, OMSOrder> wSourveDic = wSourceList.ToDictionary(p => p.OrderNo, p => p);


        //        int i = 0;
        //        foreach (OMSOrder wOrder in wOrderList)
        //        {
        //            i++;
        //            if (wOrder == null)
        //            {
        //                wResult.Result.Add(StringUtils.Format("第{0}条数据不完整  !", i));
        //                continue;
        //            }
        //            if (StringUtils.isEmpty(wOrder.OrderNo))
        //            {
        //                wResult.Result.Add(StringUtils.Format("第{0}条数据不完整 OrderNo:{1} !", i,
        //                       wOrder.OrderNo));
        //                continue;
        //            }

        //            //检查设备是否存在 不存在提示报错
        //            if (!wSourveDic.ContainsKey(wOrder.OrderNo))
        //            {

        //                wResult.Result.Add(StringUtils.Format("OrderNo:{0} Error:Order Not Found!!",
        //                                             wOrder.OrderNo));
        //                continue;

        //            }


        //            if (wTeamManageSourveDic.ContainsKey(wOrder.StationNo))
        //            {
        //                wSourveDic[wOrder.StationNo].StationID = wTeamManageSourveDic[wOrder.StationNo].ID;
        //            }

        //            if (wTeamManageSourveDic.ContainsKey(wOrder.TeamNo))
        //            {
        //                wSourveDic[wOrder.StationNo].TeamID = wTeamManageSourveDic[wOrder.TeamNo].ID;
        //            }


        //            wOrder.WorkerIDList = StringUtils.parseIntList(wOrder.WorkerName, wEmployeeIDSourveDic);
        //            if (wOrder.WorkerIDList != null && wOrder.WorkerIDList.Count > 0)
        //            {
        //                wSourveDic[wOrder.StationNo].WorkerIDList = wOrder.WorkerIDList;
        //            }

        //            DateTime wBaseTime = new DateTime(2020, 1, 1);

        //            if (wOrder.PlanFinishDate > wBaseTime)
        //            {
        //                wSourveDic[wOrder.StationNo].PlanFinishDate = wOrder.PlanFinishDate;
        //            }
        //            if (wOrder.PlanReceiveDate > wBaseTime)
        //            {
        //                wSourveDic[wOrder.StationNo].PlanReceiveDate = wOrder.PlanReceiveDate;
        //            }
        //            if (wOrder.RealFinishDate > wBaseTime)
        //            {
        //                wSourveDic[wOrder.StationNo].RealFinishDate = wOrder.RealFinishDate;
        //            }
        //            if (wOrder.RealStartDate > wBaseTime)
        //            {
        //                wSourveDic[wOrder.StationNo].RealStartDate = wOrder.RealStartDate;
        //            }
        //            if (wOrder.RealSendDate > wBaseTime)
        //            {
        //                wSourveDic[wOrder.StationNo].RealSendDate = wOrder.RealSendDate;
        //            }



        //            Boolean wIsAllow = false;
        //            switch ((OMSOrderStatus)wSourveDic[wOrder.StationNo].Status)
        //            {
        //                case OMSOrderStatus.Default:
        //                    break;
        //                case OMSOrderStatus.HasOrder:
        //                    break;
        //                case OMSOrderStatus.PlantOrder:
        //                    break;
        //                case OMSOrderStatus.WeekPlantOrder:
        //                    if (wOrder.Status >= (int)OMSOrderStatus.PlantOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.ProductOrder:
        //                    if (wOrder.Status >= (int)OMSOrderStatus.PlantOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.FinishOrder:
        //                    if (wOrder.Status == (int)OMSOrderStatus.SendOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.StopOrder:
        //                    if (wOrder.Status >= (int)OMSOrderStatus.FinishOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.StockOrder:
        //                    if (wOrder.Status == (int)OMSOrderStatus.SendOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.SendOrder:
        //                    if (wOrder.Status == (int)OMSOrderStatus.SendOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.CloseOrder:
        //                    if (wOrder.Status == (int)OMSOrderStatus.CloseOrder)
        //                        wIsAllow = true;
        //                    break;
        //                case OMSOrderStatus.OverOrder:
        //                    break;
        //                default:
        //                    break;
        //            }
        //            if (!wIsAllow)
        //            {

        //                wResult.Result.Add(StringUtils.Format("OrderNo:{0} OldStatus:{1} NewStatus:{2} Error:Status Not Allow Change !!",
        //                                             wOrder.OrderNo, wSourveDic[wOrder.StationNo].Status, wOrder.Status));
        //                continue;
        //            }


        //            wOrder.FactoryID = 1;
        //            wOrder.CustomerID = 1;
        //            wOrder.LineID = 0;

        //            OMSOrderDAO.getInstance().OMS_UpdateOrder(wLoginUser, wOrder, wErrorCode);
        //            if (wErrorCode.Result != 0)
        //            {
        //                wResult.Result.Add(StringUtils.Format("OrderNo:{0} Update Error:{1}",
        //                    wOrder.OrderNo, MESException.getEnumType(wErrorCode.get()).getLabel()));

        //            }

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        wResult.FaultCode += e.ToString();

        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        public ServiceResult<List<OMSLESOrder>> OMS_QueryOMSLESOrderList(BMSEmployee wBMSEmployee, int wID, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wNestID, int wOptionID, double wThickness, int wDXFGetState, int wNCGetState, int wIssueState, int wDisplayed)
        {
            ServiceResult<List<OMSLESOrder>> wResult = new ServiceResult<List<OMSLESOrder>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = OMSLESOrderDAO.Instance.OMS_QueryOMSLESOrderList(wID, wStartTime, wEndTime, wPagination, wNestID, wOptionID, wThickness, wDXFGetState, wNCGetState, wIssueState, wDisplayed, out wErrorCode);
                wResult.Result = wResult.Result.OrderBy(p => p.CreateTime).ToList();
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveOMSLESOrder(BMSEmployee wBMSEmployee, OMSLESOrder wOMSLESOrder)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSLESOrder.EditID = wBMSEmployee.ID;
                wOMSLESOrder.EditTime = DateTime.Now;
                wResult.Result = OMSLESOrderDAO.Instance.OMS_SaveOMSLESOrder(wOMSLESOrder, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<int> OMS_AddOMSLESOrder(BMSEmployee wBMSEmployee, OMSLESOrder wOMSLESOrder)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSLESOrder.CreateID = wBMSEmployee.ID;
                wOMSLESOrder.CreateTime = DateTime.Now;
                wOMSLESOrder.EditID = wBMSEmployee.ID;
                wOMSLESOrder.EditTime = DateTime.Now;
                wResult.Result = OMSLESOrderDAO.Instance.OMS_SaveOMSLESOrder(wOMSLESOrder, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<int> OMS_DeleteOMSLESOrderList(BMSEmployee wBMSEmployee, List<OMSLESOrder> wOMSLESOrderList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = OMSLESOrderDAO.Instance.OMS_DeleteOMSLESOrderList(wOMSLESOrderList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_ActiveOMSLESOrderList(BMSEmployee wBMSEmployee, int wActive, List<OMSLESOrder> wOMSLESOrderList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (OMSLESOrder wOMSLESOrder in wOMSLESOrderList)
                {
                    //wOMSLESOrder.Active = wActive;
                    wOMSLESOrder.EditID = wBMSEmployee.ID;
                    wOMSLESOrder.EditTime = DateTime.Now;
                    OMSLESOrderDAO.Instance.OMS_SaveOMSLESOrder(wOMSLESOrder, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<OMSLESSpareParts>> OMS_QueryOMSLESSparePartsList(BMSEmployee wBMSEmployee, int wID, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wPartID, string wPartName, string wTechnics, string wORD_XLBG, string wABLAD, int wLesOrderID)
        {
            ServiceResult<List<OMSLESSpareParts>> wResult = new ServiceResult<List<OMSLESSpareParts>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = OMSLESSparePartsDAO.Instance.OMS_QueryOMSLESSparePartsList(wID, wStartTime, wEndTime, wPagination, wPartID, wPartName, wTechnics, wORD_XLBG, wABLAD, wLesOrderID, out wErrorCode);
                wResult.Result = wResult.Result.OrderBy(p => p.CreateTime).ToList();
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveOMSLESSpareParts(BMSEmployee wBMSEmployee, OMSLESSpareParts wOMSLESSpareParts)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSLESSpareParts.EditID = wBMSEmployee.ID;
                wOMSLESSpareParts.EditTime = DateTime.Now;
                wResult.Result = OMSLESSparePartsDAO.Instance.OMS_SaveOMSLESSpareParts(wOMSLESSpareParts, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<int> OMS_AddOMSLESSpareParts(BMSEmployee wBMSEmployee, OMSLESSpareParts wOMSLESSpareParts)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSLESSpareParts.CreateID = wBMSEmployee.ID;
                wOMSLESSpareParts.CreateTime = DateTime.Now;
                wOMSLESSpareParts.EditID = wBMSEmployee.ID;
                wOMSLESSpareParts.EditTime = DateTime.Now;
                wResult.Result = OMSLESSparePartsDAO.Instance.OMS_SaveOMSLESSpareParts(wOMSLESSpareParts, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<int> OMS_DeleteOMSLESSparePartsList(BMSEmployee wBMSEmployee, List<OMSLESSpareParts> wOMSLESSparePartsList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = OMSLESSparePartsDAO.Instance.OMS_DeleteOMSLESSparePartsList(wOMSLESSparePartsList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_ActiveOMSLESSparePartsList(BMSEmployee wBMSEmployee, int wActive, List<OMSLESSpareParts> wOMSLESSparePartsList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (OMSLESSpareParts wOMSLESSpareParts in wOMSLESSparePartsList)
                {
                    //wOMSLESOrder.Active = wActive;
                    wOMSLESSpareParts.EditID = wBMSEmployee.ID;
                    wOMSLESSpareParts.EditTime = DateTime.Now;
                    OMSLESSparePartsDAO.Instance.OMS_SaveOMSLESSpareParts(wOMSLESSpareParts, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


    }
}
