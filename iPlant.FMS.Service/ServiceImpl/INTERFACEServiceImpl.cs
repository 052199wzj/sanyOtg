using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{


    public class INTERFACEServiceImpl : INTERFACEService
    {


        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INTERFACEServiceImpl));

        private static INTERFACEService Instance = null;

        public static INTERFACEService getInstance()
        {
            if (Instance == null)
                Instance = new INTERFACEServiceImpl();

            return Instance;
        }

        private LogService _LogService = new LogService();


        public DAQResponse INTERFACE_DAQContainerStatus(FMS.Models.DAQContainerStatus.DAQContainerStatus data)
        {
            return DAQInterfaceDAO.getInstance().INTERFACE_DAQContainerStatus(data);
        }


        public ServiceResult<Int32> INTERFACE_QueryContainer(BMSEmployee wBMSEmployee, MSSMaterialPoint wMSSMaterialPoint)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>();
            try
            {
                int wErrorCode = 0;
                int wID = wMSSMaterialPoint.ID;
                MSSMaterialFrame wMSSMaterialFrame = MSSMaterialFrameDAO.Instance.MSS_QueryMSSMaterialFrameList(wID, "", "", "",-1,1, Pagination.MaxSize, out wErrorCode).FirstOrDefault();
                MOMInterfaceDAO.getInstance().ContainerApply(wBMSEmployee, wMSSMaterialFrame, wMSSMaterialPoint.ReqID, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<String>> INTERFACE_SendStationState(BMSEmployee wBMSEmployee)
        {
            ServiceResult<List<String>> wResult = new ServiceResult<List<String>>();
            try
            {
                wResult.Result = new List<string>();
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);

                MOMInterfaceDAO.getInstance().SendStationState(wBMSEmployee, wErrorCode);
            }
            catch (Exception e)
            {
                wResult.FaultCode += e.ToString();
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        //public void WriteLog(string TextContent, string FileType, string SystemType, string VersionNo)
        //{
        //    _LogService.LogInfo(SystemType, FileType,"",0,"", TextContent);
        //    // SortSysInterfaceDAO.getInstance().WriteLog(TextContent, FileType, SystemType, VersionNo);
        //}
        public void WriteLog(string systemName, string interfaceName, string processName, int stepNo, string info, string details, string partID = "")
        {
            _LogService.LogInfo(systemName, interfaceName, processName, stepNo, info, details, partID);
            // SortSysInterfaceDAO.getInstance().WriteLog(TextContent, FileType, SystemType, VersionNo);
        }
    }
}
