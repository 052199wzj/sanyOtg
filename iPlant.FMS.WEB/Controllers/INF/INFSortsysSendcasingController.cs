using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.WEB
{
    public class INFSortsysSendcasingController : BaseController
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFSortsysSendcasingController));

        [HttpGet]
        public ActionResult All()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();

            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wProductionLline = StringUtils.parseString(Request.QueryParamString("ProductionLline"));
                String wSortStationNo = StringUtils.parseString(Request.QueryParamString("SortStationNo"));
                String wCutStationNo = StringUtils.parseString(Request.QueryParamString("CutStationNo"));
                String wMissionNo = StringUtils.parseString(Request.QueryParamString("MissionNo"));
                SByte wStatus = StringUtils.parseSByte(Request.QueryParamString("Status"), -1);
                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));
                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<INFSortsysSendcasing>> wServerRst = ServiceInstance.mINFService.INF_QueryINFSortsysSendcasingList(
                    wBMSEmployee, wID, wProductionLline, wSortStationNo, wCutStationNo, wMissionNo, wStatus, wStartTime, wEndTime, wPagination);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), wPagination.TotalPage);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }

            return Json(wResult);
        }
    }
}
