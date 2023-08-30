using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Service
{
    public class INFServiceImpl : INFService
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFServiceImpl));
        private static INFService _instance = new INFServiceImpl();
        public static INFService getInstance()
        {
            if (_instance == null)
                _instance = new INFServiceImpl();

            return _instance;
        }

        public ServiceResult<List<INFLesCuttingProcess>> INF_QueryINFLesCuttingProcessList(BMSEmployee wBMSEmployee,
            int wID, string wNestId, int wStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFLesCuttingProcess>> wResult = new ServiceResult<List<INFLesCuttingProcess>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFLesCuttingProcessDAO.Instance.INF_QueryINFLesCuttingProcessList(
                    wID, wNestId, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<List<INFDataManage>> INF_QueryINFDataManageList(BMSEmployee wBMSEmployee,
        int wID, int wSaveTime, int wStatus, Pagination wPagination)
        {
            ServiceResult<List<INFDataManage>> wResult = new ServiceResult<List<INFDataManage>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFDataManageDAO.Instance.INF_QueryINFDataManageList(
                    wID, wSaveTime, wStatus, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<int> INF_SaveINFDataManageList(BMSEmployee wBMSEmployee, INFDataManage wINFDataManage)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = INFDataManageDAO.Instance.INF_SaveINFDataManageList(wINFDataManage, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> INF_CleanData(BMSEmployee wBMSEmployee, INFDataManage wINFDataManage)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wResult.Result = INFDataManageDAO.Instance.INF_CleanData(wINFDataManage, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<INFLesStationState>> INF_QueryINFLesStationStateList(BMSEmployee wBMSEmployee,
            int wID, string wPalletCode, string wStationCode, sbyte wStationStatus, sbyte wStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFLesStationState>> wResult = new ServiceResult<List<INFLesStationState>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFLesStationStateDAO.Instance.INF_QueryINFLesStationStateList(
                    wID, wPalletCode, wStationCode, wStationStatus, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<List<INFLesOnCompletion>> INF_QueryINFLesOnCompletionList(BMSEmployee wBMSEmployee,
            int wID, string wOrderId, string wSeqNo, int wStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFLesOnCompletion>> wResult = new ServiceResult<List<INFLesOnCompletion>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFLesOnCompletionDAO.Instance.INF_QueryINFLesOnCompletionList(
                    wID, wOrderId, wSeqNo, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }
        /// <summary>
        /// by Demin 20221117
        ///  报工信息统计
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// </returns>
        public ServiceResult<List<INFLesUpDownMaterial>> INF_QueryINFLesUpDownMaterialList(BMSEmployee wBMSEmployee,
            int wID, string wFrameCode, string wNestId, string wOrder, string wProductNo, string wSeq, string wStationCode,
            string wSub, int wUseType, sbyte wStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFLesUpDownMaterial>> wResult = new ServiceResult<List<INFLesUpDownMaterial>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFLesUpDownMaterialDAO.Instance.INF_QueryINFLesUpDownMaterialList(
                    wID, wFrameCode, wNestId, wOrder, wProductNo, wSeq, wStationCode, wSub,
                    wUseType, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<List<INFSortsysSendcasing>> INF_QueryINFSortsysSendcasingList(BMSEmployee wBMSEmployee,
          int wID, String wProductionLline, String wSortStationNo, String wCutStationNo, String wMissionNo, SByte wStatus,
          DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFSortsysSendcasing>> wResult = new ServiceResult<List<INFSortsysSendcasing>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFSortsysSendcasingDAO.Instance.INF_QueryINFSortsysSendcasingList(
                  wID, wProductionLline, wSortStationNo, wCutStationNo, wMissionNo, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }



        public ServiceResult<List<INFSortsysEmptycontainerarrival>> INF_QueryINFSortsysEmptycontainerarrivalList(BMSEmployee wBMSEmployee,
          int wID, String wPalletPosition, String wPalletId, SByte wStatus,
          DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFSortsysEmptycontainerarrival>> wResult = new ServiceResult<List<INFSortsysEmptycontainerarrival>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFSortsysEmptycontainerarrivalDAO.Instance.INF_QueryINFSortsysEmptycontainerarrivalList(
                  wID, wPalletPosition, wPalletId, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<List<INFSortsysContainertakenaway>> INF_QueryINFSortsysContainertakenawayList(BMSEmployee wBMSEmployee,
        int wID, String wPalletPosition, String wPalletId, SByte wStatus,
        DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {
            ServiceResult<List<INFSortsysContainertakenaway>> wResult = new ServiceResult<List<INFSortsysContainertakenaway>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = INFSortsysContainertakenawayDAO.Instance.INF_QueryINFSortsysContainertakenawayList(
                  wID, wPalletPosition, wPalletId, wStatus, wStartTime, wEndTime, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }
    }
}
