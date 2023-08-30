using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public interface RFIDService
    {
        ServiceResult<Int32> RFID_Save(BMSEmployee wLoginUser, RFIDConfigure wRFIDConfigure);
        ServiceResult<List<RFIDConfigure>> RFID_SearchDate(int wId, String wStationCode, String wStationName, String wWorkshopName);
        ServiceResult<Int32> RFID_Detele(BMSEmployee wLoginUser, int wID);
    
        ServiceResult<List<RFIDErrorLog>> RFID_SearchErrorLog(String wStationName, int wLogTypeID, int wInteractiveObjectID, String wInterfaceName, DateTime wStartTime, DateTime wEndTime);
    }
}
