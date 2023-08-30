using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using ShrisCommunicationCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iPlant.FMS.Communication
{

    public abstract class BasicDevice
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BasicDevice));
        protected readonly CommunicationServerManager mCommunicationServerManager;

        protected readonly Dictionary<int, ServerClient> mSimpleServerClientDic = new Dictionary<int, ServerClient>();

        protected readonly List<DataSourceEntity> mDataSourceEntities;

        private LockHelper mLockStatus = new LockHelper();

        public String WorkpieceNo { get; set; } = "";
        public String ProductNo { get; set; } = "";
        public String OrderNo { get; set; } = "";


        protected int DeviceStatusBuffer = 0;

        protected Boolean FirstClose = false;


        protected Boolean mMatchineStatusState = false;

        public virtual void MachineStatus()
        {

            try
            {
                if (mMatchineStatusState)
                    return;

                mMatchineStatusState = true;

                DataSourceEntity wAlarmCodeEntity = mDataSourceEntities.FirstOrDefault(i => i.DataAction == 1 && i.DataCatalog == ((int)DMSDataClass.Alarm) && "MachineAlarmCode".Equals(i.DataName));
                if (wAlarmCodeEntity == null || wAlarmCodeEntity.ID <= 0)
                    return;

                

                //设备数控报警 
            }
            catch (Exception ex)
            {
                logger.Error("MachineAlarm", ex);
            }

        }

        protected Dictionary<String, bool> DeviceAlarmBuffer = new Dictionary<string, bool>();

        protected Dictionary<String, String> DeviceAlarmCodeBuffer = new Dictionary<string, String>();


        protected Dictionary<String, String> DeviceParmasBuffer = new Dictionary<string, String>();

        protected ServerClient GetServerClient(int wServerID)
        {

            if (mSimpleServerClientDic.ContainsKey(wServerID))
                return mSimpleServerClientDic[wServerID];
            else
                return null;
        }


        protected T GetServerClient<T>(int wServerID) where T : ServerClient
        {

            if (mSimpleServerClientDic.ContainsKey(wServerID) && (mSimpleServerClientDic[wServerID] is T))

                return (T)mSimpleServerClientDic[wServerID];
            else
                return null;
        }

        protected List<DataSourceEntity> GetOpcDataSourceEntities(int wDataCatalog)
        {
            return mDataSourceEntities.FindAll(p => p.DataCatalog == wDataCatalog).ToList();
        }


        public BasicDevice(DeviceEntity deviceEntity, CommunicationServerManager wCommunicationServerManager, List<DataSourceEntity> wOPCDataSourceEntities)
        {
            mDataSourceEntities = wOPCDataSourceEntities;
            if (mDataSourceEntities == null)
                mDataSourceEntities = new List<DataSourceEntity>();
            DeviceEntity = deviceEntity ?? throw new ArgumentNullException(nameof(deviceEntity));
            mCommunicationServerManager = wCommunicationServerManager ?? throw new ArgumentNullException(nameof(wCommunicationServerManager));

            var wServerIDs = mDataSourceEntities.Select(p => p.ServerId).Distinct().ToList();
            foreach (var item in wServerIDs)
            {
                mSimpleServerClientDic.Add(item, wCommunicationServerManager.GetServerClient(item));
            }

        }

        public DeviceEntity DeviceEntity { get; private set; }

        public event EventHandler<DeviceValueChangedEventArgs> PropertyChanged;

        protected void OnPropertyChanged(string propertyName, object previewValue, object currentValue)
        {
            PropertyChanged?.Invoke(this, new DeviceValueChangedEventArgs(DeviceEntity, propertyName, previewValue, currentValue));
        }

        public bool SetParameter<T>(string properName, T value)
        {
            bool result = false;
            PropertyInfo property = GetType().GetProperty(properName);
            if (property != null)
            {
                try
                {
                    property.SetValue(this, value, null);
                    result = true;
                }
                catch (Exception ex)
                {
                    logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                }
            }

            return result;
        }

        public object GetParameter(string properName)
        {
            PropertyInfo property = GetType().GetProperty(properName);

            return property.GetValue(this, null);
        }

        public string DeviceCode { get { return DeviceEntity.Code; } }




        public string DeviceAssetNo { get { return DeviceEntity.AssetNo; } }


        protected Dictionary<String, String> AlarmDic = new Dictionary<string, string>();


        public string GetDeviceName()
        {
            return DeviceEntity.Name;
        }

        public int GetDeviceTypeCode()
        {
            return DeviceEntity.DeviceType;
        }
        public abstract void InitalDevice();


        /// <summary>
        /// 设备报警保存
        /// </summary>
        /// <returns></returns>
        public virtual Task DeviceAlarms(DataSourceEntity dataSource, object wValue)
        {


            if ((dataSource.DataTypeCode == ((int)DMSDataTypes.Bool)) && StringUtils.parseBoolean(wValue, out bool wResult))
            {
                if (dataSource.Reversed)
                {
                    wResult = !wResult;
                }

                if (!DeviceAlarmBuffer.ContainsKey(dataSource.Code))
                {
                    DeviceAlarmBuffer[dataSource.Code] = false;
                }
                if (wResult == DeviceAlarmBuffer[dataSource.Code])
                {
                    return Task.CompletedTask; ;
                }
                DeviceAlarmBuffer[dataSource.Code] = wResult;
                //判断报警 
                ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, wResult ? 1 : 2, 0);
            }
            else
            {
                return this.DeviceCodeAlarms(dataSource, wValue);
            }
            return Task.CompletedTask;

        }



        /// <summary>
        /// 设备报警码报警保存
        /// </summary>
        /// <returns></returns>
        public virtual Task DeviceCodeAlarms(DataSourceEntity dataSource, object wValue)
        {
            String wStringValue = StringUtils.parseString(wValue);

            bool wIsSync = false;
            IEnumerable<DataSourceEntity> wOPCDataSourceList;
            if (StringUtils.isNotEmpty(wStringValue) && UInt32.TryParse(wStringValue, out UInt32 wAlarms))
            {


                wOPCDataSourceList = mDataSourceEntities.Where(i => i.DataCatalog == ((int)DMSDataClass.Alarm) && i.DataAction == 1 && i.DescriptionValue.Contains(dataSource.DataName));

                if (wOPCDataSourceList != null && wOPCDataSourceList.Count() > 0)
                {


                    wIsSync = false;
                    foreach (var item in wOPCDataSourceList)
                    {
                        wIsSync = false;
                        if (!DeviceAlarmBuffer.ContainsKey(item.Code))
                        {
                            wIsSync = true;
                            DeviceAlarmBuffer[item.Code] = false;
                        }

                        if (DeviceAlarmBuffer[item.Code] != ((1 << item.DataIndex) & wAlarms) > 0)
                        {
                            DeviceAlarmBuffer[item.Code] = ((1 << item.DataIndex) & wAlarms) > 0;
                            wIsSync = true;

                        }
                        if (wIsSync)
                        {
                            ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item.Code, DeviceAlarmBuffer[item.Code] ? 1 : 2, 0); ;
                        }
                    }

                    return Task.CompletedTask;
                }


            }


            wOPCDataSourceList = mDataSourceEntities.Where(i => i.DataCatalog == ((int)DMSDataClass.Alarm) && i.DataAction == 0);
            if (wOPCDataSourceList == null || wOPCDataSourceList.Count() <= 0)
            {
                lock (DeviceAlarmCodeBuffer)
                {

                    if (!DeviceAlarmCodeBuffer.ContainsKey(dataSource.Code))
                    {
                        DeviceAlarmCodeBuffer[dataSource.Code] = "";
                    }

                    if (wStringValue.Equals(DeviceAlarmCodeBuffer[dataSource.Code]))
                    {
                        return Task.CompletedTask;
                    }
                    else
                    {
                        DeviceAlarmCodeBuffer[dataSource.Code] = wStringValue;
                        ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, StringUtils.isNotEmpty(wStringValue) ? 1 : 2, 1, wStringValue);
                    }
                }
            }
            else
            {

                if (StringUtils.isEmpty(wStringValue))
                {

                    //关闭所有报警

                    foreach (var item in wOPCDataSourceList)
                    {

                        wIsSync = false;
                        if (!DeviceAlarmBuffer.ContainsKey(item.Code))
                        {
                            wIsSync = true;
                            DeviceAlarmBuffer[item.Code] = false;
                        }

                        if (DeviceAlarmBuffer[item.Code])
                        {
                            DeviceAlarmBuffer[item.Code] = false;
                            wIsSync = true;
                            ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item.Code, DeviceAlarmBuffer[item.Code] ? 1 : 2, 0);

                        }

                        if (wIsSync)
                        {
                            ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item.Code, DeviceAlarmBuffer[item.Code] ? 1 : 2, 0);
                        }

                    }
                    ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, 2, 1);


                }
                else
                {


                    foreach (var item in wOPCDataSourceList)
                    {
                        wIsSync = false;
                        if (!DeviceAlarmBuffer.ContainsKey(item.Code))
                        {
                            wIsSync = true;
                            DeviceAlarmBuffer[item.Code] = false;
                        }

                        if (item.DescriptionValue.Contains(wStringValue) != DeviceAlarmBuffer[item.Code])
                        {
                            wIsSync = true;
                            DeviceAlarmBuffer[item.Code] = item.DescriptionValue.Contains(wStringValue);
                        }

                        if (wIsSync)
                        {
                            ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item.Code, DeviceAlarmBuffer[item.Code] ? 1 : 2, 0);
                        }
                    }

                }

            }
            return Task.CompletedTask;

        }



        protected Task DeviceStatusClose()
        {
            if (FirstClose && DeviceStatusBuffer == 0)
                return Task.CompletedTask;
            lock (mLockStatus)
            {
                if (FirstClose && DeviceStatusBuffer == 0)
                    return Task.CompletedTask;

                FirstClose = true;
                DeviceStatusBuffer = 0;

                ServiceInstance.mDMSService.DMS_SyncDeviceStatus(BaseDAO.SysAdmin, DeviceAssetNo, DeviceStatusBuffer);

                //关机关闭所有报警

                foreach (var item in DeviceAlarmBuffer.Keys)
                {
                    if (!DeviceAlarmBuffer[item])
                    {
                        continue;
                    }
                    DeviceAlarmBuffer[item] = false;
                    ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item, DeviceAlarmBuffer[item] ? 1 : 2, 0);

                }
            }

            lock (DeviceAlarmCodeBuffer)
            {
                foreach (var item in DeviceAlarmCodeBuffer.Keys)
                {
                    if (StringUtils.isEmpty(DeviceAlarmCodeBuffer[item]))
                    {
                        continue;
                    }
                    DeviceAlarmCodeBuffer[item] = "";
                    ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item, 2, 1, DeviceAlarmCodeBuffer[item]);

                }
            }

            return Task.CompletedTask;

        }


        /// <summary>
        /// 设备状态保存
        /// </summary>
        /// <returns></returns>
        public virtual Task DeviceStatus(DataSourceEntity dataSource, object wValue)
        {


            if ((dataSource.DataTypeCode == ((int)DMSDataTypes.Bool)) && StringUtils.parseBoolean(wValue, out bool wResult))
            {

                if (dataSource.Reversed)
                {
                    wResult = !wResult;
                }
                lock (mLockStatus)
                {
                    if (wResult)
                    {
                        //已有此状态
                        if ((DeviceStatusBuffer & (1 << dataSource.DataIndex)) > 0)
                            return Task.CompletedTask;

                        DeviceStatusBuffer = DeviceStatusBuffer | (1 << dataSource.DataIndex);
                    }
                    else
                    {
                        //没有此状态
                        if ((DeviceStatusBuffer & (1 << dataSource.DataIndex)) <= 0)
                            return Task.CompletedTask;

                        DeviceStatusBuffer = DeviceStatusBuffer & (~(1 << dataSource.DataIndex));
                    }

                    ServiceInstance.mDMSService.DMS_SyncDeviceStatus(BaseDAO.SysAdmin, DeviceAssetNo, DeviceStatusBuffer);

                    //关机关闭所有报警
                    if ((DeviceStatusBuffer & ((int)StatusEnum.TurnOn)) == 0)
                    {
                        foreach (var item in DeviceAlarmBuffer.Keys)
                        {
                            if (!DeviceAlarmBuffer[item])
                            {
                                continue;
                            }
                            DeviceAlarmBuffer[item] = false;
                            ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item, DeviceAlarmBuffer[item] ? 1 : 2, 0);

                        }
                    }
                }
                lock (DeviceAlarmCodeBuffer)
                {
                    foreach (var item in DeviceAlarmCodeBuffer.Keys)
                    {
                        if (StringUtils.isEmpty(DeviceAlarmCodeBuffer[item]))
                        {
                            continue;
                        }
                        DeviceAlarmCodeBuffer[item] = "";
                        ServiceInstance.mDMSService.DMS_SyncDeviceAlarm(BaseDAO.SysAdmin, DeviceAssetNo, item, 2, 1, DeviceAlarmCodeBuffer[item]);

                    }
                }

            }
            else
            {
                this.DeviceStatusCode(dataSource, wValue);

            }
            return Task.CompletedTask;
        }


        public virtual Task DeviceStatusCode(DataSourceEntity dataSource, object wValue)
        {

            String wStringValue = StringUtils.parseString(wValue);


            int wDeviceStatus = 0;
            if (StringUtils.isNotEmpty(wStringValue) && Int32.TryParse(wStringValue, out int wResult))
            {
                if (wResult != 0)
                {
                    var wOPCDataSourceList = mDataSourceEntities.Where(i => i.DataCatalog == ((int)DMSDataClass.Status) && (i.DataAction == 0 || i.DataAction == 1));

                    if (wOPCDataSourceList != null && wOPCDataSourceList.Count() > 0)
                    {
                        foreach (var item in wOPCDataSourceList)
                        {
                            if (!DMSDeviceStatusEnumTool.getInstance().ValuesDic.ContainsKey(item.DataName))
                                continue;

                            if (item.Reversed)
                            {
                                wDeviceStatus |= (((wResult & 1 << item.DataIndex) == 0) ? DMSDeviceStatusEnumTool.getInstance().ValuesDic[item.DataName] : 0);
                            }
                            else
                            {
                                wDeviceStatus |= (((wResult & 1 << item.DataIndex) > 0) ? DMSDeviceStatusEnumTool.getInstance().ValuesDic[item.DataName] : 0);
                            }


                        }
                    }
                }

            }
            if (DeviceStatusBuffer == wDeviceStatus)
                return Task.CompletedTask;
            DeviceStatusBuffer = wDeviceStatus;

            lock (DeviceEntity)
            {
                ServiceInstance.mDMSService.DMS_SyncDeviceStatus(BaseDAO.SysAdmin, DeviceAssetNo, DeviceStatusBuffer);
            }
            return Task.CompletedTask;
        }


        /// <summary>
        /// 设备参数保存
        /// </summary>
        /// <returns></returns>
        public virtual Task DeviceParameters(DataSourceEntity dataSource, object wValue)
        {
            String wStringValue = StringUtils.parseString(wValue);

            if (!DeviceParmasBuffer.ContainsKey(dataSource.Code))
            {
                DeviceParmasBuffer[dataSource.Code] = "";
            }
            if (wStringValue.Equals(DeviceParmasBuffer[dataSource.Code]))
            {
                return Task.CompletedTask;
            }
            DeviceParmasBuffer[dataSource.Code] = wStringValue;
            ServiceInstance.mDMSService.DMS_SyncDeviceRealParameter(BaseDAO.SysAdmin, DeviceAssetNo, dataSource.Code, wStringValue);

            if (dataSource.DescriptionValue.Contains("TranferToOther"))
            {
                List<String> wDataNmmes = dataSource.DescriptionValue.FindAll(p => StringUtils.isNotEmpty(p) && !p.Equals("TranferToOther", StringComparison.CurrentCultureIgnoreCase)).ToList();
                List<DataSourceEntity> wTranferEntityList = mDataSourceEntities.Where(i => (
                            (i.DataCatalog == ((int)DMSDataClass.Params) || i.DataCatalog == ((int)DMSDataClass.PowerParams)) && (i.DataAction == 2 || i.DataAction == 3)) && wDataNmmes.Contains(i.DataName)).ToList();

                foreach (DataSourceEntity wDataSourceEntity in wTranferEntityList)
                {
                    this.WriteNode(wDataSourceEntity, wStringValue);
                }

            }

            return Task.CompletedTask;
        }


        public virtual List<String> ReadNodes(List<DataSourceEntity> wEntitys, int wServerId)
        {

            List<String> wResult = new List<string>();
            if (wEntitys == null || wEntitys.Count <= 0 || wServerId <= 0)
                return wResult;
            ServerClient wServerClient = this.GetServerClient(wServerId);
            switch (wServerClient.ServerType)
            {
                case ((int)DMSServerTypes.OPC):

                    wResult = ((SimpleOpcUaClient)wServerClient).ReadNodesValue(wEntitys.Select(p => p.SourceAddress).ToList());
                    break;
                case ((int)DMSServerTypes.Fanuc):

                    foreach (var item in wEntitys)
                    {
                        wResult.Add(((SimpleFanucClient)wServerClient).ReadNodeInt(item.SourceAddress).ToString());
                    }

                    break;
                default:

                    break;
            }
            return wResult;
        }

        public String ReadNodeBase(DataSourceEntity wEntity)
        {

            String wResult = "";
            if (wEntity == null || wEntity.ID <= 0)
                return wResult;
            ServerClient wServerClient = this.GetServerClient(wEntity.ServerId);
            switch (wServerClient.ServerType)
            {
                case ((int)DMSServerTypes.OPC):

                    wResult = ((SimpleOpcUaClient)wServerClient).ReadNodeValue(wEntity.SourceAddress);
                    break;
                case ((int)DMSServerTypes.Fanuc):


                    wResult = ((SimpleFanucClient)wServerClient).ReadNodeInt(wEntity.SourceAddress).ToString();

                    break;
                default:

                    break;
            }
            return wResult;
        }


        public void WriteNode(DataSourceEntity wEntity, string wValue)
        {

            if (wEntity == null || wEntity.ID <= 0)
                return;

            ServerClient wServerClient = this.GetServerClient(wEntity.ServerId);


            switch (wServerClient.ServerType)
            {
                case ((int)DMSServerTypes.OPC):

                    ((SimpleOpcUaClient)wServerClient).WriteNode(wEntity.SourceAddress, wValue);
                    break;
                case ((int)DMSServerTypes.Fanuc):

                    ((SimpleFanucClient)wServerClient).WriteNode(wEntity.SourceAddress, wValue);

                    break;
                default:

                    break;
            }


        }

        public void WriteNode<T>(DataSourceEntity wEntity, T wValue)
        {

            if (wEntity == null || wEntity.ID <= 0)
                return;

            ServerClient wServerClient = this.GetServerClient(wEntity.ServerId);


            switch (wServerClient.ServerType)
            {
                case ((int)DMSServerTypes.OPC):

                    ((SimpleOpcUaClient)wServerClient).WriteNode(wEntity.SourceAddress, wValue);
                    break;
                case ((int)DMSServerTypes.Fanuc):

                    ((SimpleFanucClient)wServerClient).WriteNode(wEntity.SourceAddress, wValue);

                    break;
                default:

                    break;
            }


        }


        public virtual Task ProgramNC() { return Task.CompletedTask; }

        public virtual Task TechnologyChange()
        {
            //获取产品型号/工件号

            try
            {

                List<DataSourceEntity> wWorkPartDataSourceListDic = mDataSourceEntities.FindAll(i =>
              (i.DataCatalog == (int)DMSDataClass.WorkParams));


                //设备当前工件号 （产线中获取） 
                DataSourceEntity wWorkpieceDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("WorkpieceNo", StringComparison.CurrentCultureIgnoreCase));

                WorkpieceNo = this.ReadNodeBase(wWorkpieceDataSourceEntity);

                if (StringUtils.isEmpty(WorkpieceNo))
                {
                    if (!LineManager.getInstance(DeviceEntity.LineID).WorkpiecePosition.ContainsKey(DeviceAssetNo))
                        return Task.CompletedTask;

                    List<String> wWorkpieceNoList = LineManager.getInstance(DeviceEntity.LineID).WorkpiecePosition[DeviceAssetNo];

                    if (wWorkpieceNoList == null || wWorkpieceNoList.Count <= 0)
                        return Task.CompletedTask;

                    for (int i = 0; i < wWorkpieceNoList.Count && i < 10; i++)
                    {
                        if (StringUtils.isEmpty(wWorkpieceNoList[i]))
                            continue;
                        WorkpieceNo = wWorkpieceNoList[i];
                        break;
                    }



                    if (StringUtils.isEmpty(WorkpieceNo))
                    {
                        //获取产线订单号

                        return Task.CompletedTask;
                    }
                }

                //设备OrderNo
                DataSourceEntity wOrderDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("OrderNo", StringComparison.CurrentCultureIgnoreCase));
                //设备ProductNo
                DataSourceEntity wProductDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("ProductNo", StringComparison.CurrentCultureIgnoreCase));
                DataSourceEntity wNCDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => (i.DataAction == 2 || i.DataAction == 3) && i.DataName.Equals("NCNo", StringComparison.CurrentCultureIgnoreCase));

                DataSourceEntity wOrderResponseEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => (i.DataAction == 2 || i.DataAction == 3) && i.DataName.Equals("OrderResponse", StringComparison.CurrentCultureIgnoreCase));



                logger.Info("Step1:" + JsonTool.ObjectToJson(wWorkPartDataSourceListDic));

                logger.Info("Step2:" + JsonTool.ObjectToJson(wOrderResponseEntity));


                OrderNo = this.ReadNodeBase(wOrderDataSourceEntity);

                ProductNo = this.ReadNodeBase(wProductDataSourceEntity);


                QMSWorkpiece wQMSWorkpiece = ServiceInstance.mQMSService.QMS_GetWorkpiece(BaseDAO.SysAdmin, -1, WorkpieceNo).Result;
                if (wQMSWorkpiece == null || wQMSWorkpiece.ID <= 0)
                {
                    return Task.CompletedTask;
                }
                if (!wQMSWorkpiece.OrderNo.Equals(OrderNo, StringComparison.CurrentCultureIgnoreCase) && wOrderDataSourceEntity != null)
                {
                    this.WriteNode(wOrderDataSourceEntity, wQMSWorkpiece.OrderNo);

                }
                OrderNo = wQMSWorkpiece.OrderNo;

                if (!wQMSWorkpiece.ProductNo.Equals(ProductNo, StringComparison.CurrentCultureIgnoreCase) && wProductDataSourceEntity != null)
                {
                    this.WriteNode(wProductDataSourceEntity, wQMSWorkpiece.ProductNo);

                }
                ProductNo = wQMSWorkpiece.ProductNo;

                if (wNCDataSourceEntity != null)
                {
                    //获取此设备这个产品的NC号
                    ServiceResult<DMSProgramNC> wDMSProgramNCServiceResult = ServiceInstance.mDMSService.DMS_SelectCurrentProgramNC(BaseDAO.SysAdmin, -1, "", DeviceAssetNo, wQMSWorkpiece.ProductID, wQMSWorkpiece.ProductNo);
                    if (wDMSProgramNCServiceResult != null && wDMSProgramNCServiceResult.Result != null && wDMSProgramNCServiceResult.Result.ID > 0 && StringUtils.isNotEmpty(wDMSProgramNCServiceResult.Result.ProgramName))
                    {
                        this.WriteNode(wNCDataSourceEntity, wDMSProgramNCServiceResult.Result.ProgramName);
                    }
                    else
                    {
                        logger.Error(
                           StringUtils.Format("{0}  AssetNo:{1},ProductID:{2} ProductNo:{3} NC NotFound! Error:{4}",
                           System.Reflection.MethodBase.GetCurrentMethod().Name, DeviceAssetNo, wQMSWorkpiece.ProductID, wQMSWorkpiece.ProductNo, wDMSProgramNCServiceResult.FaultCode));
                    }
                }
                //写入完成写入信号 
                logger.Info("Step3:" + JsonTool.ObjectToJson(wOrderResponseEntity));
                if (wOrderResponseEntity != null)
                {
                    logger.Info("Step4:" + JsonTool.ObjectToJson(wOrderResponseEntity));
                    this.WriteNode(wOrderResponseEntity, true.ToString());
                }


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


                //调用下发
                this.ProgramNC();
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return Task.CompletedTask;
        }


        /// <summary>
        /// 直接写入订单 不管请求信号
        /// </summary>
        /// <param name="wOMSOrder"></param>
        /// <returns></returns>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public Task WriteOrderInfo(OMSOrder wOMSOrder, List<DataSourceEntity> wWorkPartDataSourceListDic = null)
        {
            try
            {

                List<DataSourceEntity> wTechnologyEntityList = mDataSourceEntities.FindAll(i => (i.DataCatalog == (int)DMSDataClass.TechnologyData) && i.DataAction == ((int)DMSDataActions.Monitor));
                if (wTechnologyEntityList != null && wTechnologyEntityList.Count > 0)
                    return Task.CompletedTask;

                if (wWorkPartDataSourceListDic == null || wWorkPartDataSourceListDic.Count <= 0)
                    wWorkPartDataSourceListDic = mDataSourceEntities.FindAll(i =>
                    (i.DataCatalog == (int)DMSDataClass.WorkParams));


                DataSourceEntity wOrderDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("OrderNo", StringComparison.CurrentCultureIgnoreCase));
                DataSourceEntity wProductDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("ProductNo", StringComparison.CurrentCultureIgnoreCase));
                DataSourceEntity wOrderNumberDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("OrderNumber", StringComparison.CurrentCultureIgnoreCase));
                DataSourceEntity wRemainNumberDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("RemainNumber", StringComparison.CurrentCultureIgnoreCase));
                DataSourceEntity wFinishNumberDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("FinishNumber", StringComparison.CurrentCultureIgnoreCase));


                if (wOrderDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wOrderDataSourceEntity, wOMSOrder.OrderNo);
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wOrderDataSourceEntity.DeviceCode, wOrderDataSourceEntity.SourceAddress));
                    }

                }
                if (wProductDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wProductDataSourceEntity, wOMSOrder.ProductNo);
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wProductDataSourceEntity.DeviceCode, wProductDataSourceEntity.SourceAddress));
                    }

                }
                if (wOrderNumberDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wOrderNumberDataSourceEntity, ((int)wOMSOrder.PlanFQTY).ToString());
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wOrderNumberDataSourceEntity.DeviceCode, wOrderNumberDataSourceEntity.SourceAddress));
                    }

                }
                if (wRemainNumberDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wRemainNumberDataSourceEntity, ((int)(wOMSOrder.PlanFQTY - wOMSOrder.DoneFQTY)).ToString());
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wRemainNumberDataSourceEntity.DeviceCode, wRemainNumberDataSourceEntity.SourceAddress));
                    }

                }
                if (wFinishNumberDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wFinishNumberDataSourceEntity, ((int)(wOMSOrder.DoneFQTY)).ToString());
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wFinishNumberDataSourceEntity.DeviceCode, wFinishNumberDataSourceEntity.SourceAddress));
                    }

                }

                DataSourceEntity wOrderResponseEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => i.DataName.Equals("OrderResponse", StringComparison.CurrentCultureIgnoreCase));
                if (wProductDataSourceEntity != null)
                {
                    try
                    {
                        this.WriteNode(wOrderResponseEntity, true);
                    }
                    catch (Exception e)
                    {
                        logger.Error(StringUtils.Format("WriteNodeError:Device:{0} Note{1}", wProductDataSourceEntity.DeviceCode, wProductDataSourceEntity.SourceAddress));
                    }


                }

                if (DeviceEntity.NCEnable)
                {
                    DataSourceEntity wNCDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => (i.DataAction == 2 || i.DataAction == 3) && i.DataName.Equals("NCNo", StringComparison.CurrentCultureIgnoreCase));
                    DataSourceEntity wNC_BDataSourceEntity = wWorkPartDataSourceListDic.FirstOrDefault(i => (i.DataAction == 2 || i.DataAction == 3) && i.DataName.Equals("NCNo_B", StringComparison.CurrentCultureIgnoreCase));
                    if (wNCDataSourceEntity != null)
                    {
                        ServiceResult<DMSProgramNC> wDMSProgramNCServiceResult = ServiceInstance.mDMSService.DMS_SelectCurrentProgramNC(BaseDAO.SysAdmin, -1, "", DeviceAssetNo, wOMSOrder.ProductID, wOMSOrder.ProductNo);
                        if (wDMSProgramNCServiceResult != null && wDMSProgramNCServiceResult.Result != null && wDMSProgramNCServiceResult.Result.ID > 0 && StringUtils.isNotEmpty(wDMSProgramNCServiceResult.Result.ProgramName))
                        {
                            this.WriteNode(wNCDataSourceEntity, wDMSProgramNCServiceResult.Result.ProgramName);


                            if (wNC_BDataSourceEntity != null && StringUtils.isNotEmpty(wDMSProgramNCServiceResult.Result.ProgramName_B))
                            {
                                this.WriteNode(wNC_BDataSourceEntity, wDMSProgramNCServiceResult.Result.ProgramName_B);
                            }
                        }
                        else
                        {
                            logger.Error(
                               StringUtils.Format("{0}  AssetNo:{1},ProductID:{2} ProductNo:{3} NC NotFound! Error:{4}",
                               System.Reflection.MethodBase.GetCurrentMethod().Name, DeviceAssetNo, wOMSOrder.ProductID, wOMSOrder.ProductNo, wDMSProgramNCServiceResult.FaultCode));
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return Task.CompletedTask;

        }


        public abstract Task<bool> ToolOffset(int wGroupNum, int wToolNum, double? offX = null, double? offZ = null, double? offR = null);

        public abstract List<DMSToolInfoEntity> GetToolInfo(int wGroupNum = 1);

    }

    public class DeviceValueChangedEventArgs : EventArgs
    {
        public DeviceEntity DeviceEntity { get; }
        public string PropertyName { get; }
        public object PreviewValue { get; }
        public object CurrentValue { get; }
        public DeviceValueChangedEventArgs(DeviceEntity deviceEntity, string propertyName, object preValue, object currentValue)
        {
            DeviceEntity = deviceEntity;
            PropertyName = propertyName;
            PreviewValue = preValue;
            CurrentValue = currentValue;
        }
    }




}
