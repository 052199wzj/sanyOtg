using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Data.EF;

namespace iPlant.FMC.Service
{
    class ConfigureDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ConfigureDAO));
        private static ConfigureDAO Instance = null;

        private ConfigureDAO() : base()
        {

        }

        public static ConfigureDAO getInstance()
        {

            if (Instance == null)
                Instance = new ConfigureDAO();
            return Instance;
        }

        private static int RoleManageEnable = StringUtils
                .parseInt(GlobalConstant.GlobalConfiguration.GetValue("Role.Manager.Enable"));


        public List<RFIDConfigure> RFID_SearchDate(int wId, String wStationCode, String wStationName, String wWorkshopName, OutResult<Int32> wErrorCode)
        {
            wErrorCode.set(0);
            List<RFIDConfigure> wRFIDConfigureList = new List<RFIDConfigure>();

            try
            {
                String wSQLText = StringUtils.Format(
                           "select * FROM {0}.tb_rfid_info where (@Id<=0 or Id=@Id) and (@StationCode='' or StationCode like @StationCode) and (@StationName='' or StationName like @StationName)and(@WorkshopName='' or WorkshopName like @WorkshopName);",
                        //"select * FROM {0}.tb_rfid_info",
                        MESDBSource.Basic.getDBName());
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("Id", wId);
                wParms.Add("StationCode", StringUtils.isEmpty(wStationCode) ? wStationCode : "%" + wStationCode + "%");
                wParms.Add("StationName", StringUtils.isEmpty(wStationName) ? wStationName : "%" + wStationName + "%");
                wParms.Add("WorkshopName", wWorkshopName);

                List<Dictionary<String, Object>> wQueryResultList = base.mDBPool.queryForList(wSQLText, wParms);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    RFIDConfigure wRole = new RFIDConfigure();
                    wRole.Id = StringUtils.parseInt(wSqlDataReader["Id"]);
                    wRole.WorkshopCode = StringUtils.parseString(wSqlDataReader["WorkshopCode"]);
                    wRole.WorkshopName = StringUtils.parseString(wSqlDataReader["WorkshopName"]);
                    wRole.StationCode = StringUtils.parseString(wSqlDataReader["StationCode"]);
                    wRole.StationName = StringUtils.parseString(wSqlDataReader["StationName"]);
                    wRole.IPAddress = StringUtils.parseString(wSqlDataReader["IPAddress"]);
                    wRole.IsCheck = StringUtils.parseInt(wSqlDataReader["IsCheck"]);
                    wRole.IsToMES = StringUtils.parseInt(wSqlDataReader["IsToMES"]);
                    wRole.IsToScada = StringUtils.parseInt(wSqlDataReader["IsToScada"]);
                    wRole.RFIDModel = StringUtils.parseString(wSqlDataReader["RFIDModel"]);
                    wRole.MotorPLCIP = StringUtils.parseString(wSqlDataReader["MotorPLCIP"]);
                    wRole.CycleTime = StringUtils.parseInt(wSqlDataReader["CycleTime"]);
                    wRole.RunningStatus = StringUtils.parseString(wSqlDataReader["RunningStatus"]);
                    wRole.ConnectionStatus = StringUtils.parseString(wSqlDataReader["ConnectionStatus"]);
                    wRole.CarTypeCode = StringUtils.parseString(wSqlDataReader["CarTypeCode"]);
                    wRole.OrderCode = StringUtils.parseString(wSqlDataReader["OrderCode"]);
                    wRole.VINCode = StringUtils.parseString(wSqlDataReader["VINCode"]);
                    wRole.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                    wRole.RemarkInfo = StringUtils.parseString(wSqlDataReader["RemarkInfo"]);

                    wRFIDConfigureList.Add(wRole);
                }

            }
            catch (Exception ex)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error("RFID_SearchDate", ex);
            }
            //if (wRFIDConfigureList.Count > 0)
            //{
            return wRFIDConfigureList;
            //}
            //else
            //{
            //    return null;
            //}

        }

        public void RFID_Save(BMSEmployee wLoginUser, RFIDConfigure wRFIDConfigure,
               OutResult<Int32> wErrorCode)
        {

            try
            {
                wErrorCode.set(0);
                List<RFIDConfigure> wRFIDConfigureInfo = this.RFID_SearchDate(-1, "", "", "", wErrorCode);

                String wSQLText = "";
                if (wRFIDConfigure.Id == 0)
                {
                    wSQLText = StringUtils.Format(
                            "Insert into {0}.tb_rfid_info (" +
                            "WorkshopCode,WorkshopName,StationCode,StationName,IPAddress,IsCheck," +
                            "IsToMES,IsToScada,RFIDModel,MotorPLCIP,CycleTime,RunningStatus," +
                            "ConnectionStatus,OrderCode,VINCode,CarTypeCode,UpdateTime,RemarkInfo" +
                            ")" +
                            " Values (" +
                            "@WorkshopCode,@WorkshopName,@StationCode,@StationName,@IPAddress,@IsCheck," +
                            "@IsToMES,@IsToScada,@RFIDModel,@MotorPLCIP,@CycleTime,@RunningStatus," +
                            "@ConnectionStatus,@OrderCode,@VINCode,@CarTypeCode,@UpdateTime,@RemarkInfo" +
                            ");",
                            MESDBSource.Basic.getDBName());
                }
                else
                {
                    wSQLText = StringUtils.Format(
                            "Update {0}.tb_rfid_info set " +
                            "WorkshopCode=@WorkshopCode ,WorkshopName=@WorkshopName,StationCode=@StationCode,StationName=@StationName,IPAddress=@IPAddress ,IsCheck=@IsCheck ," +
                            "IsToMES=@IsToMES ,IsToScada=@IsToScada,RFIDModel=@RFIDModel,MotorPLCIP=@MotorPLCIP,CycleTime=@CycleTime,RunningStatus=@RunningStatus," +
                            "ConnectionStatus=@ConnectionStatus ,OrderCode=@OrderCode,VINCode=@VINCode,CarTypeCode=@CarTypeCode,UpdateTime=@UpdateTime ,RemarkInfo=@RemarkInfo Where Id=@Id ;",
                            MESDBSource.Basic.getDBName());
                }

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                if (wRFIDConfigure.Id > 0)
                {
                    wParms.Add("Id", wRFIDConfigure.Id);
                }
                wParms.Add("WorkshopCode", wRFIDConfigure.WorkshopCode);
                wParms.Add("WorkshopName", wRFIDConfigure.WorkshopName);
                wParms.Add("StationCode", wRFIDConfigure.StationCode);
                wParms.Add("StationName", wRFIDConfigure.StationName);
                wParms.Add("IPAddress", wRFIDConfigure.IPAddress);
                wParms.Add("IsCheck", wRFIDConfigure.IsCheck);
                wParms.Add("IsToMES", wRFIDConfigure.IsToMES);
                wParms.Add("IsToScada", wRFIDConfigure.IsToScada);
                wParms.Add("RFIDModel", wRFIDConfigure.RFIDModel);
                wParms.Add("MotorPLCIP", wRFIDConfigure.MotorPLCIP);
                wParms.Add("CycleTime", wRFIDConfigure.CycleTime);
                wParms.Add("RunningStatus", wRFIDConfigure.RunningStatus);
                wParms.Add("ConnectionStatus", wRFIDConfigure.ConnectionStatus);
                wParms.Add("OrderCode", wRFIDConfigure.OrderCode);
                wParms.Add("VINCode", wRFIDConfigure.VINCode);
                wParms.Add("CarTypeCode", wRFIDConfigure.CarTypeCode);
                wParms.Add("UpdateTime", wRFIDConfigure.UpdateTime);
                wParms.Add("RemarkInfo", wRFIDConfigure.RemarkInfo);

                //int wID;
                //base.mDBPool.update(wSQLText, wParms);
                //if (wRFIDConfigure.Id == 0)
                //    wRFIDConfigure.Id = wID;
            }
            catch (Exception ex)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error("RFID_Save", ex);
            }
        }
        public void RFID_Detele(BMSEmployee wLoginUser, int wID, OutResult<Int32> wErrorCode)
        {
            wErrorCode.set(0);

            try
            {
                String wSQLText = "";
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();


                wSQLText = StringUtils.Format("Delete from {0}.tb_rfid_info where Id=@Id",
                        MESDBSource.Basic.getDBName());

                wParms.Clear();
                wParms.Add("Id", wID);
                base.mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("RFID_Detele", ex);
                wErrorCode.set(MESException.DBSQL.Value);
            }

        }

        public List<RFIDErrorLog> RFID_SearchErrorLog(String wStationName, int wLogTypeID, int wInteractiveObjectID, String wInterfaceName, DateTime wStartTime, DateTime wEndTime, OutResult<Int32> wErrorCode)
        {
            wErrorCode.set(0);
            List<RFIDErrorLog> wRFIDErrorLogList = new List<RFIDErrorLog>();

            try
            {
                String wSQLText = StringUtils.Format(
                        "select * FROM {0}.log_information where (@LogTypeID<=0 or LogTypeID=@LogTypeID) and (@InteractiveObjectID<=0 or InteractiveObjectID=@InteractiveObjectID) and(@InterfaceName='' or InterfaceName like @InterfaceName) and (@StationName='' or StationName like @StationName)and((@StartTime< '2010-1-1' and @EndTime< '2010-1-1')or(UpdateTime>@StartTime and UpdateTime<@EndTime));",
                        //"select * FROM {0}.log_information",
                        MESDBSource.Basic.getDBName());
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("LogTypeID", wLogTypeID);
                wParms.Add("InteractiveObjectID", wInteractiveObjectID);
                wParms.Add("StationName", StringUtils.isEmpty(wStationName) ? wStationName : "%" + wStationName + "%");
                wParms.Add("InterfaceName", StringUtils.isEmpty(wInterfaceName) ? wInterfaceName : "%" + wInterfaceName + "%");
                wParms.Add("StartTime", StringUtils.parseDate(wStartTime));
                wParms.Add("EndTime", StringUtils.parseDate(wEndTime));

                List<Dictionary<String, Object>> wQueryResultList = base.mDBPool.queryForList(wSQLText, wParms);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    RFIDErrorLog wRole = new RFIDErrorLog();
                    wRole.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wRole.StationCode = StringUtils.parseString(wSqlDataReader["StationCode"]);
                    wRole.StationName = StringUtils.parseString(wSqlDataReader["StationName"]);
                    wRole.LogTypeID = StringUtils.parseInt(wSqlDataReader["LogTypeID"]);
                    wRole.InteractiveObjectID = StringUtils.parseInt(wSqlDataReader["InteractiveObjectID"]);
                    wRole.InterfaceName = StringUtils.parseString(wSqlDataReader["InterfaceName"]);
                    wRole.LogInformation = StringUtils.parseString(wSqlDataReader["LogInformation"]);
                    wRole.UserName = StringUtils.parseString(wSqlDataReader["UserName"]);
                    wRole.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                    wRFIDErrorLogList.Add(wRole);
                }

            }
            catch (Exception ex)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error("RFID_SearchErrorLog", ex);
            }
            return wRFIDErrorLogList;
        }
    }
}
