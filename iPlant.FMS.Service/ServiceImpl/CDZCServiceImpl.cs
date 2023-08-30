using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{


    public class CDZCServiceImpl : CDZCService
    {


        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CDZCServiceImpl));

        private static CDZCService Instance = null;

        public static CDZCService getInstance()
        {
            if (Instance == null)
                Instance = new CDZCServiceImpl();

            return Instance;
        }

        public ServiceResult<AnnualIndicators> CDZC_SearchDate()
        {
            ServiceResult<AnnualIndicators> wResult = new ServiceResult<AnnualIndicators>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                //wResult.Result = BMSRoleDAO.getInstance().BMS_GetRoleList(wLoginUser, wName, wDepartmentID, wUserID, wActive, wPagination, wErrorCode);
               wResult.Result = CDYearDAO.getInstance().CDZC_SearchDate(wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<Int32> CDZC_Save(BMSEmployee wLoginUser, AnnualIndicators wAnnualIndicators)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>();
                wErrorCode.set(0);
                CDYearDAO.getInstance().CDZC_Save(wLoginUser, wAnnualIndicators, wErrorCode);
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
