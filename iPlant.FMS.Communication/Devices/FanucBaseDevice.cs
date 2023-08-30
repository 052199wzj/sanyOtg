using System;
using System.Collections.Generic;
using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System.Linq;
using ShrisCommunicationCore;
using System.Threading.Tasks;
using iPlant.FMC.Service;
using Opc.Ua;
using Opc.Ua.Client;
using System.IO;

namespace iPlant.FMS.Communication
{

    public class FanucBasicDevice : BasicDevice
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FanucBasicDevice));
        public FanucBasicDevice(DeviceEntity deviceEntity, CommunicationServerManager wCommunicationServerManager, List<DataSourceEntity> wOPCDataSourceEntities)
            : base(deviceEntity, wCommunicationServerManager, wOPCDataSourceEntities)
        {

        }





        //写timer  状态 报警 参数

        public override void InitalDevice()
        {
            if (mDataSourceEntities == null || mDataSourceEntities.Count() <= 0)
                return;

            //创建 订阅  就分成一个通道，数据收集由LineManager完成


            //状态通道  程序号；状态  模式   
            var subscriptionStatus = mDataSourceEntities.Where(i => i.DataAction == 4);
            //找出订阅数据的服务器列表 

            if (mSimpleServerClientDic != null)
            {
                mCommunicationServerManager.CreateOpcSubscription(subscriptionStatus, DataHandlerStatus, OPCDataHandlerStatus);
            }


            this.MachineStatus();
        }


        public void OPCDataHandlerStatus(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {

            try
            {
                if (monitoredItem == null || e == null)
                    return;
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification != null)
                {
                    int dataId = 0;
                    bool isId = int.TryParse(monitoredItem.DisplayName, out dataId);
                    if (isId && dataId > 0)
                    {
                        var dataSource = mDataSourceEntities.Where(i => i.ID == dataId).First();
                        _ = DataHandlerStatusOPC(dataSource, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }



        protected void DataHandlerStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender == null || e == null)

                    return;

                if ((sender is FanucMonitoredItem) && (e is FanucMonitoredEventArgs))
                {
                    FanucMonitoredItem monitoredItem = (FanucMonitoredItem)sender;
                    FanucMonitoredEventArgs wEventArgs = (FanucMonitoredEventArgs)e;
                    int dataId = 0;
                    bool isId = int.TryParse(monitoredItem.ID, out dataId);
                    if (isId && dataId > 0)
                    {
                        var dataSource = mDataSourceEntities.Where(i => i.ID == dataId).First();
                        _ = DataHandlerStatusFanuc(dataSource, wEventArgs.Notification);
                    }

                }
                else if ((sender is MonitoredItem) && (e is MonitoredItemNotificationEventArgs))
                {


                    MonitoredItem monitoredItem = (MonitoredItem)sender;
                    MonitoredItemNotification notification = ((MonitoredItemNotificationEventArgs)e).NotificationValue as MonitoredItemNotification;
                    if (notification != null)
                    {
                        int dataId = 0;
                        bool isId = int.TryParse(monitoredItem.DisplayName, out dataId);
                        if (isId && dataId > 0)
                        {
                            var dataSource = mDataSourceEntities.Where(i => i.ID == dataId).First();
                            _ = DataHandlerStatusOPC(dataSource, notification);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

        }

        protected async Task DataHandlerStatusOPC(DataSourceEntity dataSource, MonitoredItemNotification notification)
        {
            if (notification == null || notification.Value == null || notification.Value.Value == null)
            {
                return;
            }

            switch (dataSource.DataName)
            {
                //上料请求
                case "MaterialUpLoadRequest":
                    break;
                //订单请求
                case "OrderRequest":
                    break;
                //什么变量有触发效果
                default:
                    break;
            }

            switch (dataSource.DataCatalog)
            {
                case (int)DMSDataClass.Alarm:
                    if (DeviceEntity.AlarmEnable)
                    {
                        await this.DeviceAlarms(dataSource, notification.Value.Value);
                    }
                    break;
                case (int)DMSDataClass.Status:
                    if (DeviceEntity.StatusEnable)
                    {
                        await this.DeviceStatus(dataSource, notification.Value.Value);
                    }
                    break;
                case (int)DMSDataClass.Params:
                case (int)DMSDataClass.PowerParams:
                    if (DeviceEntity.ParmaterEnable)
                    {
                        await this.DeviceParameters(dataSource, notification.Value.Value);
                    }
                    break;
                case (int)DMSDataClass.WorkParams:
                    if (DeviceEntity.WorkParmaterEnable)
                    {
                        if (notification.Value.Value.ParseToInt() == 1)
                        {
                            await this.ProcessData();
                        }
                    }
                    break;
                case (int)DMSDataClass.TechnologyData:
                    if (DeviceEntity.NCEnable)
                    {
                        if (notification.Value.Value.ParseToInt() == 1)
                        {
                            await this.TechnologyChange();
                        }
                    }

                    break;
                case (int)DMSDataClass.PositionData:

                    //await this.PositionData(dataSource, notification.Value.Value);

                    break;
                default:
                    break;
            }


        }



        public override async Task DeviceAlarms(DataSourceEntity dataSource, object wValue)
        {
            if (wValue is Dictionary<String, String>)
            {
                Dictionary<String, String> wValueDic = (Dictionary<String, String>)wValue;

                Dictionary<String, DataSourceEntity> wDataSourceEntityDic = mDataSourceEntities.Where(i => i.DataCatalog == ((int)DMSDataClass.Alarm)).GroupBy(p => p.DataName).ToDictionary(p => p.Key, p => p.First());

                if (mMatchineStatusState)
                {

                    DMSDeviceParameter wDMSDeviceParameter;
                    DataSourceEntity wDataSourceEntity;
                    //判断配置中是否包含报警
                    foreach (var item in wValueDic.Keys)
                    {
                        if (wValueDic.ContainsKey(item))
                        {
                            continue;
                        }

                        wDMSDeviceParameter = DMSDeviceParameter.Create(StringUtils.isEmpty(wValueDic[item]) ? item : wValueDic[item],
                            item, DeviceEntity.ID, dataSource.ServerId, ((int)DMSDataTypes.String), (int)DMSDataClass.Alarm, DMSServerTypes.Fanuc.ToString(), "customer", "", 0);

                        ServiceInstance.mDMSService.DMS_UpdateDeviceParameter(BaseDAO.SysAdmin, wDMSDeviceParameter);
                        if (wDMSDeviceParameter.ID > 0)
                        {
                            wDataSourceEntity = new DataSourceEntity(wDMSDeviceParameter);
                            mDataSourceEntities.Add(wDataSourceEntity);
                            wDataSourceEntityDic.Add(item, wDataSourceEntity);
                        }
                    }
                }

                foreach (var item in wDataSourceEntityDic.Keys)
                {
                    if (wValueDic.ContainsKey(item) && !AlarmDic.ContainsKey(item))
                    {
                        //1 
                        ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, 1, 0);
                        AlarmDic.Add(item, wValueDic[item]);
                    }
                    else if (!wValueDic.ContainsKey(item) && AlarmDic.ContainsKey(item))
                    {
                        //2
                        ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, 2, 0);
                        AlarmDic.Remove(item);
                    }

                }

            }
            else if (StringUtils.parseBoolean(wValue, out bool wResult))
            {
                //判断报警 
                ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, wResult ? 1 : 2, 0);
            }
            else
            {
                await this.DeviceCodeAlarms(dataSource, wValue);

            }


        }


        private Dictionary<String, String> ParamsDic = new Dictionary<string, string>();
        public override Task DeviceParameters(DataSourceEntity dataSource, object wValue)
        {
            if (wValue is Dictionary<String, String>)
            {
                Dictionary<String, String> wValueDic = (Dictionary<String, String>)wValue;

                Dictionary<String, DataSourceEntity> wDataSourceEntityDic = mDataSourceEntities.Where(i => i.DataCatalog == ((int)DMSDataClass.Params)).GroupBy(p => p.DataName).ToDictionary(p => p.Key, p => p.First());


                //根据数据生成配置
                //

                bool wIsSync = false;
                foreach (var item in wDataSourceEntityDic.Keys)
                {
                    wIsSync = false;
                    if (!ParamsDic.ContainsKey(item))
                    {
                        ParamsDic.Add(item, "");
                        wIsSync = true;
                    }
                    if (wValueDic.ContainsKey(item))
                    {
                        if (wValueDic[item] == null)
                        {
                            wValueDic[item] = "";
                        }

                        if (!wValueDic[item].Equals(ParamsDic[item], StringComparison.CurrentCultureIgnoreCase))
                        {
                            ParamsDic[item] = wValueDic[item];
                            wIsSync = true;
                        }

                    }
                    else
                    {
                        ParamsDic[item] = "";
                        wIsSync = true;
                    }
                    if (wIsSync)
                    {

                        ServiceInstance.mDMSService.DMS_SyncDeviceRealParameter(BaseDAO.SysAdmin, DeviceAssetNo, wDataSourceEntityDic[item].Code, ParamsDic[item]);
                        ;
                    }

                }

            }
            else
            {
                ServiceInstance.mDMSService.DMS_SyncDeviceRealParameter(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, StringUtils.parseString(wValue));

            }
            return Task.CompletedTask;

        }



        protected async Task DataHandlerStatusFanuc(DataSourceEntity dataSource, FanucMonitoredItemNotification wValue)
        {
            if (wValue == null)
            {
                return;
            }

            switch (dataSource.DataName)
            {
                //什么变量有触发效果
                default:
                    break;
            }
            switch (dataSource.DataCatalog)
            {
                case (int)DMSDataClass.Alarm:
                    if (DeviceEntity.AlarmEnable)
                    {

                        await this.DeviceAlarms(dataSource, wValue.RealValue);
                    }
                    break;
                case (int)DMSDataClass.Status:
                    if (DeviceEntity.StatusEnable)
                    {

                        await this.DeviceStatus(dataSource, wValue.RealValue);

                    }
                    break;
                case (int)DMSDataClass.Params:
                case (int)DMSDataClass.PowerParams:
                    if (DeviceEntity.ParmaterEnable)
                    {
                        if (wValue.IsGeneral)
                        {

                            await this.DeviceParameters(dataSource, wValue.RealValue);
                        }
                        else
                        {

                        }
                    }
                    break;
                case (int)DMSDataClass.WorkParams:
                    if (DeviceEntity.WorkParmaterEnable)
                    {
                        if (wValue.RealValue.ToString().ParseToInt() == 1)
                        {
                            await this.ProcessData();
                        }
                    }
                    break;
                case (int)DMSDataClass.TechnologyData:
                    if (DeviceEntity.NCEnable)
                    {
                        if (wValue.RealValue.ToString().ParseToInt() == 1)
                        {
                            await this.TechnologyChange();
                        }
                    }

                    break;
                case (int)DMSDataClass.PositionData:

                    //await this.PositionData(dataSource, wValue);

                    break;
                default:
                    break;
            }


        }


        public override Task DeviceStatus(DataSourceEntity dataSource, object wValue)
        {

            int wDeviceStatus = StringUtils.parseInt(wValue);

            if (FirstClose && DeviceStatusBuffer == wDeviceStatus)
                return Task.CompletedTask;

            FirstClose = true;
            DeviceStatusBuffer = wDeviceStatus;

            lock (DeviceEntity)
            {
                ServiceInstance.mDMSService.DMS_SyncDeviceStatus(BaseDAO.SysAdmin, DeviceAssetNo, DeviceStatusBuffer);
            }
            return Task.CompletedTask;
        }



        /// <summary>
        /// 生成过程数据并保存
        /// </summary>
        /// <returns></returns>
        public async Task ProcessData()
        {
            //如何单独获取大批变量的值
            Dictionary<int, List<DataSourceEntity>> wOPCDataSourceListDic = mDataSourceEntities.Where(i =>
            (i.DataCatalog == ((int)DMSDataClass.WorkParams) || i.DataCatalog == ((int)DMSDataClass.QualityParams)) && (i.DataAction == 1 || i.DataAction == 3)).GroupBy(p => p.ServerId).ToDictionary(p => p.Key, p => p.ToList());

            ServerClient wServerClient;

            List<DataValue> wDataValueList;

            Dictionary<String, DMSProcessRecordItem> wDMSProcessRecordItemList = new Dictionary<String, DMSProcessRecordItem>();
            DMSProcessRecordItem wDMSProcessRecordItem;
            foreach (int wServerId in wOPCDataSourceListDic.Keys)
            {
                wServerClient = this.GetServerClient(wServerId);

                if (wServerClient == null)
                    continue;

                if (wServerClient.ServerType == ((int)DMSServerTypes.OPC))
                {
                    wDataValueList = ((SimpleOpcUaClient)wServerClient).ReadNodes(wOPCDataSourceListDic[wServerId].Select(p => p.SourceAddress).ToList());
                }
                else
                {
                    wDataValueList = new List<DataValue>(wOPCDataSourceListDic[wServerId].Count);
                }
                if (wDataValueList == null || wDataValueList.Count <= 0)
                    continue;

                for (int i = 0; i < wOPCDataSourceListDic[wServerId].Count; i++)
                {
                    wDMSProcessRecordItem = new DMSProcessRecordItem();

                    wDMSProcessRecordItem.AssetNo = DeviceAssetNo;
                    wDMSProcessRecordItem.DeviceNo = DeviceEntity.Code;
                    wDMSProcessRecordItem.DeviceID = DeviceEntity.ID;
                    wDMSProcessRecordItem.DataClass = wOPCDataSourceListDic[wServerId][i].DataCatalog;
                    wDMSProcessRecordItem.DataType = wOPCDataSourceListDic[wServerId][i].DataTypeCode;
                    wDMSProcessRecordItem.ParameterDesc = wOPCDataSourceListDic[wServerId][i].Description;
                    wDMSProcessRecordItem.ParameterID = wOPCDataSourceListDic[wServerId][i].ID;
                    wDMSProcessRecordItem.ParameterName = wOPCDataSourceListDic[wServerId][i].Name;
                    wDMSProcessRecordItem.ParameterNo = wOPCDataSourceListDic[wServerId][i].Code;
                    wDMSProcessRecordItem.SampleTime = DateTime.Now;

                    if (wOPCDataSourceListDic[wServerId][i].DataName.Equals("NCNo"))
                    {
                        wDMSProcessRecordItem.ParameterValue = ((SimpleFanucClient)wServerClient).NCProgramNo + "";
                    }
                    else
                    {
                        switch (wServerClient.ServerType)
                        {
                            case ((int)DMSServerTypes.OPC):

                                wDMSProcessRecordItem.ParameterValue = wDataValueList[i].GetStringValue(wDMSProcessRecordItem.DataType);
                                break;
                            case ((int)DMSServerTypes.Fanuc):
                                wDMSProcessRecordItem.ParameterValue = ((SimpleFanucClient)wServerClient).ReadNodeInt(wOPCDataSourceListDic[wServerId][i].SourceAddress)
                                    .ToString();
                                break;
                            default:
                                wDMSProcessRecordItem.ParameterValue = "";
                                break;
                        }
                    }

                    if (wDMSProcessRecordItemList.ContainsKey(wOPCDataSourceListDic[wServerId][i].DataName))
                        wDMSProcessRecordItemList[wOPCDataSourceListDic[wServerId][i].DataName] = wDMSProcessRecordItem;
                    else
                        wDMSProcessRecordItemList.Add(wOPCDataSourceListDic[wServerId][i].DataName, wDMSProcessRecordItem);
                }
            }
            await Task.Run(() =>
            {
                ServiceInstance.mDMSService.DMS_SyncProcessRecord(BaseDAO.SysAdmin, DeviceAssetNo, WorkpieceNo, OrderNo, ProductNo, wDMSProcessRecordItemList);
            });

        }


        /// <summary>
        /// NC程序变更
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public override async Task ProgramNC()
        {

            //var wProgramResult = ServiceInstance.mDMSService.DMS_SelectCurrentProgramNC(BaseDAO.SysAdmin, -1, "", DeviceAssetNo, -1, ProductNo);
            //if (StringUtils.isNotEmpty(wProgramResult.FaultCode))
            //    return;
            //if (wProgramResult.Result == null || wProgramResult.Result.ID <= 0
            //    || StringUtils.isEmpty(wProgramResult.Result.FileSourcePath)
            //    || StringUtils.isEmpty(wProgramResult.Result.ProgramName)
            //    || StringUtils.isEmpty(wProgramResult.Result.DeviceFilePath))
            //    return;

            //String wText = File.ReadAllText(wProgramResult.Result.FileSourcePath);
            //String wProgramName = wProgramResult.Result.ProgramName;
            ////根据ProductNo 获取文件

            ////NC下发
            ////文件内容写入
            ////文件下发路径 //存储路径
            //String wUrl = wProgramResult.Result.DeviceFilePath;


            //本意不下发  主要发送产品型号


            //调用下发

        }

        /// <summary>
        /// Fanuc机床设置刀补信息  不启用
        /// </summary>
        /// <param name="wGroupNum"></param>
        /// <param name="wToolNum"></param>
        /// <param name="offX"></param>
        /// <param name="offZ"></param>
        /// <param name="offR"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public override async Task<bool> ToolOffset(int wGroupNum, int wToolNum, double? offX = null, double? offZ = null, double? offR = null)
        {
            try
            {
                if (!DeviceEntity.ToolEnable)
                    return false;

                DataSourceEntity wCurrentToolNoEntity = mDataSourceEntities.FirstOrDefault(i => i.DataName.Equals("CurrentToolNo", StringComparison.CurrentCultureIgnoreCase));
                if (wCurrentToolNoEntity == null || wCurrentToolNoEntity.ID <= 0)
                    return false;

                SimpleFanucClient wSimpleFanucClient = this.GetServerClient<SimpleFanucClient>(wCurrentToolNoEntity.ServerId);
                if (wSimpleFanucClient == null)
                {
                    return false;
                }
                if (wGroupNum <= 0)
                    wGroupNum = 1;
                if (offX != null)
                {
                    //string toolXAddress = string.Format("ns=2;s=/Tool/Compensation/edgeData[u{0},c{1},12]", wGroupNum, wToolNum);
                    // wSimpleFanucClient.WriteNode(toolXAddress, offX);
                }
                if (offZ != null)
                {
                    //string toolZAddress = string.Format("ns=2;s=/Tool/Compensation/edgeData[u{0},c{1},13]", wGroupNum, wToolNum);
                    // wSimpleFanucClient.WriteNode(toolZAddress, offZ);
                }
                if (offR != null)
                {
                    //string toolRAddress = string.Format("ns=2;s=/Tool/Compensation/edgeData[u{0},c{1},15]", wGroupNum, wToolNum);
                    // wSimpleFanucClient.WriteNode(toolRAddress, offR);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }

        /// <summary>
        /// Fanuc机床获取刀具信息
        /// </summary>
        /// <param name="wGroupNum"></param>
        /// <returns></returns>
        public override List<DMSToolInfoEntity> GetToolInfo(int wGroupNum = 1)
        {
            List<DMSToolInfoEntity> wResult = new List<DMSToolInfoEntity>();
            try
            {
                if (!DeviceEntity.ToolEnable)
                    return wResult;

                DataSourceEntity wCurrentToolNoEntity = mDataSourceEntities.FirstOrDefault(i => i.DataName.Equals("CurrentToolNo", StringComparison.CurrentCultureIgnoreCase));
                if (wCurrentToolNoEntity == null || wCurrentToolNoEntity.ID <= 0)
                    return wResult;

                SimpleOpcUaClient wSimpleOpcUaClient = this.GetServerClient<SimpleOpcUaClient>(wCurrentToolNoEntity.ServerId);
                if (wSimpleOpcUaClient == null)
                {
                    return wResult;
                }
                //让机床厂家给出对应地址 



            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return wResult;
        }
    }
}
