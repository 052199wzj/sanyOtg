using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{


    public class RFIDServiceImpl : RFIDService
    {


        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(RFIDServiceImpl));

        private static RFIDService Instance = null;

        public static RFIDService getInstance()
        {
            if (Instance == null)
                Instance = new RFIDServiceImpl();

            return Instance;
        }
        public ServiceResult<List<RFIDConfigure>> RFID_SearchDate(int wId, String wStationCode, String wStationName, String wWorkshopName)
        {
            ServiceResult<List<RFIDConfigure>> wResult = new ServiceResult<List<RFIDConfigure>>();
            try
            {
                wResult.Result = new List<RFIDConfigure>();
              
                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                wResult.Result = ConfigureDAO.getInstance().RFID_SearchDate(wId, wStationCode, wStationName, wWorkshopName, wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<Int32> RFID_Save(BMSEmployee wLoginUser, RFIDConfigure wRFIDConfigure)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                ConfigureDAO.getInstance().RFID_Save(wLoginUser, wRFIDConfigure, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> RFID_Detele(BMSEmployee wLoginUser, int wID)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                if (wID <= 0)
                {
                    return wResult;
                }
                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                ConfigureDAO.getInstance().RFID_Detele(wLoginUser, wID, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<RFIDErrorLog>> RFID_SearchErrorLog(String wStationName, int wLogTypeID, int wInteractiveObjectID, String wInterfaceName, DateTime wStartTime, DateTime wEndTime)
        {
            ServiceResult<List<RFIDErrorLog>> wResult = new ServiceResult<List<RFIDErrorLog>>();
            try
            {
                wResult.Result = new List<RFIDErrorLog>();

                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                wResult.Result = ConfigureDAO.getInstance().RFID_SearchErrorLog(wStationName, wLogTypeID, wInteractiveObjectID, wInterfaceName, wStartTime, wEndTime, wErrorCode);

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
