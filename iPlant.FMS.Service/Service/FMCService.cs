using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public interface FMCService
    {  
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
        ServiceResult<OMSOrderItemStatistics> OMS_OrderStatistics(BMSEmployee wBMSEmployee, int wCutType, DateTime wStartTime, DateTime wEndTime, int wStatus, int wOrderType,
            Pagination wPagination);

        ServiceResult<int> FMC_SaveShift(BMSEmployee wLoginUser, FMCShift wFMCShift);
        ServiceResult<int> FMC_AddShift(BMSEmployee wLoginUser, FMCShift wFMCShift);
        ServiceResult<List<FMCShift>> FMC_QueryShfitList(BMSEmployee wLoginUser, int wID, string wName);
        ServiceResult<int> FMC_DeleteShiftList(BMSEmployee wLoginUser, List<FMCShift> wFMCShiftList);

        ServiceResult<int> FMC_SaveShiftItem(BMSEmployee wLoginUser, FMCShiftItem wFMCShiftItem);
        ServiceResult<int> FMC_AddShiftItem(BMSEmployee wLoginUser, FMCShiftItem wFMCShiftItem);
        ServiceResult<List<FMCShiftItem>> FMC_QueryShfitItemList(BMSEmployee wLoginUser, int wID, int wShiftID, string wName, int wType, int wActive);
        ServiceResult<int> FMC_ActiveShiftItemList(BMSEmployee wLoginUser, int wActive, List<FMCShiftItem> wFMCShiftList);
        ServiceResult<int> FMC_SaveScheduling(BMSEmployee wBMSEmployee, FMCScheduling wFMCScheduling);
        ServiceResult<int> FMC_AddScheduling(BMSEmployee wBMSEmployee, FMCScheduling wFMCScheduling);
        ServiceResult<int> OMS_SaveUploadRecord(BMSEmployee wBMSEmployee, OMSUploadRecord wOMSUploadRecord);
        ServiceResult<int> OMS_SaveSpareParts(BMSEmployee wBMSEmployee, OMSSpareParts wOMSSpareParts);
        ServiceResult<int> OMS_AddUploadRecord(BMSEmployee wBMSEmployee, OMSUploadRecord wOMSUploadRecord);
        ServiceResult<int> OMS_AddSpareParts(BMSEmployee wBMSEmployee, OMSSpareParts wOMSSpareParts);
        ServiceResult<int> MSS_SaveMaterialPoint(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint);
        ServiceResult<int> MCS_SaveInterfaceConfig(BMSEmployee wBMSEmployee, MCSInterfaceConfig wFPCStructuralPart);
        ServiceResult<List<MSSMaterialStock>> MSS_QueryAllMaterialStock(BMSEmployee wBMSEmployee, string wCode, int wActive, int wMaterialPointID, int wPlateID, Pagination wPagination);
        ServiceResult<int> MSS_AddMaterialPoint(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint);
        ServiceResult<int> OMS_SaveOrder(BMSEmployee wBMSEmployee, OMSOrder wFPCStructuralPart);
        ServiceResult<int> MCS_AddInterfaceConfig(BMSEmployee wBMSEmployee, MCSInterfaceConfig wFPCStructuralPart);
        ServiceResult<int> OMS_AddOrder(BMSEmployee wBMSEmployee, OMSOrder wFPCStructuralPart);
        ServiceResult<List<FPCGasVelocity>> FPC_QueryGasVelocityList(BMSEmployee wBMSEmployee, int wID, int wType, double wThickness, string wName, string wDescription);
        ServiceResult<List<MSSCallMaterial>> MSS_QueryCallMaterialList(BMSEmployee wBMSEmployee, int wID, string wName, string wCode, int wActive, int wPlateID, int wMaterialPointID, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);
        ServiceResult<List<MCSLogInfo>> MCS_QueryLogInfoList(BMSEmployee wBMSEmployee, int wID, string wVersionNo, string wFileType, string wSystemType,string wProcessName,string wInfo, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);
        ServiceResult<int> FPC_SaveRoute(BMSEmployee wBMSEmployee, FPCRoute wFPCStructuralPart);
        ServiceResult<int> FPC_AddRoute(BMSEmployee wBMSEmployee, FPCRoute wFPCStructuralPart);
        ServiceResult<int> OMS_DeleteSparePartsList(BMSEmployee wBMSEmployee, List<OMSSpareParts> wOMSSparePartsList);
        ServiceResult<List<OMSUploadRecord>> OMS_QueryUploadRecordList(BMSEmployee wBMSEmployee, int wID, string wCode, string wNCFileName, string wDXFFileName, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);
        ServiceResult<int> FMC_ActiveSchedulingList(BMSEmployee wBMSEmployee, int wActive, List<FMCScheduling> wFMCSchedulingList);
        ServiceResult<int> FPC_SaveStructuralPart(BMSEmployee wBMSEmployee, FPCStructuralPart wFPCStructuralPart);
        ServiceResult<int> FPC_AddStructuralPart(BMSEmployee wBMSEmployee, FPCStructuralPart wFPCStructuralPart);
        ServiceResult<List<FMCScheduling>> FMC_QuerySchedulingList(BMSEmployee wBMSEmployee, int wID, string wSerialNo, int wActive, DateTime wQueryDate);
        ServiceResult<int> FMC_UpdateSchedulingItemList(BMSEmployee wBMSEmployee, List<FMCSchedulingItem> wFMCShiftItemList);
        ServiceResult<int> MSS_DeleteMaterialPointList(BMSEmployee wBMSEmployee, List<MSSMaterialPoint> wMSSMaterialPointList);
        //ServiceResult<int> MSS_ManualEntryCallMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial);
        ServiceResult<int> MCS_TestWriteLog(BMSEmployee wBMSEmployee);
        ServiceResult<MCSLogInfo> MCS_addLogInfo(BMSEmployee wBMSEmployee,string wTextContent,string wFileType,string wSystemType,string wVersionNow, string wProcessName, string wStepNo, string wInfo);
        ServiceResult<List<FMCSchedulingItem>> FMC_QuerySchedulingItemList(BMSEmployee wBMSEmployee, int wID, int wFMCSchedulingID, int wStationID, int wPersonID, DateTime wStartTime, DateTime wEndTime, int wShiftID);
        ServiceResult<int> MCS_DeleteInterfaceConfigList(BMSEmployee wBMSEmployee, List<MCSInterfaceConfig> wFPCStructuralPartList);
        ServiceResult<List<FMCSchedulingItem>> FMC_CreateSchedulingItemTemplate(BMSEmployee wBMSEmployee, DateTime wStartDate, DateTime wEndDate, int wShiftID);
        ServiceResult<int> FPC_ActiveStructuralPartList(BMSEmployee wBMSEmployee, int wActive, List<FPCStructuralPart> wFPCStructuralPartList);
        ServiceResult<List<OMSSpareParts>> OMS_QuerySparePartsList(BMSEmployee wBMSEmployee, int wID, int wActive, int wType, int wOrderID, int wLesOrderID, int wPartType, string wPlanNumber, string wPieceNo, Pagination wPagination);
        ServiceResult<List<FPCStructuralPart>> FPC_QueryStructuralPartList(BMSEmployee wBMSEmployee, int wID, string wName, string wCode, int wActive, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, string wMaterialNo, string wMaterialTypeNo);
        ServiceResult<List<FPCGasVelocity>> FPC_ImportGasVelocity(Stream stream, string fileName, out string wMsg);
        ServiceResult<int> OMS_DeleteOrderList(BMSEmployee wBMSEmployee, List<OMSOrder> wFPCStructuralPartList);
        ServiceResult<int> OMS_DeleteOrderItemList(BMSEmployee wBMSEmployee, List<OMSOrderItem> wOrderItemList);

        ServiceResult<int> FPC_DeleteStructuralPartList(BMSEmployee wBMSEmployee, List<FPCStructuralPart> wFPCStructuralPartList);
        //ServiceResult<int> MSS_ManualCallMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial);
        ServiceResult<int> FPC_DeleteRouteList(BMSEmployee wBMSEmployee, List<FPCRoute> wFPCStructuralPartList);
        ServiceResult<string> MCS_DownloadLog(BMSEmployee wBMSEmployee, int wID);
        ServiceResult<List<FPCRoute>> FPC_QueryRouteList(BMSEmployee wBMSEmployee, int wID, string wRouteName, string wCode, int wActive, int wIsStandard, Pagination wPagination);
        ServiceResult<int> FPC_ActiveRouteList(BMSEmployee wBMSEmployee, int wActive, List<FPCRoute> wFPCStructuralPartList);
        ServiceResult<List<MSSMaterialPoint>> MSS_QueryMaterialPointList(BMSEmployee wBMSEmployee, int wID, int wLineID,int wAssetID,string wName, string wStationPoint,string wDeliveryPoint, string wMaterialNo,int wPlanNo, DateTime wUpdateTime, Pagination wPagination);
        ServiceResult<int> OMS_ActiveSparePartsList(BMSEmployee wBMSEmployee, int wActive, List<OMSSpareParts> wOMSSparePartsList);
        ServiceResult<int> FPC_SaveRoutePart(BMSEmployee wBMSEmployee, FPCRoutePart wFPCStructuralPart);
        ServiceResult<int> FPC_AddRoutePart(BMSEmployee wBMSEmployee, FPCRoutePart wFPCStructuralPart);
        ServiceResult<int> FPC_DeleteRoutePartList(BMSEmployee wBMSEmployee, List<FPCRoutePart> wFPCStructuralPartList);
        ServiceResult<List<MCSInterfaceConfig>> MCS_QueryInterfaceConfigList(BMSEmployee wBMSEmployee, int wID, string wName, int wType, string wEnumFlag, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);
        ServiceResult<string> OMS_TestQRCode(BMSEmployee wBMSEmployee);
        //ServiceResult<int> MSS_ConfirmMaterial(BMSEmployee wBMSEmployee, MSSCallMaterial wMSSCallMaterial);
        ServiceResult<List<FPCRoutePart>> FPC_QueryRoutePartList(BMSEmployee wBMSEmployee, int wID, int wRouteID, string wName, string wCode, int wPartID, Pagination wPagination);
        ServiceResult<List<OMSOrder>> OMS_QueryOrderList(BMSEmployee wBMSEmployee, int wID, string wCuttingNumber, string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime, Pagination wPagination,int wOrderType,int wDisplayed);
        ServiceResult<string> MCS_PreviewLogInfo(BMSEmployee wBMSEmployee, int wID);
        ServiceResult<int> OMS_SaveOrderItem(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart);
        ServiceResult<int> OMS_AddOrderItem(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart);
        ServiceResult<int> INF_AddSortsysSendcasing(BMSEmployee wBMSEmployee, OMSOrderItem wFPCStructuralPart);
        ServiceResult<List<OMSOrderItem>> OMS_QueryOrderItemList(BMSEmployee wBMSEmployee, int wID, int wOrderID, string wOrderNo, string wCuttingNumber, string wNestingNumber, int wCutType, string wPlateMaterialNo, int wStructuralpartID, string wGas, string wCuttingMouth, DateTime wStartTime, DateTime wEndTime, int wStatus, Pagination wPagination, int wOrderType,int wActive,int wDXFAnalysisStatus, int wLesOrderID,int wDisplayed);
        ServiceResult<int> MSS_ActiveMaterialPointList(BMSEmployee wBMSEmployee, int wActive, List<MSSMaterialPoint> wMSSMaterialPointList);

        ServiceResult<List<OMSOrderItem>> OMS_GenerateWorkOrder(BMSEmployee wBMSEmployee, int wOrderID);
        ServiceResult<List<FPCFlowPart>> FPC_QueryFlowDataPart(BMSEmployee wBMSEmployee, int wRouteID);
        ServiceResult<int> OMS_QueryOrderList(BMSEmployee wBMSEmployee, int wID, int wMoveType,List<OMSOrderItem> DataList);
        ServiceResult<int> MCS_DeleteLogInfoList(BMSEmployee wBMSEmployee, List<MCSLogInfo> wFPCStructuralPartList);
        ServiceResult<List<MSSMaterialPointStock>> MSS_QueryPointDetail(BMSEmployee wBMSEmployee, int wMaterialPointID);
        //ServiceResult<List<MSSMaterialPoint>> MSS_QueryAllStockMaterialPointList(BMSEmployee wBMSEmployee, string wName, Pagination wPagination);

        ServiceResult<List<MSSMaterialStock>> MSS_QueryAllMaterialStockActive(BMSEmployee wBMSEmployee, int wBinID);

        ServiceResult<int> FMC_SaveStation(BMSEmployee wBMSEmployee, FMCStation wFMCStation);

        ServiceResult<int> FMC_AddStation(BMSEmployee wBMSEmployee, FMCStation wFMCStation);

        ServiceResult<int> FMC_DeleteStationList(BMSEmployee wBMSEmployee, List<FMCStation> wFMCStationList);

        ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wBMSEmployee, int wID, String wName, String wCode, int wLineID, int wActive, Pagination wPagination);

        ServiceResult<int> FMC_ActiveStationList(BMSEmployee wBMSEmployee, int wActive, List<FMCStation> wFMCStationList);

        ServiceResult<int> FMC_SaveDataDictionary(BMSEmployee wBMSEmployee, FMCDataDictionary wFMCDataDictionary);

        ServiceResult<int> FMC_AddDataDictionary(BMSEmployee wBMSEmployee, FMCDataDictionary wFMCDataDictionary);

        ServiceResult<int> FMC_DeleteDataDictionaryList(BMSEmployee wBMSEmployee, List<FMCDataDictionary> wFMCDataDictionaryList);

        ServiceResult<List<FMCDataDictionary>> FMC_QueryDataDictionaryList(BMSEmployee wBMSEmployee, int wID, String wCode, String wName, int wActive, int wType, Pagination wPagination);

        ServiceResult<int> FMC_ActiveDataDictionaryList(BMSEmployee wBMSEmployee, int wActive, List<FMCDataDictionary> wFMCDataDictionaryList);

        ServiceResult<int> MSS_SaveFeedGroup(BMSEmployee wBMSEmployee, MSSFeedGroup wMSSFeedGroup);

        ServiceResult<int> MSS_AddFeedGroup(BMSEmployee wBMSEmployee, MSSFeedGroup wMSSFeedGroup);

        ServiceResult<int> MSS_DeleteFeedGroupList(BMSEmployee wBMSEmployee, List<MSSFeedGroup> wMSSFeedGroupList);

        ServiceResult<List<MSSFeedGroup>> MSS_QueryFeedGroupList(BMSEmployee wBMSEmployee, int wID, String wCode, String wName, int wActive, Pagination wPagination);

        ServiceResult<int> MSS_ActiveFeedGroupList(BMSEmployee wBMSEmployee, int wActive, List<MSSFeedGroup> wMSSFeedGroupList);

        ServiceResult<int> MCS_SaveOperationLog(BMSEmployee wBMSEmployee, MCSOperationLog wMCSOperationLog);

        ServiceResult<int> MCS_AddOperationLog(BMSEmployee wBMSEmployee, MCSOperationLog wMCSOperationLog);

        ServiceResult<int> MCS_DeleteOperationLogList(BMSEmployee wBMSEmployee, List<MCSOperationLog> wMCSOperationLogList);

        ServiceResult<List<MCSOperationLog>> MCS_QueryOperationLogList(BMSEmployee wBMSEmployee, int wID, int wModuleID, int wType, String wContent, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<int> MCS_ActiveOperationLogList(BMSEmployee wBMSEmployee, int wActive, List<MCSOperationLog> wMCSOperationLogList);
        ServiceResult<List<OMSOrderItem>> OMS_QueryOrderItemListByDeviceNo(BMSEmployee wBMSEmployee, string wDeviceNo);

        ServiceResult<OMSOrderItem> OMS_QueryOrderItemListByCutType(BMSEmployee wBMSEmployee, int wCutType);
        ServiceResult<int> OMS_UpdateOrderItemByCode(BMSEmployee wBMSEmployee, string wOrderNo, int wStatus);
        ServiceResult<int> OMS_UpdateOrderItemByCuttingNumber(BMSEmployee wBMSEmployee, string wCuttingNumber, int wStatus);
        // 车间

        ServiceResult<Int32> FMC_AddFactory(BMSEmployee wLoginUser, FMCFactory wFactory);

        ServiceResult<Int32> FMC_SaveFactory(BMSEmployee wLoginUser, FMCFactory wFactory);

        ServiceResult<Int32> FMC_DisableFactory(BMSEmployee wLoginUser, FMCFactory wFactory);

        ServiceResult<Int32> FMC_ActiveFactory(BMSEmployee wLoginUser, FMCFactory wFactory);
        ServiceResult<Int32> FMC_DeleteFactory(BMSEmployee wLoginUser, FMCFactory wFactory);
        ServiceResult<FMCFactory> FMC_QueryFactoryByID(BMSEmployee wLoginUser, int wID);

        ServiceResult<FMCFactory> FMC_QueryFactoryByCode(BMSEmployee wLoginUser, String wCode);

        ServiceResult<List<FMCFactory>> FMC_QueryFactoryList(BMSEmployee wLoginUser, String wName, int wCountryID,
            int wProvinceID,
                int wCityID, int wActive);


        // 车间

        ServiceResult<Int32> FMC_AddWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop);

        ServiceResult<Int32> FMC_SaveWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop);

        ServiceResult<Int32> FMC_DisableWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop);

        ServiceResult<Int32> FMC_ActiveWorkShop(BMSEmployee wLoginUser, FMCWorkShop wWorkShop);

        ServiceResult<FMCWorkShop> FMC_QueryWorkShopByID(BMSEmployee wLoginUser, int wID);

        ServiceResult<FMCWorkShop> FMC_QueryWorkShopByCode(BMSEmployee wLoginUser, String wCode);

        ServiceResult<List<FMCWorkShop>> FMC_QueryWorkShopList(BMSEmployee wLoginUser, int wFactoryID,
                int wBusinessUnitID, int wActive);

        // 产线

        ServiceResult<Int32> FMC_AddLine(BMSEmployee wLoginUser, FMCLine wLine);

        ServiceResult<Int32> FMC_SaveLine(BMSEmployee wLoginUser, FMCLine wLine);

        ServiceResult<Int32> FMC_DisableLine(BMSEmployee wLoginUser, FMCLine wLine);

        ServiceResult<Int32> FMC_ActiveLine(BMSEmployee wLoginUser, FMCLine wLine);


        ServiceResult<FMCLine> FMC_QueryLineByID(BMSEmployee wLoginUser, int wID);

        ServiceResult<FMCLine> FMC_QueryLineByCode(BMSEmployee wLoginUser, String wCode);

        ServiceResult<List<FMCLine>> FMC_QueryLineList(BMSEmployee wLoginUser, int wBusinessUnitID, int wFactoryID,
                int wWorkShopID, int wActive);

        ServiceResult<Dictionary<Int32, FMCLine>> FMC_QueryLineDic();

        //// 产线工艺配置

        //ServiceResult<Int32> FMC_AddLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit);

        //ServiceResult<Int32> FMC_CopyLineUnit(BMSEmployee wLoginUser, int wOldLineID, int wOldProductID,
        //        int wOldCustomerID, int wLineID, int wProductID, int wCustomerID);

        //ServiceResult<Int32> FMC_SaveLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit);

        //ServiceResult<Int32> FMC_DeleteLineUnitByID(BMSEmployee wLoginUser, int wID);

        //ServiceResult<Int32> FMC_ActiveLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit);

        //ServiceResult<Int32> FMC_DisableLineUnit(BMSEmployee wLoginUser, FMCLineUnit wLineUnit);

        //ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByLineID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wID, boolean wIsList);

        //ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByPartID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wPartID);

        //ServiceResult<List<FMCStation>> FMC_QueryStationListByPartID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wPartID);

        //ServiceResult<List<FMCLineUnit>> FMC_QueryLineUnitListByStationID(BMSEmployee wLoginUser, int wProductID,
        //        int wCustomerID, int wLineID, int wStationID);

        // 制造资源

        ServiceResult<Int32> FMC_AddResource(BMSEmployee wLoginUser, FMCResource wResource);

        ServiceResult<Int32> FMC_SaveResource(BMSEmployee wLoginUser, FMCResource wResource);

        ServiceResult<Int32> FMC_DisableResource(BMSEmployee wLoginUser, FMCResource wResource);

        ServiceResult<Int32> FMC_ActiveResource(BMSEmployee wLoginUser, FMCResource wResource);


        ServiceResult<Int32> FMC_DeleteResource(BMSEmployee wLoginUser, FMCResource wResource);

        ServiceResult<FMCResource> FMC_QueryResourceByID(BMSEmployee wLoginUser, int wID);

        ServiceResult<List<FMCResource>> FMC_QueryResourceList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
                int wStationID, int wAreaID, int wResourceID, int wType, int wActive);

        //// 工区

        //ServiceResult<Int32> FMC_AddWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea);

        //ServiceResult<Int32> FMC_SaveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea);

        //ServiceResult<Int32> FMC_DisableWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea);

        //ServiceResult<Int32> FMC_ActiveWorkArea(BMSEmployee wLoginUser, FMCWorkArea wWorkArea);

        //ServiceResult<FMCWorkArea> FMC_QueryWorkArea(BMSEmployee wLoginUser, int wID, String wCode);

        //ServiceResult<List<FMCWorkArea>> FMC_QueryWorkAreaList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
        //        int wParentID, int wActive);

        // 工位

        ServiceResult<Int32> FMC_DisableStation(BMSEmployee wLoginUser, FMCStation wStation);

        ServiceResult<Int32> FMC_ActiveStation(BMSEmployee wLoginUser, FMCStation wStation);

        ServiceResult<Int32> FMC_DeleteStation(BMSEmployee wLoginUser, FMCStation wStation);
        ServiceResult<FMCStation> FMC_QueryStation(BMSEmployee wLoginUser, int wID, String wCode);

        ServiceResult<List<String>> FMC_SyncStationList(BMSEmployee wLoginUser, List<FMCStation> wStationList);

        ServiceResult<List<FMCStation>> FMC_QueryStationList(BMSEmployee wLoginUser, int wWorkShopID, int wLineID,
                int wWorkAreaID, int wActive, Pagination wPagination);

        // 班次模板管理

        ServiceResult<Int32> FMC_AddWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift);

        ServiceResult<Int32> FMC_SaveWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift);

        ServiceResult<Int32> FMC_DisableWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift);

        ServiceResult<Int32> FMC_ActiveWorkDay(BMSEmployee wLoginUser, FMCWorkDay wShift);



        ServiceResult<FMCWorkDay> FMC_QueryWorkDayByID(BMSEmployee wLoginUser, int wID);

        ServiceResult<FMCWorkDay> FMC_QueryActiveWorkDay(BMSEmployee wLoginUser, int wFactoryID, int wWorkShopID);

        ServiceResult<List<FMCWorkDay>> FMC_QueryWorkDayList(BMSEmployee wLoginUser, int wFactoryID, int wWorkShopID,
                int wActive);

        ServiceResult<List<FMCTimeZone>> FMC_QueryShiftTimeZoneList(BMSEmployee wLoginUser, int wShiftID);

        ServiceResult<Int32> FMC_SaveShiftTimeZoneList(BMSEmployee wLoginUser, List<FMCTimeZone> wTimeZoneList,
                int wShiftID);

        ServiceResult<List<FMCShift>> FMC_QueryShiftList(BMSEmployee wLoginUser, int wWorkDayID, int wActive);

        ServiceResult<Int32> FMC_SaveShiftList(BMSEmployee wLoginUser, List<FMCShift> wShiftList);

        ServiceResult<FMCShift> FMC_QueryShiftByID(BMSEmployee wLoginUser, int wWorkDayID);

        ServiceResult<Int32> FMC_DeleteShiftByID(BMSEmployee wLoginUser, int wID);
        // 工作日历设置

        //ServiceResult<List<FMCWorkspace>> FMC_GetFMCWorkspaceList(BMSEmployee wLoginUser, int wProductID, int wPartID,
        //        String wPartNo, int wPlaceType, int wActive);

        //ServiceResult<FMCWorkspace> FMC_GetFMCWorkspace(BMSEmployee wLoginUser, int wID, String wCode);

        //ServiceResult<Int32> FMC_SaveFMCWorkspace(BMSEmployee wLoginUser, FMCWorkspace wFMCWorkspace);

        //ServiceResult<Int32> FMC_BindFMCWorkspace(BMSEmployee wLoginUser, FMCWorkspace wFMCWorkspace);

        //ServiceResult<Int32> FMC_ActiveFMCWorkspace(BMSEmployee wLoginUser, int wActive,
        //        FMCWorkspace wFMCWorkspace);

        //ServiceResult<List<FMCWorkspaceRecord>> FMC_GetFMCWorkspaceRecordList(BMSEmployee wLoginUser, int wProductID,
        //        int wPartID, String wPartNo, int wPlaceID, int wPlaceType, int wLimit, DateTime wStartTime,
        //        DateTime wEndTime);

        ServiceResult<Int32> FMC_QueryShiftID(BMSEmployee wLoginUser, int wWorkShopID, DateTime wShiftTime,
                 int wShifts, OutResult<Int32> wShiftIndex);

        ServiceResult<int> OMS_ActiveOrderItemLis(BMSEmployee wBMSEmployee,
    int wStatus, List<OMSOrderItem> wOMSOrderItemList);

        ServiceResult<List<OMSDXFAnalysis>> OMS_QueryDXFAnalysisList(BMSEmployee wBMSEmployee, int wID, int wOrderItemID, String wMissionNo, String wSteelNo, String wCasingModel, Pagination wPagination);

        ServiceResult<List<OMSDXFAnalysisParts>> OMS_QueryDXFAnalysisPartsList(BMSEmployee wBMSEmployee, int wID, int wDxfAnalysisID, String wPlanNo, String wPartName, String wPartModel, Pagination wPagination);


    }
}