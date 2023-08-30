using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public interface INTERFACEService
    {




        //#endregion

        #region MOM系统接口
        public ServiceResult<Int32> INTERFACE_QueryContainer(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint);
        ServiceResult<List<String>> INTERFACE_SendStationState(BMSEmployee wBMSEmployee);


        #endregion

        #region 数采系统接口
        DAQResponse INTERFACE_DAQContainerStatus(FMS.Models.DAQContainerStatus.DAQContainerStatus data);


        #endregion

        void WriteLog(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string partID = "");
    }
}
