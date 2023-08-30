using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMS.Service
{
    public interface INFService
    {
        ServiceResult<List<INFLesCuttingProcess>> INF_QueryINFLesCuttingProcessList(BMSEmployee wBMSEmployee,
            int wID, String wNestId, int wStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<List<INFLesStationState>> INF_QueryINFLesStationStateList(BMSEmployee wBMSEmployee,
            int wID, String wPalletCode, String wStationCode, SByte wStationStatus, SByte wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<List<INFLesOnCompletion>> INF_QueryINFLesOnCompletionList(BMSEmployee wBMSEmployee,
            int wID, String wOrderId, String wSeqNo, int wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<List<INFLesUpDownMaterial>> INF_QueryINFLesUpDownMaterialList(BMSEmployee wBMSEmployee,
            int wID, String wFrameCode, String wNestId, String wOrder, String wProductNo,
            String wSeq, String wStationCode, String wSub, int wUseType, SByte wStatus,
            DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<List<INFSortsysSendcasing>> INF_QueryINFSortsysSendcasingList(BMSEmployee wBMSEmployee,
           int wID, String wProductionLline, String wSortStationNo, String wCutStationNo, String wMissionNo, SByte wStatus,
           DateTime wStartTime, DateTime wEndTime, Pagination wPagination);


        ServiceResult<List<INFSortsysEmptycontainerarrival>> INF_QueryINFSortsysEmptycontainerarrivalList(BMSEmployee wBMSEmployee,
           int wID, String wPalletPosition, String wPalletId, SByte wStatus,
           DateTime wStartTime, DateTime wEndTime, Pagination wPagination);

        ServiceResult<List<INFSortsysContainertakenaway>> INF_QueryINFSortsysContainertakenawayList(BMSEmployee wBMSEmployee,
         int wID, String wPalletPosition, String wPalletId, SByte wStatus,
         DateTime wStartTime, DateTime wEndTime, Pagination wPagination);
        ServiceResult<List<INFDataManage>> INF_QueryINFDataManageList(BMSEmployee wBMSEmployee,
        int wID, int wSaveTime, int wStatus, Pagination wPagination);
        ServiceResult<int> INF_SaveINFDataManageList(BMSEmployee wBMSEmployee, INFDataManage wINFDataManage);

        ServiceResult<int> INF_CleanData(BMSEmployee wBMSEmployee, INFDataManage wINFDataManage);
    }
}
