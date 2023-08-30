using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Data.EF;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace iPlant.FMC.Service
{
    public class MOMInterfaceDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MOMInterfaceDAO));
        private static MOMInterfaceDAO Instance = null;
        private static string MOMSysQueryContainer = "MOMSysQueryContainer";
        private static string MOMSysProcedreManage = "MOMSysProcedreManage";
        private static string MOMSysQuerySeqNo = "MOMSysQuerySeqNo";
        private static string SeqNo = DateTime.Now.Date.ToString("yyyyMMdd") + "000001";

        private MOMInterfaceDAO() : base()
        {

        }

        public static MOMInterfaceDAO getInstance()
        {

            if (Instance == null)
                Instance = new MOMInterfaceDAO();
            return Instance;
        }

        //public void ContainerTakenAwayByMan(BMSEmployee wLoginUser, MSSMaterialFrame wMSSMaterialFrame, out int wErrorCode)
        //{
        //    try
        //    {

        //        if (wMSSMaterialFrame == null)
        //        {
        //            wErrorCode = MESException.Parameter.Value;
        //            return;
        //        }
        //        wErrorCode = 0;
        //        String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
        //        string wSQL = "";
        //        Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
        //        //判断点位编号与当前料框号是否匹配
        //        wSQL = StringUtils.Format("select * from {0}.mss_materialframe where Code='{1}' and FrameCode='{2}'", wInstance, wMSSMaterialFrame.Code, wMSSMaterialFrame.FrameCode);
        //        wSQL = this.DMLChange(wSQL);
        //        MSSMaterialFrame mSSMaterialFrame = mDBPool.queryForList<MSSMaterialFrame>(wSQL, wParamMap).FirstOrDefault();
        //        if (mSSMaterialFrame == null)
        //        {
        //            wErrorCode = MESException.NotFound.Value;
        //            return;
        //        }
        //        List<string> wSQLList = new List<string>();
        //        //更新点位料框号、有无料框
        //        //wSQL = StringUtils.Format("update {0}.mss_materialframe set FrameCode='',IsExistFrame=0,ArrivalTime=null where ID='{1}'", wInstance, mSSMaterialFrame.ID);
        //        //wSQL = this.DMLChange(wSQL);
        //        //wSQLList.Add(wSQL);
        //        //将点位对应零件清空
        //        //wSQL = StringUtils.Format("delete from {0}.mss_materialframe_parts where materialframeID='{1}'", wInstance, mSSMaterialFrame.ID);
        //        //wSQL = this.DMLChange(wSQL);
        //        //wSQLList.Add(wSQL);
        //        wSQL = StringUtils.Format("update {0}.mss_materialpoint set IsExistFrame=0 where ID='{1}'", wInstance, mSSMaterialFrame.ID);
        //        wSQL = this.DMLChange(wSQL);
        //        wSQLList.Add(wSQL);
        //        wSQL = StringUtils.Format("insert into {0}.inf_les_stationstate (palletCode,stationCode,stationStatus,CreateTime) values('{1}','{2}',6,NOW())", wInstance, mSSMaterialFrame.FrameCode, mSSMaterialFrame.Code);
        //        wSQL = this.DMLChange(wSQL);
        //        wSQLList.Add(wSQL);
        //        this.ExecuteSqlTransaction(wSQLList);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
        //        wErrorCode = MESException.DBSQL.Value;
        //    }
        //}

        #region 查询料点料框信息(中控→MOM)
        public void ContainerApply(BMSEmployee wLoginUser, MSSMaterialFrame wMSSMaterialFrame, int wReqID,out int wErrorCode)
        {
            try
            {
                wErrorCode = 0;
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                string wSQL = "";
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                //判断点位编号与当前料框号是否匹配
                wSQL = StringUtils.Format("select * from {0}.mss_materialframe where Code='{1}' ", wInstance, wMSSMaterialFrame.Code);
                wSQL = this.DMLChange(wSQL);
                MSSMaterialFrame mSSMaterialFrame = mDBPool.queryForList<MSSMaterialFrame>(wSQL, wParamMap).FirstOrDefault();
                if (mSSMaterialFrame == null)
                {
                    logger.Error("料点点位不存在！");
                    return;
                }
                #region
                //获取接口地址
                //wSQL = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysQueryContainer);
                //wSQL = this.DMLChange(wSQL);
                //MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL, wParamMap).FirstOrDefault();
                //if (mCSInterfaceConfig == null)
                //{
                //    logger.Error("查询料点料框接口[接口地址未配置！]");
                //    return;
                //}
                ////调用MOM料框查询接口
                //MOMSysQueryContainer postData = new MOMSysQueryContainer();
                //postData.reported.pointNo = mSSMaterialFrame.Code;
                //postData.reported.palletNo = mSSMaterialFrame.FrameCode;

                //ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统查询料框信息", " 中控系统查询料框信息", 1, "中控系统查询料框信息", "[请求报文:" + JsonConvert.SerializeObject(postData) + "]");

                //string postResult = HttpHelper.HttpPost(mCSInterfaceConfig.Uri, JsonConvert.SerializeObject(postData), "application/json");

                //ServiceInstance.mINTERFACEService.WriteLog("中控系统", " 中控系统查询料框信息", " 中控系统查询料框信息", 1, "中控系统查询料框信息", "[应答报文:" + postResult + "]");

                //FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse dtoDtoResultQueryContainer = JsonConvert.DeserializeObject<FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse>(postResult);

                //if (dtoDtoResultQueryContainer.code != "0")
                //{
                //    logger.Error($"中控系统向MOM系统查询料框状态返回异常，服务反馈代码：{ dtoDtoResultQueryContainer.code}" +
                //    $"服务反馈信息：{ dtoDtoResultQueryContainer.msg}");
                //    return;
                //}
                #endregion
                //请求空料框
                if (wReqID==2)
                {
                    if (mSSMaterialFrame.FrameStatus == 2)
                        SendStationStateForfail(wLoginUser, mSSMaterialFrame);
                    if (mSSMaterialFrame.FrameStatus!=2)
                    {
                        logger.Error("料点有料框，不能发送呼叫请求！");
                        return;
                    }
                }
                //移走满料框
                if (wReqID == 3)
                {
                    if (mSSMaterialFrame.FrameStatus == 3)
                        SendStationStateForfail(wLoginUser, mSSMaterialFrame);
                    if (mSSMaterialFrame.FrameStatus != 3)
                    {
                        logger.Error("料点无料框或未完成工件码盘，不能发送呼叫请求");
                        return;
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
                wErrorCode = MESException.DBSQL.Value;
            }
        }
        #endregion

        #region 自动请求工序配送（中控→MOM）
        public void SendStationState(BMSEmployee wBMSEmployee, OutResult<Int32> wErrorCode)
        {
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = "";
                //获取接口地址
                wSQL = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysProcedreManage);
                wSQL = this.DMLChange(wSQL);
                MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL, wParamMap).FirstOrDefault();
                if (mCSInterfaceConfig == null)
                {
                    logger.Error("站点状态反馈[接口地址未配置！]");
                    return;
                }

                //取前5条未发送的数据调用接口发送
                wSQL = StringUtils.Format("select * from {0}.inf_mom_stationstate where Status=0 LIMIT 5", wInstance);
                wSQL = this.DMLChange(wSQL);
                List<INFMOMStationState> dataList = mDBPool.queryForList<INFMOMStationState>(wSQL, wParamMap);
                foreach (INFMOMStationState data in dataList)
                {
                    //无框，请求料框
                    if (data.StationStatus == 2)
                    {
                        data.reqType = "0";
                        INTERFACE_MOMStationState(data, mCSInterfaceConfig.Uri);
                    }
                    if (data.StationStatus == 3)
                    {
                        data.reqType = "1";
                        INTERFACE_MOMStationState(data, mCSInterfaceConfig.Uri);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        //手动配送
        public void SendStationStateForfail(BMSEmployee wBMSEmployee, MSSMaterialFrame wMSSMaterialFrame)
        {
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                string wSQL = "";
                //获取接口地址
                wSQL = StringUtils.Format("SELECT * FROM {0}.mcs_interfaceconfig where EnumFlag='{1}'", wInstance, MOMSysProcedreManage);
                wSQL = this.DMLChange(wSQL);
                MCSInterfaceConfig mCSInterfaceConfig = mDBPool.queryForList<MCSInterfaceConfig>(wSQL, wParamMap).FirstOrDefault();
                if (mCSInterfaceConfig == null)
                {
                    logger.Error("站点状态反馈[接口地址未配置！]");
                    return;
                }

                //取最新未发送的1条数据调用接口发送
                wSQL = StringUtils.Format("select * from {0}.inf_mom_stationstate where PointNo='{1}'  and Status=2 order by ID DESC limit 1", wInstance, wMSSMaterialFrame.Code);
                wSQL = this.DMLChange(wSQL);
                List<INFMOMStationState> dataList = mDBPool.queryForList<INFMOMStationState>(wSQL, wParamMap);
                foreach (INFMOMStationState data in dataList)
                {
                    //无框，请求料框
                    if (data.StationStatus == 2)
                    {
                        data.reqType = "0";
                        INTERFACE_MOMStationState(data, mCSInterfaceConfig.Uri);
                    }
                    //满料框，请求料框
                    if (data.StationStatus == 3)
                    {
                        data.reqType = "1";
                        INTERFACE_MOMStationState(data, mCSInterfaceConfig.Uri);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        public void INTERFACE_MOMStationState(INFMOMStationState data, string url)
        {
            string wSQL = "";
            string wSQL1 = "";
            string wSQL2 = "";
            string wSQL3 = "";
            MSSMaterialPoint wMSSMaterialPoint = new MSSMaterialPoint();
            String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();
            Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
            try
            {
                //调用MOM工序配送
                FMS.Models.MOMSysProcedureManage.MOMSysProcedureManage postData = new FMS.Models.MOMSysProcedureManage.MOMSysProcedureManage();
                postData.reported.reqId = GetReqID();
                postData.reported.reqType = data.reqType;
                postData.reported.palletType = data.PalletType;
                postData.reported.palletNo = data.PalletNo;
                postData.reported.sourceNo = data.PointNo;
                //postData.reported.destNo = "";
                postData.reported.requireTime = DateTime.Now;
                postData.reported.sendTime = DateTime.Now;
                wSQL = StringUtils.Format("select t.* from {0}.inf_frame_material t left join inf_mom_stationstate t1 on t.PalletNo=t1.PalletNo where t.PalletNo='{1}' and  t.Status=0;", wInstance, data.PalletNo);

                List<INFFarmeMetrial> dataList = mDBPool.queryForList<INFFarmeMetrial>(wSQL, null);
                foreach(INFFarmeMetrial wdataList in dataList)
                {
                    postData.reported.materialList.Add(new FMS.Models.MOMSysProcedureManage.materialList
                    {
                        wipOrderNo = wdataList.OrderNo,
                        materialNo = wdataList.MaterialNo,
                        quantity = wdataList.quantity,
                        serialNo = "",
                        sourceSequenceNo = wdataList.SourceSequenceNo,
                        sourceOprSequenceNo = wdataList.SourceOprSequenceNo
                    }
                    );
                }

                ServiceInstance.mINTERFACEService.WriteLog("MOM系统", " 中控系统请求工序配送", " 中控系统请求工序配送", 1, "中控系统请求工序配送", "[请求报文:" + JsonConvert.SerializeObject(postData) + "]");
                string postResult = HttpHelper.HttpPost(url, JsonConvert.SerializeObject(postData), "application/json");
                ServiceInstance.mINTERFACEService.WriteLog("MOM系统", " 中控系统请求工序配送", " 中控系统请求工序配送", 1, "中控系统请求工序配送", "[应答报文:" + postResult + "]");

                FMS.Models.MOMSysProdureManageResponse.MOMSysProdureManageResponse dtoResultProcedureManage = JsonConvert.DeserializeObject<FMS.Models.MOMSysProdureManageResponse.MOMSysProdureManageResponse>(postResult);

                if (dtoResultProcedureManage.code == "0")
                {
                    //发送成功，将状态更新为发送成功、更新发送时间
                    wSQL = StringUtils.Format("UPDATE {0}.inf_mom_stationstate SET STATUS=1,SendTime=NOW(),ErroMsg='' where ID={1}", wInstance, data.ID);
                    wSQL = this.DMLChange(wSQL);
                    mDBPool.update(wSQL, wParamMap);
                    //更新料点呼叫状态 0：请求空料框 1:移走满料框 

                    if (data.reqType == "1")
                    {
                        wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=1 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                        mDBPool.update(wSQL1, wParamMap);
                        wSQL2 = StringUtils.Format("UPDATE {0}.inf_frame_material SET Status=1 where PalletNo='{1}' AND Status=0 AND ID>0", wInstance, data.PalletNo);
                        mDBPool.update(wSQL2, wParamMap);

                        //插入AGV调度记录，类型为移走满料框
                        wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                        " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                        List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                        foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                        {
                            wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                            wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                            wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                            //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                            wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                            wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                            wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                            wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                            wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                            wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                            //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                            wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                            wMSSMaterialPoint.Code = GetAGVNoSerial();

                        }
                        wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',2,'{3}',NOW(),2) ",
                            wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                        mDBPool.update(wSQL3, wParamMap);
                    }
                    if (data.reqType == "0")
                    {
                        wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=2 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                        mDBPool.update(wSQL1, wParamMap);
                        //插入AGV调度记录，类型为请求上料
                         wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                         " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                        List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                        foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                        {
                            wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                            wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                            wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                            //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                            wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                            wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                            wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                            wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                            wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                            wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                            //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                            wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                            wMSSMaterialPoint.Code = GetAGVNoSerial();

                        }
                        wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',1,'{3}',NOW(),2) ", 
                            wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                        mDBPool.update(wSQL3, wParamMap);

                    }

                }
                else
                {
                    //发送失败，将状态更新为发送失败、更新发送时间、失败原因
                    wSQL = StringUtils.Format("UPDATE {0}.inf_mom_stationstate SET STATUS=2,ErroMsg=N'{1}',SendTime=NOW() where ID={2}", wInstance, dtoResultProcedureManage.msg, data.ID);
                    wSQL = this.DMLChange(wSQL);

                    if (data.reqType == "1")
                    {
                        wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=1 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                        mDBPool.update(wSQL1, wParamMap);
                        //插入AGV调度记录，类型为移走满料框
                        wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                        " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                        List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                        foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                        {
                            wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                            wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                            wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                            //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                            wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                            wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                            wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                            wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                            wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                            wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                            //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                            wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                            wMSSMaterialPoint.Code = GetAGVNoSerial();

                        }
                        wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',2,'{3}',NOW(),1) ",
                            wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                        mDBPool.update(wSQL3, wParamMap);
                    }
                    if (data.reqType == "0")
                    {
                        wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=2 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                        mDBPool.update(wSQL1, wParamMap);
                        //插入AGV调度记录，类型为请求上料
                        wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                        " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                        List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                        foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                        {
                            wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                            wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                            wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                            //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                            wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                            wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                            wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                            wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                            wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                            wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                            //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                            wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                            wMSSMaterialPoint.Code = GetAGVNoSerial();

                        }
                        wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',1,'{3}',NOW(),1) ",
                            wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                        mDBPool.update(wSQL3, wParamMap);

                    }
                }
                mDBPool.update(wSQL, wParamMap);
            }
            catch (Exception ex)
            {
                //发送失败，将状态更新为发送失败、更新发送时间、失败原因
                wSQL = StringUtils.Format("UPDATE {0}.inf_mom_stationstate SET STATUS=2,ErroMsg=N'{1}',SendTime=NOW() where ID={2}", wInstance, ex.Message, data.ID);
                wSQL = this.DMLChange(wSQL);
                mDBPool.update(wSQL, wParamMap);

                if (data.reqType == "1")
                {
                    wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=1 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                    mDBPool.update(wSQL1, wParamMap);
                    //插入AGV调度记录，类型为移走满料框
                    wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                    " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                    List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                    foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                    {
                        wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                        wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                        wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                        //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                        wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                        wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                        wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                        wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                        wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                        wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                        //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                        wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                        wMSSMaterialPoint.Code = GetAGVNoSerial();

                    }
                    wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',2,'{3}',NOW(),1) ",
                        wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                    mDBPool.update(wSQL3, wParamMap);
                }
                if (data.reqType == "0")
                {
                    wSQL1 = StringUtils.Format("UPDATE {0}.mss_materialframe SET CallStatus=2 where Code='{1}' AND ID>0", wInstance, data.PointNo);
                    mDBPool.update(wSQL1, wParamMap);
                    //插入AGV调度记录，类型为请求上料
                    wSQL2 = StringUtils.Format("select m.*  from {0}.mss_materialpoint m" +
                    " left join {0}.mss_materialframe m1  on m.ID=m1.ID where m1.FrameCode='{1}' ", wInstance, data.PalletNo);
                    List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL2, null);

                    foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                    {
                        wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                        wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                        wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                        //wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                        wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                        wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                        wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                        wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                        wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                        wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                        //wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                        wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                        wMSSMaterialPoint.Code = GetAGVNoSerial();

                    }
                    wSQL3 = StringUtils.Format("insert into {0}.wms_agvtask (Code,DeviceID,TaskType,SourcePositionID,CreateTime,Status) values('{1}','{2}',1,'{3}',NOW(),1) ",
                        wInstance, wMSSMaterialPoint.Code, wMSSMaterialPoint.AssetID, wMSSMaterialPoint.ID);
                    mDBPool.update(wSQL3, wParamMap);

                }
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion

        //AGV调度序列 按照年月日加四位流水号生成，
        public string GetAGVNoSerial()
        {
            string wResultList = "";

            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("select Code from {0}.wms_agvtask  order by ID desc limit 1;", wInstance);
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);
                string wAgvNo = "";
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    wAgvNo = StringUtils.parseString(wSqlDataReader["Code"]);
                }
                //202208250001
                int wIndex = 1;
                if (string.IsNullOrEmpty(wAgvNo) || wAgvNo.Length != 12)
                {
                    wIndex = 1;
                }
                else
                {
                    int wOldDay = StringUtils.parseInt(wAgvNo.Substring(6, 2));
                    int wCurentDay = DateTime.Now.Day;
                    if (wOldDay != wCurentDay)
                        wIndex = 1;
                    else
                        wIndex = StringUtils.parseInt(wAgvNo.Substring(8)) + 1;
                }
                wResultList = string.Format("{0}{1}{2}{3}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), wIndex.ToString("0000"));
            }
            catch (Exception ex)
            {
                logger.Error("WMS_QuerySaveTotalSize", ex);
            }
            return wResultList;
        }
        //按照年月日加六位流水号生成，
        public string GetReqID()
        {

            try
            {
                int wIndex = 1;
                //202208250000001

                    int wOldDay = StringUtils.parseInt(SeqNo.Substring(6, 2));
                    int wCurentDay = DateTime.Now.Day;
                    if (wOldDay != wCurentDay)
                        wIndex = 1;
                    else
                        wIndex = StringUtils.parseInt(SeqNo.Substring(8)) + 1;

                SeqNo = string.Format("{0}{1}{2}{3}", DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), wIndex.ToString("000000"));

            }
            catch (Exception ex)
            {
                logger.Error("WMS_QuerySaveTotalSize", ex);
            }
            return SeqNo;
        }

    }
}
