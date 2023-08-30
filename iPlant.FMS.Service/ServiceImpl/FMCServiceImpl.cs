using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iPlant.FMC.Service
{
    public class FMCServiceImpl : FMCService
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCServiceImpl));

        private static FMCService Instance = null;

        public static FMCService getInstance()
        {
            if (Instance == null)
                Instance = new FMCServiceImpl();

            return Instance;
        }

        public ServiceResult<int> FMC_ActiveSchedulingList(BMSEmployee wBMSEmployee, int wActive, List<FMCScheduling> wFMCSchedulingList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FMCScheduling wFMCScheduling in wFMCSchedulingList)
                {
                    wFMCScheduling.Active = wActive;
                    FMCSchedulingDAO.Instance.FMC_SaveFMCScheduling(wFMCScheduling, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_ActiveShiftItemList(BMSEmployee wLoginUser, int wActive, List<FMCShiftItem> wFMCShiftList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FMCShiftItem wFMCShiftItem in wFMCShiftList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wLoginUser.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    FMCShiftItemDAO.Instance.FMC_SaveFMCShiftItem(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_AddScheduling(BMSEmployee wBMSEmployee, FMCScheduling wFMCScheduling)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFMCScheduling.CreateID = wBMSEmployee.ID;
                wFMCScheduling.CreateTime = DateTime.Now;
                wFMCScheduling.SerialNo = FMCSchedulingDAO.Instance.FMC_GetNewCode(out wErrorCode);
                FMCSchedulingDAO.Instance.FMC_SaveFMCScheduling(wFMCScheduling, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_AddShift(BMSEmployee wLoginUser, FMCShift wFMCShift)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                FMCShiftDAO.Instance.FMC_SaveFMCShift(wFMCShift, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_AddShiftItem(BMSEmployee wLoginUser, FMCShiftItem wFMCShiftItem)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFMCShiftItem.CreateID = wLoginUser.ID;
                wFMCShiftItem.CreateTime = DateTime.Now;
                wFMCShiftItem.EditID = wLoginUser.ID;
                wFMCShiftItem.EditTime = DateTime.Now;
                wResult.Result = FMCShiftItemDAO.Instance.FMC_SaveFMCShiftItem(wFMCShiftItem, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCSchedulingItem>> FMC_CreateSchedulingItemTemplate(BMSEmployee wBMSEmployee, DateTime wStartDate, DateTime wEndDate, int wShiftID)
        {
            ServiceResult<List<FMCSchedulingItem>> wResult = new ServiceResult<List<FMCSchedulingItem>>();
            wResult.Result = new List<FMCSchedulingItem>();
            try
            {
                int wErrorCode = 0;

                int wDays = (int)(wEndDate - wStartDate).TotalDays;

                //①获取工位列表
                Dictionary<int, string> wStationDic = FMCSchedulingDAO.Instance.FMC_QueryStationDic(out wErrorCode);
                wStartDate = new DateTime(wStartDate.Year, wStartDate.Month, wStartDate.Day, 0, 0, 0);

                List<FMCScheduling> wList = FMCSchedulingDAO.Instance.FMC_QueryFMCSchedulingList(-1, "", -1, wStartDate, out wErrorCode);
                //if (wList.Count > 0)
                //{
                //    DateTime wBaseTime = new DateTime(2000, 1, 1);
                //    List<FMCSchedulingItem> wItemList = FMCSchedulingItemDAO.Instance.FMC_QueryFMCSchedulingItemList(-1, wList[0].ID, -1, -1, wBaseTime, wBaseTime, -1, out wErrorCode);
                //    if (wItemList.Count <= 0)
                //    {
                //        FMCSchedulingDAO.Instance.FMC_DeleteFMCSchedulingList(wList);
                //    }
                //    else
                //    {
                //        wResult.FaultCode += String.Format("【{0}】-【{1}】已排班!", wList[0].StartDate.ToString("yyyy/MM/dd"), wList[0].EndDate.ToString("yyyy/MM/dd"));
                //        return wResult;
                //    }
                //}

                if (wList.Count < 0)
                    wList = FMCSchedulingDAO.Instance.FMC_QueryFMCSchedulingList(-1, "", -1, wEndDate, out wErrorCode);

                //if (wList.Count > 0)
                //{
                //    DateTime wBaseTime = new DateTime(2000, 1, 1);
                //    List<FMCSchedulingItem> wItemList = FMCSchedulingItemDAO.Instance.FMC_QueryFMCSchedulingItemList(-1, wList[0].ID, -1, -1, wBaseTime, wBaseTime, -1, out wErrorCode);
                //    if (wItemList.Count <= 0)
                //    {
                //        FMCSchedulingDAO.Instance.FMC_DeleteFMCSchedulingList(wList);
                //    }
                //    else
                //    {
                //        wResult.FaultCode += String.Format("【{0}】-【{1}】已排班!", wList[0].StartDate.ToString("yyyy/MM/dd"), wList[0].EndDate.ToString("yyyy/MM/dd"));
                //        return wResult;
                //    }
                //}

                wEndDate = new DateTime(wEndDate.Year, wEndDate.Month, wEndDate.Day, 23, 59, 59);

                wResult.CustomResult.Add("StationList", wStationDic);
                List<DateTime> wDateList = new List<DateTime>();

                //创建排班记录
                FMCScheduling wFMCScheduling = new FMCScheduling();
                wFMCScheduling.ID = 0;
                if (wList.Count > 0)
                    wFMCScheduling.ID = wList[0].ID;
                wFMCScheduling.Active = 0;
                wFMCScheduling.StartDate = wStartDate;
                wFMCScheduling.EndDate = wEndDate;
                wFMCScheduling.Days = (int)(wEndDate - wStartDate).TotalDays;
                wFMCScheduling.CreateID = wBMSEmployee.ID;
                wFMCScheduling.CreateTime = DateTime.Now;
                wFMCScheduling.SerialNo = FMCSchedulingDAO.Instance.FMC_GetNewCode(out wErrorCode);
                FMCSchedulingDAO.Instance.FMC_SaveFMCScheduling(wFMCScheduling, out wErrorCode);
                if (wFMCScheduling.ID > 0)
                {
                    //查询最新的排班模板
                    //List<FMCSchedulingItem> wMaxList = FMCSchedulingItemDAO.Instance.FMC_QueryMaxFMCSchedulingItemList(out wErrorCode);
                    //查询班次列表
                    List<FMCShift> wFMCShiftList = FMCShiftDAO.Instance.FMC_QueryFMCShiftList(wShiftID, "", out wErrorCode);
                    //创建排班模板
                    for (int i = 0; i < wDays + 1; i++)
                    {
                        DateTime wTempDate = wStartDate.AddDays(i);
                        wDateList.Add(wTempDate);
                        foreach (int wStationID in wStationDic.Keys)
                        {
                            string wStationName = wStationDic[wStationID];


                            foreach (FMCShift wFMCShift in wFMCShiftList)
                            {
                                FMCSchedulingItem wFMCSchedulingItem = new FMCSchedulingItem();

                                wFMCSchedulingItem.ID = 0;
                                wFMCSchedulingItem.FMCSchedulingID = wFMCScheduling.ID;
                                wFMCSchedulingItem.StationID = wStationID;
                                wFMCSchedulingItem.StationName = wStationName;
                                wFMCSchedulingItem.SerialNo = wFMCScheduling.SerialNo;
                                wFMCSchedulingItem.CreateID = wBMSEmployee.ID;
                                wFMCSchedulingItem.CreateTime = DateTime.Now;
                                wFMCSchedulingItem.EditID = wBMSEmployee.ID;
                                wFMCSchedulingItem.EditTime = DateTime.Now;
                                wFMCSchedulingItem.ShiftID = wFMCShift.ID;
                                wFMCSchedulingItem.ShiftName = wFMCShift.Name;
                                wFMCSchedulingItem.ScheduleDate = wTempDate;

                                //List<FMCSchedulingItem> wTempList = wMaxList.FindAll(p => p.StationID == wStationID).ToList();
                                //if (wTempList.Count == wDays)
                                //{
                                //    wTempList = wTempList.OrderBy(p => p.ScheduleDate).ToList();
                                //    wFMCSchedulingItem.PersonID = wTempList[i].PersonID;
                                //    wFMCSchedulingItem.PersonName = wTempList[i].PersonName;
                                //    wFMCSchedulingItem.ShiftID = wTempList[i].ShiftID;
                                //    wFMCSchedulingItem.ShiftName = wTempList[i].ShiftName;
                                //}

                                wResult.Result.Add(wFMCSchedulingItem);
                            }
                        }
                    }
                }
                wResult.CustomResult.Add("DateList", wDateList);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_DeleteShiftList(BMSEmployee wLoginUser, List<FMCShift> wFMCShiftList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FMCShiftDAO.Instance.FMC_DeleteFMCShiftList(wFMCShiftList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCSchedulingItem>> FMC_QuerySchedulingItemList(BMSEmployee wBMSEmployee, int wID, int wFMCSchedulingID, int wStationID, int wPersonID, DateTime wStartTime, DateTime wEndTime, int wShiftID)
        {
            ServiceResult<List<FMCSchedulingItem>> wResult = new ServiceResult<List<FMCSchedulingItem>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FMCSchedulingItemDAO.Instance.FMC_QueryFMCSchedulingItemList(wID, wFMCSchedulingID, wStationID, wPersonID, wStartTime, wEndTime, wShiftID, out wErrorCode);

                //查询工位列表
                Dictionary<int, string> wStationDic = FMCSchedulingDAO.Instance.FMC_QueryStationDic(out wErrorCode);
                wStartTime = new DateTime(wStartTime.Year, wStartTime.Month, wStartTime.Day, 0, 0, 0);

                int wDays = (int)(wEndTime - wStartTime).TotalDays;

                wEndTime = new DateTime(wEndTime.Year, wEndTime.Month, wEndTime.Day, 23, 59, 59);

                wResult.CustomResult.Add("StationList", wStationDic);
                List<DateTime> wDateList = new List<DateTime>();
                //日期列表
                for (int i = 0; i < wDays + 1; i++)
                {
                    DateTime wTempDate = wStartTime.AddDays(i);
                    wDateList.Add(wTempDate);
                }
                wResult.CustomResult.Add("DateList", wDateList);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCScheduling>> FMC_QuerySchedulingList(BMSEmployee wBMSEmployee, int wID, string wSerialNo, int wActive, DateTime wQueryDate)
        {
            ServiceResult<List<FMCScheduling>> wResult = new ServiceResult<List<FMCScheduling>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FMCSchedulingDAO.Instance.FMC_QueryFMCSchedulingList(wID, wSerialNo, wActive, wQueryDate, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCShiftItem>> FMC_QueryShfitItemList(BMSEmployee wLoginUser, int wID, int wShiftID, string wName, int wType, int wActive)
        {
            ServiceResult<List<FMCShiftItem>> wResult = new ServiceResult<List<FMCShiftItem>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FMCShiftItemDAO.Instance.FMC_QueryFMCShiftItemList(wID, wShiftID, wName, wType, wActive, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCShift>> FMC_QueryShfitList(BMSEmployee wLoginUser, int wID, string wName)
        {
            ServiceResult<List<FMCShift>> wResult = new ServiceResult<List<FMCShift>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FMCShiftDAO.Instance.FMC_QueryFMCShiftList(wID, wName, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_SaveScheduling(BMSEmployee wBMSEmployee, FMCScheduling wFMCScheduling)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                FMCSchedulingDAO.Instance.FMC_SaveFMCScheduling(wFMCScheduling, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_SaveShift(BMSEmployee wLoginUser, FMCShift wFMCShift)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                FMCShiftDAO.Instance.FMC_SaveFMCShift(wFMCShift, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_SaveShiftItem(BMSEmployee wLoginUser, FMCShiftItem wFMCShiftItem)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFMCShiftItem.EditID = wLoginUser.ID;
                wFMCShiftItem.EditTime = DateTime.Now;
                wResult.Result = FMCShiftItemDAO.Instance.FMC_SaveFMCShiftItem(wFMCShiftItem, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_UpdateSchedulingItemList(BMSEmployee wBMSEmployee, List<FMCSchedulingItem> wFMCShiftItemList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                foreach (FMCSchedulingItem wFMCSchedulingItem in wFMCShiftItemList)
                {
                    if (wFMCSchedulingItem.ID > 0)
                    {
                        wFMCSchedulingItem.EditID = wBMSEmployee.ID;
                        wFMCSchedulingItem.EditTime = DateTime.Now;
                    }
                    else
                    {
                        wFMCSchedulingItem.CreateID = wBMSEmployee.ID;
                        wFMCSchedulingItem.CreateTime = DateTime.Now;
                        wFMCSchedulingItem.EditID = wBMSEmployee.ID;
                        wFMCSchedulingItem.EditTime = DateTime.Now;
                    }
                    FMCSchedulingItemDAO.Instance.FMC_SaveFMCSchedulingItem(wFMCSchedulingItem, out wErrorCode);
                }
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();

                if (string.IsNullOrEmpty(wResult.FaultCode))
                {
                    MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                    wMCSOperationLog.ModuleID = (int)MCSModuleType.ShiftManage;
                    wMCSOperationLog.Type = (int)MCSOperateType.Update;
                    wMCSOperationLog.CreateID = wBMSEmployee.ID;
                    wMCSOperationLog.CreateTime = DateTime.Now;
                    wMCSOperationLog.EditID = wBMSEmployee.ID;
                    wMCSOperationLog.EditTime = DateTime.Now;
                    wMCSOperationLog.Content = String.Format("{0}在{1}保存了一次排班记录", wBMSEmployee.Name, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_ActiveRouteList(BMSEmployee wBMSEmployee, int wActive, List<FPCRoute> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FPCRoute wFPCStructuralPart in wFPCStructuralPartList)
                {
                    wFPCStructuralPart.Active = wActive;
                    wFPCStructuralPart.EditID = wBMSEmployee.ID;
                    wFPCStructuralPart.EditTime = DateTime.Now;
                    FPCRouteDAO.Instance.FPC_SaveFPCRoute(wFPCStructuralPart, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_ActiveStructuralPartList(BMSEmployee wBMSEmployee, int wActive, List<FPCStructuralPart> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FPCStructuralPart wFPCStructuralPart in wFPCStructuralPartList)
                {
                    wFPCStructuralPart.Active = wActive;
                    wFPCStructuralPart.EditID = wBMSEmployee.ID;
                    wFPCStructuralPart.EditTime = DateTime.Now;
                    FPCStructuralPartDAO.Instance.FPC_SaveFPCStructuralPart(wFPCStructuralPart, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_AddRoute(BMSEmployee wBMSEmployee, FPCRoute wFPCRoute)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCRoute.CreateID = wBMSEmployee.ID;
                wFPCRoute.CreateTime = DateTime.Now;
                wFPCRoute.EditID = wBMSEmployee.ID;
                wFPCRoute.EditTime = DateTime.Now;
                wResult.Result = FPCRouteDAO.Instance.FPC_SaveFPCRoute(wFPCRoute, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_AddRoutePart(BMSEmployee wBMSEmployee, FPCRoutePart wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.CreatorID = wBMSEmployee.ID;
                wFPCStructuralPart.CreateTime = DateTime.Now;
                wResult.Result = FPCRoutePartDAO.Instance.FPC_SaveFPCRoutePart(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();

                if (wFPCStructuralPart.RouteID > 0)
                {
                    List<FPCRoutePart> wList = FPCRoutePartDAO.Instance.FPC_QueryFPCRoutePartList(-1, wFPCStructuralPart.RouteID, "", "", -1, Pagination.MaxSize, out wErrorCode);
                    List<FPCRoute> wRouteList = FPCRouteDAO.Instance.FPC_QueryFPCRouteList(wFPCStructuralPart.RouteID, "", "", -1, -1, out wErrorCode);
                    if (wRouteList.Count > 0)
                    {
                        wRouteList[0].SonNumber = wList.Count;
                        FPCRouteDAO.Instance.FPC_SaveFPCRoute(wRouteList[0], out wErrorCode);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_AddStructuralPart(BMSEmployee wBMSEmployee, FPCStructuralPart wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.CreateID = wBMSEmployee.ID;
                wFPCStructuralPart.CreateTime = DateTime.Now;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                wResult.Result = FPCStructuralPartDAO.Instance.FPC_SaveFPCStructuralPart(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_DeleteRouteList(BMSEmployee wBMSEmployee, List<FPCRoute> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FPCRouteDAO.Instance.FPC_DeleteFPCRouteList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_DeleteRoutePartList(BMSEmployee wBMSEmployee, List<FPCRoutePart> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FPCRoutePartDAO.Instance.FPC_DeleteFPCRoutePartList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_DeleteStructuralPartList(BMSEmployee wBMSEmployee, List<FPCStructuralPart> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FPCStructuralPartDAO.Instance.FPC_DeleteFPCStructuralPartList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCGasVelocity>> FPC_ImportGasVelocity(Stream wStream, string wFileName, out string wMsg)
        {
            ServiceResult<List<FPCGasVelocity>> wResult = new ServiceResult<List<FPCGasVelocity>>();
            wMsg = "";
            try
            {
                int wErrorCode = 0;

                if (wStream == null || wStream.Length == 0)
                {
                    wResult.FaultCode = "请选择要导入的Excel文件";
                    return wResult;
                }
                ExcelExtType excelType = ServerExcelUtils.Instance.GetExcelFileType(wFileName);
                if (excelType == ExcelExtType.error)
                {
                    wResult.FaultCode = "请选择正确的Excel文件";
                    return wResult;
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wStream.Position = 0;
                    wStream.CopyTo(stream);
                    List<Dictionary<String, Object>> dt = ServerExcelUtils.Instance.ImportExcel(stream, excelType, "");
                    if (dt == null)
                    {
                        wResult.FaultCode = "导入失败,请选择正确的Excel文件";
                        return wResult;
                    }

                    wResult.Result = FPCGasVelocityDAO.Instance.FPC_ImportGasVelocity(dt, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCFlowPart>> FPC_QueryFlowDataPart(BMSEmployee wBMSEmployee, int wRouteID)
        {
            ServiceResult<List<FPCFlowPart>> wResult = new ServiceResult<List<FPCFlowPart>>();
            try
            {
                int wErrorCode = 0;

                List<FPCRoutePart> wRoutePartList = FPCRoutePartDAO.Instance.FPC_QueryFPCRoutePartList(-1, wRouteID, "", "", -1, Pagination.MaxSize, out wErrorCode);
                // ②获取工位节点集合
                wResult.Result = GetFlowPartList(wRoutePartList);
                // ③获取线集合
                List<FPCFlowLine> wLineList = GetFlowLineList(wRoutePartList, wResult.Result);
                wResult.CustomResult.Add("LineList", wLineList);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        private List<FPCFlowLine> GetFlowLineList(List<FPCRoutePart> wRoutePartList, List<FPCFlowPart> wFlowPartList)
        {
            List<FPCFlowLine> wResult = new List<FPCFlowLine>();
            try
            {
                if (wRoutePartList == null || wRoutePartList.Count <= 0)
                {
                    return wResult;
                }

                foreach (FPCRoutePart wFPCRoutePart in wRoutePartList)
                {
                    // ①查询所有后节点
                    List<FPCRoutePart> wNextList = wRoutePartList.FindAll
                            (p => p.PrevPartID == wFPCRoutePart.PartID
                                    || wFPCRoutePart.NextPartIDMap.ContainsKey(p.PartID.ToString()));

                    int wRow1 = GetRow(wFPCRoutePart, wRoutePartList);

                    foreach (FPCRoutePart wNextNode in wNextList)
                    {
                        int wRow2 = GetRow(wNextNode, wRoutePartList);

                        FPCFlowLine wFPCFlowLine = new FPCFlowLine();

                        wFPCFlowLine.anode = new FPCFlowPoint();
                        wFPCFlowLine.anode.id = wFPCRoutePart.PartID.ToString();
                        wFPCFlowLine.anode.anchor = "Right";
                        wFPCFlowLine.anode.uuid = wFPCFlowLine.anode.id + "_r";

                        wFPCFlowLine.bnode = new FPCFlowPoint();
                        wFPCFlowLine.bnode.id = wNextNode.PartID.ToString();
                        wFPCFlowLine.bnode.anchor = "Left";
                        wFPCFlowLine.bnode.uuid = wFPCFlowLine.bnode.id + "_l";

                        // ①a节点和b节点在同一层，列相隔大于1，连上节点
                        if (wRow1 == wRow2 && wNextNode.OrderID - wFPCRoutePart.OrderID > 1)
                        {
                            wFPCFlowLine.anode.anchor = "Top";
                            wFPCFlowLine.anode.uuid = wFPCFlowLine.anode.id + "_t";

                            wFPCFlowLine.bnode.anchor = "Top";
                            wFPCFlowLine.bnode.uuid = wFPCFlowLine.bnode.id + "_t";
                        }
                        // ②a节点层数大于b节点层数，且不存在和A在同一层，且列小于B的工位
                        else if (wRow1 > wRow2 && !wFlowPartList.Exists(p => p.row == wRow1
                                && p.col < wNextNode.OrderID && !p.id.Equals(wFPCRoutePart.PartID.ToString())))
                        {
                            wFPCFlowLine.anode.anchor = "Right";
                            wFPCFlowLine.anode.uuid = wFPCFlowLine.anode.id + "_r";

                            wFPCFlowLine.bnode.anchor = "Bottom";
                            wFPCFlowLine.bnode.uuid = wFPCFlowLine.bnode.id + "_b";
                        }

                        wResult.Add(wFPCFlowLine);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        private List<FPCFlowPart> GetFlowPartList(List<FPCRoutePart> wRoutePartList)
        {
            List<FPCFlowPart> wResult = new List<FPCFlowPart>();
            try
            {
                if (wRoutePartList == null || wRoutePartList.Count <= 0)
                {
                    return wResult;
                }

                foreach (FPCRoutePart wFPCRoutePart in wRoutePartList)
                {
                    FPCFlowPart wItem = new FPCFlowPart();

                    // ⑥row
                    wItem.row = GetRow(wFPCRoutePart, wRoutePartList);
                    // ⑦col
                    wItem.col = wFPCRoutePart.OrderID;
                    // ①id
                    wItem.id = wFPCRoutePart.PartID.ToString();
                    // ②name
                    wItem.name = wFPCRoutePart.PartName;
                    // ③left
                    wItem.left = ((wItem.col - 1) * 180 + 20).ToString();
                    // ④top
                    wItem.top = ((wItem.row - 1) * 75 + 35).ToString();
                    // ⑤showclass
                    wItem.showclass = wFPCRoutePart.ChangeControl == 2 ? "mytipshow" : "mytiphide";

                    wResult.Add(wItem);
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        private int GetRow(FPCRoutePart wItem, List<FPCRoutePart> wRoutePartList)
        {
            int wResult = 0;
            try
            {
                List<FPCRoutePart> wList = wRoutePartList.FindAll(p => p.OrderID == wItem.OrderID).ToList();

                foreach (FPCRoutePart wFPCRoutePart in wList)
                    wFPCRoutePart.OrderNumber = 1;

                wList = wList.OrderByDescending(p => p.OrderNumber).ToList();

                for (int i = 0; i < wList.Count; i++)
                {
                    if (wList[i].PartID == wItem.PartID)
                    {
                        wResult = i + 1;
                        return wResult;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCGasVelocity>> FPC_QueryGasVelocityList(BMSEmployee wBMSEmployee, int wID, int wType, double wThickness, string wName, string wDescription)
        {
            ServiceResult<List<FPCGasVelocity>> wResult = new ServiceResult<List<FPCGasVelocity>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FPCGasVelocityDAO.Instance.FPC_QueryFPCGasVelocityList(wID, wType, wThickness, wName, wDescription, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCRoute>> FPC_QueryRouteList(BMSEmployee wBMSEmployee, int wID, string wRouteName, string wCode, int wActive, int wIsStandard, Pagination wPagination)
        {
            ServiceResult<List<FPCRoute>> wResult = new ServiceResult<List<FPCRoute>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FPCRouteDAO.Instance.FPC_QueryFPCRouteList(wID, wRouteName, wCode, wActive, wIsStandard, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCRoutePart>> FPC_QueryRoutePartList(BMSEmployee wBMSEmployee, int wID, int wRouteID, string wName, string wCode, int wPartID, Pagination wPagination)
        {
            ServiceResult<List<FPCRoutePart>> wResult = new ServiceResult<List<FPCRoutePart>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FPCRoutePartDAO.Instance.FPC_QueryFPCRoutePartList(wID, wRouteID, wName, wCode, wPartID, wPagination, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FPCStructuralPart>> FPC_QueryStructuralPartList(BMSEmployee wBMSEmployee, int wID, string wName, string wCode, int wActive, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, String wMaterialNo, String wMaterialTypeNo)
        {
            ServiceResult<List<FPCStructuralPart>> wResult = new ServiceResult<List<FPCStructuralPart>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FPCStructuralPartDAO.Instance.FPC_QueryFPCStructuralPartList(wID, wName, wCode, wActive, wStartTime, wEndTime, wPagination, wMaterialNo, wMaterialTypeNo, out wErrorCode);
                wResult.Result = wResult.Result.OrderBy(p => p.CreateTime).ToList();
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_SaveRoute(BMSEmployee wBMSEmployee, FPCRoute wFPCRoute)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCRoute.EditID = wBMSEmployee.ID;
                wFPCRoute.EditTime = DateTime.Now;
                wResult.Result = FPCRouteDAO.Instance.FPC_SaveFPCRoute(wFPCRoute, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_SaveRoutePart(BMSEmployee wBMSEmployee, FPCRoutePart wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = FPCRoutePartDAO.Instance.FPC_SaveFPCRoutePart(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FPC_SaveStructuralPart(BMSEmployee wBMSEmployee, FPCStructuralPart wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                wResult.Result = FPCStructuralPartDAO.Instance.FPC_SaveFPCStructuralPart(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_AddOrder(BMSEmployee wBMSEmployee, OMSOrder wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                bool wIsExit = OMSOrderDAO.Instance.IsExsit(wFPCStructuralPart.CuttingNumber, out wErrorCode);
                if (wIsExit)
                {
                    wResult.FaultCode += string.Format("【{0}】中控订单号重复", wFPCStructuralPart.CuttingNumber);

                    return wResult;
                }

                wFPCStructuralPart.CreateID = wBMSEmployee.ID;
                wFPCStructuralPart.CreateTime = DateTime.Now;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                if (wFPCStructuralPart.OrderType == 1)
                {
                    //NC文件换地址存储
                    if (!string.IsNullOrWhiteSpace(wFPCStructuralPart.NCFileUri))
                    {
                        string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                        int wIndex = wBaseDir.LastIndexOf('\\');
                        wBaseDir = wBaseDir.Substring(0, wIndex);
                        wIndex = wBaseDir.LastIndexOf('\\');
                        wBaseDir = wBaseDir.Substring(0, wIndex + 1);
                        string dirpath = wBaseDir + "NCFiles\\";
                        if (!Directory.Exists(dirpath))
                            Directory.CreateDirectory(dirpath);
                        string wPath = "";
                        if (wFPCStructuralPart.NCFileUri.Contains("_"))
                        {
                            wPath = wBaseDir + "NCFiles\\" + wFPCStructuralPart.NCFileUri.Split("_")[1];
                        }
                        else
                        {
                            wResult.FaultCode = "NC文件路径错误！";
                            return wResult;
                        }
                        //文件复制
                        string wUploadAddress = GlobalConstant.GlobalConfiguration.GetValue("Service.UploadAddress");
                        //老原始路径 在upload下
                        FileInfo wFileInfo = new FileInfo(wUploadAddress + wFPCStructuralPart.NCFileUri.Replace("/iPlantCore", ""));
                        //wPath 新路径 跟iplantCore平级的文件夹
                        wFileInfo.CopyTo(wPath, true);

                        wFPCStructuralPart.NCFileUri = "/NCFiles/" + wFPCStructuralPart.NCFileUri.Split("_")[1]; ;
                    }

                    //DXF文件换地址存储
                    if (!string.IsNullOrWhiteSpace(wFPCStructuralPart.DXFFileUri))
                    {
                        string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                        int wIndex = wBaseDir.LastIndexOf('\\');
                        wBaseDir = wBaseDir.Substring(0, wIndex);
                        wIndex = wBaseDir.LastIndexOf('\\');
                        wBaseDir = wBaseDir.Substring(0, wIndex + 1);
                        string dirpath = wBaseDir + "DXFFiles\\";
                        if (!Directory.Exists(dirpath))
                            Directory.CreateDirectory(dirpath);
                        //string wPath = wBaseDir + "DXFFiles\\" + wFPCStructuralPart.DXFFileName;

                        string wPath = "";
                        if (wFPCStructuralPart.DXFFileUri.Contains("_"))
                        {
                            wPath = wBaseDir + "DXFFiles\\" + wFPCStructuralPart.DXFFileUri.Split("_")[1];
                        }
                        else
                        {
                            wResult.FaultCode = "DXF文件路径错误！";
                            return wResult;
                        }

                        //文件复制
                        string wUploadAddress = GlobalConstant.GlobalConfiguration.GetValue("Service.UploadAddress");
                        FileInfo wFileInfo = new FileInfo(wUploadAddress + wFPCStructuralPart.DXFFileUri.Replace("/iPlantCore", ""));
                        wFileInfo.CopyTo(wPath, true);

                        wFPCStructuralPart.DXFFileUri = "/DXFFiles/" + wFPCStructuralPart.DXFFileUri.Split("_")[1];
                    }

                    wFPCStructuralPart.CuttingNumber = OMSOrderDAO.Instance.GetOrdreNoSerial();
                }
                wResult.Result = OMSOrderDAO.Instance.OMS_SaveOMSOrder(wFPCStructuralPart, out wErrorCode);

                if (wFPCStructuralPart.ID > 0)
                {
                    MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                    wMCSOperationLog.ModuleID = (int)MCSModuleType.OrderManage;
                    wMCSOperationLog.Type = (int)MCSOperateType.Add;
                    wMCSOperationLog.CreateID = wBMSEmployee.ID;
                    wMCSOperationLog.CreateTime = DateTime.Now;
                    wMCSOperationLog.EditID = wBMSEmployee.ID;
                    wMCSOperationLog.EditTime = DateTime.Now;
                    wMCSOperationLog.Content = String.Format("{0}在{1}新增了订单,套料编号为【{2}】", wBMSEmployee.Name, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), wFPCStructuralPart.NestingNumber);
                    MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();

                //计划叫料
                PlanCallMaterial(wBMSEmployee, wFPCStructuralPart);
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        /// <summary>
        /// 计划叫料
        /// </summary>
        private void PlanCallMaterial(BMSEmployee wBMSEmployee, OMSOrder wOMSOrder)
        {
            try
            {
                int wMaterialPointID = StringUtils.parseInt(wOMSOrder.PlateMaterialNo);
                int wFQTY = StringUtils.parseInt(wOMSOrder.Plate);
                if (wMaterialPointID <= 0 || wMaterialPointID <= 0)
                    return;

                int wErrorCode = 0;
                int wRFQTY = 0;
                List<MSSMaterialStock> wStockList = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, wMaterialPointID, wOMSOrder.StructuralPartID, -1, Pagination.MaxSize, out wErrorCode);
                wRFQTY = wStockList.Count;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MSSCallMaterial> wMSSCallMaterialList = MSSCallMaterialDAO.Instance.MSS_QueryMSSCallMaterialList(-1, "", "", -1, wOMSOrder.StructuralPartID, wMaterialPointID, 1, -1, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);
                foreach (MSSCallMaterial wItem in wMSSCallMaterialList)
                    wRFQTY += wItem.DemandNumber;

                if (wRFQTY >= wFQTY)
                    return;

                wRFQTY = wFQTY - wRFQTY;
                MSSCallMaterial wMSSCallMaterial = new MSSCallMaterial();
                wMSSCallMaterial.ID = 0;
                wMSSCallMaterial.MaterialPointID = wMaterialPointID;
                wMSSCallMaterial.Name = "";
                wMSSCallMaterial.StartTime = DateTime.Now;
                wMSSCallMaterial.EndTime = DateTime.Now;
                wMSSCallMaterial.CreateID = wBMSEmployee.ID;
                wMSSCallMaterial.CreateTime = DateTime.Now;
                wMSSCallMaterial.EditID = wBMSEmployee.ID;
                wMSSCallMaterial.EditTime = DateTime.Now;
                wMSSCallMaterial.Status = 1;
                wMSSCallMaterial.Type = 3;
                wMSSCallMaterial.PlateID = wOMSOrder.StructuralPartID;
                wMSSCallMaterial.DemandNumber = wRFQTY;
                wMSSCallMaterial.Active = 1;
                wMSSCallMaterial.Code = MSSCallMaterialDAO.Instance.GetNewCode(out wErrorCode);
                wMSSCallMaterial.ConfirmTime = wBaseTime;
                MSSCallMaterialDAO.Instance.MSS_SaveMSSCallMaterial(wMSSCallMaterial, out wErrorCode);
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
        }

        public ServiceResult<int> OMS_AddOrderItem(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.CreateID = wBMSEmployee.ID;
                wFPCStructuralPart.CreateTime = DateTime.Now;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                wResult.Result = OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> INF_AddSortsysSendcasing(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                INFSortsysSendcasing wINFSortsysSendcasingInfo = new INFSortsysSendcasing();

                wINFSortsysSendcasingInfo.ProductionLline = "A";
                wINFSortsysSendcasingInfo.SortStationNo = "A302";
                wINFSortsysSendcasingInfo.CutStationNo = "QG02";
                wINFSortsysSendcasingInfo.CasingLocalUrl = GlobalConstant.GlobalConfiguration.GetValue("Service.DXFUri") + wFPCStructuralPart.DXFFileUri;
                wINFSortsysSendcasingInfo.MissionNo = wFPCStructuralPart.CuttingNumber;
                wINFSortsysSendcasingInfo.Status = 0;
                wINFSortsysSendcasingInfo.ErroMsg = "";
                wINFSortsysSendcasingInfo.CreateTime = DateTime.Now;
                wINFSortsysSendcasingInfo.SendTime = new DateTime(2000, 1, 1);

                wResult.Result = OMSOrderItemDAO.Instance.OMS_SaveSortsysSendcasing(wINFSortsysSendcasingInfo, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();

                if (wINFSortsysSendcasingInfo.ID > 0)
                {
                    wFPCStructuralPart.DXFAnalysisStatus = 0;
                    OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wFPCStructuralPart, out wErrorCode);
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_DeleteOrderList(BMSEmployee wBMSEmployee, List<OMSOrder> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = OMSOrderDAO.Instance.OMS_DeleteOMSOrderList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_DeleteOrderItemList(BMSEmployee wBMSEmployee, List<OMSOrderItem> wOrderItemList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = OMSOrderDAO.Instance.OMS_DeleteOMSOrderItemList(wOrderItemList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        //生成工单
        public ServiceResult<List<OMSOrderItem>> OMS_GenerateWorkOrder(BMSEmployee wBMSEmployee, int wOrderID)
        {
            ServiceResult<List<OMSOrderItem>> wResult = new ServiceResult<List<OMSOrderItem>>();
            wResult.Result = new List<OMSOrderItem>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<OMSOrderItem> wItemList = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(-1, wOrderID, "", "", "", -1, "", -1, "", "", wBaseTime, wBaseTime, -1, Pagination.MaxSize, -1, -1, -1, -1,1, out wErrorCode);
                if (wItemList.Count > 0)
                {
                    wResult.FaultCode += "该订单已生成工单!";
                    return wResult;
                }

                List<OMSOrder> wOrderList = OMSOrderDAO.Instance.OMS_QueryOMSOrderList(wOrderID, "", "", -1, "", -1, "", "", wBaseTime, wBaseTime, Pagination.MaxSize, -1, 1,out wErrorCode);
                if (wOrderList.Count <= 0)
                {
                    wResult.FaultCode += "未查询到订单数据，请联系系统管理员!";
                    return wResult;
                }

                OMSOrder wOrder = wOrderList[0];

                if (string.IsNullOrEmpty(wOrder.NCFileUri) || string.IsNullOrEmpty(wOrder.DXFFileUri))
                {
                    wResult.FaultCode += "请先上传NC文件和DXF文件!";
                    return wResult;
                }

                //if (!wOrder.NestingNumber.Equals(wOrder.CuttingNumber))
                //{
                //    wResult.FaultCode += "NC文件和DXF文件不一致!";
                //    return wResult;
                //}

                string wNo = "";
                int OrderNum = OMSOrderItemDAO.Instance.GetMaxOrdreNumBy();
                int wTotalSize = OMSOrderItemDAO.Instance.OMS_QuerySaveTotalSize(out wErrorCode);
                for (int i = 1; i <= wOrder.CutTimes; i++)
                {
                    wNo = string.Format("{0}", wOrder.CuttingNumber);

                    if (wOrder.OrderType == 1)
                    {
                        wNo = OMSOrderItemDAO.Instance.GetOrdreNoSerial();
                    }

                    OMSOrderItem wOMSOrderItem = CloneTool.Clone<OMSOrderItem>(wOrder);
                    wOMSOrderItem.ID = 0;
                    if (wOrder.OrderType == 1)
                    {
                        wOMSOrderItem.OrderID = wOrder.ID;
                        wOMSOrderItem.LesOrderID = 0;
                    }
                    else if (wOrder.OrderType == 2)
                    {
                        wOMSOrderItem.OrderID = wOrder.ID;
                        wOMSOrderItem.LesOrderID = wOrder.LesOrderID;
                    }
                    else
                    {
                        wOMSOrderItem.OrderID = 0;
                        wOMSOrderItem.LesOrderID = 0;
                    }

                    wOMSOrderItem.OrderType = wOrder.OrderType;
                    wOMSOrderItem.OrderNo = wNo;
                    wOMSOrderItem.CuttingNumber = wNo;
                    //wOMSOrderItem.NestingNumber = wNo;
                    wOMSOrderItem.CreateID = wBMSEmployee.ID;
                    wOMSOrderItem.CreateTime = DateTime.Now;
                    wOMSOrderItem.EditID = wBMSEmployee.ID;
                    wOMSOrderItem.EditTime = DateTime.Now;
                    wOMSOrderItem.Status = 1;
                    wOMSOrderItem.OrderNum = OrderNum + i;
                    wOMSOrderItem.Material = wOrder.Material;

                    //获取C盘文件夹（配置文件）
                    string wDXFlocalURL = GlobalConstant.GlobalConfiguration.GetValue("Service.DXFlocalURL");
                    string wNClocalURL = GlobalConstant.GlobalConfiguration.GetValue("Service.NClocalURL");
                    //获取C:\IIS.Service\iPlantSanyWEB\DXFFiles
                    string wFlieAddress = GlobalConstant.GlobalConfiguration.GetValue("Service.FlieAddress");

                    //如果字符串为null或者空字符Empty的时候，string.IsNullOrWhiteSpace将会返回true,否则返回false。
                    ///NCFiles/O220623SG10006A03.nc
                    if (!string.IsNullOrWhiteSpace(wOMSOrderItem.NCFileUri))
                    {
                        if (!Directory.Exists(wNClocalURL))
                        {
                            Directory.CreateDirectory(wNClocalURL);
                        }
                        //老原始路径 NCFiles里面
                        FileInfo wFileInfo = new FileInfo(wFlieAddress + wOMSOrderItem.NCFileUri);
                        wFileInfo.CopyTo(wNClocalURL + wOMSOrderItem.NCFileUri.Replace("/NCFiles", ""), true);
                    }
                    wOMSOrderItem.NCLocalUrl = wNClocalURL + wOMSOrderItem.NCFileUri.Replace("/NCFiles", "");
                    if (!string.IsNullOrWhiteSpace(wOMSOrderItem.DXFFileUri))
                    {
                        if (!Directory.Exists(wDXFlocalURL))
                        {
                            Directory.CreateDirectory(wDXFlocalURL);
                        }

                        //老原始路径 DXFFiles里面
                        FileInfo wFileInfo = new FileInfo(wFlieAddress + wOMSOrderItem.DXFFileUri);
                        wFileInfo.CopyTo(wDXFlocalURL + wOMSOrderItem.DXFFileUri.Replace("/DXFFiles", ""), true);
                    }
                    wOMSOrderItem.DXFLocalUrl = wDXFlocalURL + wOMSOrderItem.DXFFileUri.Replace("/DXFFiles", "");
                    OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wOMSOrderItem, out wErrorCode);

                    if (wOMSOrderItem.ID > 0)
                    {
                        //下发解析
                        ServiceInstance.mFMCService.INF_AddSortsysSendcasing(wBMSEmployee, wOMSOrderItem);
                    }

                    wResult.Result.Add(wOMSOrderItem);

                    if (wOMSOrderItem.ID > 0)
                    {
                        MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                        wMCSOperationLog.ModuleID = (int)MCSModuleType.OrderManage;
                        wMCSOperationLog.Type = (int)MCSOperateType.Add;
                        wMCSOperationLog.CreateID = wBMSEmployee.ID;
                        wMCSOperationLog.CreateTime = DateTime.Now;
                        wMCSOperationLog.EditID = wBMSEmployee.ID;
                        wMCSOperationLog.EditTime = DateTime.Now;
                        wMCSOperationLog.Content = String.Format("{0}在{1}生成了工单【{2}】", wBMSEmployee.Name, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), wOMSOrderItem.OrderNo);
                        MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                    }
                }

                wOrder.Flag = 1;
                OMSOrderDAO.Instance.OMS_SaveOMSOrder(wOrder, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_ActiveOrderItemLis(BMSEmployee wBMSEmployee,
         int wStatus, List<OMSOrderItem> wOMSOrderItemList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (OMSOrderItem wOMSOrderItem in wOMSOrderItemList)
                {
                    wOMSOrderItem.Status = wStatus;
                    wOMSOrderItem.EditID = wBMSEmployee.ID;
                    wOMSOrderItem.EditTime = DateTime.Now;
                    OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wOMSOrderItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrderItem>> OMS_QueryOrderItemList(BMSEmployee wBMSEmployee, int wID, int wOrderID, string wOrderNo, string wCuttingNumber,
            string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, 
            DateTime wStartTime, DateTime wEndTime, int wStatus, Pagination wPagination, int wOrderType, int wActive, int wDXFAnalysisStatus, int wLesOrderID, int wDisplayed)
        {
            ServiceResult<List<OMSOrderItem>> wResult = new ServiceResult<List<OMSOrderItem>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(wID, wOrderID, wOrderNo, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo, wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wStatus, wPagination, wOrderType, wActive, wDXFAnalysisStatus, wLesOrderID, wDisplayed, out wErrorCode);

                for (int wIndex = 0; wIndex < wResult.Result.Count; wIndex++)
                {
                    wResult.Result[wIndex] = QueryINFSortsysSendcasingObject(wResult.Result[wIndex]);
                }
                wResult.Result = wResult.Result.OrderByDescending(p => p.OrderNum).ToList();
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public OMSOrderItem QueryINFSortsysSendcasingObject(OMSOrderItem OMSOrderItemObject)
        {
            int wErrorCode = 0;
            OMSOrderItem wResultOMSOrderItem = CloneTool.Clone<OMSOrderItem>(OMSOrderItemObject);
            List<INFSortsysSendcasing> wResultINFSortsysSendcasingList = OMSOrderItemDAO.Instance.INF_QueryINFSortsysSendcasingList(
             OMSOrderItemObject.CuttingNumber, out wErrorCode);

            if (wResultINFSortsysSendcasingList.Count > 0)
            {
                INFSortsysSendcasing wResultINFSortsysSendcasingListInfo = wResultINFSortsysSendcasingList.OrderByDescending(p => p.ID).ToList()[0];

                wResultOMSOrderItem.DXFIssuedStatus = wResultINFSortsysSendcasingListInfo.Status;
                wResultOMSOrderItem.DXFIssuedFailReason = wResultINFSortsysSendcasingListInfo.ErroMsg;
            }
            else
            {
                wResultOMSOrderItem.DXFIssuedStatus = 0;
                wResultOMSOrderItem.DXFIssuedFailReason = "未找到套料图反馈信息数据！";
            }

            return wResultOMSOrderItem;
        }
        public ServiceResult<List<OMSOrder>> OMS_QueryOrderList(BMSEmployee wBMSEmployee, int wID, string wCuttingNumber, string wNestingNumber, 
            int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime,
            Pagination wPagination, int wOrderType, int wDisplayed)
        {
            ServiceResult<List<OMSOrder>> wResult = new ServiceResult<List<OMSOrder>>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = OMSOrderDAO.Instance.OMS_QueryOMSOrderList(wID, wCuttingNumber, wNestingNumber, wCutType, wPlateMaterialNo,
                    wStructuralpartID, wGas, wCuttingMouth, wStartTime, wEndTime, wPagination, wOrderType, wDisplayed, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveOrder(BMSEmployee wBMSEmployee, OMSOrder wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;

                wResult.Result = OMSOrderDAO.Instance.OMS_SaveOMSOrder(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
                if (wFPCStructuralPart.ID > 0)
                {
                    MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                    wMCSOperationLog.ModuleID = (int)MCSModuleType.OrderManage;
                    wMCSOperationLog.Type = (int)MCSOperateType.Update;
                    wMCSOperationLog.CreateID = wBMSEmployee.ID;
                    wMCSOperationLog.CreateTime = DateTime.Now;
                    wMCSOperationLog.EditID = wBMSEmployee.ID;
                    wMCSOperationLog.EditTime = DateTime.Now;
                    wMCSOperationLog.Content = String.Format("{0}在{1}修改了订单,套料编号为【{2}】", wBMSEmployee.Name, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), wFPCStructuralPart.NestingNumber);
                    MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                }
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveOrderItem(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                if (wFPCStructuralPart.Status == 2)
                    wFPCStructuralPart.StartTime = DateTime.Now;
                wResult.Result = OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_QueryOrderList(BMSEmployee wBMSEmployee, int wID, int wMoveType, List<OMSOrderItem> wDataList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                Dictionary<int, int> wDic = new Dictionary<int, int>();
                int wIndex = 1;
                foreach (OMSOrderItem wOMSOrderItem in wDataList)
                {
                    wDic.Add(wIndex, wOMSOrderItem.OrderNum);
                    wIndex++;
                }

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                //List<OMSOrderItem> wList = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(-1, -1, "", "", "", -1, "", -1, "", "", wBaseTime, wBaseTime, 1, Pagination.MaxSize, -1, -1,-1,-1, out wErrorCode);
                //wList = wList.OrderBy(p => p.OrderNum).ToList();
                //int wIndex = 1;
                //foreach (OMSOrderItem wOMSOrderItem in wList)
                //{
                //    wOMSOrderItem.OrderNum = wIndex++;
                //}
                switch (wMoveType)
                {
                    case 1://上移
                        {
                            OMSOrderItem wOrder = wDataList.Find(p => p.ID == wID);

                            List<OMSOrderItem> wSmallList = wDataList.FindAll(p => p.OrderNum < wOrder.OrderNum);
                            if (wSmallList.Count <= 0)
                            {
                                wResult.FaultCode += "已移到顶部!";
                                return wResult;
                            }

                            //逆序
                            OMSOrderItem wUp = wSmallList.OrderByDescending(p => p.OrderNum).ToList()[0];

                            int wOrderNum1 = wOrder.OrderNum;
                            int wOrderNum2 = wUp.OrderNum;
                            wOrder.OrderNum = wOrderNum2;
                            wUp.OrderNum = wOrderNum1;
                            OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wOrder, out wErrorCode);
                            OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wUp, out wErrorCode);

                            //List<OMSOrderItem> wList = new List<OMSOrderItem>();
                            //wList.Add(wOrder);
                            //wList.AddRange(wDataList.FindAll(p => p.ID != wOrder.ID));
                            //int wFlag = 1;
                            //foreach (OMSOrderItem wItem in wList)
                            //{
                            //    wItem.OrderNum = wDic[wFlag];
                            //    OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wItem, out wErrorCode);
                            //    wFlag++;
                            //}

                            //OMSOrderItem wItem = wList.Find(p => p.ID == wID);
                            //if (wItem.OrderNum == 1)
                            //{
                            //    wResult.FaultCode += "已移到顶部!";
                            //    return wResult;
                            //}
                            //if (wList.Exists(p => p.OrderNum == (wItem.OrderNum - 1)))
                            //{
                            //    OMSOrderItem wTarItem = wList.Find(p => p.OrderNum == (wItem.OrderNum - 1));
                            //    wTarItem.OrderNum += 1;
                            //    wItem.OrderNum -= 1;
                            //}
                        }
                        break;
                    case 2://下移
                        {
                            OMSOrderItem wOrder = wDataList.Find(p => p.ID == wID);

                            List<OMSOrderItem> wSmallList = wDataList.FindAll(p => p.OrderNum > wOrder.OrderNum);
                            if (wSmallList.Count <= 0)
                            {
                                wResult.FaultCode += "已移到底部!";
                                return wResult;
                            }

                            OMSOrderItem wUp = wSmallList.OrderBy(p => p.OrderNum).ToList()[0];

                            int wOrderNum1 = wOrder.OrderNum;
                            int wOrderNum2 = wUp.OrderNum;
                            wOrder.OrderNum = wOrderNum2;
                            wUp.OrderNum = wOrderNum1;
                            OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wOrder, out wErrorCode);
                            OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wUp, out wErrorCode);
                            //OMSOrderItem wItem = wList.Find(p => p.ID == wID);
                            //if (wItem.OrderNum == wList[wList.Count - 1].OrderNum)
                            //{
                            //    wResult.FaultCode += "已移到底部!";
                            //    return wResult;
                            //}
                            //if (wList.Exists(p => p.OrderNum == (wItem.OrderNum + 1)))
                            //{
                            //    OMSOrderItem wTarItem = wList.Find(p => p.OrderNum == (wItem.OrderNum + 1));
                            //    wTarItem.OrderNum -= 1;
                            //    wItem.OrderNum += 1;
                            //}
                        }
                        break;
                    case 3://置顶
                        {
                            OMSOrderItem wOrder = wDataList.Find(p => p.ID == wID);

                            List<OMSOrderItem> wSmallList = wDataList.FindAll(p => p.OrderNum < wOrder.OrderNum);
                            if (wSmallList.Count <= 0)
                            {
                                wResult.FaultCode += "已移到顶部!";
                                return wResult;
                            }

                            List<OMSOrderItem> wList = new List<OMSOrderItem>();
                            wList.Add(wOrder);
                            wList.AddRange(wDataList.FindAll(p => p.ID != wOrder.ID));
                            int wFlag = 1;
                            foreach (OMSOrderItem wItem in wList)
                            {
                                wItem.OrderNum = wDic[wFlag];
                                OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wItem, out wErrorCode);
                                wFlag++;
                            }

                            //OMSOrderItem wItem = wList.Find(p => p.ID == wID);
                            //if (wItem.OrderNum == 1)
                            //{
                            //    wResult.FaultCode += "已移到顶部!";
                            //    return wResult;
                            //}
                            //wItem.OrderNum = 0;
                            //wList = wList.OrderBy(p => p.OrderNum).ToList();
                            //int wFlag = 1;
                            //foreach (OMSOrderItem wOMSOrderItem in wList)
                            //    wOMSOrderItem.OrderNum = wFlag++;
                        }
                        break;
                    case 4://置底
                        {
                            OMSOrderItem wOrder = wDataList.Find(p => p.ID == wID);

                            List<OMSOrderItem> wSmallList = wDataList.FindAll(p => p.OrderNum < wOrder.OrderNum);
                            if (wSmallList.Count <= 0)
                            {
                                wResult.FaultCode += "已移到顶部!";
                                return wResult;
                            }

                            List<OMSOrderItem> wList = new List<OMSOrderItem>();
                            wList.AddRange(wDataList.FindAll(p => p.ID != wOrder.ID));
                            wList.Add(wOrder);
                            int wFlag = 1;
                            foreach (OMSOrderItem wItem in wList)
                            {
                                wItem.OrderNum = wDic[wFlag];
                                OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wItem, out wErrorCode);
                                wFlag++;
                            }

                            //OMSOrderItem wItem = wList.Find(p => p.ID == wID);
                            //if (wItem.OrderNum == wList[wList.Count - 1].OrderNum)
                            //{
                            //    wResult.FaultCode += "已移到底部!";
                            //    return wResult;
                            //}
                            //wItem.OrderNum = wList[wList.Count - 1].OrderNum + 1;
                            //wList = wList.OrderBy(p => p.OrderNum).ToList();
                            //int wFlag = 1;
                            //foreach (OMSOrderItem wOMSOrderItem in wList)
                            //    wOMSOrderItem.OrderNum = wFlag++;
                        }
                        break;
                    default:
                        break;
                }
                //foreach (OMSOrderItem wOMSOrderItem in wList)
                //    OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wOMSOrderItem, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MCSLogInfo>> MCS_QueryLogInfoList(BMSEmployee wBMSEmployee, int wID, string wVersionNo, string wFileType, string wSystemType, string wProcessName, string wInfo, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<MCSLogInfo>> wResult = new ServiceResult<List<MCSLogInfo>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MCSLogInfoDAO.Instance.MCS_QueryMCSLogInfoList(wID, wVersionNo, wFileType, wSystemType, wProcessName, wInfo, wStartTime, wEndTime, wPagination, ""
                    , out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_TestWriteLog(BMSEmployee wBMSEmployee)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                string wTextContent = "测试内容::7401136923cdec15" + "::" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string wFileType = "测试类型";
                wResult.Result = MCSLogInfoDAO.Instance.MCS_WriteContentToDB(wTextContent, wFileType, "");

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<MCSLogInfo> MCS_addLogInfo(BMSEmployee wBMSEmployee, string wTextContent, string wFileType, string wSystemType, string wVersionNo, string wProcessName, string wStepNo, string wInfo)
        {
            ServiceResult<MCSLogInfo> wResult = new ServiceResult<MCSLogInfo>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MCSLogInfoDAO.Instance.MCS_AddWriteContentToDB(wTextContent, wFileType, wSystemType, wVersionNo, wProcessName, wStepNo, wInfo);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<string> MCS_DownloadLog(BMSEmployee wBMSEmployee, int wID)
        {
            ServiceResult<string> wResult = new ServiceResult<string>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MCSLogInfo> wList = MCSLogInfoDAO.Instance.MCS_QueryMCSLogInfoList(wID, "", "", "", "", "", wBaseTime, wBaseTime, Pagination.MaxSize, "", out wErrorCode);
                if (wList == null || wList.Count <= 0)
                    return wResult;

                wResult.Result = GlobalConstant.GlobalConfiguration.GetValue("Service.LogUri") + wList[0].FileName;
                //wResult.Result = wList[0].FilePath;
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<string> MCS_PreviewLogInfo(BMSEmployee wBMSEmployee, int wID)
        {
            ServiceResult<string> wResult = new ServiceResult<string>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MCSLogInfo> wList = MCSLogInfoDAO.Instance.MCS_QueryMCSLogInfoList(wID, "", "", "", "", "", wBaseTime, wBaseTime, Pagination.MaxSize, "", out wErrorCode);
                if (wList == null || wList.Count <= 0)
                    return wResult;

                wResult.Result = File.ReadAllText(wList[0].FilePath);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_SaveInterfaceConfig(BMSEmployee wBMSEmployee, MCSInterfaceConfig wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                wResult.Result = MCSInterfaceConfigDAO.Instance.MCS_SaveMCSInterfaceConfig(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_AddInterfaceConfig(BMSEmployee wBMSEmployee, MCSInterfaceConfig wFPCStructuralPart)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                if (string.IsNullOrEmpty(wFPCStructuralPart.EnumFlag))
                {
                    wResult.FaultCode += "枚举标识不能为空!";
                    return wResult;
                }
                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MCSInterfaceConfig> wList = MCSInterfaceConfigDAO.Instance.MCS_QueryMCSInterfaceConfigList(-1, "", -1, wFPCStructuralPart.EnumFlag, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode += "枚举标识已存在!";
                    return wResult;
                }

                wFPCStructuralPart.CreateID = wBMSEmployee.ID;
                wFPCStructuralPart.CreateTime = DateTime.Now;
                wFPCStructuralPart.EditID = wBMSEmployee.ID;
                wFPCStructuralPart.EditTime = DateTime.Now;
                wResult.Result = MCSInterfaceConfigDAO.Instance.MCS_SaveMCSInterfaceConfig(wFPCStructuralPart, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_DeleteInterfaceConfigList(BMSEmployee wBMSEmployee, List<MCSInterfaceConfig> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MCSInterfaceConfigDAO.Instance.MCS_DeleteMCSInterfaceConfigList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MCSInterfaceConfig>> MCS_QueryInterfaceConfigList(BMSEmployee wBMSEmployee, int wID, string wName, int wType, string wEnumFlag, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<MCSInterfaceConfig>> wResult = new ServiceResult<List<MCSInterfaceConfig>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MCSInterfaceConfigDAO.Instance.MCS_QueryMCSInterfaceConfigList(wID, wName, wType, wEnumFlag, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_DeleteLogInfoList(BMSEmployee wBMSEmployee, List<MCSLogInfo> wFPCStructuralPartList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MCSLogInfoDAO.Instance.MCS_DeleteMCSLogInfoList(wFPCStructuralPartList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_SaveMaterialPoint(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wMSSMaterialPoint.EditID = wBMSEmployee.ID;
                wMSSMaterialPoint.EditTime = DateTime.Now;
                wResult.Result = MSSMaterialPointDAO.Instance.MSS_SaveMSSMaterialPoint(wMSSMaterialPoint, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_AddMaterialPoint(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                if (string.IsNullOrEmpty(wMSSMaterialPoint.Name))
                {
                    wResult.FaultCode += "料点名称不能为空!";
                    return wResult;
                }
                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MSSMaterialPoint> wList = MSSMaterialPointDAO.Instance.MSS_QueryMSSMaterialPointList(-1, wMSSMaterialPoint.LineID, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.Name, wMSSMaterialPoint.StationPoint, wMSSMaterialPoint.DeliveryPoint, wMSSMaterialPoint.MaterialNo, wMSSMaterialPoint .PlanNo, wMSSMaterialPoint.UpdateTime, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode += "料点名称已存在!";
                    return wResult;
                }
                wMSSMaterialPoint.CreateID = wBMSEmployee.ID;
                wMSSMaterialPoint.CreateTime = DateTime.Now;
                wMSSMaterialPoint.EditID = wBMSEmployee.ID;
                wMSSMaterialPoint.EditTime = DateTime.Now;
                wMSSMaterialPoint.UpdateTime = DateTime.Now;
                wResult.Result = MSSMaterialPointDAO.Instance.MSS_SaveMSSMaterialPoint(wMSSMaterialPoint, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_DeleteMaterialPointList(BMSEmployee wBMSEmployee, List<MSSMaterialPoint> wMSSMaterialPointList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MSSMaterialPointDAO.Instance.MSS_DeleteMSSMaterialPointList(wMSSMaterialPointList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialPoint>> MSS_QueryMaterialPointList(BMSEmployee wBMSEmployee, int wID,int  wLineID,int  wAssetID, string wName, string wStationPoint, string wDeliveryPoint, string wMaterialNo,int  wPlanNo, DateTime wUpdateTime, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialPoint>> wResult = new ServiceResult<List<MSSMaterialPoint>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialPointDAO.Instance.MSS_QueryMSSMaterialPointList(wID,wLineID, wAssetID, wName, wStationPoint,wDeliveryPoint,wMaterialNo,wPlanNo,wUpdateTime, wPagination, out wErrorCode);

                wResult.Result = wResult.Result.OrderBy(p => p.ID).ToList();

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_ActiveMaterialPointList(BMSEmployee wBMSEmployee, int wActive, List<MSSMaterialPoint> wMSSMaterialPointList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (MSSMaterialPoint wFPCStructuralPart in wMSSMaterialPointList)
                {
                    wFPCStructuralPart.Active = wActive;
                    wFPCStructuralPart.EditID = wBMSEmployee.ID;
                    wFPCStructuralPart.EditTime = DateTime.Now;
                    MSSMaterialPointDAO.Instance.MSS_SaveMSSMaterialPoint(wFPCStructuralPart, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSCallMaterial>> MSS_QueryCallMaterialList(BMSEmployee wBMSEmployee, int wID, string wName, string wCode, int wActive, int wPlateID, int wMaterialPointID, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<MSSCallMaterial>> wResult = new ServiceResult<List<MSSCallMaterial>>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                wResult.Result = MSSCallMaterialDAO.Instance.MSS_QueryMSSCallMaterialList(wID, wCode, wName, wActive, wPlateID, wMaterialPointID, wStatus, wType, wBaseTime, wBaseTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        //public ServiceResult<int> MSS_ManualEntryCallMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial)
        //{
        //    ServiceResult<int> wResult = new ServiceResult<int>();
        //    try
        //    {
        //        int wErrorCode = 0;

        //        if (wMSSCallMaterial.DemandNumber <= 0)
        //        {
        //            wResult.FaultCode += "需求数量不能小于0!";
        //            return wResult;
        //        }

        //        if (wMSSCallMaterial.MaterialPointID <= 0)
        //        {
        //            wResult.FaultCode += "料点必填!";
        //            return wResult;
        //        }

        //        //查询料点属性，判断是否能混叠
        //        DateTime wBaseTime = new DateTime(2000, 1, 1);
        //        List<MSSMaterialPoint> wPointList = MSSMaterialPointDAO.Instance.MSS_QueryMSSMaterialPointList(wMSSCallMaterial.MaterialPointID, "", "", 1, -1, -1, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);
        //        if (wPointList.Count <= 0)
        //        {
        //            wResult.FaultCode += "未获取到料点信息，请联系管理员!";
        //            return wResult;
        //        }
        //        //查询料点的是实时库存
        //        List<MSSMaterialStock> wStockList = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, wMSSCallMaterial.MaterialPointID, -1, -1, Pagination.MaxSize, out wErrorCode);
        //        MSSMaterialPoint wMSSMaterialPoint = wPointList[0];
        //        if (wMSSMaterialPoint.IsAliasing != 1)
        //        {
        //            if (wStockList.Count > 0)
        //            {
        //                wStockList = wStockList.OrderByDescending(p => p.OrderID).ToList();
        //                if (wStockList[0].PlateID != wMSSCallMaterial.PlateID)
        //                {
        //                    wResult.FaultCode += "该料点不支持混叠!";
        //                    return wResult;
        //                }
        //            }
        //        }

        //        wMSSCallMaterial.Code = MSSCallMaterialDAO.Instance.GetNewCode(out wErrorCode);
        //        wMSSCallMaterial.CreateID = wBMSEmployee.ID;
        //        wMSSCallMaterial.CreateTime = DateTime.Now;
        //        wMSSCallMaterial.EditID = wBMSEmployee.ID;
        //        wMSSCallMaterial.EditTime = DateTime.Now;
        //        wMSSCallMaterial.ArriveNumber = wMSSCallMaterial.DemandNumber;
        //        wMSSCallMaterial.ArriveTime = DateTime.Now;
        //        wMSSCallMaterial.ConfirmID = wBMSEmployee.ID;
        //        wMSSCallMaterial.ConfirmTime = DateTime.Now;
        //        wMSSCallMaterial.Status = 20;
        //        wResult.Result = MSSCallMaterialDAO.Instance.MSS_SaveMSSCallMaterial(wMSSCallMaterial, out wErrorCode);

        //        //入库
        //        wStockList = wStockList.OrderByDescending(p => p.OrderID).ToList();
        //        int wNumber = 1;
        //        if (wStockList.Count > 0)
        //        {
        //            wNumber = wStockList[0].OrderID + 1;
        //        }
        //        for (int i = 1; i <= wMSSCallMaterial.DemandNumber; i++)
        //        {
        //            MSSMaterialStock wMSSMaterialStock = new MSSMaterialStock();
        //            wMSSMaterialStock.ID = 0;
        //            wMSSMaterialStock.Code = MSSMaterialStockDAO.Instance.GetNewCode(out wErrorCode);
        //            wMSSMaterialStock.Name = "";
        //            wMSSMaterialStock.Remark = "手动录入";
        //            wMSSMaterialStock.Active = 1;
        //            wMSSMaterialStock.CreateID = wBMSEmployee.ID;
        //            wMSSMaterialStock.CreateTime = DateTime.Now;
        //            wMSSMaterialStock.EditID = wBMSEmployee.ID;
        //            wMSSMaterialStock.EditTime = DateTime.Now;
        //            wMSSMaterialStock.MaterialPointID = wMSSCallMaterial.MaterialPointID;
        //            wMSSMaterialStock.PlateID = wMSSCallMaterial.PlateID;
        //            wMSSMaterialStock.OrderID = wNumber++;
        //            MSSMaterialStockDAO.Instance.MSS_SaveMSSMaterialStock(wMSSMaterialStock, out wErrorCode);
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}

        public ServiceResult<int> MSS_ManualCallMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                if (wMSSCallMaterial.DemandNumber <= 0)
                {
                    wResult.FaultCode += "需求数量不能小于0!";
                    return wResult;
                }

                if (wMSSCallMaterial.MaterialPointID <= 0)
                {
                    wResult.FaultCode += "料点必填!";
                    return wResult;
                }

                if (wMSSCallMaterial.PlateID <= 0)
                {
                    wResult.FaultCode += "钢板必填";
                    return wResult;
                }

                DateTime wBaseTime = new DateTime(2000, 1, 1);

                wMSSCallMaterial.Code = MSSCallMaterialDAO.Instance.GetNewCode(out wErrorCode);
                wMSSCallMaterial.CreateID = wBMSEmployee.ID;
                wMSSCallMaterial.CreateTime = DateTime.Now;
                wMSSCallMaterial.EditID = wBMSEmployee.ID;
                wMSSCallMaterial.EditTime = DateTime.Now;
                wMSSCallMaterial.Status = 1;
                wMSSCallMaterial.ArriveTime = wBaseTime;
                wMSSCallMaterial.ConfirmTime = wBaseTime;
                MSSCallMaterialDAO.Instance.MSS_SaveMSSCallMaterial(wMSSCallMaterial, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        //public ServiceResult<int> MSS_ConfirmMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial)
        //{
        //    ServiceResult<int> wResult = new ServiceResult<int>();
        //    try
        //    {
        //        int wErrorCode = 0;

        //        if (wMSSCallMaterial.ArriveNumber <= 0)
        //        {
        //            wResult.FaultCode += "到达数量不能小于0!";
        //            return wResult;
        //        }

        //        //查询料点属性，判断是否能混叠
        //        DateTime wBaseTime = new DateTime(2000, 1, 1);
        //        List<MSSMaterialPoint> wPointList = MSSMaterialPointDAO.Instance.MSS_QueryMSSMaterialPointList(wMSSCallMaterial.MaterialPointID, "", "", 1, -1, -1, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);
        //        if (wPointList.Count <= 0)
        //        {
        //            wResult.FaultCode += "未获取到料点信息，请联系管理员!";
        //            return wResult;
        //        }
        //        //查询料点的是实时库存
        //        List<MSSMaterialStock> wStockList = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, wMSSCallMaterial.MaterialPointID, -1, -1, Pagination.MaxSize, out wErrorCode);
        //        MSSMaterialPoint wMSSMaterialPoint = wPointList[0];
        //        if (wMSSMaterialPoint.IsAliasing != 1)
        //        {
        //            if (wStockList.Count > 0)
        //            {
        //                wStockList = wStockList.OrderByDescending(p => p.OrderID).ToList();
        //                if (wStockList[0].PlateID != wMSSCallMaterial.PlateID)
        //                {
        //                    wResult.FaultCode += "该料点不支持混叠!";
        //                    return wResult;
        //                }
        //            }
        //        }

        //        wMSSCallMaterial.EditID = wBMSEmployee.ID;
        //        wMSSCallMaterial.EditTime = DateTime.Now;
        //        wMSSCallMaterial.ConfirmID = wBMSEmployee.ID;
        //        wMSSCallMaterial.ConfirmTime = DateTime.Now;
        //        wMSSCallMaterial.Status = 20;
        //        MSSCallMaterialDAO.Instance.MSS_SaveMSSCallMaterial(wMSSCallMaterial, out wErrorCode);

        //        //入库
        //        wStockList = wStockList.OrderByDescending(p => p.OrderID).ToList();
        //        int wNumber = 1;
        //        if (wStockList.Count > 0)
        //        {
        //            wNumber = wStockList[0].OrderID + 1;
        //        }
        //        for (int i = 1; i <= wMSSCallMaterial.ArriveNumber; i++)
        //        {
        //            MSSMaterialStock wMSSMaterialStock = new MSSMaterialStock();
        //            wMSSMaterialStock.ID = 0;
        //            wMSSMaterialStock.Code = MSSMaterialStockDAO.Instance.GetNewCode(out wErrorCode);
        //            wMSSMaterialStock.Name = "";
        //            wMSSMaterialStock.Remark = "手动叫料";
        //            wMSSMaterialStock.Active = 1;
        //            wMSSMaterialStock.CreateID = wBMSEmployee.ID;
        //            wMSSMaterialStock.CreateTime = DateTime.Now;
        //            wMSSMaterialStock.EditID = wBMSEmployee.ID;
        //            wMSSMaterialStock.EditTime = DateTime.Now;
        //            wMSSMaterialStock.MaterialPointID = wMSSCallMaterial.MaterialPointID;
        //            wMSSMaterialStock.PlateID = wMSSCallMaterial.PlateID;
        //            wMSSMaterialStock.OrderID = wNumber++;
        //            MSSMaterialStockDAO.Instance.MSS_SaveMSSMaterialStock(wMSSMaterialStock, out wErrorCode);
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}

        //public ServiceResult<List<MSSMaterialPoint>> MSS_QueryAllStockMaterialPointList(BMSEmployee wBMSEmployee, string wName, Pagination wPagination)
        //{
        //    ServiceResult<List<MSSMaterialPoint>> wResult = new ServiceResult<List<MSSMaterialPoint>>();
        //    try
        //    {
        //        int wErrorCode = 0;

        //        DateTime wBaseTime = new DateTime(2000, 1, 1);
        //        wResult.Result = MSSMaterialPointDAO.Instance.MSS_QueryMSSMaterialPointList(-1, "", "","", "", "", "",-1,  wBaseTime, wPagination, out wErrorCode);
        //        wResult.Result = wResult.Result.OrderBy(p => p.ID).ToList();

        //        if (!string.IsNullOrWhiteSpace(wName))
        //            wResult.Result = wResult.Result.FindAll(p => p.Name.Contains(wName));

        //        List<MSSMaterialStock> wStockList = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, -1, -1, -1, Pagination.MaxSize, out wErrorCode);
        //        wStockList = wStockList.OrderByDescending(p => p.OrderID).ToList();

        //        List<MSSCallMaterial> wCallList = MSSCallMaterialDAO.Instance.MSS_QueryMSSCallMaterialList(-1, "", "", -1, -1, -1, 1, -1, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);

        //        foreach (MSSMaterialPoint wMSSMaterialPoint in wResult.Result)
        //        {
        //            List<MSSMaterialStock> wList = wStockList.FindAll(p => p.MaterialPointID == wMSSMaterialPoint.ID);
        //            wList = wList.OrderByDescending(p => p.OrderID).ToList();
        //            wMSSMaterialPoint.Stock = wList.Count;
        //            wMSSMaterialPoint.PlateName = wList.Count > 0 ? wList[0].PlateName : "";
        //            wMSSMaterialPoint.InStock = wList.Count;
        //            wMSSMaterialPoint.OutStock = 0;

        //            List<MSSCallMaterial> wCallPointList = wCallList.FindAll(p => p.MaterialPointID == wMSSMaterialPoint.ID);
        //            int wDemandNumber = 0;
        //            foreach (MSSCallMaterial wMSSCallMaterial in wCallPointList)
        //                wDemandNumber += wMSSCallMaterial.DemandNumber;
        //            wMSSMaterialPoint.CallStock = wDemandNumber;
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}

        public ServiceResult<List<MSSMaterialStock>> MSS_QueryAllMaterialStockActive(BMSEmployee wBMSEmployee, int wBinID)
        {
            ServiceResult<List<MSSMaterialStock>> wResult = new ServiceResult<List<MSSMaterialStock>>();
            try
            {
                int wErrorCode = 0;


                wResult.Result = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, wBinID, -1, -1, Pagination.MaxSize, out wErrorCode);
                //降序从大到小
                wResult.Result = wResult.Result.OrderByDescending(p => p.OrderID).ToList();

                MSSMaterialStock wMSSMaterialStock = new MSSMaterialStock();
                wMSSMaterialStock = wResult.Result[0];
                wMSSMaterialStock.Active = 2;
                MSSMaterialStockDAO.Instance.MSS_SaveMSSMaterialStock(wMSSMaterialStock, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialStock>> MSS_QueryAllMaterialStock(BMSEmployee wBMSEmployee, string wCode, int wActive, int wMaterialPointID, int wPlateID, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialStock>> wResult = new ServiceResult<List<MSSMaterialStock>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, wCode, "", wActive, wMaterialPointID, wPlateID, -1, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveSpareParts(BMSEmployee wBMSEmployee, OMSSpareParts wOMSSpareParts)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSSpareParts.EditID = wBMSEmployee.ID;
                wOMSSpareParts.EditTime = DateTime.Now;
                wResult.Result = OMSSparePartsDAO.Instance.OMS_SaveOMSSpareParts(wOMSSpareParts, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_AddSpareParts(BMSEmployee wBMSEmployee, OMSSpareParts wOMSSpareParts)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                wOMSSpareParts.CreateID = wBMSEmployee.ID;
                wOMSSpareParts.Creator = wBMSEmployee.Name;
                wOMSSpareParts.CreateTime = DateTime.Now;
                wOMSSpareParts.EditID = wBMSEmployee.ID;
                wOMSSpareParts.Editor = wBMSEmployee.Name;
                wOMSSpareParts.EditTime = DateTime.Now;
                wOMSSpareParts.QRCode = GlobalConstant.GlobalConfiguration.GetValue("Service.QRCodeUri") + QRCodeTool.CreateQRImg(wOMSSpareParts.PlanNumber + "," + wOMSSpareParts.PieceNo);
                wResult.Result = OMSSparePartsDAO.Instance.OMS_SaveOMSSpareParts(wOMSSpareParts, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_DeleteSparePartsList(BMSEmployee wBMSEmployee, List<OMSSpareParts> wOMSSparePartsList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = OMSSparePartsDAO.Instance.OMS_DeleteOMSSparePartsList(wOMSSparePartsList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<OMSSpareParts>> OMS_QuerySparePartsList(BMSEmployee wBMSEmployee, int wID, int wActive, int wType, int wOrderID, int wLesOrderID, int wPartType, string wPlanNumber, string wPieceNo, Pagination wPagination)
        {
            ServiceResult<List<OMSSpareParts>> wResult = new ServiceResult<List<OMSSpareParts>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = OMSSparePartsDAO.Instance.OMS_QueryOMSSparePartsList(wID, "", wActive, wType, wOrderID, wLesOrderID, wPartType, wPlanNumber, wPieceNo, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_ActiveSparePartsList(BMSEmployee wBMSEmployee, int wActive, List<OMSSpareParts> wOMSSparePartsList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (OMSSpareParts wFMCShiftItem in wOMSSparePartsList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    OMSSparePartsDAO.Instance.OMS_SaveOMSSpareParts(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_SaveUploadRecord(BMSEmployee wBMSEmployee, OMSUploadRecord wOMSUploadRecord)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wOMSUploadRecord.EditID = wBMSEmployee.ID;
                wOMSUploadRecord.EditTime = DateTime.Now;
                wResult.Result = OMSUploadRecordDAO.Instance.OMS_SaveOMSUploadRecord(wOMSUploadRecord, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_AddUploadRecord(BMSEmployee wBMSEmployee, OMSUploadRecord wOMSUploadRecord)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                //NC文件换地址存储
                if (!string.IsNullOrWhiteSpace(wOMSUploadRecord.NCFileUri))
                {
                    string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    int wIndex = wBaseDir.LastIndexOf('\\');
                    wBaseDir = wBaseDir.Substring(0, wIndex);
                    wIndex = wBaseDir.LastIndexOf('\\');
                    wBaseDir = wBaseDir.Substring(0, wIndex + 1);
                    string dirpath = wBaseDir + "NCFiles\\";
                    if (!Directory.Exists(dirpath))
                        Directory.CreateDirectory(dirpath);
                    string wPath = wBaseDir + "NCFiles\\" + wOMSUploadRecord.NCFileName;

                    //文件复制
                    string wUploadAddress = GlobalConstant.GlobalConfiguration.GetValue("Service.UploadAddress");
                    FileInfo wFileInfo = new FileInfo(wUploadAddress + wOMSUploadRecord.NCFileUri.Replace("/iPlantCore", ""));
                    wFileInfo.CopyTo(wPath, true);

                    wOMSUploadRecord.NCFileUri = "/NCFiles/" + wOMSUploadRecord.NCFileName;
                }

                //DXF文件换地址存储
                if (!string.IsNullOrWhiteSpace(wOMSUploadRecord.DXFFileUri))
                {
                    string wBaseDir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    int wIndex = wBaseDir.LastIndexOf('\\');
                    wBaseDir = wBaseDir.Substring(0, wIndex);
                    wIndex = wBaseDir.LastIndexOf('\\');
                    wBaseDir = wBaseDir.Substring(0, wIndex + 1);
                    string dirpath = wBaseDir + "DXFFiles\\";
                    if (!Directory.Exists(dirpath))
                        Directory.CreateDirectory(dirpath);
                    string wPath = wBaseDir + "DXFFiles\\" + wOMSUploadRecord.DXFFileName;

                    //文件复制
                    string wUploadAddress = GlobalConstant.GlobalConfiguration.GetValue("Service.UploadAddress");
                    FileInfo wFileInfo = new FileInfo(wUploadAddress + wOMSUploadRecord.DXFFileUri.Replace("/iPlantCore", ""));
                    wFileInfo.CopyTo(wPath, true);

                    wOMSUploadRecord.DXFFileUri = "/DXFFiles/" + wOMSUploadRecord.DXFFileName;
                }

                wOMSUploadRecord.CreateID = wBMSEmployee.ID;
                wOMSUploadRecord.CreateTime = DateTime.Now;
                wOMSUploadRecord.EditID = wBMSEmployee.ID;
                wOMSUploadRecord.EditTime = DateTime.Now;
                wOMSUploadRecord.Code = OMSUploadRecordDAO.Instance.GetNewCode(out wErrorCode);
                wResult.Result = OMSUploadRecordDAO.Instance.OMS_SaveOMSUploadRecord(wOMSUploadRecord, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();

                if (wOMSUploadRecord.ID > 0)
                {
                    MCSOperationLog wMCSOperationLog = new MCSOperationLog();
                    wMCSOperationLog.ModuleID = (int)MCSModuleType.PicParse;
                    wMCSOperationLog.Type = (int)MCSOperateType.Upload;
                    wMCSOperationLog.CreateID = wBMSEmployee.ID;
                    wMCSOperationLog.CreateTime = DateTime.Now;
                    wMCSOperationLog.EditID = wBMSEmployee.ID;
                    wMCSOperationLog.EditTime = DateTime.Now;
                    wMCSOperationLog.Content = String.Format("{0}在{1}上传了套料图解析文件【{2}】", wBMSEmployee.Name, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), wOMSUploadRecord.DXFFileName);
                    MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                }
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_AddUploadRecord)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return wResult;
        }

        public ServiceResult<List<OMSUploadRecord>> OMS_QueryUploadRecordList(BMSEmployee wBMSEmployee, int wID, string wCode, string wNCFileName, string wDXFFileName, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<OMSUploadRecord>> wResult = new ServiceResult<List<OMSUploadRecord>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = OMSUploadRecordDAO.Instance.OMS_QueryOMSUploadRecordList(wID, wCode, wNCFileName, wDXFFileName, wStatus, wType, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_SaveStation(BMSEmployee wBMSEmployee, FMCStation wFMCStation)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wFMCStation.EditorID = wBMSEmployee.ID;
                wFMCStation.EditTime = DateTime.Now;
                wResult.Result = FMCStationDAO.Instance.FMC_SaveFMCStation(wFMCStation, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_AddStation(BMSEmployee wBMSEmployee, FMCStation wFMCStation)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                wFMCStation.CreatorID = wBMSEmployee.ID;
                wFMCStation.CreateTime = DateTime.Now;
                wFMCStation.EditorID = wBMSEmployee.ID;
                wFMCStation.EditTime = DateTime.Now;
                wResult.Result = FMCStationDAO.Instance.FMC_SaveFMCStation(wFMCStation, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_DeleteStationList(BMSEmployee wBMSEmployee, List<FMCStation> wFMCStationList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FMCStationDAO.Instance.FMC_DeleteFMCStationList(wFMCStationList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wBMSEmployee, int wID, String wName, String wCode, int wLineID, int wActive, Pagination wPagination)
        {
            ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = FMCStationDAO.Instance.FMC_QueryFMCStationList(wID, wName, wCode, wLineID, wActive, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_ActiveStationList(BMSEmployee wBMSEmployee, int wActive, List<FMCStation> wFMCStationList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FMCStation wFMCShiftItem in wFMCStationList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditorID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    FMCStationDAO.Instance.FMC_SaveFMCStation(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<string> OMS_TestQRCode(BMSEmployee wBMSEmployee)
        {
            ServiceResult<string> wResult = new ServiceResult<string>();
            try
            {
                wResult.Result = QRCodeTool.CreateQRImg("测试测试");
            }
            catch (Exception ex)
            {
                MCSLogInfoDAO.Instance.MCS_WriteContentToDB(StringUtils.Format("{0} ERROR(OMS_TestQRCode)-{1}::{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, ex.StackTrace), "系统内部错误", "iPlantSany");
                logger.Error(ex);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialPointStock>> MSS_QueryPointDetail(BMSEmployee wBMSEmployee, int wMaterialPointID)
        {
            ServiceResult<List<MSSMaterialPointStock>> wResult = new ServiceResult<List<MSSMaterialPointStock>>();
            wResult.Result = new List<MSSMaterialPointStock>();
            try
            {
                int wErrorCode = 0;

                //在线库存明细
                List<MSSMaterialStock> wStockList = MSSMaterialStockDAO.Instance.MSS_QueryMSSMaterialStockList(-1, "", "", 1, wMaterialPointID, -1, -1, Pagination.MaxSize, out wErrorCode);
                foreach (MSSMaterialStock wMSSMaterialStock in wStockList)
                {
                    if (wResult.Result.Exists(p => p.ID == wMSSMaterialStock.PlateID))
                        continue;

                    List<MSSMaterialStock> wStockItemList = wStockList.FindAll(p => p.PlateID == wMSSMaterialStock.PlateID);
                    MSSMaterialPointStock wMSSMaterialPointStock = new MSSMaterialPointStock();
                    wMSSMaterialPointStock.ID = wMSSMaterialStock.PlateID;
                    wMSSMaterialPointStock.Name = wMSSMaterialStock.PlateName;
                    wMSSMaterialPointStock.FQTY = wStockItemList.Count;
                    wResult.Result.Add(wMSSMaterialPointStock);
                }
                //叫料库存明细
                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<MSSCallMaterial> wCallList = MSSCallMaterialDAO.Instance.MSS_QueryMSSCallMaterialList(-1, "", "", -1, -1, wMaterialPointID, 1, -1, wBaseTime, wBaseTime, Pagination.MaxSize, out wErrorCode);
                List<MSSMaterialPointStock> wCallDic = new List<MSSMaterialPointStock>();
                foreach (MSSCallMaterial wMSSCallMaterial in wCallList)
                {
                    if (wCallDic.Exists(p => p.ID == wMSSCallMaterial.PlateID))
                        continue;

                    List<MSSCallMaterial> wCallItemList = wCallList.FindAll(p => p.PlateID == wMSSCallMaterial.PlateID);
                    int wNumber = 0;
                    foreach (MSSCallMaterial wItemCall in wCallItemList)
                        wNumber += wItemCall.DemandNumber;
                    MSSMaterialPointStock wMSSMaterialPointStock = new MSSMaterialPointStock();
                    wMSSMaterialPointStock.ID = wMSSCallMaterial.PlateID;
                    wMSSMaterialPointStock.Name = wMSSCallMaterial.PlateName;
                    wMSSMaterialPointStock.FQTY = wNumber;
                    wCallDic.Add(wMSSMaterialPointStock);
                }
                wResult.CustomResult.Add("CallDetail", wCallDic);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_SaveDataDictionary(BMSEmployee wBMSEmployee, FMCDataDictionary wFMCDataDictionary)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                List<FMCDataDictionary> wList = FMCDataDictionaryDAO.Instance.FMC_QueryFMCDataDictionaryList(-1, wFMCDataDictionary.Code, "", -1, wFMCDataDictionary.Type, Pagination.MaxSize, out wErrorCode);
                if (wList.Exists(p => p.ID != wFMCDataDictionary.ID))
                {
                    wResult.FaultCode += "字典键重复!";
                    return wResult;
                }

                wList = FMCDataDictionaryDAO.Instance.FMC_QueryFMCDataDictionaryList(-1, "", wFMCDataDictionary.Name, -1, wFMCDataDictionary.Type, Pagination.MaxSize, out wErrorCode);
                if (wList.Exists(p => p.ID != wFMCDataDictionary.ID))
                {
                    wResult.FaultCode += "字典项名称重复!";
                    return wResult;
                }

                wFMCDataDictionary.EditID = wBMSEmployee.ID;
                wFMCDataDictionary.EditTime = DateTime.Now;
                wResult.Result = FMCDataDictionaryDAO.Instance.FMC_SaveFMCDataDictionary(wFMCDataDictionary, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_AddDataDictionary(BMSEmployee wBMSEmployee, FMCDataDictionary wFMCDataDictionary)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                if (string.IsNullOrEmpty(wFMCDataDictionary.Code))
                {
                    wResult.FaultCode += "字典键不能为空!";
                    return wResult;
                }

                if (string.IsNullOrEmpty(wFMCDataDictionary.Name))
                {
                    wResult.FaultCode += "字典项名称不能为空!";
                    return wResult;
                }

                List<FMCDataDictionary> wList = FMCDataDictionaryDAO.Instance.FMC_QueryFMCDataDictionaryList(-1, wFMCDataDictionary.Code, "", -1, wFMCDataDictionary.Type, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode += "字典键重复!";
                    return wResult;
                }

                wList = FMCDataDictionaryDAO.Instance.FMC_QueryFMCDataDictionaryList(-1, "", wFMCDataDictionary.Name, -1, wFMCDataDictionary.Type, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode += "字典项名称重复!";
                    return wResult;
                }

                wFMCDataDictionary.CreateID = wBMSEmployee.ID;
                wFMCDataDictionary.CreateTime = DateTime.Now;
                wFMCDataDictionary.EditID = wBMSEmployee.ID;
                wFMCDataDictionary.EditTime = DateTime.Now;
                wResult.Result = FMCDataDictionaryDAO.Instance.FMC_SaveFMCDataDictionary(wFMCDataDictionary, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_DeleteDataDictionaryList(BMSEmployee wBMSEmployee, List<FMCDataDictionary> wFMCDataDictionaryList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = FMCDataDictionaryDAO.Instance.FMC_DeleteFMCDataDictionaryList(wFMCDataDictionaryList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<FMCDataDictionary>> FMC_QueryDataDictionaryList(BMSEmployee wBMSEmployee, int wID, String wCode, String wName, int wActive, int wType, Pagination wPagination)
        {
            ServiceResult<List<FMCDataDictionary>> wResult = new ServiceResult<List<FMCDataDictionary>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = FMCDataDictionaryDAO.Instance.FMC_QueryFMCDataDictionaryList(wID, wCode, wName, wActive, wType, wPagination, out wErrorCode);
                wResult.Result = wResult.Result.OrderBy(p => p.OrderID).ToList();

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> FMC_ActiveDataDictionaryList(BMSEmployee wBMSEmployee, int wActive, List<FMCDataDictionary> wFMCDataDictionaryList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (FMCDataDictionary wFMCShiftItem in wFMCDataDictionaryList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    FMCDataDictionaryDAO.Instance.FMC_SaveFMCDataDictionary(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_SaveFeedGroup(BMSEmployee wBMSEmployee, MSSFeedGroup wMSSFeedGroup)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                List<MSSFeedGroup> wList = MSSFeedGroupDAO.Instance.MSS_QueryMSSFeedGroupList(-1, "", wMSSFeedGroup.Name, -1, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode = "名称重复";
                    return wResult;
                }

                wMSSFeedGroup.EditID = wBMSEmployee.ID;
                wMSSFeedGroup.EditTime = DateTime.Now;
                wResult.Result = MSSFeedGroupDAO.Instance.MSS_SaveMSSFeedGroup(wMSSFeedGroup, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_AddFeedGroup(BMSEmployee wBMSEmployee, MSSFeedGroup wMSSFeedGroup)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                List<MSSFeedGroup> wList = MSSFeedGroupDAO.Instance.MSS_QueryMSSFeedGroupList(-1, "", wMSSFeedGroup.Name, -1, Pagination.MaxSize, out wErrorCode);
                if (wList.Count > 0)
                {
                    wResult.FaultCode = "名称重复";
                    return wResult;
                }

                wMSSFeedGroup.CreateID = wBMSEmployee.ID;
                wMSSFeedGroup.CreateTime = DateTime.Now;
                wMSSFeedGroup.EditID = wBMSEmployee.ID;
                wMSSFeedGroup.EditTime = DateTime.Now;
                wResult.Result = MSSFeedGroupDAO.Instance.MSS_SaveMSSFeedGroup(wMSSFeedGroup, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_DeleteFeedGroupList(BMSEmployee wBMSEmployee, List<MSSFeedGroup> wMSSFeedGroupList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MSSFeedGroupDAO.Instance.MSS_DeleteMSSFeedGroupList(wMSSFeedGroupList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSFeedGroup>> MSS_QueryFeedGroupList(BMSEmployee wBMSEmployee, int wID, String wCode, String wName, int wActive, Pagination wPagination)
        {
            ServiceResult<List<MSSFeedGroup>> wResult = new ServiceResult<List<MSSFeedGroup>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSFeedGroupDAO.Instance.MSS_QueryMSSFeedGroupList(wID, wCode, wName, wActive, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_ActiveFeedGroupList(BMSEmployee wBMSEmployee, int wActive, List<MSSFeedGroup> wMSSFeedGroupList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (MSSFeedGroup wFMCShiftItem in wMSSFeedGroupList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    MSSFeedGroupDAO.Instance.MSS_SaveMSSFeedGroup(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_SaveOperationLog(BMSEmployee wBMSEmployee, MCSOperationLog wMCSOperationLog)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wMCSOperationLog.EditID = wBMSEmployee.ID;
                wMCSOperationLog.EditTime = DateTime.Now;
                wResult.Result = MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_AddOperationLog(BMSEmployee wBMSEmployee, MCSOperationLog wMCSOperationLog)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                wMCSOperationLog.CreateID = wBMSEmployee.ID;
                wMCSOperationLog.CreateTime = DateTime.Now;
                wMCSOperationLog.EditID = wBMSEmployee.ID;
                wMCSOperationLog.EditTime = DateTime.Now;
                wResult.Result = MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wMCSOperationLog, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_DeleteOperationLogList(BMSEmployee wBMSEmployee, List<MCSOperationLog> wMCSOperationLogList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MCSOperationLogDAO.Instance.MCS_DeleteMCSOperationLogList(wMCSOperationLogList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MCSOperationLog>> MCS_QueryOperationLogList(BMSEmployee wBMSEmployee, int wID, int wModuleID, int wType, String wContent, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<MCSOperationLog>> wResult = new ServiceResult<List<MCSOperationLog>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MCSOperationLogDAO.Instance.MCS_QueryMCSOperationLogList(wID, wModuleID, wType, wContent, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MCS_ActiveOperationLogList(BMSEmployee wBMSEmployee, int wActive, List<MCSOperationLog> wMCSOperationLogList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (MCSOperationLog wFMCShiftItem in wMCSOperationLogList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    MCSOperationLogDAO.Instance.MCS_SaveMCSOperationLog(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<OMSOrderItem>> OMS_QueryOrderItemListByDeviceNo(BMSEmployee wBMSEmployee, string wDeviceNo)
        {
            ServiceResult<List<OMSOrderItem>> wResult = new ServiceResult<List<OMSOrderItem>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = OMSOrderItemDAO.Instance.OMS_QueryOrderItemListByDeviceNo(wDeviceNo, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<OMSOrderItem> OMS_QueryOrderItemListByCutType(BMSEmployee wBMSEmployee, int wCutType)
        {
            ServiceResult<OMSOrderItem> wResult = new ServiceResult<OMSOrderItem>();
            wResult.Result = new OMSOrderItem();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<OMSOrderItem> wItemList = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(-1, -1, "", "", "", wCutType, "", -1, "", "", wBaseTime, wBaseTime, 4, Pagination.MaxSize, -1, -1, -1, -1,1, out wErrorCode);
                if (wItemList != null && wItemList.Count > 0)
                    wResult.Result = wItemList.OrderBy(p => p.OrderNum).ToList()[0];

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_UpdateOrderItemByCode(BMSEmployee wBMSEmployee, string wOrderNo, int wStatus)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<OMSOrderItem> wList = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(-1, -1, wOrderNo, "", "", -1, "", -1, "", "", wBaseTime, wBaseTime, -1, Pagination.MaxSize, -1, -1, -1, -1,1, out wErrorCode);
                if (wList.Count <= 0)
                {
                    wResult.FaultCode += "工单不存在!";
                    return wResult;
                }

                wList[0].Status = wStatus;
                wList[0].EditID = wBMSEmployee.ID;
                wList[0].EditTime = DateTime.Now;
                OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wList[0], out wErrorCode);
                wResult.Result = wList[0].ID;

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> OMS_UpdateOrderItemByCuttingNumber(BMSEmployee wBMSEmployee, string wCuttingNumber, int wStatus)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<OMSOrderItem> wList = OMSOrderItemDAO.Instance.OMS_QueryOMSOrderItemList(-1, -1, "", wCuttingNumber, "", -1, "", -1, "", "", wBaseTime, wBaseTime, -1, Pagination.MaxSize, -1, -1, -1, -1,1, out wErrorCode);
                if (wList.Count <= 0)
                {
                    wResult.FaultCode += "工单不存在!";
                    return wResult;
                }

                wList[0].Status = wStatus;
                wList[0].EditID = wBMSEmployee.ID;
                wList[0].EditTime = DateTime.Now;
                OMSOrderItemDAO.Instance.OMS_SaveOMSOrderItem(wList[0], out wErrorCode);
                wResult.Result = wList[0].ID;

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> FMC_AddFactory(BMSEmployee wLoginUser, FMCFactory wFactory)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCFactoryDAO.getInstance().FMC_AddFactory(wLoginUser, wFactory, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveFactory(BMSEmployee wLoginUser, FMCFactory wFactory)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCFactoryDAO.getInstance().FMC_SaveFactory(wLoginUser, wFactory, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DisableFactory(BMSEmployee wLoginUser, FMCFactory wFactory)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_DisableFactory(wLoginUser, wFactory, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveFactory(BMSEmployee wLoginUser, FMCFactory wFactory)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_ActiveFactory(wLoginUser, wFactory, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DeleteFactory(BMSEmployee wLoginUser, FMCFactory wFactory)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_DeleteFactory(wLoginUser, wFactory, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<FMCFactory> FMC_QueryFactoryByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCFactory> wResult = new ServiceResult<FMCFactory>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCFactory> FMC_QueryFactoryByCode(BMSEmployee wLoginUser, String wCode)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCFactory> wResult = new ServiceResult<FMCFactory>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryByCode(wLoginUser, wCode,
                        wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCFactory>> FMC_QueryFactoryList(BMSEmployee wLoginUser, String wName, int wCountryID,
            int wProvinceID,
                int wCityID, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCFactory>> wResult = new ServiceResult<List<FMCFactory>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCFactoryDAO.getInstance().FMC_QueryFactoryList(wLoginUser, wName, wCountryID,
             wProvinceID,
                 wCityID, wActive, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> FMC_AddWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_AddWorkShop(wLoginUser, wWorkShop, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_SaveWorkShop(wLoginUser, wWorkShop, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DisableWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_DisableWorkShop(wLoginUser, wWorkShop,
                        wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_ActiveWorkShop(wLoginUser, wWorkShop,
                        wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }




        public ServiceResult<FMCWorkShop> FMC_QueryWorkShopByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCWorkShop> wResult = new ServiceResult<FMCWorkShop>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_QueryWorkShopByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCWorkShop> FMC_QueryWorkShopByCode(BMSEmployee wLoginUser, String wCode)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCWorkShop> wResult = new ServiceResult<FMCWorkShop>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_QueryWorkShopByCode(wLoginUser, wCode,
                        wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCWorkShop>> FMC_QueryWorkShopList(BMSEmployee wLoginUser, int wFactoryID,
                int wBusinessUnitID, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCWorkShop>> wResult = new ServiceResult<List<FMCWorkShop>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCWorkShopDAO.getInstance().FMC_QueryWorkShopList(wLoginUser, wFactoryID,
                        wBusinessUnitID, wActive, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_AddLine(BMSEmployee wLoginUser, FMCLine wLine)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCLineDAO.getInstance().FMC_AddLine(wLoginUser, wLine, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveLine(BMSEmployee wLoginUser, FMCLine wLine)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCLineDAO.getInstance().FMC_SaveLine(wLoginUser, wLine, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DisableLine(BMSEmployee wLoginUser, FMCLine wLine)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCLineDAO.getInstance().FMC_DisableLine(wLoginUser, wLine, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveLine(BMSEmployee wLoginUser, FMCLine wLine)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCLineDAO.getInstance().FMC_ActiveLine(wLoginUser, wLine, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCLine> FMC_QueryLineByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCLine> wResult = new ServiceResult<FMCLine>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCLine> FMC_QueryLineByCode(BMSEmployee wLoginUser, String wCode)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCLine> wResult = new ServiceResult<FMCLine>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineByCode(wLoginUser, wCode, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCLine>> FMC_QueryLineList(BMSEmployee wLoginUser, int wBusinessUnitID,
                int wFactoryID, int wWorkShopID, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCLine>> wResult = new ServiceResult<List<FMCLine>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCLineDAO.getInstance().FMC_QueryLineList(wLoginUser, wBusinessUnitID,
                        wFactoryID, wWorkShopID, wActive, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Dictionary<Int32, FMCLine>> FMC_QueryLineDic()
        {
            // TODO Auto-generated method stub
            ServiceResult<Dictionary<Int32, FMCLine>> wResult = new ServiceResult<Dictionary<Int32, FMCLine>>(
                    new Dictionary<Int32, FMCLine>());
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                List<FMCLine> wFMCLineList = FMCLineDAO.getInstance().FMC_QueryLineList(BaseDAO.SysAdmin, -1,
                       -1, -1, -1, wErrorCode);
                wResult.Result = wFMCLineList.ToDictionary(p => p.ID, p => p);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        //public ServiceResult<Int32> FMC_AddLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);

        //        FMCLineUnitDAO.getInstance().FMC_AddLineUnit(wLoginUser, wLineUnit, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_CopyLineUnit(BMSEmployee wLoginUser, int wOldLineID, int wOldProductID,
        //        int wOldCustomerID, int wLineID, int wProductID, int wCustomerID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCLineUnitDAO.getInstance().FMC_CopyLineUnit(wLoginUser, wOldLineID, wOldProductID,
        //                wOldCustomerID, wLineID, wProductID, wCustomerID, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_SaveLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCLineUnitDAO.getInstance().FMC_SaveLineUnit(wLoginUser, wLineUnit, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_DeleteLineUnitByID(BMSEmployee wLoginUser, int wID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCLineUnitDAO.getInstance().FMC_DeleteLineUnitByID(wLoginUser, wID, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_ActiveLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCLineUnitDAO.getInstance().FMC_ActiveLineUnit(wLoginUser, wLineUnit,
        //                wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_DisableLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCLineUnitDAO.getInstance().FMC_DisableLineUnit(wLoginUser, wLineUnit,
        //                wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByLineID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wID, boolean wIsList)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wLoginUser,
        //                wProductID, wCustomerID, wLineID, wID, wIsList, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByStationID(BMSEmployee wLoginUser,
        //        int wProductID, int wCustomerID, int wLineID, int wStationID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
        //    wResult.Result = new List<FMCLineUnit>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
        //                wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

        //        for (FMCLineUnit wPartUnit : wLineUnitList)
        //        {
        //            for (FMCLineUnit wStepUnit : wPartUnit.UnitList)
        //            {
        //                if (wStepUnit.UnitList != null && wStepUnit.UnitList.Count > 0
        //                        && wStepUnit.UnitList.stream().anyMatch(p=>p.UnitID == wStationID))
        //                {
        //                    wResult.Result.Add(wPartUnit);
        //                    break;
        //                }
        //            }
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByPartID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wPartID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCLineUnit>> wResult = new ServiceResult<List<FMCLineUnit>>();
        //    wResult.Result = new List<FMCLineUnit>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
        //                wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

        //        for (FMCLineUnit wPartUnit : wLineUnitList)
        //        {

        //            if (wPartUnit.UnitList == null || wPartUnit.UnitList.Count <= 0 || wPartUnit.UnitID != wPartID
        //                    || wPartUnit.Active != 1)
        //                continue;

        //            for (FMCLineUnit wStepUnit : wPartUnit.UnitList)
        //            {

        //                if (wStepUnit.UnitList == null || wStepUnit.UnitList.Count <= 0 || wStepUnit.Active != 1)
        //                    continue;

        //                for (FMCLineUnit wStationUnit : wStepUnit.UnitList)
        //                {
        //                    if (wStepUnit.Active != 1 || wStepUnit.ID <= 0 || wStepUnit.UnitID <= 0)
        //                        continue;

        //                    wResult.Result.Add(wStationUnit);
        //                }
        //            }
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCStation>> FMC_QueryStationListByPartID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wPartID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
        //    wResult.Result = new List<FMCStation>();
        //    try
        //    {

        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        List<FMCLineUnit> wLineUnitList = FMCLineUnitDAO.getInstance().FMC_QueryLineUnitListByLineID(wCompanyID,
        //                wLoginID, wProductID, wCustomerID, wLineID, 0, false, wErrorCode);

        //        List<Int32> wStationIDList = new List<Int32>();
        //        for (FMCLineUnit wPartUnit : wLineUnitList)
        //        {

        //            if (wPartUnit.UnitList == null || wPartUnit.UnitList.Count <= 0 || wPartUnit.UnitID != wPartID
        //                    || wPartUnit.Active != 1)
        //                continue;

        //            for (FMCLineUnit wStepUnit : wPartUnit.UnitList)
        //            {

        //                if (wStepUnit.UnitList == null || wStepUnit.UnitList.Count <= 0 || wStepUnit.Active != 1)
        //                    continue;

        //                for (FMCLineUnit wStationUnit : wStepUnit.UnitList)
        //                {
        //                    if (wStepUnit.Active != 1 || wStepUnit.ID <= 0 || wStepUnit.UnitID <= 0
        //                            || wStationIDList.contains(wStepUnit.UnitID))
        //                        continue;
        //                    wStationIDList.Add(wStationUnit.UnitID);
        //                    wResult.Result.Add(FMCConstants.GetFMCStation(wStationUnit.UnitID));
        //                }
        //            }
        //        }

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_AddResource(BMSEmployee wLoginUser, FMCResource wResource)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCResourceDAO.getInstance().FMC_AddResource(wLoginUser, wResource, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_SaveResource(BMSEmployee wLoginUser, FMCResource wResource)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCResourceDAO.getInstance().FMC_SaveResource(wLoginUser, wResource, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_DisableResource(BMSEmployee wLoginUser, FMCResource wResource)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCResourceDAO.getInstance().FMC_DisableResource(wLoginUser, wResource, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_ActiveResource(BMSEmployee wLoginUser, FMCResource wResource)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCResourceDAO.getInstance().FMC_ActiveResource(wLoginUser, wResource, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<FMCResource> FMC_QueryResourceByID(BMSEmployee wLoginUser, int wID)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<FMCResource> wResult = new ServiceResult<FMCResource>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceByID(wLoginUser, wID, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCResource>> FMC_QueryResourceList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
        //        int wStationID, int wAreaID, int wResourceID, int wType, int wActive)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCResource>> wResult = new ServiceResult<List<FMCResource>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceList(wLoginUser, wWorkShopID, wLineID,
        //                wStationID, wAreaID, wResourceID, wType, wActive, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_AddWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCWorkAreaDAO.getInstance().FMC_AddWorkArea(wLoginUser, wWorkArea, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_SaveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCWorkAreaDAO.getInstance().FMC_SaveWorkArea(wLoginUser, wWorkArea, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_DisableWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCWorkAreaDAO.getInstance().FMC_DisableWorkArea(wLoginUser, wWorkArea, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_ActiveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCWorkAreaDAO.getInstance().FMC_ActiveWorkArea(wLoginUser, wWorkArea, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<FMCWorkArea> FMC_QueryWorkArea(BMSEmployee wLoginUser, int wID, String wCode)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<FMCWorkArea> wResult = new ServiceResult<FMCWorkArea>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkAreaDAO.getInstance().FMC_QueryWorkArea(wLoginUser, wID, wCode, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCWorkArea>> FMC_QueryWorkAreaList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
        //        int wParentID, int wActive)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCWorkArea>> wResult = new ServiceResult<List<FMCWorkArea>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkAreaDAO.getInstance().FMC_QueryWorkAreaList(wLoginUser, "", wWorkShopID, wLineID,
        //                wParentID, wActive, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_AddStation(BMSEmployee wLoginUser, FMCStation wStation)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCStationDAO.getInstance().FMC_AddStation(wLoginUser, wStation, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_SaveStation(BMSEmployee wLoginUser, FMCStation wStation)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCStationDAO.getInstance().FMC_SaveStation(wLoginUser, wStation, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_DisableStation(BMSEmployee wLoginUser, FMCStation wStation)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCStationDAO.getInstance().FMC_DisableStation(wLoginUser, wStation, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_ActiveStation(BMSEmployee wLoginUser, FMCStation wStation)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        FMCStationDAO.getInstance().FMC_ActiveStation(wLoginUser, wStation, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<FMCStation> FMC_QueryStation(BMSEmployee wLoginUser, int wID, String wCode)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<FMCStation> wResult = new ServiceResult<FMCStation>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCStationDAO.getInstance().FMC_QueryStation(wLoginUser, wID, wCode, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
        //        int wWorkAreaID, int wActive)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "", wWorkShopID, wLineID,
        //                wWorkAreaID, wActive, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}




        public ServiceResult<Int32> FMC_AddResource(BMSEmployee wLoginUser, FMCResource wResource)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCResourceDAO.getInstance().FMC_AddResource(wLoginUser, wResource, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveResource(BMSEmployee wLoginUser, FMCResource wResource)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCResourceDAO.getInstance().FMC_SaveResource(wLoginUser, wResource, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DisableResource(BMSEmployee wLoginUser, FMCResource wResource)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCResourceDAO.getInstance().FMC_DisableResource(wLoginUser, wResource, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveResource(BMSEmployee wLoginUser, FMCResource wResource)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCResourceDAO.getInstance().FMC_ActiveResource(wLoginUser, wResource, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DeleteResource(BMSEmployee wLoginUser, FMCResource wResource)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                FMCResourceDAO.getInstance().FMC_DeleteResource(wLoginUser, wResource, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCResource> FMC_QueryResourceByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCResource> wResult = new ServiceResult<FMCResource>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceByID(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCResource>> FMC_QueryResourceList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
                int wStationID, int wAreaID, int wResourceID, int wType, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCResource>> wResult = new ServiceResult<List<FMCResource>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                wResult.Result = FMCResourceDAO.getInstance().FMC_QueryResourceList(wLoginUser, wWorkShopID, wLineID,
                        wStationID, wAreaID, wResourceID, wType, wActive, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> FMC_DisableStation(BMSEmployee wLoginUser, FMCStation wStation)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //FMCStationDAO.getInstance().FMC_DisableStation(wLoginUser, wStation, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveStation(BMSEmployee wLoginUser, FMCStation wStation)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //FMCStationDAO.getInstance().FMC_ActiveStation(wLoginUser, wStation, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<Int32> FMC_DeleteStation(BMSEmployee wLoginUser, FMCStation wStation)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //FMCStationDAO.getInstance().FMC_DeleteStation(wLoginUser, wStation, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<String>> FMC_SyncStationList(BMSEmployee wLoginUser, List<FMCStation> wStationList)
        {

            ServiceResult<List<String>> wResult = new ServiceResult<List<String>>();
            try
            {
                //wResult.Result = new List<string>();
                //if (wStationList == null || wStationList.Count <= 0)
                //    return wResult;

                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //List<FMCStation> wSourveList = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "",
                //        -1, -1, -1, -1, Pagination.MaxSize, wErrorCode);
                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}
                //List<BMSRegion> wRegionSourceList = BMSRegionDAO.getInstance().BMS_SelectRegionList(wLoginUser, -1, wErrorCode);
                //if (wErrorCode.Result != 0)
                //{
                //    wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
                //    return wResult;
                //}

                //List<DMSDeviceLedger> wDeviceLedgerSourveList = DMSDeviceLedgerDAO.getInstance().DMS_SelectDeviceLedgerList(wLoginUser,
                //       "", "", -1, -1, -1, -1, -1, -1, -1, -1, Pagination.Create(1, Int32.MaxValue), wErrorCode);
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


                //Dictionary<string, DMSDeviceLedger> wDeviceLedgerSourveDic = wDeviceLedgerSourveList.ToDictionary(p => p.Code, p => p);


                //Dictionary<string, Int32> wEmployeeIDSourveDic = wEmployeeSourveList.ToDictionary(p => p.LoginID, p => p.ID);

                //Dictionary<string, BMSRegion> wRegionSourveDic = wRegionSourceList.ToDictionary(p => p.Code, p => p);


                //Dictionary<string, FMCStation> wSourveDic = wSourveList.ToDictionary(p => p.Code, p => p);
                //int i = 0;
                //foreach (FMCStation wStation in wStationList)
                //{
                //    i++;
                //    if (wStation == null)
                //    {
                //        wResult.Result.Add(StringUtils.Format("第{0}条数据不完整  !", i));
                //        continue;
                //    }
                //    if (StringUtils.isEmpty(wStation.Code) || StringUtils.isEmpty(wStation.Name))
                //    {
                //        wResult.Result.Add(StringUtils.Format("第{0}条数据不完整 Code:{1} Name:{2} !", i,
                //              wStation.Code, wStation.Name));
                //        continue;
                //    }
                //    if (wRegionSourveDic.ContainsKey(wStation.AreaCode))
                //    {
                //        wStation.AreaID = wRegionSourveDic[wStation.AreaCode].ID;
                //    }
                //    else
                //    {
                //        wResult.Result.Add(StringUtils.Format("Code:{0} AreaCode：{1}  Error:Area Not Found!",
                //               wStation.Code, wStation.AreaCode));
                //        continue;
                //    }


                //    if (wSourveDic.ContainsKey(wStation.Code))
                //    {
                //        wSourveDic[wStation.Code].Name = wStation.Name;
                //        wSourveDic[wStation.Code].Active = wStation.Active;
                //        wSourveDic[wStation.Code].AreaName = wStation.AreaName;
                //        wSourveDic[wStation.Code].AreaCode = wStation.AreaCode;
                //        wSourveDic[wStation.Code].AreaID = wStation.AreaID;
                //        wSourveDic[wStation.Code].ResourceList = wStation.ResourceList;
                //        wSourveDic[wStation.Code].Remark = wStation.Remark;
                //        wSourveDic[wStation.Code].WorkName = wStation.WorkName;
                //        FMCStationDAO.getInstance().FMC_SaveStation(wLoginUser, wSourveDic[wStation.Code], wErrorCode);
                //        if (wErrorCode.Result != 0)
                //        {
                //            wResult.Result.Add(StringUtils.Format("Code:{0} Update Error:{1}",
                //                wStation.Code, MESException.getEnumType(wErrorCode.get()).getLabel()));
                //            continue;

                //        }
                //        wStation.ID = wSourveDic[wStation.Code].ID;
                //    }
                //    else
                //    {
                //        FMCStationDAO.getInstance().FMC_AddStation(wLoginUser, wStation, wErrorCode);

                //        if (wErrorCode.Result != 0)
                //        {
                //            wResult.Result.Add(StringUtils.Format("Code:{0} Add Error:{1}", wStation.Code,
                //                MESException.getEnumType(wErrorCode.get()).getLabel()));
                //            continue;
                //        }
                //        if (wStation.ID > 0)
                //            wSourveDic.Add(wStation.Code, wStation);
                //    }

                //    if (wStation.ID <= 0 || wStation.ResourceList == null || wStation.ResourceList.Count <= 0)
                //    {
                //        //全部禁用
                //        continue;
                //    }


                //    FMCResourceDAO.getInstance().FMC_DeleteResource(wLoginUser, wStation.ID,
                //        (int)FMCResourceType.Device, wErrorCode);
                //    if (wErrorCode.Result != 0)
                //    {
                //        wResult.Result.Add(StringUtils.Format("Code:{0} DeleteResource Error:{1}", wStation.Code,
                //            MESException.getEnumType(wErrorCode.get()).getLabel()));
                //        continue;
                //    }


                //    foreach (FMCResource wResource in wStation.ResourceList)
                //    {
                //        if (wResource.Type != (int)FMCResourceType.Device)
                //            continue;
                //        if (!wDeviceLedgerSourveDic.ContainsKey(wResource.Code))
                //        {
                //            continue;
                //        }
                //        wResource.StationID = wStation.ID;
                //        wResource.ResourceID = wDeviceLedgerSourveDic[wResource.Code].ID;

                //        FMCResourceDAO.getInstance().FMC_AddResource(wLoginUser, wResource, wErrorCode);
                //        if (wErrorCode.Result != 0)
                //        {
                //            wResult.Result.Add(StringUtils.Format("Code:{0} ResourceCode:{1} Add Resource Error:{2}", wStation.Code, wResource.Code,
                //                MESException.getEnumType(wErrorCode.get()).getLabel()));
                //            continue;
                //        }
                //    }

                //}
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;

        }


        public ServiceResult<FMCStation> FMC_QueryStation(BMSEmployee wLoginUser, int wID, String wCode)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCStation> wResult = new ServiceResult<FMCStation>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCStationDAO.getInstance().FMC_QueryStation(wLoginUser, wID, wCode, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
                int wWorkAreaID, int wActive, Pagination wPagination)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCStation>> wResult = new ServiceResult<List<FMCStation>>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCStationDAO.getInstance().FMC_QueryStationList(wLoginUser, "", wWorkShopID, wLineID,
                //        wWorkAreaID, wActive, wPagination, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> FMC_AddWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_AddWorkDay(wLoginUser, wShift, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //wResult.Result = FMCShiftDAO.getInstance().FMC_SaveWorkDay(wLoginUser, wShift, wErrorCode);

                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_DisableWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_DisableWorkDay(wLoginUser, wShift, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_ActiveWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_ActiveWorkDay(wLoginUser, wShift, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }



        public ServiceResult<FMCWorkDay> FMC_QueryWorkDayByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCWorkDay> wResult = new ServiceResult<FMCWorkDay>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);

                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryWorkDayByID(wLoginUser, wID, wErrorCode);
                //if (wResult.Result != null && wResult.Result.ID > 0)
                //{
                //    wResult.Result.ShiftList = FMCShiftDAO.getInstance().FMC_QueryShiftList(wLoginUser,
                //            wResult.Result.ID, -1, wErrorCode);
                //    if (wResult.Result.ShiftList != null && wResult.Result.ShiftList.Count > 0)
                //    {

                //        wResult.Result.ShiftList.Sort((o1, o2) => o1.LevelID - o2.LevelID);
                //    }
                //}
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<FMCWorkDay> FMC_QueryActiveWorkDay(BMSEmployee wLoginUser, int wFactoryID,
                int wWorkShopID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCWorkDay> wResult = new ServiceResult<FMCWorkDay>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryActiveWorkDay(wLoginUser, wFactoryID,
                //        wWorkShopID, wErrorCode);
                //if (wResult.Result != null && wResult.Result.ID > 0)
                //{
                //    wResult.Result.ShiftList = FMCShiftDAO.getInstance().FMC_QueryShiftList(wLoginUser,
                //            wResult.Result.ID, 1, wErrorCode);

                //    if (wResult.Result.ShiftList != null && wResult.Result.ShiftList.Count > 0)
                //    {
                //        wResult.Result.ShiftList.RemoveAll(p => p.Active != 1);

                //        wResult.Result.ShiftList.Sort((o1, o2) => o1.LevelID - o2.LevelID);
                //    }
                //}

                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCWorkDay>> FMC_QueryWorkDayList(BMSEmployee wLoginUser, int wFactoryID,
                int wWorkShopID, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCWorkDay>> wResult = new ServiceResult<List<FMCWorkDay>>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryWorkDayList(wLoginUser, wFactoryID,
                //        wWorkShopID, wActive, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCTimeZone>> FMC_QueryShiftTimeZoneList(BMSEmployee wLoginUser, int wShiftID)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCTimeZone>> wResult = new ServiceResult<List<FMCTimeZone>>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftTimeZoneList(wLoginUser, wShiftID,
                //        wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveShiftTimeZoneList(BMSEmployee wLoginUser,
                List<FMCTimeZone> wTimeZoneList, int wShiftID)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShiftTimeZoneList(wLoginUser, wTimeZoneList,
                //        wShiftID, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<FMCShift>> FMC_QueryShiftList(BMSEmployee wLoginUser, int wWorkDayID, int wActive)
        {
            // TODO Auto-generated method stub
            ServiceResult<List<FMCShift>> wResult = new ServiceResult<List<FMCShift>>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftList(wLoginUser, wWorkDayID, wActive,
                //        wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<Int32> FMC_SaveShiftList(BMSEmployee wLoginUser, List<FMCShift> wShiftList)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShiftList(wLoginUser, wShiftList, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        //public ServiceResult<Int32> FMC_SaveShift(BMSEmployee wLoginUser, FMCShift wShift)
        //{
        //    // TODO Auto-generated method stub
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCShiftDAO.getInstance().FMC_SaveShift(wLoginUser, wShift, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        public ServiceResult<FMCShift> FMC_QueryShiftByID(BMSEmployee wLoginUser, int wWorkDayID)
        {
            // TODO Auto-generated method stub
            ServiceResult<FMCShift> wResult = new ServiceResult<FMCShift>();
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_QueryShiftByID(wLoginUser, wWorkDayID, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> FMC_DeleteShiftByID(BMSEmployee wLoginUser, int wID)
        {
            // TODO Auto-generated method stub
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                //wErrorCode.set(0);
                //wResult.Result = FMCShiftDAO.getInstance().FMC_DeleteShiftByID(wLoginUser, wID, wErrorCode);
                //wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        //public ServiceResult<List<FMCWorkspace>> FMC_GetFMCWorkspaceList(BMSEmployee wLoginUser, int wProductID,
        //        int wPartID, String wPartNo, int wPlaceType, int wActive)
        //{
        //    ServiceResult<List<FMCWorkspace>> wResult = new ServiceResult<List<FMCWorkspace>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspaceList(wLoginUser, wProductID,
        //                wPartID, wPartNo, wPlaceType, wActive, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<FMCWorkspace> FMC_GetFMCWorkspace(BMSEmployee wLoginUser, int wID, String wCode)
        //{
        //    ServiceResult<FMCWorkspace> wResult = new ServiceResult<FMCWorkspace>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspace(wLoginUser, wID, wCode,
        //                wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_SaveFMCWorkspace(BMSEmployee wLoginUser, FMCWorkspace wFMCWorkspace)
        //{
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        if (wFMCWorkspace.ID <= 0)
        //        {
        //            FMCWorkspaceDAO.getInstance().FMC_AddFMCWorkspace(wLoginUser, wFMCWorkspace, wErrorCode);
        //        }
        //        else
        //        {
        //            FMCWorkspaceDAO.getInstance().FMC_EditFMCWorkspace(wLoginUser, wFMCWorkspace, wErrorCode);
        //        }
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_BindFMCWorkspace(BMSEmployee wLoginUser, FMCWorkspace wFMCWorkspace)
        //{
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);

        //        wResult.Result = FMCWorkspaceDAO.getInstance().FMC_BindFMCWorkspace(wLoginUser, wFMCWorkspace,
        //                wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.error("FMCServiceImpl FMC_BindFMCWorkspace Error:", e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<Int32> FMC_ActiveFMCWorkspace(BMSEmployee wLoginUser, int wActive,
        //        FMCWorkspace wFMCWorkspace)
        //{
        //    ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkspaceDAO.getInstance().FMC_ActiveFMCWorkspace(wLoginUser, wActive,
        //                wFMCWorkspace, wErrorCode);
        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}


        //public ServiceResult<List<FMCWorkspaceRecord>> FMC_GetFMCWorkspaceRecordList(BMSEmployee wLoginUser,
        //        int wProductID, int wPartID, String wPartNo, int wPlaceID, int wPlaceType, int wLimit, DateTime wStartTime,
        //        DateTime wEndTime)
        //{
        //    ServiceResult<List<FMCWorkspaceRecord>> wResult = new ServiceResult<List<FMCWorkspaceRecord>>();
        //    try
        //    {
        //        OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
        //        wErrorCode.set(0);
        //        wResult.Result = FMCWorkspaceDAO.getInstance().FMC_GetFMCWorkspaceRecordList(wLoginUser,
        //                wProductID, wPartID, wPartNo, wPlaceID, wPlaceType, wStartTime, wEndTime, wLimit, wErrorCode);

        //        wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLable();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //    }
        //    return wResult;
        //}

        public ServiceResult<Int32> FMC_QueryShiftID(BMSEmployee wLoginUser, int wWorkShopID, DateTime wShiftTime,
                int wShifts, OutResult<Int32> wShiftIndex)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                //wResult.Result = MESServer.MES_QueryShiftID(wWorkShopID, wShiftTime, wShiftIndex);
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public ServiceResult<List<OMSDXFAnalysis>> OMS_QueryDXFAnalysisList(BMSEmployee wBMSEmployee,
            int wID, int wOrderItemID, String wMissionNo, String wSteelNo, String wCasingModel, Pagination wPagination)
        {
            ServiceResult<List<OMSDXFAnalysis>> wResult = new ServiceResult<List<OMSDXFAnalysis>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = OMSDXFAnalysisDAO.Instance.OMS_QueryOMSDXFAnalysisList(wID, wOrderItemID, wMissionNo, wSteelNo, wCasingModel, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<OMSDXFAnalysisParts>> OMS_QueryDXFAnalysisPartsList(BMSEmployee wBMSEmployee, int wID, int wDxfAnalysisID, String wPlanNo, String wPartName, String wPartModel, Pagination wPagination)
        {
            ServiceResult<List<OMSDXFAnalysisParts>> wResult = new ServiceResult<List<OMSDXFAnalysisParts>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = OMSDXFAnalysisPartsDAO.Instance.OMS_QueryOMSDXFAnalysisPartsList(wID, wDxfAnalysisID, wPlanNo, wPartName, wPartModel, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
       
        /// <summary>
        /// by Demin20221117
        /// 统计工单信息
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// <param name="wCutType"></param>
        /// <param name="wStartTime"></param>
        /// <param name="wEndTime"></param>
        /// <param name="wStatus"></param>
        /// <param name="wPagination"></param>
        /// <param name="wOrderType"></param>
        /// <returns></returns>
        public ServiceResult<OMSOrderItemStatistics> OMS_OrderStatistics(BMSEmployee wBMSEmployee, int wCutType, DateTime wStartTime, DateTime wEndTime, int wStatus, int wOrderType,
            Pagination wPagination)
        {
            ServiceResult<OMSOrderItemStatistics> wResult = new ServiceResult<OMSOrderItemStatistics>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<int>();
                wErrorCode.set(0);
                wResult.Result = OMSOrderItemDAO.Instance.OMS_OrderStatistics(wCutType, wStartTime, wEndTime, wStatus, wPagination, wOrderType, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

    }
}
