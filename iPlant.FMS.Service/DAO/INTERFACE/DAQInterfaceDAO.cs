using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Data.EF;
using System.Net;
using System.IO;
using System.Data;
using System.Web;
using Newtonsoft.Json;

namespace iPlant.FMC.Service
{
    public class DAQInterfaceDAO: BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DAQInterfaceDAO));
        private static DAQInterfaceDAO Instance = null;
        private static string MOMSysQuerySeqNo = "MOMSysQuerySeqNo";
        private static string MOMSysQueryContainer = "MOMSysQueryContainer";

        private DAQInterfaceDAO() : base()
        {

        }

        public static DAQInterfaceDAO getInstance()
        {
            if (Instance == null)
                Instance = new DAQInterfaceDAO();
            return Instance;
        }
        #region 中控系统接收单件工件报工接口(数采→中控)
        //public DAQResponse INTERFACE_DAQReceiveStackFinish(FMS.Models.DAQReceiveStackFinish.DAQReceiveStackFinish data)
        //{
        //    DAQResponse result = new DAQResponse();
        //    result.request_code = data.request_code;
        //    List<string> wSQLList = new List<string>();
        //    try
        //    {
        //        String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
        //        Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
        //        string wSQL = "";
        //        //判断切割编号是否正确，对应工单切割编号
        //        wSQL = StringUtils.Format("select * from {0}.oms_orderitem where CuttingNumber='{1}'", wInstance, data.request_data.mission_no);
        //        wSQL = this.DMLChange(wSQL);
        //        OMSOrderItem oMSOrderItem = mDBPool.queryForList<OMSOrderItem>(wSQL, wParamMap).FirstOrDefault();
        //        if (oMSOrderItem == null)
        //        {
        //            logger.Error("中控系统接收工件分拣报工接口[任务编号:" + data.request_data.mission_no + ",不存在！]");
        //            result.response_result = "1";
        //            result.response_data = "任务编号:" + data.request_data.mission_no + ",不存在！";
        //            return result;
        //        }
        //        wSQL = StringUtils.Format("Insert Into {0}.sfc_sortresult", wInstance)
        //                        + "(OrderItemID,ProductionLine,SortStationNo,CutStationNo,MissionNo,SteelNo,CasingModel,PlanNo," +
        //                        "PartModel,SizeType,SortWay,SortResult,ErrorMsg,StartTime,EndTime,CreateTime) "
        //                        + " Values(@OrderItemID,@ProductionLine,@SortStationNo,@CutStationNo,@MissionNo,@SteelNo,@CasingModel,@PlanNo," +
        //                        "@PartModel,@SizeType,@SortWay,@SortResult,@ErrorMsg,@StartTime,@EndTime,NOW())";
        //        wParamMap.Clear();

        //        wParamMap.Add("OrderItemID", oMSOrderItem.ID);
        //        wParamMap.Add("ProductionLine", data.request_data.production_line);
        //        wParamMap.Add("SortStationNo", data.request_data.sort_station_no);
        //        wParamMap.Add("CutStationNo", data.request_data.cut_station_no);
        //        wParamMap.Add("MissionNo", data.request_data.mission_no);
        //        wParamMap.Add("SteelNo", data.request_data.steel_no);
        //        wParamMap.Add("CasingModel", data.request_data.casing_model);
        //        wParamMap.Add("PlanNo", data.request_data.plan_no);
        //        wParamMap.Add("PartModel", data.request_data.part_model);
        //        wParamMap.Add("SizeType", data.request_data.size_type);
        //        wParamMap.Add("SortWay", data.request_data.sort_way);
        //        wParamMap.Add("SortResult", data.request_data.sort_result);
        //        wParamMap.Add("ErrorMsg", data.request_data.error_msg);
        //        wParamMap.Add("StartTime", data.request_data.start_time);
        //        wParamMap.Add("EndTime", data.request_data.end_time);
        //        wSQL = this.DMLChange(wSQL);
        //        mDBPool.insert(wSQL, wParamMap);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //        result.response_result = "1";
        //        result.response_data = ex.Message;
        //    }
        //    return result;
        //}
        #endregion

        #region 中控系统接收料框状态(数采→中控)
        public DAQResponse INTERFACE_DAQContainerStatus(FMS.Models.DAQContainerStatus.DAQContainerStatus data)
        {
            DAQResponse result = new DAQResponse();
            result.response_ID = data.request_ID;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                MSSMaterialFrame wMSSMaterialFrame = new MSSMaterialFrame();
                MSSMaterialPoint wMSSMaterialPoint = new MSSMaterialPoint();

                string wSQL = "";
                string wSQL1 = "";
                String wSQL2 = "";

                //判断点位编号是否正确
                wSQL = StringUtils.Format("select m.* from {0}.mss_materialframe m where m.ID='{1}' ", wInstance, data.request_data.UnloadPositionNo);
                wMSSMaterialFrame = mDBPool.queryForList<MSSMaterialFrame>(wSQL, null).FirstOrDefault();
                if (wMSSMaterialFrame == null)
                {
                    logger.Error("中控系统接收料点料框状态信息[点位编号:" + data.request_data.UnloadPositionNo + ",不存在！]");
                    result.response_result = "999";
                    result.response_data = "点位编号:" + data.request_data.UnloadPositionNo + ",不存在！";
                    return result;
                }

                //空料框到达，获取料框编号
                if (data.request_data.FrameStatus == 1)
                {
                    //获取接口地址
                    string wSQL3 = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysQueryContainer);
                    wSQL3 = this.DMLChange(wSQL3);
                    MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL3, wParamMap).FirstOrDefault();
                    if (mCSInterfaceConfig == null)
                    {
                        logger.Error("料框查询接口未配置[接口地址未配置！]");
                        result.response_result = "999";
                        result.response_data = "料框查询接口未配置[接口地址未配置！]";
                        return result;
                    }

                    //查询料框信息
                    MOMSysQueryContainer postData = new MOMSysQueryContainer();
                    postData.reported.pointNo = wMSSMaterialFrame.Code;
                    ServiceInstance.mINTERFACEService.WriteLog("MOM系统", " 中控系统查询料框信息", " 中控系统查询料框信息", 1, "中控系统查询料框信息", "[请求报文:" + JsonConvert.SerializeObject(postData) + "]");
                    string postResult = HttpHelper.HttpPost(mCSInterfaceConfig.Uri, JsonConvert.SerializeObject(postData), "application/json");
                    ServiceInstance.mINTERFACEService.WriteLog("MOM系统", " 中控系统查询料框信息", " 中控系统查询料框信息", 1, "中控系统查询料框信息", "[应答报文:" + postResult + "]");
                    FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse dtoResultQueryContainer = JsonConvert.DeserializeObject<FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse>(postResult);
                    foreach (FMS.Models.MOMSysQueryContainerResponse.materialList wMaterialList in dtoResultQueryContainer.returnData.materialList)
                    {
                        //插入中间过程表
                        wSQL3 = StringUtils.Format("insert into {0}.inf_frame_material (PalletNo,PalletType,OrderNo,MaterialNo,quantity,CreateTime,SourceOprSequenceNo,SourceSequenceNo) values('{1}','{2}','{3}','{4}','{5}',NOW(),'{6}','{7}')",
                        wInstance, dtoResultQueryContainer.returnData.palletNo, dtoResultQueryContainer.returnData.palletType, wMaterialList.wiporderId, wMaterialList.materialNo, wMaterialList.quantity, wMaterialList.sourceOprSequenceNo, wMaterialList.sourceSequenceNo);
                        mDBPool.update(wSQL3, null);
                    }
                    wSQL3 = StringUtils.Format("update  {0}.mss_materialframe set FrameCode='{1}',Name='{2}',FrameStatus='{3}',ArrivalTime=Now() where ID={4};", 
                        wInstance, dtoResultQueryContainer.returnData.palletNo, dtoResultQueryContainer.returnData.palletType, data.request_data.FrameStatus, data.request_data.UnloadPositionNo);
                        mDBPool.update(wSQL3, null);

                    //更新AGV调度信息，调度类型为上料
                    string wSQL4 = StringUtils.Format("update  {0}.wms_agvtask  SET Status=3,ArriveTime=NOW()" +
                    " where SourcePositionID='{1}' and TaskType=1 and ID>0 order by ID desc limit 1;", wInstance, data.request_data.UnloadPositionNo);
                    mDBPool.update(wSQL4, null);
                    return result;
                }
                //如果为空料框，满料框已被移走，更新AGV日志信息
                if (data.request_data.FrameStatus == 2)
                {
                    //更新加工追溯表，料框工件状态
                    string wSQL6 = StringUtils.Format("update  {0}.mss_parttrace  SET StepNo=40,UpdateDate=NOW()" +
                    " where UnloadPositionNo='{1}' ;", wInstance, data.request_data.UnloadPositionNo);
                    mDBPool.update(wSQL6, null);
                    //更新AGV调度信息，调度类型为上料
                    string wSQL4 = StringUtils.Format("update  {0}.wms_agvtask  SET Status=3,ArriveTime=NOW()" +
                    " where SourcePositionID='{1}' and TaskType=2 and ID>0 order by ID desc limit 1;", wInstance, data.request_data.UnloadPositionNo);
                    mDBPool.update(wSQL4, null);
                }
                //如果为满料框，需获取订单、物料等信息
                 if (data.request_data.FrameStatus == 3)
                {
                    //判断点位编号是否正确
                    #region
                    wSQL = StringUtils.Format("select m.*,m2.Name as WorkCenter from {0}.mss_materialframe m" +
                    " left join {0}.mss_materialpoint m2  on m.ID=m2.ID where m.ID='{1}' ", wInstance, data.request_data.UnloadPositionNo);
                    List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, null);

                    foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                    {
                        wMSSMaterialFrame.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                        wMSSMaterialFrame.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                        wMSSMaterialFrame.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                        wMSSMaterialFrame.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                        wMSSMaterialFrame.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                        wMSSMaterialFrame.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                        wMSSMaterialFrame.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                        wMSSMaterialFrame.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                        wMSSMaterialFrame.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                        wMSSMaterialFrame.FrameHeight = StringUtils.parseDouble(wSqlDataReader["FrameHeight"]);
                        wMSSMaterialFrame.SteelPlateHeight = StringUtils.parseDouble(wSqlDataReader["SteelPlateHeight"]);
                        wMSSMaterialFrame.SteelPlateNum = StringUtils.parseDouble(wSqlDataReader["SteelPlateNum"]);
                        wMSSMaterialFrame.FrameCode = StringUtils.parseString(wSqlDataReader["FrameCode"]);
                        wMSSMaterialFrame.IsExistFrame = StringUtils.parseBoolean(wSqlDataReader["IsExistFrame"]);
                        wMSSMaterialFrame.ArrivalTime = StringUtils.parseDate(wSqlDataReader["ArrivalTime"]);

                        wMSSMaterialFrame.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                        wMSSMaterialFrame.CallStatus = StringUtils.parseInt(wSqlDataReader["CallStatus"]);
                        wMSSMaterialFrame.WorkCenter = StringUtils.parseString(wSqlDataReader["WorkCenter"]);
                        //wMSSMaterialFrame.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                    }
                    string wSQL6 = "";
                    string wSQL7 = "";
                    wSQL6 = StringUtils.Format("select * from {0}.inf_frame_material " +
                   " where PalletNo='{1}';", wInstance, wMSSMaterialFrame.FrameCode);
                    List<Dictionary<String, Object>> wQueryResultList1 = mDBPool.queryForList(wSQL6, null);
                    foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList1)
                    {
                        wMSSMaterialFrame.OrderNo = StringUtils.parseString(wSqlDataReader["OrderNo"]);
                        wMSSMaterialFrame.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                        wMSSMaterialFrame.PartID = StringUtils.parseString(wSqlDataReader["PartID"]);
                        wMSSMaterialFrame.PartType = StringUtils.parseString(wSqlDataReader["PartType"]);

                        //获取接口地址
                        string wSQL4 = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysQuerySeqNo);
                        wSQL4 = this.DMLChange(wSQL4);
                        MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL4, wParamMap).FirstOrDefault();
                        if (mCSInterfaceConfig == null)
                        {
                            logger.Error("工序查询接口未配置[接口地址未配置！]");
                            result.response_result = "999";
                            result.response_data = "工序查询接口未配置[接口地址未配置！]";
                            return result;
                        }
                        //调用MOM料框查询接口
                        FMS.Models.MOMSysQuerySeqNo.MOMSysQuerySeqNo postData = new FMS.Models.MOMSysQuerySeqNo.MOMSysQuerySeqNo();
                        postData.reported.Wiporder = wMSSMaterialFrame.OrderNo;
                        postData.reported.MaterialNumber = wMSSMaterialFrame.MaterialNo;
                        postData.reported.WorkCenter = wMSSMaterialFrame.WorkCenter;

                        ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统获取工序信息", " 中控系统获取工序信息", 1, "中控系统获取工序信息", "[请求报文:" + JsonConvert.SerializeObject(postData) + "]");

                        string postResult = HttpHelper.HttpPost(mCSInterfaceConfig.Uri, JsonConvert.SerializeObject(postData), "application/json");

                        ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统获取工序信息", " 中控系统获取工序信息", 1, "中控系统获取工序信息", "[应答报文:" + postResult + "]");

                        FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse dtoResultQuerySeqNo = JsonConvert.DeserializeObject<FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse>(postResult);
                        //更新工序
                        wMSSMaterialFrame.SourceOprSequenceNo = dtoResultQuerySeqNo.returnData.OprSequenceNo;
                        wMSSMaterialFrame.SourceSequenceNo = "000000";

                        //插入中间过程表
                        wSQL7 = StringUtils.Format("update {0}.inf_frame_material set SourceOprSequenceNo='{1},SourceSequenceNo='{2}' where PalletNo='{3}' and ID>0;",
                        wInstance, wMSSMaterialFrame.SourceOprSequenceNo, wMSSMaterialFrame.SourceSequenceNo, wMSSMaterialFrame.FrameCode);
                        mDBPool.update(wSQL7, null);
                    }
                    #endregion
                    #region
                    //获取工序编号
                    //获取接口地址
                    //string wSQL4 = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysQuerySeqNo);
                    //wSQL4 = this.DMLChange(wSQL4);
                    //MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL, wParamMap).FirstOrDefault();
                    //if (mCSInterfaceConfig == null)
                    //{
                    //    logger.Error("工序查询接口未配置[接口地址未配置！]");
                    //    result.response_result = "999";
                    //    result.response_data = "工序查询接口未配置[接口地址未配置！]";
                    //    return result;
                    //}
                    //调用MOM料框查询接口
                    // FMS.Models.MOMSysQuerySeqNo.MOMSysQuerySeqNo postData = new FMS.Models.MOMSysQuerySeqNo.MOMSysQuerySeqNo();
                    //postData.reported.Wiporder = wMSSMaterialFrame.OrderID;
                    //postData.reported.MaterialNumber = wMSSMaterialFrame.MaterialNo;
                    // postData.reported.WorkCenter = wMSSMaterialFrame.WorkCenter;

                    //ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统获取工序信息", " 中控系统获取工序信息", 1, "中控系统获取工序信息", "[请求报文:" + JsonConvert.SerializeObject(postData) + "]");

                    // string postResult = HttpHelper.HttpPost(mCSInterfaceConfig.Uri, JsonConvert.SerializeObject(postData), "application/json");

                    //ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统获取工序信息", " 中控系统获取工序信息", 1, "中控系统获取工序信息", "[应答报文:" + postResult + "]");

                    //FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse dtoResultQuerySeqNo = JsonConvert.DeserializeObject<FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse>(postResult);
                    //更新工序
                    //wMSSMaterialFrame.OprSequenceNo = dtoResultQuerySeqNo.returnData.OprSequenceNo;
                    //wMSSMaterialFrame.OprSequenceName = dtoResultQuerySeqNo.returnData.OprSequenceName;
                    #endregion
                }
                    //更新料点料框状态
                    wSQL1 = StringUtils.Format("update  {0}.mss_materialframe set FrameStatus={1}  where ID={2}", wInstance, data.request_data.FrameStatus, data.request_data.UnloadPositionNo);
                wSQL1 = this.DMLChange(wSQL1);
                mDBPool.update(wSQL1, null);

                //写入中间表inf_mom_stationstate
                wSQL2 = StringUtils.Format("insert into {0}.inf_mom_stationstate (PointNo,PalletNo,StationStatus,PalletType,CreateTime) values('{1}','{2}','{3}','{4}',NOW())", 
                    wInstance, wMSSMaterialFrame.Code, wMSSMaterialFrame.FrameCode, data.request_data.FrameStatus, wMSSMaterialFrame.Name);
                mDBPool.update(wSQL2, null);

            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                result.response_result = "999";
                result.response_data = ex.Message;
            }
            return result;
        }
        #endregion


    }
}
