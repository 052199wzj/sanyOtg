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
    public class MSSServiceImpl : MSSService
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSServiceImpl));
        private static MSSService _instance = new MSSServiceImpl();

        public static MSSService getInstance()
        {
            if (_instance == null)
                _instance = new MSSServiceImpl();

            return _instance;
        }

        #region Material

        public ServiceResult<List<MSSMaterial>> MSS_GetMaterialList(BMSEmployee wLoginUser, string wMaterialNo, string wMaterialName, string wGroes, int wActive, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterial>> wResult = new ServiceResult<List<MSSMaterial>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wResult.setResult(MSSMaterialDAO.getInstance().GetAll(wLoginUser, wMaterialNo, wMaterialName, wGroes, wActive, wPagination, wErrorCode));
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> MSS_SaveMaterial(BMSEmployee wLoginUser, MSSMaterial wMSSMaterial)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                MSSMaterialDAO.getInstance().Update(wLoginUser, wMSSMaterial, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> MSS_ActiveMaterialList(BMSEmployee wLoginUser, List<int> wIDList, int wActive)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                MSSMaterialDAO.getInstance().Active(wLoginUser, wIDList, wActive, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> MSS_DeleteMaterialList(BMSEmployee wLoginUser, MSSMaterial wMaterial)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                MSSMaterialDAO.getInstance().Delete(wLoginUser, wMaterial, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        #endregion

        #region MaterialLocation

        public ServiceResult<List<MSSLocation>> MSS_GetMaterialLocation(BMSEmployee wLoginUser, int wType, Pagination wPagination)
        {
            ServiceResult<List<MSSLocation>> wResult = new ServiceResult<List<MSSLocation>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wResult.setResult(MSSLocationDAO.getInstance().SelectAll(wLoginUser, wType, wPagination, wErrorCode));
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<Int32> MSS_UpdateMaterialLocation(BMSEmployee wLoginUser, MSSLocation wMSSLocation)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                MSSLocationDAO.getInstance().Update(wLoginUser, wMSSLocation, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        #endregion

        #region MaterialStock

        public ServiceResult<List<MSSStock>> MSS_GetMaterialStock(BMSEmployee wLoginUser, int wMaterialID, int wLocationID, string wMaterialLike, Pagination wPagination)
        {
            ServiceResult<List<MSSStock>> wResult = new ServiceResult<List<MSSStock>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wResult.setResult(MSSStockDAO.getInstance().GetAll(wLoginUser, wMaterialID, wLocationID, wMaterialLike, wPagination, wErrorCode));
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialOperationRecord>> MSS_GetMaterialStockDetail(BMSEmployee wLoginUser, int wStockID, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialOperationRecord>> wResult = new ServiceResult<List<MSSMaterialOperationRecord>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wResult.setResult(MSSMaterialOperationRecordDAO.getInstance().GetMaterialStock(wLoginUser, wStockID, wPagination, wErrorCode));
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        #endregion

        #region MaterialOperationRecord

        public ServiceResult<Int32> MSS_SaveMaterialOperationRecord(BMSEmployee wLoginUser, MSSMaterialOperationRecord wMSSMaterialOperationRecord)
        {
            ServiceResult<Int32> wResult = new ServiceResult<Int32>(0);
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wErrorCode.set(0);
                MSSMaterialOperationRecordDAO.getInstance().Add(wLoginUser, wMSSMaterialOperationRecord, wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();

            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialOperationRecord>> MSS_GetMaterialOperationRecord(BMSEmployee wLoginUser, int wLocationID, String wLocationLike, String wMaterialLike,
            String wMaterialBatch, int wOperationType, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialOperationRecord>> wResult = new ServiceResult<List<MSSMaterialOperationRecord>>();
            try
            {
                OutResult<Int32> wErrorCode = new OutResult<Int32>(0);
                wResult.setResult(MSSMaterialOperationRecordDAO.getInstance().GetMaterialOperationRecord(wLoginUser, wLocationID, wLocationLike, wMaterialLike,
             wMaterialBatch, wOperationType, wPagination, wErrorCode));
                wResult.FaultCode += MESException.getEnumType(wErrorCode.get()).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        #endregion

        #region MaterialFrame

        public ServiceResult<int> MSS_SaveMaterialFrame(BMSEmployee wBMSEmployee, MSSMaterialFrame wMSSMaterialFrame)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wMSSMaterialFrame.EditID = wBMSEmployee.ID;
                wMSSMaterialFrame.EditTime = DateTime.Now;
                wResult.Result = MSSMaterialFrameDAO.Instance.MSS_SaveMSSMaterialFrame(wMSSMaterialFrame, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_AddMaterialFrame(BMSEmployee wBMSEmployee, MSSMaterialFrame wMSSMaterialFrame)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                wMSSMaterialFrame.CreateID = wBMSEmployee.ID;
                wMSSMaterialFrame.CreateTime = DateTime.Now;
                wMSSMaterialFrame.EditID = wBMSEmployee.ID;
                wMSSMaterialFrame.EditTime = DateTime.Now;
                wResult.Result = MSSMaterialFrameDAO.Instance.MSS_SaveMSSMaterialFrame(wMSSMaterialFrame, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<int> MSS_DeleteMaterialFrameList(BMSEmployee wBMSEmployee, List<MSSMaterialFrame> wMSSMaterialFrameList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;
                wErrorCode = MSSMaterialFrameDAO.Instance.MSS_DeleteMSSMaterialFrameList(wMSSMaterialFrameList);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        public ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameList(BMSEmployee wBMSEmployee,
            int wID, String wCode, String wName, string wFrameCode, SByte wIsExistFrame, int wActive, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialFrame>> wResult = new ServiceResult<List<MSSMaterialFrame>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialFrameDAO.Instance.MSS_QueryMSSMaterialFrameList(wID, wCode, wName, wFrameCode, wIsExistFrame, wActive, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameInfoList(BMSEmployee wBMSEmployee,
      int wID, String wCode, String wName, string wFrameCode, SByte wIsExistFrame, int wActive,int wStepNo, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialFrame>> wResult = new ServiceResult<List<MSSMaterialFrame>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialFrameDAO.Instance.MSS_QueryMSSMaterialFrameInfoList(wID, wCode, wName, wFrameCode, wIsExistFrame, wActive, wStepNo, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameStatusList(BMSEmployee wBMSEmployee)
        {
            ServiceResult<List<MSSMaterialFrame>> wResult = new ServiceResult<List<MSSMaterialFrame>>();
            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialFrameDAO.Instance.MSS_QueryMSSMaterialFrameStatusList(out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }
        public ServiceResult<int> MSS_ActiveMaterialFrameList(BMSEmployee wBMSEmployee,
            int wActive, List<MSSMaterialFrame> wMSSMaterialFrameList)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();
            try
            {
                int wErrorCode = 0;

                foreach (MSSMaterialFrame wFMCShiftItem in wMSSMaterialFrameList)
                {
                    wFMCShiftItem.Active = wActive;
                    wFMCShiftItem.EditID = wBMSEmployee.ID;
                    wFMCShiftItem.EditTime = DateTime.Now;
                    MSSMaterialFrameDAO.Instance.MSS_SaveMSSMaterialFrame(wFMCShiftItem, out wErrorCode);
                }

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }
            return wResult;
        }

        #endregion

        #region MaterialFrameParts

        public ServiceResult<List<MSSMaterialFramePart>> MSS_QueryMaterialFramePartList(BMSEmployee wBMSEmployee,
            int wMaterialFrameID, Pagination wPagination)
        {
            ServiceResult<List<MSSMaterialFramePart>> wResult = new ServiceResult<List<MSSMaterialFramePart>>();

            try
            {
                int wErrorCode = 0;

                wResult.Result = MSSMaterialFramePartDAO.Instance.MSS_QueryMSSMaterialFramePartList(
                    wMaterialFrameID, wPagination, out wErrorCode);

                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }

        public ServiceResult<int> MSS_AddMaterialFramePart(BMSEmployee wLoginUser, MSSMaterialFramePart wItem)
        {
            ServiceResult<int> wResult = new ServiceResult<int>();

            try
            {
                int wErrorCode = 0;

                wItem.CreateID = wLoginUser.ID;
                wItem.CreateTime = DateTime.Now;
                wItem.EditID = wLoginUser.ID;
                wItem.EditTime = DateTime.Now;

                wResult.Result = MSSMaterialFramePartDAO.Instance.MSS_InsertMSSMaterialFramePart(wItem, out wErrorCode);
                wResult.FaultCode += MESException.getEnumType(wErrorCode).getLabel();
            }
            catch (Exception e)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, e);
            }

            return wResult;
        }
        #endregion
    }
}
