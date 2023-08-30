using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public interface MSSService
    {
        #region Material
        ServiceResult<List<MSSMaterial>> MSS_GetMaterialList(BMSEmployee wLoginUser, String wMaterialNo,
                String wMaterialName, String wGroes, int wActive, Pagination wPagination);

        ServiceResult<Int32> MSS_SaveMaterial(BMSEmployee wLoginUser, MSSMaterial wMMSMaterial);

        ServiceResult<Int32> MSS_ActiveMaterialList(BMSEmployee wLoginUser, List<Int32> wIDList,
        int wActive);

        ServiceResult<Int32> MSS_DeleteMaterialList(BMSEmployee wLoginUser, MSSMaterial wMaterial);

        #endregion

        #region MaterialLocation

        ServiceResult<List<MSSLocation>> MSS_GetMaterialLocation(BMSEmployee wLoginUser, int wType, Pagination wPagination);

        ServiceResult<Int32> MSS_UpdateMaterialLocation(BMSEmployee wLoginUser, MSSLocation wMSSLocation);

        #endregion

        #region MaterialStock

        ServiceResult<List<MSSStock>> MSS_GetMaterialStock(BMSEmployee wLoginUser, int wMaterialID, int wLocationID, string wMaterialLike, Pagination wPagination);


        ServiceResult<List<MSSMaterialOperationRecord>> MSS_GetMaterialStockDetail(BMSEmployee wLoginUser, int wStockID, Pagination wPagination);

        #endregion

        #region MaterialOperationRecord

        ServiceResult<Int32> MSS_SaveMaterialOperationRecord(BMSEmployee wLoginUser, MSSMaterialOperationRecord wMMSMaterialOperationRecord);

        ServiceResult<List<MSSMaterialOperationRecord>> MSS_GetMaterialOperationRecord(BMSEmployee wLoginUser, int wLocationID, String wLocationLike, String wMaterialLike,
            String wMaterialBatch, int wOperationType, Pagination wPagination);

        #endregion

        #region MaterialFrame

        ServiceResult<int> MSS_SaveMaterialFrame(BMSEmployee wBMSEmployee, MSSMaterialFrame wMSSMaterialFrame);

        ServiceResult<int> MSS_AddMaterialFrame(BMSEmployee wBMSEmployee, MSSMaterialFrame wMSSMaterialFrame);

        ServiceResult<int> MSS_DeleteMaterialFrameList(BMSEmployee wBMSEmployee, List<MSSMaterialFrame> wMSSMaterialFrameList);

        ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameList(BMSEmployee wBMSEmployee,
            int wID, String wCode, String wName, string wFrameCode, SByte wIsExistFrame, int wActive, Pagination wPagination);

        ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameInfoList(BMSEmployee wBMSEmployee,
        int wID, String wCode, String wName, string wFrameCode, SByte wIsExistFrame, int wActive,int wStepNo, Pagination wPagination);
        public ServiceResult<List<MSSMaterialFrame>> MSS_QueryMaterialFrameStatusList(BMSEmployee wBMSEmployee);

        ServiceResult<int> MSS_ActiveMaterialFrameList(BMSEmployee wBMSEmployee,
            int wActive, List<MSSMaterialFrame> wMSSMaterialFrameList);

        #endregion

        #region MaterialFrameParts

        ServiceResult<List<MSSMaterialFramePart>> MSS_QueryMaterialFramePartList(BMSEmployee wBMSEmployee,
            int wMaterialFrameID, Pagination wPagination);

        ServiceResult<int> MSS_AddMaterialFramePart(BMSEmployee wLoginUser, MSSMaterialFramePart wItem);
        #endregion
    }
}
