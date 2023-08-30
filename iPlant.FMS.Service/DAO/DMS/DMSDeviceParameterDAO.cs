using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    class DMSDeviceParameterDAO : BaseDAO
    {


        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMSDeviceParameterDAO));

        private static DMSDeviceParameterDAO Instance;

        private DMSDeviceParameterDAO() : base()
        {

        }

        public static DMSDeviceParameterDAO getInstance()
        {
            if (Instance == null)
                Instance = new DMSDeviceParameterDAO();
            return Instance;
        }

        private List<DMSDeviceParameter> DMS_SelectDeviceParameterList(BMSEmployee wLoginUser, List<Int32> wID,
               String wCode, String wName, String wVariableName,
              int wLineID, int wDeviceID, String wDeviceNo, String wAssetNo, String wDeviceName, String wProtocol, String wOPCClass, int wDataType, int wDataClass,int wPositionID,
               int wActive, Pagination wPagination, OutResult<Int32> wErrorCode)
        {
            List<DMSDeviceParameter> wResult = new List<DMSDeviceParameter>();
            try
            {

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.DMS.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                if (wID == null)
                    wID = new List<Int32>();
                wID.RemoveAll(p => p <= 0);
                if (wCode == null)
                    wCode = "";

                String wSQL = StringUtils.Format(
                        "SELECT t.*,t1.LineID,t1.Code as  DeviceNo,t1.AssetNo, t1.Name as DeviceName,t2.ServerName,  t5.Name as CreatorName," +
                        " t6.Name as EditorName,t7.Name as PositionName FROM {0}.dms_device_parameter t"
                                + " inner join {0}.dms_device_ledger t1 on t.DeviceID=t1.ID "
                                   + " left join {0}.dms_device_server t2 on t.ServerId=t2.ID "
                                 + " left join {0}.dms_position t7 on t.PositionID=t7.ID "
                                + " left join {0}.mbs_user t5 on t.CreatorID=t5.ID "
                                + " left join {0}.mbs_user t6 on t.EditorID=t6.ID    WHERE  1=1  "
                                + " and ( @wID ='' or t.ID IN( {1} ) )  "
                        + " and ( @wCode ='' or t.Code  = @wCode)  "
                        + " and ( @wName ='' or t.Name   = @wName )  "
                        + " and ( @wDeviceID <= 0 or t.DeviceID  = @wDeviceID)   "
                        + " and ( @wLineID <= 0 or t1.LineID  = @wLineID)   "
                        + " and ( @wPositionID <= 0 or t.PositionID  = @wPositionID) "
                        + " and ( @wDeviceNo ='' or t1.Code  = @wDeviceNo)  "
                        + " and ( @wAssetNo ='' or t1.AssetNo  = @wAssetNo)  "
                        + " and ( @wDataClass <= 0 or t.DataClass  = @wDataClass)   "
                        + " and ( @wDataType <= 0 or t.DataType  = @wDataType)   "
                        + " and ( @wDeviceName ='' or t1.Name   = @wDeviceName )  "
                        + " and ( @wVariableName ='' or t.VariableName like @wVariableName )  "
                        + " and ( @wProtocol ='' or t.Protocol  like @wProtocol )  "
                        + " and ( @wOPCClass ='' or t.OPCClass  like @wOPCClass )  "
                        + " and ( @wActive < 0 or t.Active  = @wActive)   "
                        , wInstance, wID.Count > 0 ? StringUtils.Join(",", wID) : "0");
                wSQL = this.DMLChange(wSQL);
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                wParamMap.Add("wID", StringUtils.Join(",", wID));
                wParamMap.Add("wName", wName);
                wParamMap.Add("wCode", wCode);
                wParamMap.Add("wLineID", wLineID);
                wParamMap.Add("wDeviceID", wDeviceID);
                wParamMap.Add("wPositionID", wPositionID);
                wParamMap.Add("wDeviceNo", wDeviceNo);
                wParamMap.Add("wAssetNo", wAssetNo);
                wParamMap.Add("wDeviceName", wDeviceName);
                wParamMap.Add("wVariableName", StringUtils.isNotEmpty(wVariableName) ? ("%" + wVariableName + "%") : "");
                wParamMap.Add("wProtocol", StringUtils.isNotEmpty(wProtocol) ? ("%" + wProtocol + "%") : "");
                wParamMap.Add("wOPCClass", StringUtils.isNotEmpty(wOPCClass) ? ("%" + wOPCClass + "%") : "");
                wParamMap.Add("wDataType", wDataType);
                wParamMap.Add("wDataClass", wDataClass);
                wParamMap.Add("wActive", wActive);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap,wPagination);



                // wReader\[\"(\w+)\"\]
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {
                    DMSDeviceParameter wDeviceParameter = new DMSDeviceParameter();

                    wDeviceParameter.ID = StringUtils.parseInt(wReader["ID"]);
                    wDeviceParameter.Code = StringUtils.parseString(wReader["Code"]);
                    wDeviceParameter.AssetNo = StringUtils.parseString(wReader["AssetNo"]);
                    wDeviceParameter.Name = StringUtils.parseString(wReader["Name"]);
                    wDeviceParameter.VariableName = StringUtils.parseString(wReader["VariableName"]); 
                    wDeviceParameter.DeviceID = StringUtils.parseInt(wReader["DeviceID"]);
                    wDeviceParameter.DeviceName = StringUtils.parseString(wReader["DeviceName"]);
                    wDeviceParameter.DeviceNo = StringUtils.parseString(wReader["DeviceNo"]);

                    wDeviceParameter.ServerId = StringUtils.parseInt(wReader["ServerId"]);
                    wDeviceParameter.ServerName = StringUtils.parseString(wReader["ServerName"]);
                    wDeviceParameter.PositionID = StringUtils.parseInt(wReader["PositionID"]);
                    wDeviceParameter.PositionName = StringUtils.parseString(wReader["PositionName"]);


                    wDeviceParameter.Protocol = StringUtils.parseString(wReader["Protocol"]);
                    wDeviceParameter.OPCClass = StringUtils.parseString(wReader["OPCClass"]);
                    wDeviceParameter.DataType = StringUtils.parseInt(wReader["DataType"]);

                    wDeviceParameter.DataAction = StringUtils.parseInt(wReader["DataAction"]);
                    wDeviceParameter.InternalTime = StringUtils.parseInt(wReader["InternalTime"]);

                    wDeviceParameter.DataClass = StringUtils.parseInt(wReader["DataClass"]);

                    wDeviceParameter.DataLength = StringUtils.parseInt(wReader["DataLength"]);
                    wDeviceParameter.KeyChar = StringUtils.parseString(wReader["KeyChar"]);
                    wDeviceParameter.AuxiliaryChar = StringUtils.parseString(wReader["AuxiliaryChar"]);
                    wDeviceParameter.ParameterDesc = StringUtils.parseString(wReader["ParameterDesc"]);

                    wDeviceParameter.CreatorID = StringUtils.parseInt(wReader["CreatorID"]);
                    wDeviceParameter.CreatorName = StringUtils.parseString(wReader["CreatorName"]);
                    wDeviceParameter.CreateTime = StringUtils.parseDate(wReader["CreateTime"]);
                    wDeviceParameter.EditorID = StringUtils.parseInt(wReader["EditorID"]);
                    wDeviceParameter.EditorName = StringUtils.parseString(wReader["EditorName"]);
                    wDeviceParameter.EditTime = StringUtils.parseDate(wReader["EditTime"]);
                    wDeviceParameter.Active = StringUtils.parseInt(wReader["Active"]);
                    wDeviceParameter.AnalysisOrder = StringUtils.parseInt(wReader["AnalysisOrder"]);
                    wDeviceParameter.ActOrder = StringUtils.parseInt(wReader["ActOrder"]);
                    wDeviceParameter.Reversed = StringUtils.parseInt(wReader["Reversed"]); 
                    wResult.Add(wDeviceParameter);
                }

            }
            catch (Exception e)
            {

                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public List<DMSDeviceParameter> DMS_SelectDeviceParameterList(BMSEmployee wLoginUser, String wName, String wVariableName, int wLineID,

                int wDeviceID, String wDeviceNo, String wAssetNo, String wDeviceName, String wProtocol, String wOPCClass, int wDataType, int wDataClass, int wPositionID,
               int wActive, Pagination wPagination,
                OutResult<Int32> wErrorCode)
        {
            List<DMSDeviceParameter> wResult = new List<DMSDeviceParameter>();
            try
            {
                wResult = DMS_SelectDeviceParameterList(wLoginUser, null, "", wName, wVariableName, wLineID, wDeviceID, wDeviceNo, wAssetNo, wDeviceName, wProtocol,
                    wOPCClass, wDataType, wDataClass, wPositionID, wActive, wPagination,
                        wErrorCode);
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public DMSDeviceParameter DMS_SelectDeviceParameter(BMSEmployee wLoginUser, int wID, String wCode,
                OutResult<Int32> wErrorCode)
        {
            DMSDeviceParameter wResult = new DMSDeviceParameter();
            try
            {
                List<DMSDeviceParameter> wDMSDeviceParameterList = null;
                if (wID > 0)
                {
                    List<Int32> wIDList = new List<Int32>();
                    wIDList.Add(wID);
                    wDMSDeviceParameterList = this.DMS_SelectDeviceParameterList(wLoginUser, wIDList, "", "", "", -1, -1, "", "", "", "",
                    "", -1, -1, -1,-1, Pagination.Default, wErrorCode);
                }
                else if (StringUtils.isNotEmpty(wCode))
                {
                    wDMSDeviceParameterList = this.DMS_SelectDeviceParameterList(wLoginUser, null, wCode, "", "", -1, -1, "", "", "", "",
                    "", -1, -1, -1, -1, Pagination.Default, wErrorCode);
                }

                else
                {
                    return wResult;
                }

                if (wDMSDeviceParameterList.Count <= 0)
                    return wResult;

                wResult = wDMSDeviceParameterList[0];
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public List<DMSDeviceParameter> DMS_SelectDeviceParameterList(BMSEmployee wLoginUser, int wDeviceID, String wDeviceNo, String wAssetNo, int wDataClass, Pagination wPagination,
             OutResult<Int32> wErrorCode)
        {
            List<DMSDeviceParameter> wResult = new List<DMSDeviceParameter>();
            try
            {
                if (wDeviceID > 0 || StringUtils.isNotEmpty(wDeviceNo) || StringUtils.isNotEmpty(wAssetNo))

                    wResult = this.DMS_SelectDeviceParameterList(wLoginUser, null, "", "", "",-1, wDeviceID, wDeviceNo, wAssetNo, "", "",
                        "", -1, wDataClass, 1, -1, wPagination, wErrorCode);

            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }



        public List<DMSDeviceParameter> DMS_SelectDeviceParameterList(BMSEmployee wLoginUser, List<Int32> wIDList,
                OutResult<Int32> wErrorCode)
        {
            List<DMSDeviceParameter> wResult = new List<DMSDeviceParameter>();
            try
            {
                if (wIDList == null || wIDList.Count < 1)
                    return wResult;

                List<Int32> wSelectList = new List<Int32>();
                for (int i = 0; i < wIDList.Count; i++)
                {
                    wSelectList.Add(wIDList[i]);
                    if (i % 25 == 0)
                    {
                        wResult.AddRange(this.DMS_SelectDeviceParameterList(wLoginUser, wSelectList, "", "", "", -1, -1, "", "", "", "",
                    "", -1, -1, -1, -1,Pagination.MaxSize, wErrorCode));

                        wSelectList.Clear();
                    }
                    if (i == wIDList.Count - 1)
                    {
                        if (wSelectList.Count > 0)
                            wResult.AddRange(this.DMS_SelectDeviceParameterList(wLoginUser, wSelectList, "", "", "", -1, -1, "", "", "", "",
                    "", -1, -1, -1, -1, Pagination.MaxSize, wErrorCode));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        private DMSDeviceParameter DMS_CheckDeviceParameter(BMSEmployee wLoginUser, DMSDeviceParameter wDMSDeviceParameter,
             OutResult<Int32> wErrorCode)
        {
            DMSDeviceParameter wResult = new DMSDeviceParameter();
            try
            {

                if (wDMSDeviceParameter == null || StringUtils.isEmpty(wDMSDeviceParameter.Name) || StringUtils.isEmpty(wDMSDeviceParameter.VariableName)
                        || wDMSDeviceParameter.DeviceID <= 0 || wDMSDeviceParameter.DataType <= 0 || wDMSDeviceParameter.DataClass <= 0)
                {
                    return wResult;
                }

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.DMS.getDBName();
                if (wErrorCode.Result != 0)
                    return wResult;

                String wSQL = StringUtils.Format(
                        "SELECT t1.* FROM {0}.dms_device_parameter t1 WHERE t1.ID != @ID " +
                        " AND (( t1.DeviceID=@DeviceID and t1.VariableName=@VariableName and (t1.Name=@Name or t1.DataClass =@DataClass) ) or t1.Code =@Code)  ;",
                        wInstance);
                wSQL = this.DMLChange(wSQL);
                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                wParamMap.Add("DeviceID", wDMSDeviceParameter.DeviceID);
                wParamMap.Add("Name", wDMSDeviceParameter.Name);
                wParamMap.Add("VariableName", wDMSDeviceParameter.VariableName);
                wParamMap.Add("DataClass", wDMSDeviceParameter.DataClass);
                wParamMap.Add("ID", wDMSDeviceParameter.ID);
                wParamMap.Add("Code", wDMSDeviceParameter.Code);

                List<Dictionary<String, Object>> wQueryResult = mDBPool.queryForList(wSQL, wParamMap);
                // wReader\[\"(\w+)\"\]
                foreach (Dictionary<String, Object> wReader in wQueryResult)
                {

                    wResult.ID = StringUtils.parseInt(wReader["ID"]);
                    wResult.Code = StringUtils.parseString(wReader["Code"]);
                    wResult.Name = StringUtils.parseString(wReader["Name"]);
                    wResult.VariableName = StringUtils.parseString(wReader["VariableName"]);
                    wResult.ParameterDesc = StringUtils.parseString(wReader["ParameterDesc"]);

                    wResult.Active = StringUtils.parseInt(wReader["Active"]);
                }

            }
            catch (Exception e)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }


        public void DMS_UpdateDeviceParameter(BMSEmployee wLoginUser, DMSDeviceParameter wDeviceParameter,
                OutResult<Int32> wErrorCode)
        {
            wErrorCode.set(0);
            try
            {
                if (wDeviceParameter == null || StringUtils.isEmpty(wDeviceParameter.Name) || StringUtils.isEmpty(wDeviceParameter.VariableName)
                      || wDeviceParameter.DeviceID <= 0 || wDeviceParameter.DataType <= 0 || wDeviceParameter.DataClass <= 0)
                {
                    wErrorCode.set(MESException.Parameter.Value);
                    return;
                }
                if (wDeviceParameter.ID > 0 && StringUtils.isEmpty(wDeviceParameter.Code))
                {
                    wErrorCode.set(MESException.Parameter.Value);
                    return;
                }

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.DMS.getDBName();

                DMSDeviceParameter wDMSDeviceParameterDB = this.DMS_CheckDeviceParameter(wLoginUser, wDeviceParameter, wErrorCode);
                if (wDMSDeviceParameterDB.ID > 0)
                {

                    if (wDeviceParameter.ID <= 0)
                    {
                        wDeviceParameter.ID = wDMSDeviceParameterDB.ID;
                    }
                    else {
                        wErrorCode.Result = MESException.Duplication.Value;
                        return;
                    }  
                }
                //生成唯一编码
                if (StringUtils.isEmpty(wDeviceParameter.Code))
                {
                    wDeviceParameter.Code = this.CreateMaxCode(StringUtils.Format("{0}.dms_device_parameter", wInstance), StringUtils.Format("{0}{1}", wDeviceParameter.AssetNo, wDeviceParameter.DataClass), "Code",4); 
                }

                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();

                wParamMap.Add("Code", wDeviceParameter.Code);
                wParamMap.Add("Name", wDeviceParameter.Name);
                wParamMap.Add("VariableName", wDeviceParameter.VariableName);
                wParamMap.Add("DeviceID", wDeviceParameter.DeviceID);
                wParamMap.Add("ServerId", wDeviceParameter.ServerId);
                wParamMap.Add("Protocol", wDeviceParameter.Protocol);
                wParamMap.Add("OPCClass", wDeviceParameter.OPCClass);
                wParamMap.Add("DataType", wDeviceParameter.DataType);
                wParamMap.Add("DataAction", wDeviceParameter.DataAction);
                wParamMap.Add("DataClass", wDeviceParameter.DataClass);
                wParamMap.Add("DataLength", wDeviceParameter.DataLength);
                wParamMap.Add("InternalTime", wDeviceParameter.InternalTime);
                wParamMap.Add("KeyChar", wDeviceParameter.KeyChar);
                wParamMap.Add("AuxiliaryChar", wDeviceParameter.AuxiliaryChar);
                wParamMap.Add("ParameterDesc", wDeviceParameter.ParameterDesc);
                wParamMap.Add("EditorID", wLoginUser.ID);
                wParamMap.Add("EditTime", DateTime.Now);
                wParamMap.Add("Active", wDeviceParameter.Active);
                wParamMap.Add("AnalysisOrder", wDeviceParameter.AnalysisOrder);
                wParamMap.Add("ActOrder", wDeviceParameter.ActOrder);
                wParamMap.Add("Reversed", wDeviceParameter.Reversed);
                wParamMap.Add("PositionID", wDeviceParameter.PositionID);

                if (wDeviceParameter.ID <= 0)
                {
                    wParamMap.Add("CreatorID", wLoginUser.ID);
                    wParamMap.Add("CreateTime", DateTime.Now);
                    wDeviceParameter.ID = this.Insert(StringUtils.Format("{0}.dms_device_parameter", wInstance), wParamMap);

                }
                else
                {
                    wParamMap.Add("ID", wDeviceParameter.ID);
                    this.Update(StringUtils.Format("{0}.dms_device_parameter", wInstance), "ID", wParamMap);
                }

            }
            catch (Exception e)
            {
                wErrorCode.Result = MESException.DBSQL.Value;
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
        }



        public void DMS_DeleteDeviceParameter(BMSEmployee wLoginUser, DMSDeviceParameter wDeviceParameter,
                OutResult<Int32> wErrorCode)
        {
            try
            {

                if (wDeviceParameter == null || StringUtils.isEmpty(wDeviceParameter.Name) || StringUtils.isEmpty(wDeviceParameter.VariableName)
                     || wDeviceParameter.DeviceID <= 0 || wDeviceParameter.DataType <= 0 || wDeviceParameter.DataClass <= 0)
                {
                    wErrorCode.set(MESException.Parameter.Value);
                    return;
                }

                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.DMS.getDBName();
                if (wErrorCode.Result != 0)
                    return;


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();

                wParamMap.Add("ID", wDeviceParameter.ID);
                wParamMap.Add("Active", 0);

                this.Delete(StringUtils.Format("{0}.dms_device_parameter", wInstance), wParamMap);


            }
            catch (Exception e)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
        }


        public void DMS_ActiveDeviceParameter(BMSEmployee wLoginUser, List<Int32> wIDList, int wActive,
                OutResult<Int32> wErrorCode)
        {
            try
            {
                wErrorCode.set(0);
                String wInstance = iPlant.Data.EF.MESDBSource.DMS.getDBName();
                if (wErrorCode.Result != 0)
                    return;

                if (wIDList == null || wIDList.Count <= 0)
                {
                    wErrorCode.set(MESException.Parameter.Value);
                    return;
                }
                wIDList.RemoveAll(p => p <= 0);
                if (wIDList.Count <= 0)
                    return;

                if (wActive != 1)
                    wActive = 2;
                String wSql = StringUtils.Format("UPDATE {0}.dms_device_parameter SET Active ={1} WHERE ID IN({2}) ;",
                        wInstance, wActive, StringUtils.Join(",", wIDList));


                this.mDBPool.update(wSql, null);
            }
            catch (Exception e)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
        }


    }
}
